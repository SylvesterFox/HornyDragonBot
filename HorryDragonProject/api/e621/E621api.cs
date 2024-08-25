using Microsoft.Extensions.Logging;
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
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (X11; Linux x86_64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/126.0.0.0 Safari/537.36");
                    /*client.DefaultRequestHeaders.Add("Authorization", "Basic " + Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(_user + ":" + _token)));*/
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(_user + ":" + _token)));

                    var uriString = uri + $"&limit={_limit}$tags={tag}";

                    
                    HttpResponseMessage response = await client.GetAsync(uriString);

                    response.EnsureSuccessStatusCode();

                    var _response = await response.Content.ReadAsStringAsync();
                    return _response;

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

            var tag = $"{tags}+rating:{reating}";
            var _response = await _RequsetApi(_address, tag);
            var deserializedResponse = JsonSerializer.Deserialize<E621Post>(_response);

            if(deserializedResponse != null)
            {
                Response = deserializedResponse.Posts;
            }
            
        }


    }

}

