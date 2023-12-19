using Newtonsoft.Json;
using SpotifyAnarchyWebEdition.Models;
using System.Collections.Generic;

namespace SpotifyAnarchyWebEdition.Models {

    public class PlaylistItem {
        public int Index { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("href")]
        public string LinkToSpotify { get; set; }
        public string ImageUrl { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }
    }
}