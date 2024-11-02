
using Microsoft.Extensions.Logging;
using RestSharp;

namespace HornyDragonBot.api.furaffinity
{
    public class FuraffinityApi
    {
        public readonly ILogger _logger;
        private string ApiUrl { get; set; } = "https://faexport.spangle.org.uk/";
        public FuraffinityApi(ILogger<FuraffinityApi> logger) { 
            _logger = logger;
        }

        private async Task<string> RequsetApi(string uri)
        {
            try
            {
                var request = new RestRequest(uri, Method.Get);
                request.AddHeader("User-Agent", "HorryDragonProject/1.0 (by Dragofox)");

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

        private async Task GetUserGallery(string username)
        {

        }
    }
}
