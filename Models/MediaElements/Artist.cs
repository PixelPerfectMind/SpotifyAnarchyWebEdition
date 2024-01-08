using System.Collections.Generic;

namespace SpotifyAnarchyWebEdition.Models.MediaElements
{
    public class Artist
    {
        public Artist()
        {
            Genres = new List<string>();
        }

        public Artist(string spotifyId, string name, string imageUrl, int totalFollowers)
        {
            this.Name = name;
            this.ImageUrl = imageUrl;
            this.Id = spotifyId;
            this.TotalFollowers = totalFollowers;
            Genres = new List<string>();
        }

        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public string Id { get; set; }
        public string Uri { get; set; }
        public string Href { get; set; }
        public int PopularityScore { get; set; }
        public int TotalFollowers { get; set; }
        public List<string> Genres { get; set; }
    }
}