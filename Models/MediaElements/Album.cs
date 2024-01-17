using Newtonsoft.Json;
using System.Collections.ObjectModel;

namespace SpotifyAnarchyWebEdition.Models.MediaElements
{
    public class Album
    {
        /// <summary>
        /// Create empty instance of the Album class.
        /// </summary>
        public Album()
        {
            this.Songs = new ObservableCollection<Song>();
        }

        /// <summary>
        /// [DEPRECATED] Create instance of the Album class with the given parameters.
        /// </summary>
        /// Mark as deprecated
        public Album(string spotifyId, string name, string artist, string imageUrl, string uri, string releaseDate)
        {
            this.Name = name;
            this.Artist = artist;
            this.ImageUrl = imageUrl;
            this.Id = spotifyId;
            this.Uri = uri;
            this.ReleaseDate = releaseDate;
        }

        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the artist.
        /// </summary>
        public string Artist { get; set; }

        /// <summary>
        /// Gets or sets the album preview image URL.
        /// </summary>
        public string ImageUrl { get; set; }

        /// <summary>
        /// Gets or sets the album ID.
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the album URI.
        /// </summary>
        [JsonProperty("uri")]
        public string Uri { get; set; }

        /// <summary>
        /// Gets or sets the release date of the album.
        /// </summary>
        [JsonProperty("release_date")]
        public string ReleaseDate { get; set; }

        /// <summary>
        /// Gets or sets the total number of tracks on the album.
        /// </summary>
        [JsonProperty("total_tracks")]
        public int TotalTracks { get; set; }

        /// <summary>
        /// Gets or sets the songs on the album.
        /// </summary>
        public ObservableCollection<Song> Songs { get; set; }
    }
}