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

            config.Routes.MapHttpRoute("DefaultApi", "api/{controller}/{id}", new { id = RouteParameter.Optional });
            app.UseWebApi(config);
        }
    }
}