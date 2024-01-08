using System.Collections.ObjectModel;

namespace SpotifyAnarchyWebEdition.Models.MediaElements
{
    public class Album
    {
        public Album(string spotifyId, string name, string artist, string imageUrl, string uri, string releaseDate)
        {
            this.Name = name;
            this.Artist = artist;
            this.ImageUrl = imageUrl;
            this.Id = spotifyId;
            this.Uri = uri;
            this.ReleaseDate = releaseDate;
        }

        public void AddSong(string songId, string songName, string artistName, string songUrl, string previewUrl)
        {
            this.Songs.Add(new Song(songId, songName, artistName, this.Name, this.ImageUrl, songUrl, previewUrl));
        }

        public string Name { get; private set; }
        public string Artist { get; private set; }
        public string ImageUrl { get; private set; }
        public string Id { get; private set; }
        public string Uri { get; private set; }
        public string ReleaseDate { get; set; }
        public int TotalTracks { get; private set; }
        public ObservableCollection<Song> Songs { get; set; }
    }
}