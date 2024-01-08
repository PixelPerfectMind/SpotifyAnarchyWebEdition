namespace SpotifyAnarchyWebEdition.Models.MediaElements
{
    public class Category
    {
        public Category(string spotifyId, string name, string imageUrl, string url)
        {
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