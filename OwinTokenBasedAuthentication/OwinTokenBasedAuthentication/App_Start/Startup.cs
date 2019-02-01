using System;
using System.Net.Http.Formatting;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.Owin;
using Owin;
using OwinTokenBasedAuthentication.Handlers;
using System.Linq;
using Newtonsoft.Json.Serialization;
using Microsoft.Owin.Security.OAuth;
using OwinTokenBasedAuthentication.Auth;
using Microsoft.Owin.Cors;

[assembly: OwinStartup(typeof(OwinTokenBasedAuthentication.App_Start.Startup))]

namespace OwinTokenBasedAuthentication.App_Start
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
           HttpConfiguration config = new HttpConfiguration();
            config.MapHttpAttributeRoutes();
            config.MessageHandlers.Add(new TimerHandler()); 

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional });

            var jsonFormatter = config.Formatters.OfType<JsonMediaTypeFormatter>().First();
            jsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            OAuthAuthorizationServerOptions OAuthSettings = new OAuthAuthorizationServerOptions
            {
                AllowInsecureHttp = true,
                TokenEndpointPath = new PathString("/token"),
                AccessTokenExpireTimeSpan = TimeSpan.FromMinutes(30),
                Provider = new SimpleAuthorizationServerProvider()
            };

            app.UseOAuthAuthorizationServer(OAuthSettings);
            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());

            //Auth server should be added before UseWebApi for the authorization to work!!!

            app.UseCors(CorsOptions.AllowAll);
            app.UseWebApi(config);


        }
    }
}
