

using System.Text.Json.Serialization;

namespace HornyDragonBot.Api.furaffinity
{
    public class submission
    {
        [JsonPropertyName("title")]
        public required string title { get; set; }
        [JsonPropertyName("description")]
        public required string description { get; set; }
        [JsonPropertyName("description_body")]
        public required string description_body { get; set; }
        [JsonPropertyName("name")]
        public required string name { get; set; }
        [JsonPropertyName("profile")]
        public required string profile { get; set; }
        [JsonPropertyName("profile_name")]
        public required string profile_name { get; set; }
        [JsonPropertyName("avatar")]
        public required string avatar { get; set; }
        [JsonPropertyName("link")]
        public required string link { get; set; }
        [JsonPropertyName("posted")]
        public required string posted { get; set; }
        [JsonPropertyName("posted_at")]
        public required string posted_at { get; set; }
        [JsonPropertyName("download")]
        public required string download { get; set; }
        [JsonPropertyName("full")]
        public required string full { get; set; }
        [JsonPropertyName("thumbnail")]
        public required string thumbnail { get; set; }
        [JsonPropertyName("gallery")]
        public required string gallery { get; set; }
        [JsonPropertyName("category")]
        public required string category { get; set; }
        [JsonPropertyName("theme")]
        public required string theme { get; set; }
        [JsonPropertyName("species")]
        public required string species { get; set; }
        [JsonPropertyName("gender")]
        public required string gender { get; set; }
        [JsonPropertyName("favorites")]
        public required string farvorites { get; set; }
        [JsonPropertyName("comments")]
        public required string comments { get; set; }
        [JsonPropertyName("views")]
        public required string views { get; set; }
        [JsonPropertyName("resolution")]
        public required string resolution { get; set; }
        [JsonPropertyName("rating")]
        public required string rating { get; set; }
        [JsonPropertyName("keywords")]
        public required List<string> keywords { get; set; }
    }
}
