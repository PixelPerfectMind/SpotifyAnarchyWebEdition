using Newtonsoft.Json;
using System.Collections.ObjectModel;

namespace SpotifyAnarchyWebEdition.Models.MediaElements
{
    public class Podcast
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("publisher")]
        public string Publisher { get; set; }

        [JsonProperty("images")]
        public string ImageUrl { get; set; }

        [JsonProperty("id")]
        public string SpotifyId { get; set; }

        [JsonProperty("href")]
        public string SpotifyHref { get; set; }

        [JsonProperty("uri")]
        public string SpotifyUri { get; set; }

        [JsonProperty("total_episodes")]
        public int TotalEpisodes { get; set; }

        public ObservableCollection<Episode> Episodes = new ObservableCollection<Episode>();
    }
}