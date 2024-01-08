using SpotifyAnarchyWebEdition.Models;
using System.Web.Mvc;

namespace SpotifyAnarchyWebEdition.Controllers.Spotify
{
    public class WebPlaybackController : Controller
    {
        // GET: WebPlayback
        public ActionResult Playback(string songId)
        {
            // Get User 's Access Token for the web player
            SpotifyUserProfileAPIResponse apiResponse = new SpotifyUserProfileAPIResponse();
            apiResponse = (SpotifyUserProfileAPIResponse)Session["SpotifyUserProfileAPIResponse"];
            ViewBag.AccessToken = apiResponse.AccessToken;

            // Check if User has premium subscription
            SpotifyUser spotifyUser = new SpotifyUser();
            spotifyUser = (SpotifyUser)Session["SpotifyUser"];
            if(spotifyUser.Product == "premium") {
                ViewBag.UserIsPremium = true;

            } else {
                ViewBag.UserIsPremium = false;
                return View();
            }

            return View();
        }
    }
}