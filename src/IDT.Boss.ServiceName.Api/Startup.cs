using AppOptics.Instrumentation;
using IDT.Boss.ServiceName.Api.Infrastructure.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace IDT.Boss.ServiceName.Api
{
    public class Startup
    {
        /// <summary>
        /// Configuration for application loaded from the files.
        /// </summary>
        private IConfiguration Configuration { get; }

        /// <summary>
        /// Environment settings and configuration.
        /// </summary>
        private IWebHostEnvironment Environment { get; }

        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            Configuration = configuration;
            Environment = environment;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.ConfigureApiService(Configuration, Environment, true);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IApiVersionDescriptionProvider apiProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // add SolarWinds AppOptics monitoring
            app.UseAppopticsApm();

            // configure Swagger UI
            app.ConfigureSwagger(apiProvider);

            // add logger for all requests in the web server
            app.ConfigureSerilog();

            // Use routing middleware to handle requests to the controllers
            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                // create an empty endpoint for home page request
                endpoints.MapGet("/", async context => { await context.Response.WriteAsync("Hello World!"); });

                // add controllers endpoints
                endpoints.MapControllers();

                // add HealthCheck simple enpoint
                endpoints.MapHealthChecks("/healthcheck");
            });
        }
    }
}