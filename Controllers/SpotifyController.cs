using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using SpotifyAnarchyWebEdition.Models;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace SpotifyAnarchyWebEdition.Controllers {

    public class SpotifyController : Controller {

        // GET: Spotify OAuth Callback
        public ActionResult AuthCallback(string code) {
            try {
                DefaultValues defaultValues = new DefaultValues();

                var client = new RestClient();
                var request = new RestRequest("https://accounts.spotify.com/api/token", Method.Post);
                request.AddHeader("Authorization", "Basic " + defaultValues.basicAuthorization);
                request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
                request.AddParameter("grant_type", "authorization_code");
                request.AddParameter("code", code);
                request.AddParameter("redirect_uri", "https://localhost:44394/oauth");
                RestResponse response = client.Execute(request);

                // Check if response is OK
                if (response.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    ViewBag.Succeeded = false;
                    ViewBag.Exception = response.Content;
                    return View();
                }

                SpotifyUserProfileAPIResponse apiResponse = new SpotifyUserProfileAPIResponse();
                // Save SpotifyUserProfileAPIResponse to session
                apiResponse = JsonConvert.DeserializeObject<SpotifyUserProfileAPIResponse>(response.Content);
                Session["SpotifyUserProfileAPIResponse"] = apiResponse;

                ViewBag.AccessToken = apiResponse.AccessToken;
                ViewBag.RefreshToken = apiResponse.RefreshToken;
                // Expires in to minutes
                ViewBag.ExpiresIn = apiResponse.ExpiresIn / 60;

                GetUserProfile();

                if (Session["SpotifyUser"] != null) {
                    SpotifyUser spotifyUser = new SpotifyUser();
                    spotifyUser = (SpotifyUser)Session["SpotifyUser"];
                    ViewBag.User = spotifyUser;
                }
            }
            catch (Exception ex) {
                ViewBag.Error = ex.Message;
            }

            return View();
        }

        public ActionResult SearchView(string query, string type, string market) {
            ViewBag.Query = Request.QueryString["query"];
            ViewBag.Market = Request.QueryString["market"];

            // Initialize SpotifyUserProfileAPIResponse
            SpotifyUserProfileAPIResponse apiResponse = new SpotifyUserProfileAPIResponse();
            if (Session["SpotifyUserProfileAPIResponse"] != null) {
                apiResponse = (SpotifyUserProfileAPIResponse)Session["SpotifyUserProfileAPIResponse"];
            } else {
                return RedirectToAction("Index", "Home");
            }

            if (!String.IsNullOrEmpty(Request.QueryString["query"]) &&
                !String.IsNullOrEmpty(Request.QueryString["type"]) &&
                !String.IsNullOrEmpty(Request.QueryString["market"])) {
                //perform search and display results
                try {
                    var client = new RestClient();
                    var request = new RestRequest("https://api.spotify.com/v1/search?q=" + ViewBag.Query + "&type=" + Request.QueryString["type"] + "&market=" + ViewBag.Market, Method.Get);
                    request.AddHeader("Authorization", "Bearer " + apiResponse.AccessToken);
                    RestResponse response = client.Execute(request);

                    // Check if response is OK
                    if (response.StatusCode != System.Net.HttpStatusCode.OK) {
                        ViewBag.Content = "null";
                        ViewBag.Error = response.Content;
                        return View();
                    }

                    // Deserialize response
                    List<PlaylistItem> list = new List<PlaylistItem>();
                    dynamic json = JsonConvert.DeserializeObject(response.Content);
                    foreach (var item in json.albums.items) {
                        PlaylistItem playlistItem = new PlaylistItem();
                        playlistItem.Name = item.name;
                        playlistItem.Id = item.id;
                        playlistItem.LinkToSpotify = item.external_urls.spotify;
                        playlistItem.ImageUrl = item.images[0].url;
                        playlistItem.Description = item.album_type;
                        list.Add(playlistItem);
                    }


                    // Return playlistItems to view
                    ViewBag.Playlists = list;
                    ViewBag.Content = response.Content;
                } catch (Exception ex) {
                    ViewBag.Content = "null";
                    ViewBag.Error = ex.Message;
                }
            }

            return View();
        }

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

        public ActionResult UserProfileView() {
            if (Session["SpotifyUser"] != null) {
                try {
                    SpotifyUser spotifyUser = new SpotifyUser();
                    spotifyUser = (SpotifyUser)Session["SpotifyUser"];
                    ViewBag.User = spotifyUser;
                } catch (Exception ex) {
                    ViewBag.Error = ex.Message;
                }
                return View();
            } else {
                return RedirectToAction("Index", "Home");
            }
        }
    }
}