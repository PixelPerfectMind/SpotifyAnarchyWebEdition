namespace SpotifyAnarchyWebEdition.Models
{

    /// <summary>
    /// Default values for the SpotifyApi like client id and client secret<br>
    /// Feel free to replace with your own</br>
    /// </summary>
    public class DefaultValues
    {
        // Replace with your Spotify API credentials

        /// <summary>
        /// Your SpotifyApi client id
        /// </summary>
        public readonly string ClientId = "your client id";

        /// <summary>
        /// Your SpotifyApi client secret
        /// </summary>
        public readonly string ClientSecret = "your client secret";

        /// <summary>
        /// A base64 encoded string of your client id and client secret, which is used for basic authorization
        /// </summary>
        public readonly string basicAuthorization = "your basic auth string";

        /// <summary>
        /// The redirect uri for the SpotifyApi
        /// </summary>
        public readonly string RedirectUri = "https://localhost:44394/oauth";

        /// <summary>
        /// The scopes for the SpotifyApi Authentification
        /// </summary>
        public readonly string Scopes = "user-read-private user-read-email playlist-read-collaborative playlist-read-private playlist-modify-public playlist-modify-private user-read-currently-playing user-read-playback-state user-read-playback-position user-follow-modify user-library-read user-library-modify";
    }
}