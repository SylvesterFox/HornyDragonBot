using HorryDragonProject.Settings;
using Microsoft.Extensions.Logging;
using RestSharp;
using System.Text.Json;

namespace HorryDragonProject.api.e621{
    public class E621api
    {

        public readonly ILogger _logger;
        private string _address;
        private string _user;
        private string _token;
        private readonly BotConfig? _botConfig;
        private int _limit { get; set; } = 1;
        
        private string _requrstPage { get; set; } = "1";
        public string reating { get; set; } = "e";
        private string types { get; set; } = string.Empty;
        public List<Post> Response { get; set; }

        public List<string> BlockTag = new List<string>();

        public E621api(ILogger<E621api> logger)
        {
            _botConfig = BotSettingInit.Instance.LoadedConfig;
            _token = _botConfig.TOKEN_E621;
            _user = _botConfig.USER_E621;
            _address = $"https://e621.net/posts.json";
            _logger = logger;
        }

        private async Task<string> _RequsetApi(string uri, string tag, bool random)
        {
            try
            {

                var requrst = new RestRequest(uri, Method.Get);

                if (random)
                {
                    tag += $" {types} rating:{reating} order:random";
                } else
                {
                    tag += $" {types} rating:{reating}";
                }

                if (BlockTag.Count != 0)
                {
                    string urlblocklist = string.Join(" ", BlockTag.Select(x => "-" + x));
                    tag += urlblocklist;
                    _logger.LogDebug($"Blocklist {urlblocklist}");
                }

                requrst.AddParameter("tags", tag);
                requrst.AddParameter("limit", _limit);
                requrst.AddParameter("page", _requrstPage);

                requrst.AddHeader("User-Agent", "HorryDragonProject/1.0 (by Dragofox)");
                requrst.AddHeader("Authorization", "Basic " + Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(_user + ":" + _token)));

                var restApi = new RestClient();
                var response = await restApi.ExecuteAsync(requrst);
                if ((int)response.StatusCode != 200)
                {
                    _logger.LogTrace("[Error] " + response.Content);
                    return string.Empty;
                } else
                {
                    _logger.LogDebug($"{response.ResponseStatus} Code: {response.StatusCode}");
                    return response.Content;
                }
            }
            catch (Exception ex)
            {
                _logger.LogTrace("[Error] " +  ex.Message);
                throw new HttpRequestException(HttpRequestError.ConnectionError);
            }
        }

        public async Task<Post?> GetPost(string tags) {

            var _response = await _RequsetApi(_address, tags, false);
            var deserializedResponse = JsonSerializer.Deserialize<E621Post>(_response);

            if (deserializedResponse != null)
            {
                foreach (var post in deserializedResponse.Posts)
                {
                    return post;
                }
            }

            throw new NullReferenceException($"Response is null!");
        }



        public async Task GetAllResponse(string tags, int linit = 320, string? type = "", string? page = null, bool random = true)
        {
            _limit = linit;

            if (type != "type:webm") {
                types = type + " -type:webm";
            } else {
                types = type;
            }
            
            if (_limit > 320)
            {
                _limit = 320;
            }

            var _response = await _RequsetApi(_address, tags, random);

            var deserializedResponse = JsonSerializer.Deserialize<E621Post>(_response);

            if(deserializedResponse != null)
            {
                Response = deserializedResponse.Posts;
            }
            
        }


    }

}

