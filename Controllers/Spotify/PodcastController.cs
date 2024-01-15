using Newtonsoft.Json.Linq;
using RestSharp;
using SpotifyAnarchyWebEdition.Models;
using SpotifyAnarchyWebEdition.Models.MediaElements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace SpotifyAnarchyWebEdition.Controllers.Spotify
{
    public class PodcastController : Controller
    {
        // GET: Podcast
        public async Task<ActionResult> PodcastView(string podcastId)
        {
            try
            {
                Podcast podcast = new Podcast();

                // Load instance of SpotifyUser from session
                SpotifyUserProfileAPIResponse spotifyUserProfileAPIResponse = new SpotifyUserProfileAPIResponse();
                spotifyUserProfileAPIResponse = (SpotifyUserProfileAPIResponse)Session["SpotifyUserProfileAPIResponse"];

                var client = new RestClient();
                var request = new RestRequest("https://api.spotify.com/v1/shows/" + podcastId, Method.Get);
                request.AddHeader("Authorization", "Bearer " + spotifyUserProfileAPIResponse.AccessToken);
                RestResponse response = await client.ExecuteAsync(request);

                // Check if response is OK
                if(response.StatusCode == HttpStatusCode.OK)
                {
                    // Parse the response and store it in a SpotifyUser object
                    var json = JObject.Parse(response.Content);
                    
                    podcast.Name = json["name"].ToString();
                    podcast.Description = json["description"].ToString();
                    podcast.SpotifyHref = json["href"].ToString();
                    podcast.Publisher = json["publisher"].ToString();
                    podcast.ImageUrl = json["images"][0]["url"].ToString();
                    podcast.TotalEpisodes = Int32.Parse(json["total_episodes"].ToString());

                    // Foreach episode in the podcast, add it to the podcast object
                    var episodes = json["episodes"]["items"].Children().ToList();
                    foreach(var episode in episodes)
                    {
                        podcast.Episodes.Add(new Episode
                        {
                            Name = episode["name"].ToString(),
                            Description = episode["description"].ToString(),
                            DurationInMs = Int32.Parse(episode["duration_ms"].ToString()),
                            ImageUrl = episode["images"].Last["url"].ToString(),
                            SpotifyId = episode["id"].ToString(),
                            SpotifyURI = episode["uri"].ToString(),
                            ReleaseDate = episode["release_date"].ToString(),
                            AudioPreviewUrl = episode["audio_preview_url"].ToString()
                        });
                    }
                }
                ViewBag.Podcast = podcast;

                return View();
            }
            catch (Exception e)
            {
                ViewBag.ErrorMessage = e.Message;
                return View();
            }
        }
    }
}