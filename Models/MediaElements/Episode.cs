using Newtonsoft.Json;

namespace SpotifyAnarchyWebEdition.Models.MediaElements
{
    public class Episode
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("duration_ms")]
        public int DurationInMs { get; set; }

        [JsonProperty("explicit")]
        public bool IsExplicit { get; set; }

        [JsonProperty("external_urls")]
        public string SpotifyUrl { get; set; }

        [JsonProperty("uri")]
        public string SpotifyURI { get; set; }

        [JsonProperty("id")]
        public string SpotifyId { get; set; }

        [JsonProperty("images")]
        public string ImageUrl { get; set; }

        [JsonProperty("audio_preview_url")]
        public string AudioPreviewUrl { get; set; }

        [JsonProperty("release_date")]
        public string ReleaseDate { get; set; }

        [JsonProperty("is_playable")]
        public bool IsPlayableInGivenMarket { get; set; }

        [JsonProperty("languages")]
        public string[] Languages { get; set; }
    }
}