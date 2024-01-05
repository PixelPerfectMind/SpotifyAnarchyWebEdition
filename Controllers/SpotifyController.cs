using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using SpotifyAnarchyWebEdition.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Windows.Forms;

namespace SpotifyAnarchyWebEdition.Controllers {

    public class SpotifyController : Controller {

        /// <summary>
        /// Spotify OAuth2.0 Callback
        /// </summary>
        public ActionResult AuthCallback(string code) {
            try {
                DefaultValues defaultValues = new DefaultValues();

                RestClient client = new RestClient();
                RestRequest request = new RestRequest("https://accounts.spotify.com/api/token", Method.Post);
                request.AddHeader("Authorization", "Basic " + defaultValues.basicAuthorization);
                request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
                request.AddParameter("grant_type", "authorization_code");
                request.AddParameter("code", code);
                request.AddParameter("redirect_uri", "https://localhost:44394/oauth");
                RestResponse response = client.Execute(request);

                // Check if response is OK
                if (response.StatusCode != System.Net.HttpStatusCode.OK) {
                    ViewBag.Succeeded = false;
                    ViewBag.Exception = response.Content;
                    return View();
                }

                // Save SpotifyUserProfileAPIResponse to session
                SpotifyUserProfileAPIResponse apiResponse = new SpotifyUserProfileAPIResponse();
                apiResponse = JsonConvert.DeserializeObject<SpotifyUserProfileAPIResponse>(response.Content);
                Session["SpotifyUserProfileAPIResponse"] = apiResponse;

                // Save the values to the ViewBag
                ViewBag.AccessToken = apiResponse.AccessToken;
                ViewBag.RefreshToken = apiResponse.RefreshToken;
                ViewBag.ExpiresIn = apiResponse.ExpiresIn / 60; // Convert seconds to minutes

                GetUserProfile();

                if (Session["SpotifyUser"] != null) {
                    SpotifyUser spotifyUser = new SpotifyUser();
                    spotifyUser = (SpotifyUser)Session["SpotifyUser"];
                    ViewBag.User = spotifyUser;
                }

            } catch (Exception ex) {
                ViewBag.Error = ex.Message;
            }

            return View();
        }

            /// <summary>
            /// Get the user profile which belongs to the Spotify Account
            /// <br>and save it into the Session</br>
            /// </summary>
            private void GetUserProfile() {
            try {
                // Initialize SpotifyUserProfileAPIResponse
                SpotifyUserProfileAPIResponse spotifyUserProfileAPIResponse = new SpotifyUserProfileAPIResponse();

                if (Session["SpotifyUserProfileAPIResponse"] != null) {
                    // If session exists, get it and load it into the new object
                    spotifyUserProfileAPIResponse = (SpotifyUserProfileAPIResponse)Session["SpotifyUserProfileAPIResponse"];

                    var client = new RestClient();
                    var request = new RestRequest("https://api.spotify.com/v1/me", Method.Get);
                    request.AddHeader("Authorization", "Bearer " + spotifyUserProfileAPIResponse.AccessToken);
                    RestResponse response = client.Execute(request);
                    Console.WriteLine(response.Content);

                    // Check if response is OK
                    if (response.StatusCode == System.Net.HttpStatusCode.OK) {
                        // Parse the response and store it in a SpotifyUser object
                        var json = JObject.Parse(response.Content);

                        SpotifyUser spotifyUser = new SpotifyUser();
                        spotifyUser.Country = json["country"].ToString();
                        spotifyUser.DisplayName = json["display_name"].ToString();
                        spotifyUser.Email = json["email"].ToString();

                        if (json["explicit_content"]["filter_enabled"].ToString() == "true") {
                            spotifyUser.ExplicitContentEnabled = false;
                        } else {
                            spotifyUser.ExplicitContentEnabled = true;
                        }

                        if (json["explicit_content"]["filter_locked"].ToString() == "true") {
                            spotifyUser.ExplicitContentAllowed = false;
                        } else {
                            spotifyUser.ExplicitContentAllowed = true;
                        }

                        spotifyUser.SpotifyProfileUrl = json["external_urls"]["spotify"].ToString();
                        spotifyUser.TotalFollowers = Convert.ToInt32(json["followers"]["total"]);
                        spotifyUser.SpotifyId = json["id"].ToString();
                        spotifyUser.ImageUrl = json["images"][0]["url"].ToString();
                        spotifyUser.Type = json["type"].ToString();
                        spotifyUser.Uri = json["uri"].ToString();

                        // Save the SpotifyUser object in the session
                        Session["SpotifyUser"] = spotifyUser;
                    } else {
                        ViewBag.Error = "Error getting your user profile: " + response.Content;
                    }
                } else {
                    ViewBag.Error = "Error getting your user profile: You are not signed in.";
                }

            } catch (Exception ex) {
                ViewBag.Error = ex.Message;
            }
        }

        /// <summary>
        /// Shows up the user profile page
        /// </summary>
        public async Task<ActionResult> UserProfileView() {
            if (Session["SpotifyUser"] != null) {
                try {
                    SpotifyUser spotifyUser = new SpotifyUser();
                    spotifyUser = (SpotifyUser)Session["SpotifyUser"];
                    ViewBag.User = spotifyUser;

                    ObservableCollection<Playlist> Playlists = new ObservableCollection<Playlist>();

                    // Get current login
                    SpotifyUserProfileAPIResponse spotifyUserProfileAPIResponse = new SpotifyUserProfileAPIResponse();
                    spotifyUserProfileAPIResponse = (SpotifyUserProfileAPIResponse)Session["SpotifyUserProfileAPIResponse"];

                    // API request to get the user's playlists
                    var client = new RestClient();
                    var request = new RestRequest("https://api.spotify.com/v1/me/playlists?limit=10&offset=0", Method.Get);
                    request.AddHeader("Authorization", "Bearer " + spotifyUserProfileAPIResponse.AccessToken);
                    RestResponse response = await client.ExecuteAsync(request);
                    Console.WriteLine(response.Content);

                    // If response is OK, parse the response
                    if (response.StatusCode == System.Net.HttpStatusCode.OK) {
                        var json = JObject.Parse(response.Content);

                        // Add playlists to the list
                        var playlists = json["items"].Children().ToList();
                        foreach (var playlist in playlists) {
                            // Get the first image from the images array
                            var images = playlist["images"].Children();
                            var playlistImageUrl = images.First()["url"].ToString();

                            // Add new playlist to the list
                            Playlists.Add(new Playlist(playlist["id"].ToString(), playlist["name"].ToString(), playlistImageUrl, playlist["description"].ToString(), playlist["uri"].ToString()));
                        }

                        ViewBag.Playlists = Playlists;
                    } else {
                        ViewBag.Error = "Error getting your playlists: " + response.Content;
                    }
                } catch (Exception ex) {
                    ViewBag.Error = ex.Message;
                }
                return View();
            } else {
                return RedirectToAction("Index", "Home");
            }
        }

        /// <summary>
        /// Displays the playlist page
        /// </summary>
        public async Task<ActionResult> AlbumView(string albumId) {
            if (Session["SpotifyUser"] != null) {
                try {
                    // Load instance of SpotifyUser from session
                    SpotifyUserProfileAPIResponse spotifyUserProfileAPIResponse = new SpotifyUserProfileAPIResponse();
                    spotifyUserProfileAPIResponse = (SpotifyUserProfileAPIResponse)Session["SpotifyUserProfileAPIResponse"];

                    var client = new RestClient();
                    var request = new RestRequest("https://api.spotify.com/v1/albums/" + albumId, Method.Get);
                    request.AddHeader("Authorization", "Bearer " + spotifyUserProfileAPIResponse.AccessToken);
                    RestResponse response = await client.ExecuteAsync(request);
                    Console.WriteLine(response.Content);

                    // Check if response is OK
                    if(response.StatusCode == System.Net.HttpStatusCode.OK) {
                        // Parse the response and store it in a SpotifyUser object
                        var json = JObject.Parse(response.Content);

                        Album album = new Album(json["id"].ToString(), json["name"].ToString(), json["artists"][0]["name"].ToString(),
                            json["images"][1]["url"].ToString(), json["uri"].ToString(), json["release_date"].ToString());

                        // Add songs to the album item
                        var tracks = json["tracks"]["items"].Children().ToList();

                        ObservableCollection<Song> Songs = new ObservableCollection<Song>();

                        foreach (var track in tracks) {
                            // Get the first artist from the artists array
                            var artists = track["artists"].Children();
                            var songArtist = artists.First()["name"].ToString();

                            // Add new song to the album item
                            Songs.Add(new Song {
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

                } catch (Exception ex) {
                    ViewBag.Error = ex.Message;
                }
                return View();
            } else {
                GetBasicBearerToken();
                if(Session["BearerTokenForPublicUses"] != null) {
                    return RedirectToAction("Index", "Home");
                }

                return View();
            }
        }

        /// <summary>
        /// Get a new bearer token, with basic permissions.
        /// </summary>
        private void GetBasicBearerToken() {
            DefaultValues defaultValues = new DefaultValues();

            var client = new RestClient();
            var request = new RestRequest("https://accounts.spotify.com/api/token", Method.Post);
            request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            request.AddParameter("grant_type", "client_credentials");
            request.AddParameter("client_id", defaultValues.ClientId);
            request.AddParameter("client_secret", defaultValues.ClientSecret);
            RestResponse response = client.Execute(request);

            // Check if response is OK
            if (response.StatusCode == System.Net.HttpStatusCode.OK) {
                // Parse the response
                var json = JObject.Parse(response.Content);
                var bearerToken = json["access_token"].ToString();
                Session["BearerTokenForPublicUses"] = bearerToken;
            }
        }

        public ActionResult Logout() {
            Session.Clear();
            return RedirectToAction("Index", "Home");
        }


        /// <summary>
        /// Opens the playlist page
        /// </summary>
        public async Task<ActionResult> PlaylistView(string playlistId) {
            if (Session["SpotifyUserProfileAPIResponse"] != null) {
                try {
                    // Initialize SpotifyUserProfileAPIResponse
                    SpotifyUserProfileAPIResponse apiResponse = new SpotifyUserProfileAPIResponse();
                    apiResponse = (SpotifyUserProfileAPIResponse)Session["SpotifyUserProfileAPIResponse"];

                    // API request
                    var client = new RestClient();
                    var request = new RestRequest("https://api.spotify.com/v1/playlists/" + playlistId, Method.Get);
                    request.AddHeader("Authorization", "Bearer " + apiResponse.AccessToken);
                    RestResponse response = await client.ExecuteAsync(request);

                    // Check if response is OK
                    if (response.StatusCode == System.Net.HttpStatusCode.OK) {
                        // Parse the response and store it in a SpotifyUser object
                        var json = JObject.Parse(response.Content);

                        Playlist playlist = new Playlist(json["id"].ToString(), json["external_urls"]["spotify"].ToString(),
                            Convert.ToInt32(json["followers"]["total"]), json["name"].ToString(),
                            json["images"][0]["url"].ToString(), json["description"].ToString(), json["uri"].ToString(),
                            json["owner"]["id"].ToString(), json["owner"]["display_name"].ToString());


                        // Add songs to the playlist item
                        var tracks = json["tracks"]["items"].Children().ToList();

                        ObservableCollection<Song> Songs = new ObservableCollection<Song>();

                        foreach (var track in tracks) {
                            // Get the first artist from the artists array
                            var artists = track["track"]["artists"].Children();
                            var songArtist = artists.First()["name"].ToString();

                            // Get the last image from the images array
                            var albumImages = track["track"]["album"]["images"].Children();
                            var songAlbumImageUrl = albumImages.Last()["url"].ToString();


                            // Add new song to the playlist item
                            Songs.Add(new Song {
                                Name = track["track"]["name"].ToString(),
                                Artist = songArtist,
                                Album = track["track"]["album"]["name"].ToString(),
                                ImageUrl = songAlbumImageUrl,
                                Id = track["track"]["id"].ToString(),
                                Uri = track["track"]["uri"].ToString(),
                                PreviewUrl = track["track"]["preview_url"].ToString()
                            });
                        }
                        playlist.Songs = Songs;
                        ViewBag.Playlist = playlist;
                    }

                } catch (Exception ex) {
                    ViewBag.Error = ex.Message;
                }
            } else {
                // When not logged in, return to home page
                return RedirectToAction("Index", "Home");
            }
            return View();
        }
    }
}