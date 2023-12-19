namespace SpotifyAnarchyWebEdition.Models {

    public class Playlist {

        public Playlist(string spotifyId, string Name, string imageUrl, string description, string url) {
            this.spotifyId = spotifyId;
            this.Name = Name;
            this.ImageUrl = imageUrl;
            this.Description = description;
            this.Uri = url;
        }

        public string spotifyId { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public string Description { get; set; }
        public string Uri { get; set; }
    }

    public class Category {

        public Category(string spotifyId, string name, string imageUrl, string url) {
            this.Name = name;
            this.ImageUrl = imageUrl;
            this.Id = spotifyId;
            this.Uri = url;
        }

        public string Name { get; private set; }
        public string ImageUrl { get; private set; }
        public string Id { get; private set; }
        public string Uri { get; private set; }
    }
}