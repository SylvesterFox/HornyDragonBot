
using Discord.WebSocket;
using DragonData;
using DragonData.Module;
using HorryDragonProject.api.e621;
using HorryDragonProject.Custom;
using HorryDragonProject.Module;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileSystemGlobbing;
using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;


namespace HorryDragonProject.Service
{
    public class ServiceWatcherPost
    {
        private readonly List<QueryData> _queries;
        private Dictionary<ulong, Timer> _timers;
        private SocketTextChannel _textChannel;
        private readonly DiscordSocketClient _client;
        public readonly ILogger _logger;
        private ConcurrentQueue<QueryData> _queriesQueue;
        private bool _isProcessing;
        public E621api api { private get; set; }
        public DragonDataBase dragonDataBase { private get; set; }

        public ServiceWatcherPost(DiscordSocketClient client, ILogger<ServiceWatcherPost> logger, IServiceProvider service)
        {
            _queries = new List<QueryData>();
            _timers = new Dictionary<ulong, Timer>();
            _queriesQueue = new ConcurrentQueue<QueryData>();
            api = service.GetRequiredService<E621api>();
            dragonDataBase = service.GetRequiredService<DragonDataBase>();
            _client = client;
            _logger = logger;
        }

        public async Task StartWatchig()
        {
            var initialQueryies = await dragonDataBase.watchlist.GetActiveQueriesAsync();

            foreach (var query in initialQueryies)
            {
                StartTimerForQuery(query);
            }

            /*foreach (var queryData in _queries) {
               var timer = new Timer(_ =>
               {
                   QueueQuery(queryData);
                   _logger.LogDebug($"Watchig response: {queryData.Tags}");
               }, null, TimeSpan.Zero, queryData.Interval);
                _timers[queryData.channelId] = timer;
            }*/

            var dbCheckTimer = new Timer(async _ => await UpdateQueriesFromDatabase(), null, TimeSpan.Zero, TimeSpan.FromMinutes(1));

            await Task.Run(ProcessQueue);
        }

        private async Task UpdateQueriesFromDatabase()
        {
            var dbQueries = await dragonDataBase.watchlist.GetActiveQueriesAsync();

            foreach(var query in dbQueries)
            {
                if (!_timers.ContainsKey(query.channelID))
                {
                    StartTimerForQuery(query);
                }
            }

            var activeQueryIds = new HashSet<ulong>(dbQueries.Select(q => q.channelID));
            foreach (var existingTimerId in _timers.Keys.ToList())
            {
                if (!activeQueryIds.Contains(existingTimerId))
                {
                    StopWatchig(existingTimerId);
                }
            }
        }

        private void StartTimerForQuery(WatcherPostModule query)
        {
            if (!_timers.ContainsKey(query.channelID))
            {
                var guild = _client.GetGuild(query.guildID);
                var queryData = new QueryData
                {
                    channelId = query.channelID,
                    Tags = query.watchTags,
                    Interval = TimeSpan.FromMinutes(query.interval),
                    LastPostIds = new HashSet<int>(),
                    guild = guild,
                };

                var timer = new Timer(_ => QueueQuery(queryData), null, TimeSpan.Zero, queryData.Interval);
                _timers[query.channelID] = timer;
            }
        }

        private async Task ProcessQueue()
        {
            _isProcessing = true;

            while (_isProcessing)
            {
                if (_queriesQueue.TryDequeue(out var queryData))
                {
                    await CheckForNewPost(queryData);

                    await Task.Delay(2000);
                } else
                {
                    await Task.Delay(500);
                }
            }
        }

        private void QueueQuery(QueryData queryData)
        {
            _queriesQueue.Enqueue(queryData);
        }

        public void StopWatchig(ulong channelId) {
            
            if (_timers.ContainsKey(channelId))
            {
                _timers[channelId].Change(Timeout.Infinite, 0);
                _timers.Remove(channelId);
            }
        }

/*        public void AddTagQueries(Queries query)
        {
            _queries.Add(new QueryData
            {
                Tags = query.Tag,
                LastPostIds = new HashSet<int>(),
                Interval = query.interval,
                channelId = query.channelId,
                guild = query.guild
            });
        }

        public void DataTagQueries(List<WatcherPostModule> watchers)
        {
            foreach (var watcher in watchers)
            {
                var guild = _client.GetGuild(watcher.guildID);
                _queries.Add(new QueryData
                {
                    Tags = watcher.watchTags,
                    LastPostIds = new HashSet<int>(),
                    Interval = TimeSpan.FromMilliseconds(watcher.interval),
                    guild = guild,
                });
            }
        }*/

        private async Task CheckForNewPost(QueryData queryData)
        {
            var tag = queryData.Tags;
            try
            {
                SocketTextChannel _textChannel = (SocketTextChannel)_client.GetChannel(queryData.channelId);
                HashSet<int> newPostIds = new HashSet<int>();
                await api.GetAllResponse(tag, linit: 1, random: false, guild: queryData.guild);

                if (api.Response == null)
                {
                    return;
                }

                foreach (var post in api.Response)
                {
                    newPostIds.Add(post.Id);

                    if (!queryData.LastPostIds.Contains(post.Id))
                    {
                        _logger.LogDebug($"\nNew post ID: {post.Id}\nCreated At: {post.CreatedAt}\nFile url: {post.File.Url}");
                        await _textChannel.SendMessageAsync(embed: TemplateEmbeds.PostEmbedTemplate(post, tag).Build());
                    }
                }

                queryData.LastPostIds = newPostIds;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            
        }

        private class QueryData
        {
            public string Tags { get; set; }
            public TimeSpan Interval { get; set; }
            public HashSet<int> LastPostIds { get; set; }
            public ulong channelId { get; set; }
            public SocketGuild guild { get; set; }
        }

      
    }
}
