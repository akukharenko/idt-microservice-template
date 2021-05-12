using AppOptics.Instrumentation;
using IDT.Boss.Extensions.AppOptics.Middleware;
using Microsoft.AspNetCore.Builder;

namespace IDT.Boss.Extensions.AppOptics.Extensions
{
    /// <summary>
    /// Extensions for Application builder with AppOptics monitoring.
    /// </summary>
    public static class ApplicationBuilderExtensions
    {
        /// <summary>
        /// Add AppOptics usage in the web application.
        /// </summary>
        /// <param name="builder">Application middleware builder.</param>
        /// <returns>Returns updated builder.</returns>
        public static IApplicationBuilder UseAppOptics(this IApplicationBuilder builder)
        {
            // add default middleware for AppOptics
            builder.UseAppopticsApm();
            
            // add additional custom middleware from the application
            builder.UseAppOpticsScope();

            return builder;
        }

        private static IApplicationBuilder UseAppOpticsScope(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<AppOpticsScopeMiddleware>();
        }
    }
}