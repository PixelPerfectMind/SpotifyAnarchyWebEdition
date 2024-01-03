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
        internal readonly string ClientId = "replaceWithYourOwn";

        /// <summary>
        /// Your SpotifyApi client secret
        /// </summary>
        internal readonly string ClientSecret = "replaceWithYourOwn";

        /// <summary>
        /// A base64 encoded string of your client id and client secret, which is used for basic authorization
        /// </summary>
        internal readonly string basicAuthorization = "replaceWithYourOwn";
    }
}