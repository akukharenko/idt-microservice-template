using System;
using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace IDT.Boss.ServiceName.Api
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(options =>
            {
                // enable annotations for detailed descriptions
                options.EnableAnnotations();

                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "ServiceName API",
                    Version = "v1",
                    Description = "Simple service with API",
                    Contact = new OpenApiContact
                    {
                        Name = "Author",
                        Email = "author.@idt.net"
                    },
                    License = new OpenApiLicense
                    {
                        Name = "Copyright (c) 2021, IDT",
                        Url = new Uri("http://www.idt.net")
                    }
                });

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

            services.AddHealthChecks();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "Service Name V1");

                options.DocumentTitle = "ServiceName API - Swagger UI";

                options.DocExpansion(DocExpansion.None);
            });

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", async context => { await context.Response.WriteAsync("Hello World!"); });

                endpoints.MapControllers();

                endpoints.MapHealthChecks("/healthcheck");
            });
        }
    }
}