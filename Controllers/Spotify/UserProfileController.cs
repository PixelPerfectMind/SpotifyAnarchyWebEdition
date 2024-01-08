using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using SpotifyAnarchyWebEdition.Models;
using SpotifyAnarchyWebEdition.Models.MediaElements;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SpotifyAnarchyWebEdition.Controllers.Spotify
{

    public class UserProfileController : Controller
    {

        // Spotify user
        SpotifyUser spotifyUser = new SpotifyUser();
        SpotifyUserProfileAPIResponse spotifyUserProfileAPIResponse = new SpotifyUserProfileAPIResponse();

        // Create ObservableCollection instance of Playlists and Devices
        ObservableCollection<Playlist> Playlists = new ObservableCollection<Playlist>();
        ObservableCollection<Device> Devices = new ObservableCollection<Device>();

        /// <summary>
        /// Shows up the user profile page
        /// </summary>
        public async Task<ActionResult> UserProfileView()
        {

            if (Session["SpotifyUser"] != null)
            {
                try
                {
                    spotifyUser = (SpotifyUser)Session["SpotifyUser"];
                    ViewBag.User = spotifyUser;

                    // Get current login
                    spotifyUserProfileAPIResponse = (SpotifyUserProfileAPIResponse)Session["SpotifyUserProfileAPIResponse"];

                    // API request to get the user's playlists
                    var client = new RestClient();
                    var request = new RestRequest("https://api.spotify.com/v1/me/playlists?limit=10&offset=0", Method.Get);
                    request.AddHeader("Authorization", "Bearer " + spotifyUserProfileAPIResponse.AccessToken);
                    RestResponse response = await client.ExecuteAsync(request);

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

                        GetAvailableDevices();

                        ViewBag.Playlists = Playlists;
                        ViewBag.Devices = Devices;
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

        private void GetAvailableDevices()
        {
            try
            {
                // API request to get the user's devices
                var client = new RestClient();
                var request = new RestRequest("https://api.spotify.com/v1/me/player/devices", Method.Get);
                request.AddHeader("Authorization", "Bearer " + spotifyUserProfileAPIResponse.AccessToken);
                RestResponse response = client.Execute(request);

                // If response is OK, deserialize the response
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var json = JObject.Parse(response.Content);

                    // Add devices to the list
                    var devices = json["devices"].Children().ToList();
                    foreach (var device in devices)
                    {
                        Devices.Add(new Device
                        {
                            Id = device["id"].ToString(),
                            IsActive = device["is_active"].ToObject<bool>(),
                            IsPrivateSession = device["is_private_session"].ToObject<bool>(),
                            IsRestricted = device["is_restricted"].ToObject<bool>(),
                            Name = device["name"].ToString(),
                            Type = device["type"].ToString()
                        });
                    }
                }
                else
                {
                    ViewBag.Error = "Beim Holen deiner Geräte ist ein Fehler aufgetreten: " + response.Content;
                }

            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
            }
        }

    }
}