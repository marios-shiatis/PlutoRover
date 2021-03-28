using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Converters;
using PlumGuide.PlutoRover.Web.Interface;
using PlumGuide.PlutoRover.Web.Models;
using PlumGuide.PlutoRover.Web.Services;
using System.Collections.Generic;

namespace PlumGuide.PlutoRover
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

            services.AddControllers();
            services.AddMvc().AddNewtonsoftJson(opts =>
            {
                opts.SerializerSettings.Converters.Add(new StringEnumConverter());
            });

            services.AddSwaggerGen(c =>
            {
                c.EnableAnnotations();
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "PlumGuide.PlutoRover", Version = "v1" });
            });
            
            var obstacles = new List<Obstacle>() {
                new Obstacle(){X = 10, Y = 24 },
                new Obstacle(){X = 33, Y = 0 },
            };
            
            services.AddSingleton(new Planet()
            {
                Name = "Pluto",
                Obstacles = obstacles,
                GridAreaSize = new GridAreaSize() { X = 100, Y = 100 }
            });

            services.AddSingleton(new RoverPosition() { X = 0, Y = 0, Direction = CompassDirections.North });
            services.AddScoped<INavigateService, NavigateService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "PlumGuide.PlutoRover v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
