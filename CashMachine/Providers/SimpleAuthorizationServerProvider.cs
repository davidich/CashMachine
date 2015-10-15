namespace CashMachine.Providers
{
    using System.Globalization;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using System.Web.Http.Dependencies;

    using CashMachine.Domain.Entities;
    using CashMachine.Domain.Managers;

    using Microsoft.AspNet.Identity.Owin;
    using Microsoft.Owin.Security.OAuth;

    public class SimpleAuthorizationServerProvider : OAuthAuthorizationServerProvider
    {
        private readonly IDependencyResolver _resolver;

        public SimpleAuthorizationServerProvider(IDependencyResolver resolver)
        {
            _resolver = resolver;
        }

        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated();
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { "*" });

            using (var signInManager = (SignInManager)_resolver.GetService(typeof(SignInManager)))
            {
                SignInStatus status = await signInManager.PasswordSignInAsync(context.UserName, context.Password, false, true);

                switch (status)
                {
                    case SignInStatus.Success:              // returns {"access_token": "...", "token_type": "bearer", "expires_in": 86399 }
                        Card card = await signInManager.UserManager.FindByNameAsync(context.UserName);
                        var identity = new CardIdentity(card, context.Options.AuthenticationType);
                        context.Validated(identity);
                        break;
                    case SignInStatus.LockedOut:            // returns {"error": "locked_card"}
                        context.SetError("locked_card");
                        break;
                    default:                                // returns {"error": "invalid_pin"}
                        context.SetError("invalid_pin");
                        break;
                }
            }
        }
    }
}