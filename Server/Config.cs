using IdentityServer4.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server
{
    public class Config
    {
        //Scope 정의
        public static IEnumerable<ApiScope> ApiScopes =>
            new List<ApiScope>
            {
                new ApiScope("api1", "My API")
            };

        //클라이언트 정보 정의
        public static IEnumerable<Client> Clients =>
            new List<Client>
            {
                new Client
                {
                    // 클라이언트 ID
                    ClientId = "client",

                    // 아래방식은 유저등록을 따로 하지 않을때(OpenID) 사용 방식
                    AllowedGrantTypes = GrantTypes.ClientCredentials,

                    // secret for authentication
                    ClientSecrets =
                    {
                        new Secret("secret".Sha256()) //암호/ 암호 저장 방식
                    },

                    // scopes that client has access to
                    AllowedScopes = { "api1" } //해당 Scope
                }
            };
    }
}
