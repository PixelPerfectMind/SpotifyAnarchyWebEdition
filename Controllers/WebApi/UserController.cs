using Newtonsoft.Json.Linq;
using RestSharp;
using SpotifyAnarchyWebEdition.Models;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace SpotifyAnarchyWebEdition.Controllers.WebApi
{
    public class UserController : ApiController
    {
        // GET: User
        public async Task<HttpResponseMessage> Get(string accessToken)
        {
            try
            {

                // Create JSON object
                JObject spotifyUserJson = new JObject();
                var client = new RestClient();
                var request = new RestRequest("https://api.spotify.com/v1/me", Method.Get);
                request.AddHeader("Authorization", "Bearer " + accessToken);
                RestResponse response = await client.ExecuteAsync(request);

                // Check if response is OK
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    // Parse the response and store it in a SpotifyUser object
                    var json = JObject.Parse(response.Content);

                    SpotifyUser spotifyUser = new SpotifyUser();
                    spotifyUser.Country = json["country"].ToString();
                    spotifyUser.DisplayName = json["display_name"].ToString();
                    spotifyUser.SpotifyId = json["id"].ToString();

                    // Add the SpotifyUser object to the JSON object
                    spotifyUserJson.Add("country", spotifyUser.Country);
                    spotifyUserJson.Add("display_name", spotifyUser.DisplayName);
                    spotifyUserJson.Add("id", spotifyUser.SpotifyId);
                }
                else
                {
                    // Add the SpotifyUser object to the JSON object
                    spotifyUserJson.Add("error_description", "Error getting user profile");
                    spotifyUserJson.Add("error_content", response.Content);
                }

                return new HttpResponseMessage()
                {
                    Content = new StringContent(
                        spotifyUserJson.ToString(),
                        Encoding.UTF8,
                        "text/plain"
                    )
                };
            }
            catch
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }

        }
    }
}