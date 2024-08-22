using System.Net;
using System.Text.Json;

namespace HorryDragonProject.api.e621{
    public class E621api
    {
        private HttpWebRequest _request;
        private string _address;
        public int limit { get; set; } = 1;
        public string reating { get; set; } = "e";
        public string Response { get; set; }
        // private HttpClientHandler _clientHandler = new HttpClientHandler();
        public E621api(string tokenApi, string user)
        {
            _address = $"https://e621.net/posts.json?login={user}&api_key={tokenApi}&limit={limit}";
            // _clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
        }

        public async Task RunGetPost(string tags) {
            var uri = _address + $"&tags={tags}+rating:{reating}";
           try
           {
               using (HttpClient client = new HttpClient())
               {
                client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (X11; Linux x86_64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/126.0.0.0 Safari/537.36");

                HttpResponseMessage response = await client.GetAsync(uri);

                response.EnsureSuccessStatusCode();

                Response = await response.Content.ReadAsStringAsync();

                var deserializedResponse = JsonSerializer.Deserialize<E621Post>(Response);

                foreach (var post in deserializedResponse.Posts)
                {
                    Console.WriteLine($"Post ID: {post.Id}");
                    Console.WriteLine($"Created At: {post.CreatedAt}");
                    Console.WriteLine($"File URL: {post.File.Url}");
                }

               }
           }
           catch (Exception ex)
           {
                Console.WriteLine(ex);
           }
        }
    }

}

