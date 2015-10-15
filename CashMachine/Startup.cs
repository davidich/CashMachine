using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(CashMachine.Startup))]
namespace CashMachine
{
    using System;
    using System.Web.Http;
    using System.Web.Http.Dependencies;

    using CashMachine.DependencyResolution;
    using CashMachine.Providers;

    using Microsoft.Owin.Security.OAuth;

    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureIoC(app);
            ConfigureAuth(app);

            //HttpConfiguration config = new HttpConfiguration();
            //WebApiConfig.Register(config);
            app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);
            //app.UseWebApi(config);
        }

        public void ConfigureIoC(IAppBuilder app)
        {
            var container = StructuremapMvc.StructureMapDependencyScope.Container;
            GlobalConfiguration.Configuration.DependencyResolver = new StructureMapWebApiDependencyResolver(container);
        }

        // For more information on configuring authentication, please visit http://go.microsoft.com/fwlink/?LinkId=301864
        public void ConfigureAuth(IAppBuilder app)
        {
            // Enable the application to use a OAuthBearerToken to store information for the signed in user
            IDependencyResolver dependencyResolver = GlobalConfiguration.Configuration.DependencyResolver;

            // Token Generation
            app.UseOAuthAuthorizationServer(new OAuthAuthorizationServerOptions
            {
                AllowInsecureHttp = false,
                TokenEndpointPath = new PathString("/validate-pin"),
                AccessTokenExpireTimeSpan = TimeSpan.FromMinutes(30),
                Provider = new SimpleAuthorizationServerProvider(dependencyResolver)
            });

            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());
        }
    }
}
