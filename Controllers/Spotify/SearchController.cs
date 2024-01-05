using Newtonsoft.Json.Linq;
using RestSharp;
using SpotifyAnarchyWebEdition.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SpotifyAnarchyWebEdition.Controllers.Spotify
{
    public class SearchController : Controller {

        /// <summary>
        /// Opens the search page and/or performs a search
        /// </summary>
        public ActionResult SearchView(string query, string type, string market)
        {
            // Initialize the lists
            ObservableCollection<Song> Songs = new ObservableCollection<Song>();
            ObservableCollection<Playlist> Playlists = new ObservableCollection<Playlist>();
            ObservableCollection<Album> Albums = new ObservableCollection<Album>();
            ObservableCollection<Artist> Artists = new ObservableCollection<Artist>();

            ViewBag.Query = Request.QueryString["query"];
            ViewBag.Market = Request.QueryString["market"];

            // Initialize SpotifyUserProfileAPIResponse
            SpotifyUserProfileAPIResponse apiResponse = new SpotifyUserProfileAPIResponse();
            if (Session["SpotifyUserProfileAPIResponse"] != null)
            {
                apiResponse = (SpotifyUserProfileAPIResponse)Session["SpotifyUserProfileAPIResponse"];
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }

            if (!String.IsNullOrEmpty(Request.QueryString["query"]) &&
                !String.IsNullOrEmpty(Request.QueryString["type"]) &&
                !String.IsNullOrEmpty(Request.QueryString["market"]))
            {
                //perform search and display results
                try
                {
                    var client = new RestClient();
                    var request = new RestRequest("https://api.spotify.com/v1/search?q=" + ViewBag.Query + "&type=" + Request.QueryString["type"] + "&market=" + ViewBag.Market, Method.Get);
                    request.AddHeader("Authorization", "Bearer " + apiResponse.AccessToken);
                    RestResponse response = client.Execute(request);

                    // Check if response is OK
                    if (response.StatusCode != System.Net.HttpStatusCode.OK)
                    {
                        ViewBag.Content = "null";
                        ViewBag.Error = response.Content;
                        return View();
                    }

                    // Read the response
                    var responseString = response.Content;

                    // Parse the response
                    var json = JObject.Parse(responseString);

                    // If type is playlist, add the playlists to the list
                    if (Request.QueryString["type"].Contains("playlist"))
                    {
                        var playlists = json["playlists"]["items"].Children().ToList();
                        foreach (var playlist in playlists)
                        {
                            var images = playlist["images"].Children();
                            var playlistImageUrl = images.First()["url"].ToString();
                            Playlists.Add(new Playlist(playlist["id"].ToString(), playlist["name"].ToString(),
                                playlistImageUrl, playlist["description"].ToString(), playlist["uri"].ToString()));
                        }
                    }
                    ViewBag.Playlists = Playlists;


                    // If type is single, add the songs to the list
                    if (Request.QueryString["type"].Contains("track"))
                    {
                        var tracks = json["tracks"]["items"].Children().ToList();
                        foreach (var track in tracks)
                        {
                            var artists = track["artists"].Children();
                            var songArtist = artists.First()["name"].ToString();
                            var albumImages = track["album"]["images"].Children();
                            var songAlbumImageUrl = albumImages.Last()["url"].ToString();
                            Songs.Add(new Song(track["id"].ToString(), track["name"].ToString(), songArtist,
                                track["album"]["name"].ToString(), songAlbumImageUrl, track["uri"].ToString(),
                                track["preview_url"].ToString()));
                        }
                    }
                    ViewBag.Songs = Songs;


                    // If type is album, add the albums to the list
                    if (Request.QueryString["type"].Contains("album"))
                    {
                        var albums = json["albums"]["items"].Children().ToList();
                        foreach (var album in albums)
                        {
                            var artists = album["artists"].Children();
                            var albumArtist = artists.First()["name"].ToString();
                            var albumImages = album["images"].Children();
                            var albumImageUrl = albumImages.Last()["url"].ToString();
                            Albums.Add(new Album(album["id"].ToString(), album["name"].ToString(), albumArtist,
                                albumImageUrl, album["uri"].ToString(), album["release_date"].ToString()));
                        }
                    }
                    ViewBag.Albums = Albums;


                    // If type is artist, add the Artists to the list
                    if (Request.QueryString["type"].Contains("artist"))
                    {
                        var artists = json["artists"]["items"].Children().ToList();
                        foreach (var artist in artists)
                        {
                            var artistImages = artist["images"].Children();
                            var artistImageUrl = artistImages.Last()["url"].ToString();
                            var totalFollowers = Convert.ToInt32(artist["followers"]["total"]);
                            Artists.Add(new Artist(artist["id"].ToString(), artist["name"].ToString(), artistImageUrl, totalFollowers));
                        }
                    }
                    ViewBag.Artists = Artists;

                }
                catch (Exception ex)
                {
                    ViewBag.Content = "null";
                    ViewBag.Error = ex.Message + ": " + ex.Data;
                }
            }
            return View();
        }
    }
}