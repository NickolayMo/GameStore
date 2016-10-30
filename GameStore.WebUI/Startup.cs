using GameStore.WebUI.Ifrastructure.Binders;
using GameStore.WebUI.Models.Abstract;
using GameStore.WebUI.Models.Concrete;
using GameStore.WebUI.Models.Entities;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Hosting;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNet.Authentication;
using Microsoft.AspNet.Authentication.Cookies;
using Microsoft.AspNet.Http;
using System;

using Microsoft.Extensions.Logging;



//using Microsoft.Extensions.DependencyInjection;

namespace GameStore.WebUI
{
    public class Startup
    {
        public static IConfigurationRoot Configuration;

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);
            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
          
            services.AddMvc(
                options=>options.ModelBinders.Insert(0,new CartModelBinder())
                );
            services.AddCaching();
            services.AddSession();
            services.AddLogging();
            services.AddIdentity<AdminUser, IdentityRole>(
                config =>
                {
                    config.Password.RequiredLength = 2;
                }
                ).AddEntityFrameworkStores<EfDbContext>();
          
          
            services.AddScoped<IGameRepository, EfGameRepository>();
            services.AddScoped<IOrderProcessor, EmailOrderProcessor>();
            services.AddTransient<GameStoreSeedData>();
            services.AddEntityFramework()
                .AddSqlServer()
                .AddDbContext<EfDbContext>();
            /*
        services.AddEntityFramework()
           .AddSqlServer()
           .AddDbContext<ApplicationDbContext>(options =>
               options.UseSqlServer(Configuration["Data:DefaultConnection:ConnectionString"]));
               */

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public async void Configure(IApplicationBuilder app, GameStoreSeedData seeder, ILoggerFactory loggerFactory)
        {

            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();
            app.UseBrowserLink();
            app.UseDeveloperExceptionPage();
            app.UseDatabaseErrorPage();
            app.UseStaticFiles();

            

            app.UseIdentity();
            app.UseSession();
            app.UseCookieAuthentication(options =>
            {
                options.AuthenticationScheme = "Cookies";
                options.LoginPath = new PathString("/Auth/Login");
               options.AutomaticChallenge = true;
                
            });
            app.UseMvc(
                routes =>
                {
                    routes.MapRoute(
                        name: "Start",
                        template: "",
                        defaults: new
                        {
                            controller = "Game",
                            action = "List",
                            category = (string)null,
                            page =1
                        }
                        );
                    routes.MapRoute(
                        name: "Page",
                        template: "Page{page}",
                        defaults: new
                        {
                            controller = "Game",
                            action = "List",
                            category = (string)null,
                           
                        },
                        constraints: new { page = @"\d+" }
                        );
                    routes.MapRoute(
                      name: "Category",
                      template: "{category}",
                      defaults: new
                      {
                          controller = "Game",
                          action = "List",
                          page = 1
                      }
                      );
                    routes.MapRoute(
                     name: "Full",
                     template: "{category}/Page{page}",
                     defaults: new
                     {
                         controller = "Game",
                         action = "List",
                      },
                     constraints: new {page=@"\d+" }
                     );
                    routes.MapRoute(
                        name: "default",
                        template: "{controller}/{action}/{id?}",
                        defaults: new { controller = "Game", action = "List"}

                        );
                    
                });
            
           
            await seeder.EnsureSeedDataAsync();
           
            
          

        }

        // Entry point for the application.
        public static void Main(string[] args) => WebApplication.Run<Startup>(args);
    }
}
