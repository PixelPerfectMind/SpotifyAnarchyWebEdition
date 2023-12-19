using Newtonsoft.Json;
using RestSharp;
using SpotifyAnarchyWebEdition.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SpotifyAnarchyWebEdition.Controllers
{

    public class SpotifyController : Controller
    {

        // GET: Spotify OAuth Callback
        public ActionResult AuthCallback(string code) {
            try {
                DefaultValues defaultValues = new DefaultValues();

                ViewBag.Succeeded = true;
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


            }
            catch (Exception ex)
            {
                ViewBag.Succeeded = false;
                ViewBag.Exception = ex.Message;
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
                    var options = new RestClientOptions()
                    {
                        MaxTimeout = -1,
                    };
                    var client = new RestClient(options);
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
    }
}