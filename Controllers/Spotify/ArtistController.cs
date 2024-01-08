using Newtonsoft.Json.Linq;
using RestSharp;
using SpotifyAnarchyWebEdition.Models;
using SpotifyAnarchyWebEdition.Models.MediaElements;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace SpotifyAnarchyWebEdition.Controllers.Spotify {
    public class ArtistController : Controller {

        /// <summary>
        /// View artist page
        /// </summary>
        public async Task<ActionResult> ArtistView(string artistId) {

            if (Session["SpotifyUser"] != null) {

                try {

                    // Load instance of SpotifyUser from session
                    SpotifyUserProfileAPIResponse spotifyUserProfileAPIResponse = new SpotifyUserProfileAPIResponse();
                    spotifyUserProfileAPIResponse = (SpotifyUserProfileAPIResponse)Session["SpotifyUserProfileAPIResponse"];

                    Artist artist = new Artist();
                    ObservableCollection<Song> TopSongs = new ObservableCollection<Song>();
                    ObservableCollection<Artist> RelatedArtists = new ObservableCollection<Artist>();

                    var client = new RestClient();
                    var request = new RestRequest("https://api.spotify.com/v1/artists/" + artistId, Method.Get);
                    request.AddHeader("Authorization", "Bearer " + spotifyUserProfileAPIResponse.AccessToken);
                    RestResponse response = client.Execute(request);

                    // Check if response is OK
                    if (response.StatusCode == System.Net.HttpStatusCode.OK) {

                        // Parse the response and store it in a SpotifyUser object
                        var json = JObject.Parse(response.Content);

                        artist.Id = json["id"].ToString();
                        artist.Name = json["name"].ToString();
                        artist.ImageUrl = json["images"][1]["url"].ToString();
                        artist.TotalFollowers = Convert.ToInt32(json["followers"]["total"]);
                        artist.Uri = json["uri"].ToString();

                        var genres = json["genres"].Children().ToList();
                        foreach (var genre in genres)
                        {
                            artist.Genres.Add(genre.ToString());
                        }

                        artist.Href = json["href"].ToString();
                        artist.PopularityScore = Convert.ToInt32(json["popularity"]);

                        // Get the artist's top tracks
                        var topTracksRequest = new RestRequest("https://api.spotify.com/v1/artists/" + artistId + "/top-tracks?country=DE", Method.Get);
                        topTracksRequest.AddHeader("Authorization", "Bearer " + spotifyUserProfileAPIResponse.AccessToken);
                        RestResponse topTracksResponse = await client.ExecuteAsync(topTracksRequest);

                        // Check if response is OK
                        if (topTracksResponse.StatusCode == System.Net.HttpStatusCode.OK) {

                            var topTracksJson = JObject.Parse(topTracksResponse.Content);

                            var topTracks = topTracksJson["tracks"].Children().ToList();
                            foreach (var track in topTracks) {

                                // Get the first artist from the artists array
                                var artists = track["artists"].Children();
                                var songArtist = artists.First()["name"].ToString();

                                // Add new song to the album item
                                TopSongs.Add(new Song {
                                    Name = track["name"].ToString(),
                                    Artist = songArtist,
                                    Album = track["album"]["name"].ToString(),
                                    ImageUrl = track["album"]["images"].Last["url"].ToString(),
                                    Id = track["id"].ToString(),
                                    Uri = track["uri"].ToString(),
                                    PreviewUrl = track["preview_url"].ToString()
                                });
                            }
                        }

                        // Get the artist's related artists
                        var relatedArtistsRequest = new RestRequest("https://api.spotify.com/v1/artists/" + artistId + "/related-artists", Method.Get);
                        relatedArtistsRequest.AddHeader("Authorization", "Bearer " + spotifyUserProfileAPIResponse.AccessToken);
                        RestResponse relatedArtistsResponse = await client.ExecuteAsync(relatedArtistsRequest);

                        // Check if response is OK
                        if (relatedArtistsResponse.StatusCode == System.Net.HttpStatusCode.OK) {

                            var relatedArtistsJson = JObject.Parse(relatedArtistsResponse.Content);

                            var relatedArtists = relatedArtistsJson["artists"].Children().ToList();
                            foreach (var relatedArtist in relatedArtists) {

                                // Add new artist to the related artists item
                                RelatedArtists.Add(new Artist {
                                    Id = relatedArtist["id"].ToString(),
                                    Name = relatedArtist["name"].ToString(),
                                    ImageUrl = relatedArtist["images"].Last["url"].ToString()
                                });
                            }
                        }

                        ViewBag.RelatedArtists = RelatedArtists;
                        ViewBag.TopSongs = TopSongs;
                        ViewBag.Artist = artist;
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