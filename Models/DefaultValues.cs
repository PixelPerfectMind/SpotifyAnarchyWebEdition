namespace SpotifyAnarchyWebEdition.Models
{

    /// <summary>
    /// Default values for the SpotifyApi like client id and client secret<br>
    /// Feel free to replace with your own</br>
    /// </summary>
    public class DefaultValues
    {

        // Replace with your SpotifyApi credentials

        /// <summary>
        /// Your SpotifyApi client id
        /// </summary>
        internal readonly string ClientId = "3eac98750c8f4f4da6dbf4dc63d182b6";

        /// <summary>
        /// Your SpotifyApi client secret
        /// </summary>
        internal readonly string ClientSecret = "c269c041a81542c0b33780048c187475";

        /// <summary>
        /// A base64 encoded string of your client id and client secret, which is used for basic authorization
        /// </summary>
        internal readonly string basicAuthorization = "M2VhYzk4NzUwYzhmNGY0ZGE2ZGJmNGRjNjNkMTgyYjY6YzI2OWMwNDFhODE1NDJjMGIzMzc4MDA0OGMxODc0NzU=";
    }
}