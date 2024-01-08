namespace SpotifyAnarchyWebEdition.Models.MediaElements
{
    public class Song
    {
        public Song() { }

        public Song(string spotifyId, string name, string artist, string album, string imageUrl, string url)
        {
            this.Name = name;
            this.Artist = artist;
            this.Album = album;
            this.ImageUrl = imageUrl;
            this.Id = spotifyId;
            this.Uri = url;
        }

        public Song(string spotifyId, string name, string artist, string album, string imageUrl, string url, string previewUrl)
        {
            this.Name = name;
            this.Artist = artist;
            this.Album = album;
            this.ImageUrl = imageUrl;
            this.Id = spotifyId;
            this.Uri = url;
            this.PreviewUrl = previewUrl;
        }

        public string Name { get; set; }
        public string Artist { get; set; }
        public string Album { get; set; }
        public string ImageUrl { get; set; }
        public string Id { get; set; }
        public string Uri { get; set; }
        public string PreviewUrl { get; set; }
    }
}