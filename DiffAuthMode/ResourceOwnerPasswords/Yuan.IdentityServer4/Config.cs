using IdentityModel;
using IdentityServer4.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Yuan.IdentityServer4
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> IdentityResources =>
            new IdentityResource[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
            };


        public static IEnumerable<ApiScope> ApiScopes =>
            new ApiScope[]
            {
                new ApiScope("password_scope1")
            };

        public static IEnumerable<ApiResource> ApiResources =>
            new ApiResource[]
            {

                new ApiResource("api1","api1")
                {
                    Scopes={ "password_scope1" },
                    UserClaims={JwtClaimTypes.Role},  //添加Cliam 角色类型
                    ApiSecrets={new Secret("apipwd".Sha256())}
                }
            };

        public static IEnumerable<Client> Clients =>
            new Client[]
            {
               

                 new Client
                {
                    ClientId = "password_client",
                    ClientName = "Resource Owner Password",

                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                    ClientSecrets = { new Secret("511536EF-F270-4058-80CA-1C89C192F69A".Sha256()) },
                   
                    AllowedScopes = { "password_scope1" }
                },
            };
    }
}
