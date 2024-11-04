
using HornyDragonBot.Api.furaffinity;
using Microsoft.Extensions.Logging;
using RestSharp;
using System.Collections.Generic;
using System.Text.Json;

namespace HornyDragonBot.api.furaffinity
{
    public class FuraffinityApi
    {
        public readonly ILogger _logger;
        private string ApiUrl { get; set; } = "https://faexport.spangle.org.uk";
        public FuraffinityApi(ILogger<FuraffinityApi> logger) { 
            _logger = logger;
        }

        private async Task<string> RequsetApi(string uri, int page = 1)
        {
            try
            {
                var request = new RestRequest(uri, Method.Get);
                request.AddHeader("User-Agent", "HorryDragonProject/1.0 (by Dragofox)");

                request.AddParameter("page", page);

                var clientApi = new RestClient();
                var response = await clientApi.ExecuteAsync(request);

                if ((int)response.StatusCode != 200)
                {
                    _logger.LogError("[Error] " + response.StatusCode);
                    return string.Empty;
                }

                return response.Content;
            }
            catch (Exception ex) {
                _logger.LogError(ex.Message);
                return string.Empty;
            }

        }

        private async Task<Dictionary<int, List<string>>?> GetUserGallery(string username)
        {

            Dictionary<int, List<string>> dataPage = new Dictionary<int, List<string>>();

            int pageMax = 5;
            List<Task<(int Page, List<string> Ids)>> tasks = new List<Task<(int, List<string>)>>();


            for (int page = 1; page <= pageMax; page++) {
                int currentPage = page;
                tasks.Add(Task.Run(async () =>
                {
                    try
                    {
                        string requsetPath = ApiUrl + $"/user/{username}/gallery.json";
                        string content = await RequsetApi(requsetPath, currentPage);

                        List<string>? idsResponse = JsonSerializer.Deserialize<List<string>>(content);

                        return (currentPage, idsResponse ?? new List<string>());
                    }
                    catch 
                    {
                        return (currentPage, new List<string>());
                    }

                }));
            }

            var results = await Task.WhenAll(tasks);

            foreach (var result in results) {
                if (result.Ids.Count > 0) {
                    dataPage[result.Page] = result.Ids;
                }
            }


            return dataPage;
        }

        private async Task<submission?> GetSubmission(string id)
        {
            try
            {
                string requsetPath = ApiUrl + $"/submission/{id}.json";
                string content = await RequsetApi(requsetPath);

                var deserializedResponse = JsonSerializer.Deserialize<submission>(content);

                if (deserializedResponse == null) {
                    return null;
                }

                return deserializedResponse;
            }
            catch (Exception ex) { 
                _logger.LogError(ex.Message);
                return null;
            }
        }

        // Не готово

        public async Task<string?> GetRandomPost(string username)
         {
            var idPost = await GetUserGallery(username);
            if (idPost != null) {
                var rnd = new Random();
                int rPage = rnd.Next(1, idPost.Count);
                var pageGet = idPost.FirstOrDefault(x => x.Key == rPage);
                int countPage = pageGet.Value.Count;
                int rPost = rnd.Next(countPage);

                var post = await GetSubmission(pageGet.Value[rPost]);

                if (post == null)
                {
                    return string.Empty;
                }

                _logger.LogInformation($"Get random post from page: {rPage} post: {post.download}");
                return post.full;
            }
            
            return string.Empty;
         }
    }
}
