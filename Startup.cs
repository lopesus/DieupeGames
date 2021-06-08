using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DieupeGames.Models;
using DieupeGames.Models.LiteDb;
using LabirunServer.Services;
using LabirunServer.Services.CustomExceptionMiddleware;
using learnCore;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Routing;
using MongoDB.Driver;

namespace DieupeGames
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
            // configure strongly typed settings objects
            var appSettingsSection = Configuration.GetSection(nameof(AppSettings));
            services.Configure<AppSettings>(appSettingsSection);
            var appSettings = appSettingsSection.Get<AppSettings>();

            services.AddSingleton<MongoDBContext>();

            var db = new MongoClient(appSettings.MongoServer)
                .GetDatabase(appSettings.MongoDatabase);


            //services.Configure<LiteDbOptions>(Configuration.GetSection("LiteDbOptions"));
            //services.AddSingleton<ILiteDbContext, LiteDbContext>();
            //services.AddTransient<ILiteDbWordBoxService, LiteDbWordBoxService>();

            //services.AddRouting(options => options.LowercaseUrls = true);
            services.Configure<RouteOptions>(options => options.LowercaseUrls = true);
            services.AddRazorPages();
            services.AddControllers();
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
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.ConfigureCustomExceptionMiddleware();
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapRazorPages();
            });
        }
    }
}
