﻿using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using SpotifyAnarchyWebEdition.Models;
using SpotifyAnarchyWebEdition.Models.MediaElements;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web.Mvc;

namespace SpotifyAnarchyWebEdition.Controllers
{

    public class HomeController : Controller
    {

        public ActionResult Index()
        {
            try
            {
                if (Session["BearerTokenForPublicUses"] == null)
                {
                    GetBasicBearerToken();

                    // Get newest albums
                    GetNewRecommandations();
                }
                else
                {
                    // Get the newest albums from the session
                    NewRecommandations = (ObservableCollection<Album>)Session["NewestRecommandations"];
                }
                ViewBag.NewAlbums = NewRecommandations;

                // Get current user
                if (Session["SpotifyUser"] != null)
                {
                    try
                    {
                        SpotifyUser spotifyUser = new SpotifyUser();
                        spotifyUser = (SpotifyUser)Session["SpotifyUser"];

                        ViewBag.UserName = spotifyUser.DisplayName;
                        ViewBag.UserImagePath = spotifyUser.ImageUrl;
                    }
                    catch (Exception ex)
                    {
                        ViewBag.Error = ex.Message;
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
            }

            return View();
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        /// <summary>
        /// Contains the entries for the newest albums
        /// </summary>
        public ObservableCollection<Album> NewRecommandations = new ObservableCollection<Album>();

        /// <summary>
        /// Get a new bearer token, which allows you to access the Spotify API
        /// </summary>
        private void GetBasicBearerToken()
        {
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

        private void GetNewRecommandations()
        {
            // Get newest albums
            try
            {
                // Check if bearer token is already set
                if (Session["BearerTokenForPublicUses"] == null)
                {
                    GetBasicBearerToken();
                }

                RestClient client = new RestClient();
                RestRequest request = new RestRequest("https://api.spotify.com/v1/browse/new-releases?limit=20", Method.Get);
                request.AddHeader("Authorization", "Bearer " + Session["BearerTokenForPublicUses"]);

                // Execute the request
                RestResponse response = client.Execute(request);

                if (response.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    throw (new Exception("Error getting new albums:\n" + response.Content));
                }
                else
                {
                    // Read the response
                    var responseString = response.Content;

                    // Parse the response
                    var json = JObject.Parse(responseString);
                    var albums = json["albums"]["items"].Children().ToList();

                    // Add the albums to the list
                    foreach (var album in albums)
                    {
                        NewRecommandations.Add(new Album {
                            Id = album["id"].ToString(),
                            Name = album["name"].ToString(),
                            Artist = album["artists"][0]["name"].ToString(),
                            ImageUrl = album["images"][1]["url"].ToString(),
                            Uri = album["uri"].ToString(),
                            ReleaseDate = album["release_date"].ToString(),
                        });
                    }

                    Session["NewestRecommandations"] = NewRecommandations;
                    ViewBag.NewAlbums = NewRecommandations;
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
            }
        }
    }
}