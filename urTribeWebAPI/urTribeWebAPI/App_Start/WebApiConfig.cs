using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace urTribeWebAPI
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.Routes.MapHttpRoute(
                name: "Events1Api",
                routeTemplate: "api/Users/{userId}/events/{eventId}/Contacts/{contactList}",
                defaults: new { controller = "events", contactList = RouteParameter.Optional }
            );

            config.Routes.MapHttpRoute(
                name: "Events2Api",
                routeTemplate: "api/Users/{userId}/events/{eventId}/Status/{AttendStatus}",
                defaults: new { controller = "events", AttendStatus = RouteParameter.Optional }
            );

            config.Routes.MapHttpRoute(
                name: "Events3Api",
                routeTemplate: "api/Users/{userId}/events/{eventId}",
                defaults: new { controller = "events", eventId = RouteParameter.Optional }
            );

            config.Routes.MapHttpRoute(
                name: "Events4Api",
                routeTemplate: "api/Users/{userId}/events/evt",
                defaults: new { controller = "events", evt = RouteParameter.Optional }
            );

            config.Routes.MapHttpRoute(
                name: "ContactUserIdApi",
                routeTemplate: "api/Users/{userId}/contacts/{contactId}",
                defaults: new { controller = "contacts", contactId = RouteParameter.Optional }
            );

             config.Routes.MapHttpRoute(
                name: "UsersApi",
                routeTemplate: "api/Users/{userId}",
                defaults: new { controller = "users"} 
            );

           config.Routes.MapHttpRoute(
                name: "UserRouteForUserObjApi",
                routeTemplate: "api/Users/{user}",
                defaults: new { controller = "users", user = RouteParameter.Optional }
            );

            config.Routes.MapHttpRoute(
                name: "UsersTestMethod",
                routeTemplate: "api/Users/{i}",
                defaults: new { controller = "users", i = RouteParameter.Optional }
            );



            // Uncomment the following line of code to enable query support for actions with an IQueryable or IQueryable<T> return type.
            // To avoid processing unexpected or malicious queries, use the validation settings on QueryableAttribute to validate incoming queries.
            // For more information, visit http://go.microsoft.com/fwlink/?LinkId=279712.
            //config.EnableQuerySupport();

            // To disable tracing in your application, please comment out or remove the following line of code
            // For more information, refer to: http://www.asp.net/web-api
            config.EnableSystemDiagnosticsTracing();
        }
    }
}
