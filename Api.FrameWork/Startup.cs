using IdentityServer3.AccessTokenValidation;
using Microsoft.Owin;
using Owin;
using System.IdentityModel.Tokens.Jwt;
using System.Web.Http;

[assembly: OwinStartup(typeof(Api.FrameWork.Startup))]

namespace Api.FrameWork
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {

            JwtSecurityTokenHandler.DefaultInboundClaimFilter.Clear();
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            JwtSecurityTokenHandler.DefaultOutboundClaimTypeMap.Clear();
            app.UseIdentityServerBearerTokenAuthentication(new IdentityServerBearerTokenAuthenticationOptions()
            {
                Authority = "https://localhost:5001",
                //ClientId = "client1",
                //ClientSecret = "secret",
                RequiredScopes = new[] { "api1" }

            });
            // configure web api
            var config = new HttpConfiguration();
            config.MapHttpAttributeRoutes();

            // 인증 방식 등록
            config.Filters.Add(new AuthorizeAttribute());

            app.UseWebApi(config);
        }
    }
}
