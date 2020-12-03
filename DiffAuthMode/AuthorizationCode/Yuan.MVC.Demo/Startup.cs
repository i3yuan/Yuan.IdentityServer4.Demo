using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Yuan.MVC.Demo.Extendsions;

namespace Yuan.MVC.Demo
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();

            //
            #region MVC client
            //关闭了 JWT 身份信息类型映射
            //这样就允许 well-known 身份信息（比如，“sub” 和 “idp”）无干扰地流过。
            //这个身份信息类型映射的“清理”必须在调用 AddAuthentication()之前完成
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            services.AddAuthentication(options =>
            {
                options.DefaultScheme = "Cookies";
                options.DefaultChallengeScheme = "oidc";
            })
               .AddCookie("Cookies")

              .AddOpenIdConnect("oidc", options =>
              {
                  options.Authority = "http://localhost:5001";
                  options.RequireHttpsMetadata = false;
                  options.ClientId = "code_client";
                  options.ClientSecret = "511536EF-F270-4058-80CA-1C89C192F69A";
                  options.ResponseType = "code";
                  options.Scope.Add("code_scope1"); //添加授权资源
                  options.SaveTokens = true;
                  options.GetClaimsFromUserInfoEndpoint = true;

              });

            #endregion
            services.ConfigureNonBreakingSameSiteCookies();
        }
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();

            app.UseRouting();

            app.UseCookiePolicy();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
