using SpotifyAnarchyWebEdition.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SpotifyAnarchyWebEdition.Controllers
{
    public class WebApiController : ApiController
    {
        // GET: api/WebApi
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/WebApi/5
        [HttpGet]
        public IEnumerable<string> Login(string accessToken) {
            SpotifyUserProfileAPIResponse spotifyUserProfileAPIResponse = new SpotifyUserProfileAPIResponse();
            spotifyUserProfileAPIResponse.AccessToken = "placebo-bullshit-content";
            spotifyUserProfileAPIResponse.Scope = "read-private";
            spotifyUserProfileAPIResponse.RefreshToken = "placebo-bullshit-refresh";
            spotifyUserProfileAPIResponse.TokenType = "Bearer";
            spotifyUserProfileAPIResponse.ExpiresIn = 50;
            return new string[] { "value1", "value2" };
        }

        // POST: api/WebApi
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/WebApi/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/WebApi/5
        public void Delete(int id)
        {
        }
    }
}
