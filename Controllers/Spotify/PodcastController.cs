using Newtonsoft.Json.Linq;
using RestSharp;
using SpotifyAnarchyWebEdition.Models;
using SpotifyAnarchyWebEdition.Models.MediaElements;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SpotifyAnarchyWebEdition.Controllers.Spotify
{
    public class PodcastController : Controller
    {

        SpotifyUserProfileAPIResponse spotifyUserProfileAPIResponse = new SpotifyUserProfileAPIResponse();

        // GET: Podcast
        public async Task<ActionResult> PodcastView(string podcastId)
        {
            try
            {
                Podcast podcast = new Podcast();

                // Load instance of SpotifyUser from session
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
                    podcast.SpotifyId = json["id"].ToString();

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

                // Check if the user is following the podcast
                var getUserFollowingRequest = new RestRequest("https://api.spotify.com/v1/me/shows/contains?ids=" + podcastId, Method.Get);
                getUserFollowingRequest.AddHeader("Authorization", "Bearer " + spotifyUserProfileAPIResponse.AccessToken);
                RestResponse getUserFollowingResponse = await client.ExecuteAsync(getUserFollowingRequest);

                // Check if response is OK
                if (getUserFollowingResponse.StatusCode == HttpStatusCode.OK)
                {
                    // Parse the response and store it in a SpotifyUser object
                    if(getUserFollowingResponse.Content.Contains("true"))
                    {
                        ViewBag.IsFollowingPodcast = true;
                    } else
                    {
                        ViewBag.IsFollowingPodcast = false;
                    }
                } else
                {
                    throw new Exception("Something went wrong while checking if the user is following the podcast");
                }
                

                return View();
            }
            catch (Exception e)
            {
                ViewBag.ErrorMessage = e.Message;
                return View();
            }
        }

        public async Task<ActionResult> FollowPodcast(string podcastId)
        {
            try
            {
                // Get current spotify login session
                spotifyUserProfileAPIResponse = (SpotifyUserProfileAPIResponse)Session["SpotifyUserProfileAPIResponse"];

                // Create request to follow podcast
                var client = new RestClient();
                var request = new RestRequest("https://api.spotify.com/v1/me/shows?ids=" + podcastId, Method.Put);
                request.AddHeader("Authorization", "Bearer " + spotifyUserProfileAPIResponse.AccessToken);
                RestResponse response = await client.ExecuteAsync(request);

                // Check if response is OK
                if(response.StatusCode == HttpStatusCode.OK)
                {
                    return RedirectToAction("PodcastView", "Podcast", new { podcastId = podcastId });
                }
                else
                {
                    throw new Exception("Something went wrong while following the podcast");
                }
            } catch (Exception e)
            {
                ViewBag.ErrorMessage = e.Message;
                return RedirectToAction("PodcastView", "Podcast", new { podcastId = podcastId });
            }
        }

        public async Task<ActionResult> UnfollowPodcast(string podcastId)
        {
            try
            {
                // Get current spotify login session
                spotifyUserProfileAPIResponse = (SpotifyUserProfileAPIResponse)Session["SpotifyUserProfileAPIResponse"];

                // Create request to follow podcast
                var client = new RestClient();
                var request = new RestRequest("https://api.spotify.com/v1/me/shows?ids=" + podcastId, Method.Delete);
                request.AddHeader("Authorization", "Bearer " + spotifyUserProfileAPIResponse.AccessToken);
                RestResponse response = await client.ExecuteAsync(request);

                // Check if response is OK
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    return RedirectToAction("PodcastView", "Podcast", new { podcastId = podcastId });
                }
                else
                {
                    throw new Exception("Something went wrong while unfollowing the podcast");
                }
            }
            catch (Exception e)
            {
                ViewBag.ErrorMessage = e.Message;
                return RedirectToAction("PodcastView", "Podcast", new { podcastId = podcastId });
            }
        }
    }
}