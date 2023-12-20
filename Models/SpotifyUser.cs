namespace SpotifyAnarchyWebEdition.Models {

    /// <summary>
    /// Contains all the information about the user, which is returned<br></br>by the Spotify API
    /// </summary>
    public class SpotifyUser {

        public SpotifyUser() { }

        /// <summary>
        /// Creates a new SpotifyUser object
        /// </summary>
        public SpotifyUser(string displayName, string country, string email, string explicitContentEnabled,
            string explicitContentAllowed, string profileUrl, int followers, string spotifyId, string profilePictureUrl,
            string type, string uri) {
            this.DisplayName = displayName;
            this.Country = country;
            this.Email = email;

            if(explicitContentEnabled == "true") {
                this.ExplicitContentEnabled = true;
            } else {
                this.ExplicitContentEnabled = false;
            }

            if(explicitContentAllowed == "true") {
                this.ExplicitContentAllowed = true;
            } else {
                this.ExplicitContentAllowed = false;
            }

            this.SpotifyProfileUrl = profileUrl;
            this.TotalFollowers = followers;
            this.SpotifyId = spotifyId;
            this.ImageUrl = profilePictureUrl;
            this.Type = type;
            this.Uri = uri;
        }


        /// <summary>
        /// Country of the user
        /// </summary>
        public string Country { get; set; }

        /// <summary>
        /// The name displayed on the user's profile.
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// The user's email address.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Returns true, if the user has enabled explicit content
        /// </summary>
        public bool ExplicitContentEnabled { get; set; }

        /// <summary>
        /// Returns true, if the user is allowed to consume explicit content
        /// </summary>
        public bool ExplicitContentAllowed { get; set; }

        /// <summary>
        /// Returns the URL of the user's profile page on the Spotify<br></br>web site.
        /// </summary>
        public string SpotifyProfileUrl { get; set; }

        /// <summary>
        /// Returns the count of the user's followers.
        /// </summary>
        public int TotalFollowers { get; set; }

        /// <summary>
        /// Returns the user's Spotify Id.
        /// </summary>
        public string SpotifyId { get; set; }

        /// <summary>
        /// Returns the user's profile image URL.
        /// </summary>
        public string ImageUrl { get; set; }

        /// <summary>
        /// Returns the user's current subscription type: "premium",<br></br>"free", etc.
        /// </summary>
        public string Product { get; set; }

        /// <summary>
        /// Returns the object type: "user"
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Returns the Spotify URI for the user.
        /// </summary>
        public string Uri { get; set; }
    }
}