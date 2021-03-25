using AppOptics.Instrumentation;
using HealthChecks.UI.Client;
using IDT.Boss.ServiceName.Api.Infrastructure.Extensions;
using IDT.Boss.ServiceName.Common.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;

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
            app.UseAppopticsApm();

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

                if (healthCheckConfig.HealthCheckUiEnabled)
                {
                    // add Health Check UI
                    endpoints.MapHealthChecksUI(config =>
                    {
                        config.AddCustomStylesheet("wwwroot/styles/healthcheck-style.css");
                        config.UIPath = "/healthcheck-dashboard";
                    });
                }
                
                // add HealthCheck simple endpoint - for AWS Load Balancer and ECS deployment Blue/Green
                endpoints.MapHealthChecks("/healthcheck", new HealthCheckOptions
                {
                    Predicate = (check) => check.Tags.Contains("ready")
                });

                // all health checks here with details
                endpoints.MapHealthChecks("/health", new HealthCheckOptions
                {
                    Predicate = _ => true,
                    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
                });

                // add custom health checks
                // Readiness endpoint
                endpoints.MapHealthChecks("/health/ready", new HealthCheckOptions
                {
                    ResultStatusCodes =
                    {
                        [HealthStatus.Healthy] = StatusCodes.Status200OK,
                        [HealthStatus.Degraded] = StatusCodes.Status500InternalServerError,
                        [HealthStatus.Unhealthy] = StatusCodes.Status503ServiceUnavailable
                    },
                    Predicate = (check) => check.Tags.Contains("ready"),
                    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse,
                    AllowCachingResponses = false
                });

                // Liveness endpoint
                endpoints.MapHealthChecks("/health/live", new HealthCheckOptions
                {
                    Predicate = (check) => !check.Tags.Contains("ready"),
                    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse,
                    AllowCachingResponses = false
                });
            });
        }
    }
}