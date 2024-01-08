using System.Collections.ObjectModel;

namespace SpotifyAnarchyWebEdition.Models.MediaElements
{
    public class Playlist
    {
        public Playlist(string spotifyId, string name, string imageUrl, string description, string uri)
        {
            this.SpotifyId = spotifyId;
            this.Name = name;
            this.ImageUrl = imageUrl;
            this.Description = description;
            this.Uri = uri;
        }

        public Playlist(string spotifyId, string spotifyUrl, int totalFollowers, string name, string imageUrl, string description, string uri,
            string userId, string userName)
        {
            this.SpotifyId = spotifyId;
            this.SpotifyUrl = spotifyUrl;
            this.TotalFollowers = totalFollowers;
            this.Name = name;
            this.ImageUrl = imageUrl;
            this.Description = description;
            this.Uri = uri;
            this.UserId = userId;
            this.UserName = userName;
        }

        public void AddSong(string songId, string songName, string artistName, string albumName, string imageUrl, string songUrl, string previewUrl)
        {
            this.Songs.Add(new Song(songId, songName, artistName, albumName, imageUrl, songUrl, previewUrl));
        }

        public string SpotifyId { get; set; }
        public string SpotifyUrl { get; set; }
        public int TotalFollowers { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public string Description { get; set; }
        public string Uri { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public ObservableCollection<Song> Songs { get; set; }
    }
}