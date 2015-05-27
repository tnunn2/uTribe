using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace urTribeWebAPI
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            SetupWebAPI(app);
        }

        public void SetupWebAPI(IAppBuilder app)
        {
            var config = new HttpConfiguration();
            config.MapHttpAttributeRoutes();

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
               defaults: new { controller = "users" }
           );

            config.Routes.MapHttpRoute(
                 name: "UserRouteForUserObjApi",
                 routeTemplate: "api/Users/{user}",
                 defaults: new { controller = "users", user = RouteParameter.Optional }
             );
            config.Routes.MapHttpRoute(
                name: "Userss2Api",
                routeTemplate: "api/events/{eventId}/Users/{userId}/Status/{AttendStatus}",
                defaults: new { controller = "users", AttendStatus = RouteParameter.Optional }
            );
            config.Routes.MapHttpRoute("DefaultApi", "api/{controller}/{id}", new { id = RouteParameter.Optional });
            config.Routes.MapHttpRoute("DefaultApiWithAction", "api/{controller}/{action}/{id}", new { id = RouteParameter.Optional });
            app.UseWebApi(config);
        }
    }
}