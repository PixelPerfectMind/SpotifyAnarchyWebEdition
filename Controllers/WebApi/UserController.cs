using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using SpotifyAnarchyWebEdition.Models;
using System;
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
        public async Task<HttpResponseMessage> Get()
        {
            // Get the access token from the request header and cut the "Bearer " part
            string accessToken = Request.Headers.Authorization.ToString().Substring(7);
            try
            {
                // Create a new instance of the RestClient
                var client = new RestClient();

                // Create new request
                var request = new RestRequest("https://api.spotify.com/v1/me", Method.Get);

                // Add the "Authorization" header to the request
                request.AddHeader("Authorization", "Bearer " + accessToken);

                // Run the request and wait for the response
                RestResponse response = await client.ExecuteAsync(request);

                // Check if response is OK
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    // Create a new instance of the SpotifyUser object
                    SpotifyUser spotifyUser = new SpotifyUser();

                    // Deserialize the response to store the details in a SpotifyUser object automatically
                    spotifyUser = JsonConvert.DeserializeObject<SpotifyUser>(response.Content);

                    // Parse the response to get additional information, which are stored in Arrays,
                    // which don't work with the automatic deserialization
                    var json = JObject.Parse(response.Content);

                    spotifyUser.ImageUrl = json["images"].First["url"].ToString();
                    spotifyUser.TotalFollowers = Convert.ToInt32(json["followers"]["total"]);

                    // Return the JSON object with statuscode OK
                    return new HttpResponseMessage()
                    {
                        StatusCode = HttpStatusCode.OK,
                        Content = new StringContent(
                            // Serialize the SpotifyUser object/instance to JSON
                            JsonConvert.SerializeObject(spotifyUser),
                            Encoding.UTF8,
                            "text/plain"
                        )
                    };
                }
                else
                {
                    // When there was an error getting the user profile, return an error message
                    // including a simple error message and the response content fromn Spotify

                    // Create JSON object
                    JObject errorJson = new JObject
                    {
                        // Add the SpotifyUser object to the JSON object
                        { "error_description", "Error getting user profile" },
                        { "error_content", response.Content }
                    };

                    // Return the JSON object
                    return new HttpResponseMessage()
                    {
                        StatusCode = HttpStatusCode.BadRequest,
                        Content = new StringContent(
                            errorJson.ToString(),
                            Encoding.UTF8,
                            "text/plain"
                        )
                    };
                }
            }
            catch (Exception ex)
            {
                // When there is an error in the try block, return an error message,
                // which contains a simple error message and the exception message

                // Create JSON object
                JObject errorJson = new JObject
                    {
                        // Add the SpotifyUser object to the JSON object
                        { "error_description", "There was an error on performing this taks from the API's Programmer." },
                        { "error_content", ex.Message }
                    };

                // Return the JSON object with statuscode bad request
                return new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    Content = new StringContent(
                        errorJson.ToString(),
                        Encoding.UTF8,
                        "text/plain"
                    )
                };
            }

        }
    }
}