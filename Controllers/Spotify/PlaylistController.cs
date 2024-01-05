using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using SpotifyAnarchyWebEdition.Models;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SpotifyAnarchyWebEdition.Controllers.Spotify {

    public class PlaylistController : Controller {

        /// <summary>
        /// Opens the playlist page
        /// </summary>
        public async Task<ActionResult> PlaylistView(string playlistId) {

            if (Session["SpotifyUserProfileAPIResponse"] != null)
            {
                try
                {
                    // Initialize SpotifyUserProfileAPIResponse
                    SpotifyUserProfileAPIResponse apiResponse = new SpotifyUserProfileAPIResponse();
                    apiResponse = (SpotifyUserProfileAPIResponse)Session["SpotifyUserProfileAPIResponse"];

                    // API request
                    var client = new RestClient();
                    var request = new RestRequest("https://api.spotify.com/v1/playlists/" + playlistId, Method.Get);
                    request.AddHeader("Authorization", "Bearer " + apiResponse.AccessToken);
                    RestResponse response = await client.ExecuteAsync(request);

                    // Check if response is OK
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        // Parse the response and store it in a SpotifyUser object
                        var json = JObject.Parse(response.Content);

                        Playlist playlist = new Playlist(json["id"].ToString(), json["external_urls"]["spotify"].ToString(),
                            Convert.ToInt32(json["followers"]["total"]), json["name"].ToString(),
                            json["images"][0]["url"].ToString(), json["description"].ToString(), json["uri"].ToString(),
                            json["owner"]["id"].ToString(), json["owner"]["display_name"].ToString());


                        // Add songs to the playlist item
                        var tracks = json["tracks"]["items"].Children().ToList();

                        ObservableCollection<Song> Songs = new ObservableCollection<Song>();

                        foreach (var track in tracks)
                        {
                            // Get the first artist from the artists array
                            var artists = track["track"]["artists"].Children();
                            var songArtist = artists.First()["name"].ToString();

                            // Get the last image from the images array
                            var albumImages = track["track"]["album"]["images"].Children();
                            var songAlbumImageUrl = albumImages.Last()["url"].ToString();


                            // Add new song to the playlist item
                            Songs.Add(new Song
                            {
                                Name = track["track"]["name"].ToString(),
                                Artist = songArtist,
                                Album = track["track"]["album"]["name"].ToString(),
                                ImageUrl = songAlbumImageUrl,
                                Id = track["track"]["id"].ToString(),
                                Uri = track["track"]["uri"].ToString(),
                                PreviewUrl = track["track"]["preview_url"].ToString()
                            });
                        }

                        //Get if the user follows the playlist
                        SpotifyUser spotifyUser = new SpotifyUser();
                        spotifyUser = (SpotifyUser)Session["SpotifyUser"];

                        var userFollowsPlaylistRequest = new RestRequest("https://api.spotify.com/v1/playlists/" + playlistId + "/followers/contains?ids=" + spotifyUser.SpotifyId, Method.Get);
                        userFollowsPlaylistRequest.AddHeader("Authorization", "Bearer " + apiResponse.AccessToken);
                        RestResponse userFollowsPlaylistResponse = await client.ExecuteAsync(userFollowsPlaylistRequest);

                        if (userFollowsPlaylistResponse.StatusCode == System.Net.HttpStatusCode.OK) {
                            var userFollowsPlaylistJson = JArray.Parse(userFollowsPlaylistResponse.Content);
                            if (userFollowsPlaylistJson[0].ToString() == "True") {
                                ViewBag.UserFollowsPlaylist = true;
                            } else {
                                ViewBag.UserFollowsPlaylist = false;
                            }
                        } else {
                            ViewBag.UserFollowsPlaylist = false;
                        }


                        playlist.Songs = Songs;
                        ViewBag.Playlist = playlist;
                    }

                }
                catch (Exception ex)
                {
                    ViewBag.Error = ex.Message;
                }
            }
            else
            {
                // When not logged in, return to home page
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        /// <summary>
        /// Follow a playlist
        /// </summary>
        public async Task<ActionResult> FollowPlaylist(string playlistId, string scope = "false")
        {
            try
            {
                RestClient client = new RestClient();
                RestRequest request = new RestRequest("https://api.spotify.com/v1/playlists/" + playlistId + "/followers", Method.Put);
                request.AddHeader("Authorization", "Bearer " + ((SpotifyUserProfileAPIResponse)Session["SpotifyUserProfileAPIResponse"]).AccessToken);
                //Add scope string to body
                request.AddParameter("application/json", "{\"public\": " + scope + "}", ParameterType.RequestBody);
                RestResponse response = await client.ExecuteAsync(request);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
            }

            return RedirectToAction("PlaylistView", "Playlist", new { playlistId = playlistId });
        }

        /// <summary>
        /// Unfollow a playlist
        /// </summary>
        public async Task<ActionResult> UnfollowPlaylist(string playlistId)
        {
            try
            {
                RestClient client = new RestClient();
                RestRequest request = new RestRequest("https://api.spotify.com/v1/playlists/" + playlistId + "/followers", Method.Delete);
                request.AddHeader("Authorization", "Bearer " + ((SpotifyUserProfileAPIResponse)Session["SpotifyUserProfileAPIResponse"]).AccessToken);
                RestResponse response = await client.ExecuteAsync(request);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
            }

            return RedirectToAction("PlaylistView", "Playlist", new { playlistId = playlistId });
        }
    }
}