using Microsoft.Extensions.Logging;
using RestSharp;
using System.Net.Http.Headers;
using System.Text.Json;

namespace HorryDragonProject.api.e621{
    public class E621api
    {

        public readonly ILogger _logger;    

        private string _address;

        private string _user;

        private string _token;
        private int _limit { get; set; } = 1;
        public string reating { get; set; } = "e";
        public List<Post> Response { get; set; }

        public E621api(string tokenApi, string user, ILoggerFactory log)
        {
            _token = tokenApi;
            _user = user;
            _address = $"https://e621.net/posts.json";
            _logger = log.CreateLogger("E621api");
        }

        private async Task<string> _RequsetApi(string uri, string tag)
        {
            try
            {

                var requrst = new RestRequest(uri, Method.Get);
                requrst.AddParameter("tags", $"rating:{reating}");
                requrst.AddParameter("tags", tag);
                requrst.AddParameter("limit", _limit);

                requrst.AddHeader("User-Agent", "HorryDragonProject/1.0 (by Dragofox)");
                requrst.AddHeader("Authorization", "Basic " + Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(_user + ":" + _token)));

                var restApi = new RestClient();
                var response = await restApi.ExecuteAsync(requrst);
                if ((int)response.StatusCode != 200)
                {
                    _logger.LogTrace("Error: " + response.Content);
                    return null;
                } else
                {
                    return response.Content;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw new HttpRequestException(HttpRequestError.ConnectionError);
            }
        }

        public async Task<Post?> GetPost(string tags) {

            var tag = $"{tags}+rating:{reating}";
            var _response = await _RequsetApi(_address, tag);
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

        public async Task GetAllResponse(string tags, int linit = 3)
        {
            _limit = linit;
            if (_limit > 320)
            {
                _limit = 320;
            }

            var _response = await _RequsetApi(_address, tags);
            var deserializedResponse = JsonSerializer.Deserialize<E621Post>(_response);

            if(deserializedResponse != null)
            {
                Response = deserializedResponse.Posts;
            }
            
        }


    }

}

