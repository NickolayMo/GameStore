using GameStore.WebUI.Ifrastructure.Binders;
using GameStore.WebUI.Models.Abstract;
using GameStore.WebUI.Models.Concrete;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Diagnostics;
using Microsoft.AspNet.Hosting;
using Microsoft.Data.Entity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;



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
           
            services.AddScoped<IGameRepository, EfGameRepository>();
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
        public void Configure(IApplicationBuilder app)
        {
            app.UseDeveloperExceptionPage();
            app.UseStaticFiles();
            app.UseSession();
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

        }

        // Entry point for the application.
        public static void Main(string[] args) => WebApplication.Run<Startup>(args);
    }
}
