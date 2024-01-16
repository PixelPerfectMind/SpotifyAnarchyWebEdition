using Newtonsoft.Json;

namespace SpotifyAnarchyWebEdition.Models {

    /// <summary>
    /// Contains all the information about the user, which is returned<br></br>by the Spotify API
    /// </summary>
    public class SpotifyUser {

        /// <summary>
        /// Country of the user
        /// </summary>
        [JsonProperty("country")]
        public string Country { get; set; }

        /// <summary>
        /// The name displayed on the user's profile.
        /// </summary>
        [JsonProperty("display_name")]
        public string DisplayName { get; set; }

        /// <summary>
        /// The user's email address.
        /// </summary>
        [JsonProperty("email")]
        public string Email { get; set; }

        /// <summary>
        /// Returns true, if the user has enabled explicit content
        /// </summary>
        [JsonProperty("filter_enabled")]
        public bool ExplicitContentEnabled { get; set; }

        /// <summary>
        /// Returns true, if the user is allowed to consume explicit content
        /// </summary>
        [JsonProperty("filter_locked")]
        public bool ExplicitContentAllowed { get; set; }

        /// <summary>
        /// Returns the URL of the user's profile page on the Spotify<br></br>web site.
        /// </summary>
        [JsonProperty("spotify")]
        public string SpotifyProfileUrl { get; set; }

        /// <summary>
        /// Returns the count of the user's followers.
        /// </summary>
        public int TotalFollowers { get; set; }

        /// <summary>
        /// Returns the user's Spotify Id.
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; }

        /// <summary>
        /// Returns the user's profile image URL.
        /// </summary>
        public string ImageUrl { get; set; }

        /// <summary>
        /// Returns the user's current subscription type: "premium",<br></br>"free", etc.
        /// </summary>
        [JsonProperty("product")]
        public string Product { get; set; }
    }
}