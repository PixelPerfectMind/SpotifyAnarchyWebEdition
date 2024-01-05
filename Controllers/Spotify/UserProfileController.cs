using Newtonsoft.Json.Linq;
using RestSharp;
using SpotifyAnarchyWebEdition.Models;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SpotifyAnarchyWebEdition.Controllers.Spotify {

    public class UserProfileController : Controller {

        /// <summary>
        /// Shows up the user profile page
        /// </summary>
        public async Task<ActionResult> UserProfileView() {

            if (Session["SpotifyUser"] != null)
            {
                try
                {
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
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        var json = JObject.Parse(response.Content);

                        // Add playlists to the list
                        var playlists = json["items"].Children().ToList();
                        foreach (var playlist in playlists)
                        {
                            // Get the first image from the images array
                            var images = playlist["images"].Children();
                            var playlistImageUrl = images.First()["url"].ToString();

                            // Add new playlist to the list
                            Playlists.Add(new Playlist(playlist["id"].ToString(), playlist["name"].ToString(), playlistImageUrl, playlist["description"].ToString(), playlist["uri"].ToString()));
                        }

                        ViewBag.Playlists = Playlists;
                    }
                    else
                    {
                        ViewBag.Error = "Error getting your playlists: " + response.Content;
                    }
                }
                catch (Exception ex)
                {
                    ViewBag.Error = ex.Message;
                }
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

    }
}