using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Yuan.IdentityServer4.EF.Extendsions;

namespace Yuan.IdentityServer4.EF
{
    public class Startup
    {
        //const string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;
        //                                 Initial Catalog=Yuan.Idp;trusted_connection=yes;";
        public IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration; 
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            var connectionString = Configuration.GetConnectionString("DataContext");
            if (connectionString == "")
            {
                throw new Exception("数据库配置异常");
            }
            var migrationsAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;
            // in DB  config
            var builder = services.AddIdentityServer(options =>
            {
                options.Events.RaiseErrorEvents = true;
                options.Events.RaiseInformationEvents = true;
                options.Events.RaiseFailureEvents = true;
                options.Events.RaiseSuccessEvents = true;
            }).AddConfigurationStore(options => //添加配置数据（ConfigurationDbContext上下文用户配置数据）
            {
                options.ConfigureDbContext = builder => builder.UseSqlServer(connectionString, sql => sql.MigrationsAssembly(migrationsAssembly));
            }).AddOperationalStore(options =>   //添加操作数据（PersistedGrantDbContext上下文 临时数据（如授权和刷新令牌））
            {
                options.ConfigureDbContext = builder => builder.UseSqlServer(connectionString, sql => sql.MigrationsAssembly(migrationsAssembly));
                // 自动清理 token ，可选
                options.EnableTokenCleanup = true;
                // 自动清理 token ，可选
                options.TokenCleanupInterval = 30;
            }).AddTestUsers(TestUsers.Users);
            // not recommended for production - you need to store your key material somewhere secure
            builder.AddDeveloperSigningCredential();

            services.ConfigureNonBreakingSameSiteCookies();
        }
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();
            app.UseRouting();
            app.UseCookiePolicy();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseIdentityServer();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}
