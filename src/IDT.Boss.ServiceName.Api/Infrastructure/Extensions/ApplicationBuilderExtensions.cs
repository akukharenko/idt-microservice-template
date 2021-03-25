using IDT.Boss.ServiceName.Api.Infrastructure.Helpers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Serilog;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace IDT.Boss.ServiceName.Api.Infrastructure.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        /// <summary>
        /// Configure Swagger UI for web application with versions support.
        /// </summary>
        /// <param name="app">Application builder.</param>
        /// <param name="provider">Provider for API versions.</param>
        /// <returns>Returns updated object with application builder.</returns>
        public static IApplicationBuilder ConfigureSwagger(this IApplicationBuilder app, IApiVersionDescriptionProvider provider)
        {
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(options =>
            {

                // build a swagger endpoint for each discovered API version
                foreach (var description in provider.ApiVersionDescriptions)
                {
                    var apiName = $"{Constants.ApiName} {description.GroupName.ToUpperInvariant()}";
                    options.SwaggerEndpoint($"{description.GroupName}/swagger.json", apiName);
                }

                options.DocumentTitle = $"{Constants.ApiName} - Swagger UI";

                options.DocExpansion(DocExpansion.None);
            });

            return app;
        }

        /// <summary>
        /// Extends some features of Serilog. Added diagnostic context values.
        /// </summary>
        /// <param name="app">Application builder.</param>
        /// <returns>Returns updated object with application builder.</returns>
        public static IApplicationBuilder ConfigureSerilog(this IApplicationBuilder app)
        {
            app.UseSerilogRequestLogging(options => { options.EnrichDiagnosticContext = LogHelper.EnrichFromRequest; });

            return app;
        }
        
        /// <summary>
        /// Configure application to work after the load balancers and proxies.
        /// </summary>
        /// <param name="app">Application builder.</param>
        /// <returns>Returns updated object with application builder.</returns>
        /// <remarks>
        /// See details here: https://docs.microsoft.com/en-us/aspnet/core/host-and-deploy/proxy-load-balancer?view=aspnetcore-5.0
        /// </remarks>
        public static IApplicationBuilder ConfigureForwarderOptions(this IApplicationBuilder app)
        {
            var forwardedHeadersOptions = new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.All
            };
            forwardedHeadersOptions.KnownProxies.Clear();
            forwardedHeadersOptions.KnownNetworks.Clear();
            app.UseForwardedHeaders(forwardedHeadersOptions);

            return app;
        }
    }
}