using Microsoft.Owin.Security.OAuth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

namespace OwinTokenBasedAuthentication.Auth
{
    public class SimpleAuthorizationServerProvider : OAuthAuthorizationServerProvider
    {
        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated();
        }
        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { "*" });

            if ( context.UserName != "rahul" || context.Password != "test1234")
            {
                context.SetError("Invalid_grant", "The username or password is invalid"); 
            }


            var identity = new ClaimsIdentity(context.Options.AuthenticationType);
            identity.AddClaim(new Claim("user", context.UserName));
            identity.AddClaim(new Claim("id", "1"));
            identity.AddClaim(new Claim("email", "rahul@abc.com"));
            identity.AddClaim(new Claim("role", "admin"));
            context.Validated(identity);
        }

    }
}