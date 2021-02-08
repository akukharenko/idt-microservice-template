using System;
using System.IO;
using System.Text.Json.Serialization;
using IDT.Boss.ServiceName.Api.Infrastructure.Configuration;
using IDT.Boss.ServiceName.Api.Infrastructure.Swagger;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace IDT.Boss.ServiceName.Api.Infrastructure.Extensions
{
    /// <summary>
    /// Extensions for ServicesCollection.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Configure all services and dependencies for Web Api.
        /// </summary>
        /// <param name="services">Services collection with all dependencies.</param>
        /// <param name="configuration">Configuration of the whole application.</param>
        /// <param name="environment">Environment settings.</param>
        /// <param name="httpServices">Is it web server configuration?</param>
        /// <returns>Returns updates service collection after configuration.</returns>
        public static IServiceCollection ConfigureApiService(
            this IServiceCollection services,
            IConfiguration configuration,
            IWebHostEnvironment environment,
            bool httpServices = false)
        {
            // TODO: add more configuration here!

            // add all DI modules
            services.AddExternalServices();

            //-----------------------------------------------
            // Web application specific
            //-----------------------------------------------

            // configure Web Server settings
            if (httpServices)
            {
                services.ConfigureHttpServices(environment, configuration);
            }

            //-----------------------------------------------

            return services;
        }

        private static IServiceCollection AddExternalServices(this IServiceCollection services)
        {
            // TODO: register list of the dependencies here!

            return services;
        }

        /// <summary>
        /// Configure all settings related to HTTP web server.
        /// </summary>
        /// <param name="services">Services collection.</param>
        /// <param name="environment">Environment settings.</param>
        /// <param name="configuration">Configuration of the whole application.</param>
        /// <returns>Returns updates service collection.</returns>
        private static IServiceCollection ConfigureHttpServices(this IServiceCollection services,
            IWebHostEnvironment environment, IConfiguration configuration)
        {
            // add API controllers here
            services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                });

            // add API versions
            services.ConfigureApiVersions();

            // configure Swagger Gen rules to generate API documentation
            services.ConfigureSwaggerGeneration();

            // add health checks (default)
            services.AddHealthChecks();

            return services;
        }

        /// <summary>
        /// Configure settings related to versioning of the API.
        /// </summary>
        /// <param name="services">Services collection.</param>
        /// <returns>Returns updates service collection.</returns>
        private static IServiceCollection ConfigureApiVersions(this IServiceCollection services)
        {
            // add API Version support
            services.AddApiVersioning(options =>
            {
                // reporting API versions will return the headers "api-supported-versions" and "api-deprecated-versions"
                options.ReportApiVersions = true;
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.DefaultApiVersion = new ApiVersion(1, 0);
            });

            // add API Explorer - to work with API versions
            services.AddVersionedApiExplorer(
                options =>
                {
                    // add the versioned API explorer, which also adds IApiVersionDescriptionProvider service
                    // note: the specified format code will format the version as "'v'major[.minor][-status]"
                    options.GroupNameFormat = "'v'VVV";

                    // note: this option is only necessary when versioning by url segment. the SubstitutionFormat
                    // can also be used to control the format of the API version in route templates
                    options.SubstituteApiVersionInUrl = true;
                });

            return services;
        }

        /// <summary>
        /// Configure Swagger Generation for API docs.
        /// </summary>
        /// <param name="services">Services collection.</param>
        /// <returns>Returns updates service collection.</returns>
        private static IServiceCollection ConfigureSwaggerGeneration(this IServiceCollection services)
        {
            services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();

            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(options =>
            {
                // enable annotations for detailed descriptions
                options.EnableAnnotations();

                // add a custom operation filter which sets default values
                options.OperationFilter<SwaggerDefaultValues>();

                // TODO: configure here the list of the XML files with documentation for Swagger!
                // Set the comments path for the Swagger JSON and UI.
                var xmlDocFiles = new[]
                {
                    "IDT.Boss.ServiceName.Api.xml",
                };
                foreach (var xmlDocFile in xmlDocFiles)
                {
                    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlDocFile);
                    options.IncludeXmlComments(xmlPath);
                }
            });

            return services;
        }
    }
}