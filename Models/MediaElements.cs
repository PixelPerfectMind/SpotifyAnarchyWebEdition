using System.Collections.ObjectModel;

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

    public class Song {

        public Song(string spotifyId, string name, string artist, string album, string imageUrl, string url) {
            this.Name = name;
            this.Artist = artist;
            this.Album = album;
            this.ImageUrl = imageUrl;
            this.Id = spotifyId;
            this.Uri = url;
        }

        public Song(string spotifyId, string name, string artist, string album, string imageUrl, string url, string previewUrl) {
            this.Name = name;
            this.Artist = artist;
            this.Album = album;
            this.ImageUrl = imageUrl;
            this.Id = spotifyId;
            this.Uri = url;
            this.PreviewUrl = previewUrl;
        }

        public string Name { get; private set; }
        public string Artist { get; private set; }
        public string Album { get; private set; }
        public string ImageUrl { get; private set; }
        public string Id { get; private set; }
        public string Uri { get; private set; }
        public string PreviewUrl { get; private set; }
    }

    public class Album {

        public Album(string spotifyId, string name, string artist, string imageUrl, string url, string releaseDate) {
            this.Name = name;
            this.Artist = artist;
            this.ImageUrl = imageUrl;
            this.Id = spotifyId;
            this.Uri = url.Replace("spotify:album:", "");
            this.ReleaseDate = releaseDate;
        }

        public void AddSong(string songId, string songName, string artistName, string songUrl, string previewUrl) {
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