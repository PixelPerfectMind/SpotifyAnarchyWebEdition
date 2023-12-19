using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SpotifyAnarchyWebEdition.Models {

    /// <summary>
    /// Contains all the information about the user, which is returned<br></br>by the Spotify API
    /// </summary>
    public class SpotifyUser {

        /// <summary>
        /// Country of the user
        /// </summary>
        public string Country { get; set; }

        /// <summary>
        /// The name displayed on the user's profile.
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// The user's email address.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Returns true, if the user has enabled explicit content
        /// </summary>
        public bool ExplicitContentEnabled { get; set; }

        /// <summary>
        /// Returns true, if the user is allowed to consume explicit content
        /// </summary>
        public bool ExplicitContentAllowed { get; set; }


        public string SpotifyProfileUrl { get; set; }
        public string SpotifyId { get; set; }
        public string Uri { get; set; }
        public string ImageUrl { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }

    }
}