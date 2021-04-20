using Hellang.Middleware.ProblemDetails;
using IDT.Boss.Extensions.AppOptics.Extensions;
using IDT.Boss.ServiceName.Api.Infrastructure.Extensions;
using IDT.Boss.ServiceName.Common.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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
            var healthCheckConfig = Configuration.GetHealthCheckConfiguration();
            
            // configure Forwarder headers for proxies and Load Balancers
            app.ConfigureForwarderOptions();
            
            // Use HSTS default settings (default for 30 days)
            app.UseHsts();
            
            // redirect to the HTTPS connection
            // app.UseHttpsRedirection();
            
            // add SolarWinds AppOptics monitoring
            app.UseAppOptics();
            
            // Add using ProblemDetail middleware to handle errors and use RFC-7807 standard
            app.UseProblemDetails();

            // configure Swagger UI
            app.ConfigureSwagger(apiProvider);

            // add logger for all requests in the web server
            app.ConfigureSerilog();

            // use default files
            app.UseDefaultFiles();
            
            // allow to use static files
            app.UseStaticFiles();
            
            // Use routing middleware to handle requests to the controllers
            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                // add controllers endpoints
                endpoints.MapControllers();
                
                // add health checks endpoints and configurations
                endpoints.AddHealthcheckEndpoints(healthCheckConfig);
            });
        }
    }
}