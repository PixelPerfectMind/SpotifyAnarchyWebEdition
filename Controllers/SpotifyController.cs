﻿using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using SpotifyAnarchyWebEdition.Models;
using System;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SpotifyAnarchyWebEdition.Controllers {

    public class SpotifyController : Controller {

        /// <summary>
        /// Spotify OAuth2.0 Callback
        /// </summary>
        public async Task<ActionResult> AuthCallback(string code) {
            try
            {
                DefaultValues defaultValues = new DefaultValues();

                RestClient client = new RestClient();
                RestRequest request = new RestRequest("https://accounts.spotify.com/api/token", Method.Post);
                request.AddHeader("Authorization", "Basic " + defaultValues.basicAuthorization);
                request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
                request.AddParameter("grant_type", "authorization_code");
                request.AddParameter("code", code);
                request.AddParameter("redirect_uri", "https://localhost:44394/oauth");
                RestResponse response = await client.ExecuteAsync(request);

                // Check if response is OK
                if (response.StatusCode != System.Net.HttpStatusCode.OK)
                {
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

                if (Session["SpotifyUser"] != null)
                {
                    SpotifyUser spotifyUser = new SpotifyUser();
                    spotifyUser = (SpotifyUser)Session["SpotifyUser"];
                    ViewBag.User = spotifyUser;
                }

            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
            }

            return View();
        }

        /// <summary>
        /// Get the user profile which belongs to the Spotify Account
        /// <br>and save it into the Session</br>
        /// </summary>
        private void GetUserProfile() {

            try
            {
                // Initialize SpotifyUserProfileAPIResponse
                SpotifyUserProfileAPIResponse spotifyUserProfileAPIResponse = new SpotifyUserProfileAPIResponse();

                if (Session["SpotifyUserProfileAPIResponse"] != null)
                {
                    // If session exists, get it and load it into the new object
                    spotifyUserProfileAPIResponse = (SpotifyUserProfileAPIResponse)Session["SpotifyUserProfileAPIResponse"];

                    var client = new RestClient();
                    var request = new RestRequest("https://api.spotify.com/v1/me", Method.Get);
                    request.AddHeader("Authorization", "Bearer " + spotifyUserProfileAPIResponse.AccessToken);
                    RestResponse response = client.Execute(request);
                    Console.WriteLine(response.Content);

                    // Check if response is OK
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        // Parse the response and store it in a SpotifyUser object
                        var json = JObject.Parse(response.Content);

                        SpotifyUser spotifyUser = new SpotifyUser();
                        spotifyUser.Country = json["country"].ToString();
                        spotifyUser.DisplayName = json["display_name"].ToString();
                        spotifyUser.Email = json["email"].ToString();

                        if (json["explicit_content"]["filter_enabled"].ToString() == "true")
                        {
                            spotifyUser.ExplicitContentEnabled = false;
                        }
                        else
                        {
                            spotifyUser.ExplicitContentEnabled = true;
                        }

                        if (json["explicit_content"]["filter_locked"].ToString() == "true")
                        {
                            spotifyUser.ExplicitContentAllowed = false;
                        }
                        else
                        {
                            spotifyUser.ExplicitContentAllowed = true;
                        }

                        spotifyUser.SpotifyProfileUrl = json["external_urls"]["spotify"].ToString();
                        spotifyUser.TotalFollowers = Convert.ToInt32(json["followers"]["total"]);
                        spotifyUser.SpotifyId = json["id"].ToString();
                        spotifyUser.ImageUrl = json["images"][0]["url"].ToString();
                        spotifyUser.Type = json["type"].ToString();
                        spotifyUser.Uri = json["uri"].ToString();
                        spotifyUser.Product = json["product"].ToString();

                        // Save the SpotifyUser object in the session
                        Session["SpotifyUser"] = spotifyUser;
                    }
                    else
                    {
                        ViewBag.Error = "Error getting your user profile: " + response.Content;
                    }
                }
                else
                {
                    ViewBag.Error = "Error getting your user profile: You are not signed in.";
                }

            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
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
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
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
    }
}