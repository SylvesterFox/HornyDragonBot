
using Discord.WebSocket;
using HorryDragonProject.api.e621;
using HorryDragonProject.Custom;
using Microsoft.Extensions.Logging;


namespace HorryDragonProject.Service
{
    public class ServiceWatcherPost
    {
        /*private HashSet<int> _lastPostIds;*/
        private readonly List<QueryData> _queries;
        private Timer _timer;
        private readonly E621api _api;
        private SocketTextChannel _textChannel;
        private readonly DiscordSocketClient _client;
        public readonly ILogger _logger;

        public ServiceWatcherPost(DiscordSocketClient client, ILogger<ServiceWatcherPost> logger)
        {
            _queries = new List<QueryData>();
            _api = new E621api();
            _client = client;
            _logger = logger;
        }

        public void StartWatchig(TimeSpan interval)
        {
            _timer = new Timer(async _ => await CheckForNewPost(), null, TimeSpan.Zero, interval);
        }

        public void StopWatchig() {
            _timer?.Change(Timeout.Infinite, 0);
        }

        public void TagQueries(List<string> queries)
        {
            foreach (var query in queries)
            {
                _queries.Add(new QueryData
                {
                    Tags = query,
                    LastPostIds = new HashSet<int>()
                });
            }
        }

        private async Task CheckForNewPost()
        {
           foreach(var query in _queries)
            {
                var tag = query.Tags;
                try
                {
                    _textChannel = (SocketTextChannel)_client.GetChannel(918184699405426738);
                    HashSet<int> newPostIds = new HashSet<int>();
                    await _api.GetAllResponse(tag, linit: 1, random: false);

                    if (_api.Response == null)
                    {
                        return;
                    }

                    foreach (var post in _api.Response)
                    {
                        newPostIds.Add(post.Id);

                        if (!query.LastPostIds.Contains(post.Id))
                        {
                            _logger.LogDebug($"\nNew post ID: {post.Id}\nCreated At: {post.CreatedAt}\nFile url: {post.File.Url}");
                            await _textChannel.SendMessageAsync(embed: TemplateEmbeds.PostEmbedTemplate(post, tag).Build());
                        }
                    }

                    query.LastPostIds = newPostIds;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }

            
        }

        private class QueryData
        {
            public string Tags { get; set; }
            public HashSet<int> LastPostIds { get; set; }
        }
    }
}
