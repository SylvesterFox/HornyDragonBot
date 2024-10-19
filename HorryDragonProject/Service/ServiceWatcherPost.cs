
using Discord.WebSocket;
using HorryDragonProject.api.e621;
using HorryDragonProject.Custom;
using HorryDragonProject.Module;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;


namespace HorryDragonProject.Service
{
    public class ServiceWatcherPost
    {
        private readonly List<QueryData> _queries;
        private Dictionary<ulong, Timer> _timers;
        private SocketTextChannel _textChannel;
        private readonly DiscordSocketClient _client;
        public readonly ILogger _logger;
        public E621api api { private get; set; }

        public ServiceWatcherPost(DiscordSocketClient client, ILogger<ServiceWatcherPost> logger, IServiceProvider service)
        {
            _queries = new List<QueryData>();
            _timers = new Dictionary<ulong, Timer>();
            api = service.GetRequiredService<E621api>();
            _client = client;
            _logger = logger;
        }

        public void StartWatchigAll()
        {
            foreach (var queryData in _queries) {
               var timer = new Timer(async _ =>
               {
                   await CheckForNewPost(queryData);
                   _logger.LogDebug($"Watchig response: {queryData.Tags}");
               }, null, TimeSpan.Zero, queryData.Interval);
                _timers[queryData.channelId] = timer;
            }
        }

        public void StopWatchig(ulong channelId) {
            
            if (_timers.ContainsKey(channelId))
            {
                _timers[channelId].Change(Timeout.Infinite, 0);
            }
        }

        public void TagQueries(List<Queries> queries)
        {
            foreach (var query in queries)
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
        }

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
