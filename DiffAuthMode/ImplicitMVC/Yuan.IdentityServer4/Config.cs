using IdentityModel;
using IdentityServer4;
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
                new ApiScope("Implicit_scope1")
            };

        public static IEnumerable<ApiResource> ApiResources =>
            new ApiResource[]
            {

                new ApiResource("api1","api1")
                {
                    Scopes={ "Implicit_scope1" },
                    UserClaims={JwtClaimTypes.Role},  //添加Cliam 角色类型
                    ApiSecrets={new Secret("apipwd".Sha256())}
                }
            };

        public static IEnumerable<Client> Clients =>
            new Client[]
            {


                 new Client
                {
                    ClientId = "Implicit_client",
                    ClientName = "Implicit Auth",

                    AllowedGrantTypes = GrantTypes.Implicit,

                    RedirectUris ={
                    "http://localhost:5002/signin-oidc", //跳转登录到的客户端的地址
                    },
                    // RedirectUris = {"http://localhost:5002/auth.html" }, //跳转登出到的客户端的地址
                    PostLogoutRedirectUris ={
                        "http://localhost:5002/signout-callback-oidc",
                    },
                    //ClientSecrets = { new Secret("511536EF-F270-4058-80CA-1C89C192F69A".Sha256()) },
                   
                    AllowedScopes = {
                           IdentityServerConstants.StandardScopes.OpenId,
                            IdentityServerConstants.StandardScopes.Profile,
                         "Implicit_scope1"
                     },
                      //允许将token通过浏览器传递
                     AllowAccessTokensViaBrowser=true,
                     // 是否需要同意授权 （默认是false）
                      RequireConsent=true
                 }, 
                //  new Client {
                //    ClientId = "blogvuejs",
                //    ClientName = "Blog.Vue JavaScript Client",
                //    AllowedGrantTypes = GrantTypes.Implicit,
                //    AllowAccessTokensViaBrowser = true,

                //    RedirectUris =           {
                //        "http://vueblog.neters.club/callback",
                //        "http://apk.neters.club/oauth2-redirect.html"
                //    },
                //    PostLogoutRedirectUris = { "http://vueblog.neters.club" },
                //    AllowedCorsOrigins =     { "http://vueblog.neters.club" },

                //    AllowedScopes = {
                //        IdentityServerConstants.StandardScopes.OpenId,
                //        IdentityServerConstants.StandardScopes.Profile,
                //        "roles",
                //        "blog.core.api"
                //    }
                //},
            };
    }
}
