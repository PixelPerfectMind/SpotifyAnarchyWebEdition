using System.Collections.ObjectModel;

namespace SpotifyAnarchyWebEdition.Models {

    public class Playlist {

        public Playlist(string spotifyId, string name, string imageUrl, string description, string uri) {
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

        public void AddSong(string songId, string songName, string artistName, string albumName, string imageUrl, string songUrl, string previewUrl) {
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
        public Song() { }

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

        public string Name { get; set; }
        public string Artist { get; set; }
        public string Album { get; set; }
        public string ImageUrl { get; set; }
        public string Id { get; set; }
        public string Uri { get; set; }
        public string PreviewUrl { get; set; }
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