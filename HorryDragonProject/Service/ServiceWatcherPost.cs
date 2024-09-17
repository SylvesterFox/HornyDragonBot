
using HorryDragonProject.api.e621;
using HorryDragonProject.Settings;


namespace HorryDragonProject.Service
{
    internal class ServiceWatcherPost
    {
        private HashSet<int> _lastPostIds;
        private Timer _timer;
        private readonly E621api _api;
        private readonly BotConfig? _botConfig;

        public ServiceWatcherPost()
        {
            _lastPostIds = new HashSet<int>();
            _botConfig = BotSettingInit.Instance.LoadedConfig;
            _api = new E621api(_botConfig.TOKEN_E621, _botConfig.USER_E621);
            
        }

        public void StartWatchig(TimeSpan interval)
        {
            _timer = new Timer(async _ => await CheckForNewPost(), null, TimeSpan.Zero, interval);
        }

        public void StopWatchig() {
            _timer?.Change(Timeout.Infinite, 0);
        }

        private async Task CheckForNewPost()
        {
            var tag = "dragon";
            Console.WriteLine("\n\n");
            try
            {
                HashSet<int> newPostIds = new HashSet<int>();
                await _api.GetAllResponse(tag, linit: 1, random: false);

                if (_api.Response == null)
                {
                    return;
                }

                foreach (var post in _api.Response)
                {
                    newPostIds.Add(post.Id);

                    if (!_lastPostIds.Contains(post.Id))
                    {
                        Console.WriteLine($"New post ID: {post.Id}");
                        Console.WriteLine($"Created At: {post.CreatedAt}");
                        Console.WriteLine($"File url: {post.File.Url}");
                    }
                }

                _lastPostIds = newPostIds;
            } catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}
