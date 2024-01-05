using Newtonsoft.Json.Linq;
using RestSharp;
using SpotifyAnarchyWebEdition.Models;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SpotifyAnarchyWebEdition.Controllers.Spotify {

    public class AlbumController : Controller {

        /// <summary>
        /// Displays the playlist page
        /// </summary>
        public async Task<ActionResult> AlbumView(string albumId) {

            try
            {
                // Load instance of SpotifyUser from session
                SpotifyUserProfileAPIResponse spotifyUserProfileAPIResponse = new SpotifyUserProfileAPIResponse();
                spotifyUserProfileAPIResponse = (SpotifyUserProfileAPIResponse)Session["SpotifyUserProfileAPIResponse"];

                var client = new RestClient();
                var request = new RestRequest("https://api.spotify.com/v1/albums/" + albumId, Method.Get);
                request.AddHeader("Authorization", "Bearer " + spotifyUserProfileAPIResponse.AccessToken);
                RestResponse response = await client.ExecuteAsync(request);
                Console.WriteLine(response.Content);

                // Check if response is OK
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    // Parse the response and store it in a SpotifyUser object
                    var json = JObject.Parse(response.Content);

                    Album album = new Album(json["id"].ToString(), json["name"].ToString(), json["artists"][0]["name"].ToString(),
                        json["images"][1]["url"].ToString(), json["uri"].ToString(), json["release_date"].ToString());

                    // Add songs to the album item
                    var tracks = json["tracks"]["items"].Children().ToList();

                    ObservableCollection<Song> Songs = new ObservableCollection<Song>();

                    foreach (var track in tracks)
                    {
                        // Get the first artist from the artists array
                        var artists = track["artists"].Children();
                        var songArtist = artists.First()["name"].ToString();

                        // Add new song to the album item
                        Songs.Add(new Song
                        {
                            Name = track["name"].ToString(),
                            Artist = songArtist,
                            Album = json["name"].ToString(),
                            ImageUrl = json["images"][1]["url"].ToString(),
                            Id = track["id"].ToString(),
                            Uri = track["uri"].ToString(),
                            PreviewUrl = track["preview_url"].ToString()
                        });
                    }
                    album.Songs = Songs;
                    ViewBag.Album = album;
                }

            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
            }
            return View();
        }
    }
}