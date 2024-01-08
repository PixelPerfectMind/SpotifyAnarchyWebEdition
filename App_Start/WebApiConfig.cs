using System;
using System.Collections.Generic;
using System.Linq;
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

            // Login route
            config.Routes.MapHttpRoute(
                name: "SpotifyLogin",
                routeTemplate: "api/v1/UserDetails",
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
