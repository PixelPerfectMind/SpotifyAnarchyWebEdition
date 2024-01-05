using System.Web.Mvc;
using System.Web.Routing;

namespace SpotifyAnarchyWebEdition {

    public class RouteConfig {

        public static void RegisterRoutes(RouteCollection routes) {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            // OAuth route
            routes.MapRoute(
                name: "SpotifyAuth",
                url: "oauth",
                defaults: new { controller = "Spotify", action = "AuthCallback" }
            );

            // Search route
            routes.MapRoute(
                name: "SpotifySearch",
                url: "search",
                defaults: new { controller = "Search", action = "SearchView" }
            );

            // User profile page route
            routes.MapRoute(
                name: "ViewUserProfile",
                url: "profile",
                defaults: new { controller = "UserProfile", action = "UserProfileView" }
            );

            // View album route
            routes.MapRoute(
                name: "ViewAlbum",
                url: "album",
                defaults: new { controller = "Album", action = "AlbumView" }
            );


            // View playlist route
            routes.MapRoute(
                name: "ViewPlaylist",
                url: "playlist",
                defaults: new { controller = "Playlist", action = "PlaylistView" }
            );

            // Follow playlist route
            routes.MapRoute(
                name: "FollowPlaylist",
                url: "playlist/follow",
                defaults: new { controller = "Playlist", action = "FollowPlaylist" }
            );

            // Unfollow playlist route
            routes.MapRoute(
                name: "UnfollowPlaylist",
                url: "playlist/unfollow",
                defaults: new { controller = "Playlist", action = "UnfollowPlaylist" }
            );


            // View artist route
            routes.MapRoute(
                name: "ViewArtist",
                url: "artist",
                defaults: new { controller = "Artist", action = "ArtistView" }
            );

            // Logout
            routes.MapRoute(
                name: "Logout",
                url: "logout",
                defaults: new { controller = "Spotify", action = "Logout" }
            );

            // Default fallback route
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
