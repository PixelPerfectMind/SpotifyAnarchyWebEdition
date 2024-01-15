using System.Web.Mvc;
using System.Web.Routing;

namespace SpotifyAnarchyWebEdition
{

    public class RouteConfig
    {

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            /*  ╔=====================╗
                ║   OAUTH 2.0 ROUTE   ║
                ╚=====================╝  */

            // OAuth route
            routes.MapRoute(
                name: "SpotifyAuth",
                url: "oauth",
                defaults: new { controller = "Spotify", action = "AuthCallback" }
            );


            /*  ╔====================╗
                ║    SEARCH ROUTE    ║
                ╚====================╝  */

            // Search route
            routes.MapRoute(
                name: "SpotifySearch",
                url: "search",
                defaults: new { controller = "Search", action = "SearchView" }
            );


            /*  ╔====================╗
                ║ USER PROFILE ROUTE ║
                ╚====================╝  */

            // User profile page route
            routes.MapRoute(
                name: "ViewUserProfile",
                url: "profile",
                defaults: new { controller = "UserProfile", action = "UserProfileView" }
            );

            // Follow user route
            routes.MapRoute(
                name: "FollowUser",
                url: "profile/follow",
                defaults: new { controller = "UserProfile", action = "FollowUser" }
            );

            // Unfollow user route
            routes.MapRoute(
                name: "UnfollowUser",
                url: "profile/unfollow",
                defaults: new { controller = "UserProfile", action = "UnfollowUser" }
            );


            /*  ╔====================╗
                ║    ALBUM ROUTE     ║
                ╚====================╝  */

            // View album route
            routes.MapRoute(
                name: "ViewAlbum",
                url: "album",
                defaults: new { controller = "Album", action = "AlbumView" }
            );

            // Follow album route
            routes.MapRoute(
                name: "FollowAlbum",
                url: "album/follow",
                defaults: new { controller = "Album", action = "FollowAlbum" }
            );

            // Unfollow album route
            routes.MapRoute(
                name: "UnfollowAlbum",
                url: "album/unfollow",
                defaults: new { controller = "Album", action = "UnfollowAlbum" }
            );


            /*  ╔====================╗
                ║  PLAYLIST ROUTES   ║
                ╚====================╝  */

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


            /*  ╔====================╗
                ║    ARTIST ROUTE    ║
                ╚====================╝  */

            // View artist route
            routes.MapRoute(
                name: "ViewArtist",
                url: "artist",
                defaults: new { controller = "Artist", action = "ArtistView" }
            );

            // Follow artist route
            routes.MapRoute(
                name: "FollowArtist",
                url: "artist/follow",
                defaults: new { controller = "Artist", action = "FollowArtist" }
            );

            // Unfollow artist route
            routes.MapRoute(
                name: "UnfollowArtist",
                url: "artist/unfollow",
                defaults: new { controller = "Artist", action = "UnfollowArtist" }
            );


            /*  ╔====================╗
                ║   PODCAST ROUTES   ║
                ╚====================╝  */

            // View podcast route
            routes.MapRoute(
                name: "ViewPodcast",
                url: "podcast",
                defaults: new { controller = "Podcast", action = "PodcastView" }
            );

            // Follow podcast route
            routes.MapRoute(
                name: "FollowPodcast",
                url: "podcast/follow",
                defaults: new { controller = "Podcast", action = "FollowPodcast" }
            );

            // Unfollow podcast route
            routes.MapRoute(
                name: "UnfollowPodcast",
                url: "podcast/unfollow",
                defaults: new { controller = "Podcast", action = "UnfollowPodcast" }
            );


            /*  ╔ TODO: =============╗
                ║ WEB PLAYBACK ROUTE ║
                ╚====================╝  */

            // Web playback route
            routes.MapRoute(
                name: "ViewWebPlayback",
                url: "playback",
                defaults: new { controller = "WebPlayback", action = "Playback" }
            );


            /*  ╔====================╗
                ║       LOGOUT       ║
                ╚====================╝  */

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
