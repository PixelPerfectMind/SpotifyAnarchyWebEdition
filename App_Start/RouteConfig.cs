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
                defaults: new { controller = "Spotify", action = "SearchView" }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
