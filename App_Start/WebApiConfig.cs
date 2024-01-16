using System.Web.Http;

namespace SpotifyAnarchyWebEdition
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web-API-Konfiguration und -Dienste

            // Web-API-Routen
            config.MapHttpAttributeRoutes();


            /*  ╔==========================╗
                ║ GET USER PROFILE DETAILS ║
                ╚==========================╝  */

            // Login route
            config.Routes.MapHttpRoute(
                name: "SpotifyLogin",
                routeTemplate: "api/v1/me",
                defaults: new { controller = "User", action = "Get" }
            );

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
