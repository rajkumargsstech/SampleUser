using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using UserRegistrationForm.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace UserRegistrationForm
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
            services.AddMvc().SetCompatibilityVersion(Microsoft.AspNetCore.Mvc.CompatibilityVersion.Version_3_0);
            var connection = Configuration.GetConnectionString("UserRegistrationDB");
            services.AddDbContext<UserRegistration>(options=> options.UseSqlServer(connection));
            services.AddControllersWithViews();
            services.AddMvc(options => options.EnableEndpointRouting = false);//to desable the UseEndpoints for default route
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie();//Authentication And Authorization In ASP.NET Core MVC Using Cookie

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
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseAuthentication();
            app.UseRouting(
                
                
                );
            
            app.UseAuthorization();


           
            //Conventional Rounting
            app.UseMvc(routes =>
            {
               routes.MapRoute(
               name: "ListofUsers",
               template: "ListofUsers",
               defaults: new { Controller = "Users" ,action= "Index" }
               );

              routes.MapRoute(
              name: "GetUser",
              template: "GetUser/{id:int}",
              defaults: new { Controller = "Users", action = "Details" }
              );


                routes.MapRoute(
                name: "default",
                template: "{controller=Home}/{action=Index}/{id?}");

            });

            //Default routing
            //app.UseEndpoints(endpoints =>
            //{
            //    endpoints.MapControllerRoute(
            //        name: "default",
            //        pattern: "{controller=Home}/{action=Index}/{id?}");

            //});

        }
    }
}
