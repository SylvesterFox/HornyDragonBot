
using Discord.WebSocket;
using DragonData;
using DragonData.Module;
using HorryDragonProject.api.e621;
using HorryDragonProject.Custom;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;


namespace HorryDragonProject.Service
{

    public class ServiceWatcherPost
    {
        private readonly List<QueryData> _queries;
        private Dictionary<ulong, TimerInfo> _timers;
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
            _timers = new Dictionary<ulong, TimerInfo>();
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


            var dbCheckTimer = new Timer(async _ => await UpdateQueriesFromDatabase(), null, TimeSpan.Zero, TimeSpan.FromMinutes(1));

            await Task.Run(ProcessQueue);
        }

        private async Task UpdateQueriesFromDatabase()
        {
            var dbQueries = await dragonDataBase.watchlist.GetActiveQueriesAsync();

            foreach(var query in dbQueries)
            {
                if (_timers.ContainsKey(query.channelID))
                {

                    var existingQueryData = _timers[query.channelID];

                    if (existingQueryData.Interval != query.interval)
                    {
                        _logger.LogInformation($"[DB Update] Updating interval for query ID {query.channelID}");

                        StopWatchig(query.channelID);

                        StartTimerForQuery(query);
                    }

                } else
                {
                    _logger.LogInformation($"[DB Update] Adding new query with ID {query.channelID}");
                    StartTimerForQuery(query);
                }
            }

            var activeQueryIds = new HashSet<ulong>(dbQueries.Select(q => q.channelID));
            foreach (var existingTimerId in _timers.Keys.ToList())
            {
                if (!activeQueryIds.Contains(existingTimerId))
                {
                    _logger.LogInformation($"[DB Update] Stopping query with ID {existingTimerId}");
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

                var timer = new Timer(_ =>
                {
                    QueueQuery(queryData);
                }, null, TimeSpan.Zero, queryData.Interval);
                _timers[query.channelID] = new TimerInfo { Timer = timer, Interval = query.interval};
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

        private void StopWatchig(ulong channelId) {
            
            if (_timers.ContainsKey(channelId))
            {
                _timers[channelId].Timer.Change(Timeout.Infinite, 0);
                _timers.Remove(channelId);
            }
        }


        private async Task CheckForNewPost(QueryData queryData)
        {
            var tag = queryData.Tags;
            _logger.LogDebug($"The tracker executes a request with the tag: {queryData.Tags} For the guild: {queryData.guild.Name}:");
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
                _logger.LogCritical(ex.Message);
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

        private class TimerInfo
        {
            public Timer Timer { get; set; }
            public int Interval { get; set; }
        }

    }
}
