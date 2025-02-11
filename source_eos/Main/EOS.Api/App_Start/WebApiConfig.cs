using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Dispatcher;
using EOS.Api.security;
using EOS.security;
using Microsoft.Owin.Security.OAuth;
using Newtonsoft.Json.Serialization;

namespace EOS.Api
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            // Configure Web API to use only bearer token authentication.
            config.SuppressDefaultHostAuthentication();
            config.Filters.Add(new HostAuthenticationFilter(OAuthDefaults.AuthenticationType));

            var sessionHandler = new SessionHandler(new List<ISessionInspector>() { new SessionTimeInspector() }) { InnerHandler = new HttpControllerDispatcher(config) };

            // Web API routes
            config.MapHttpAttributeRoutes();
            config.EnableCors();

            config.Routes.MapHttpRoute(
                name: "Public",
                routeTemplate: "api/public/IsTokenManagement.aspx/{param}",
                defaults: new { param = System.Web.Http.RouteParameter.Optional, controller = "IsTokenManagement" });

            config.Routes.MapHttpRoute(
                name: "PublicServerDateTime",
                routeTemplate: "api/public/ServerDateTime.aspx/{param}",
                defaults: new { param = System.Web.Http.RouteParameter.Optional, controller = "ServerDateTime" });

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}.aspx/{param}",
                defaults: new { param = System.Web.Http.RouteParameter.Optional },
                constraints: null,
                handler: sessionHandler);

            config.Routes.MapHttpRoute(
               name: "Private",
               routeTemplate: "api/private/{controller}.aspx/{param}",
               defaults: new { param = System.Web.Http.RouteParameter.Optional });

            config.Routes.MapHttpRoute(
               name: "Download",
               routeTemplate: "api/private/{controller}.ashx/{param}",
               defaults: new { param = System.Web.Http.RouteParameter.Optional });

            config.Routes.MapHttpRoute(
               name: "Upload",
               routeTemplate: "api/{controller}/{action}.aspx/{param}",
               defaults: new { param = System.Web.Http.RouteParameter.Optional },
               constraints: null);

            config.Routes.MapHttpRoute(
                name: "ActionApi",
                routeTemplate: "api/{controller}/{action}/{param}",
                defaults: new { param = System.Web.Http.RouteParameter.Optional },
                constraints: null,
                handler: sessionHandler);


        }
    }
}
