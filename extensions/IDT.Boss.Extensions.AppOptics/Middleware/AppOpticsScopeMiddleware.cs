using System.Threading.Tasks;
using AppOptics.Instrumentation;
using IDT.Boss.ServiceName.Common.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace IDT.Boss.Extensions.AppOptics.Middleware
{
    /// <summary>
    /// Custom middleware for AppOptics tracing requests.
    /// </summary>
    public sealed class AppOpticsScopeMiddleware
    {
        private readonly ILogger<AppOpticsScopeMiddleware> _logger;
        private readonly RequestDelegate _next;

        public AppOpticsScopeMiddleware(RequestDelegate next, ILogger<AppOpticsScopeMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            if (Trace.IsAgentAvailable())
            {
                using (_logger.BeginPropertyScope(("AppOpticsTraceId", Trace.GetCurrentLogTraceId())))
                {
                    await _next(httpContext);
                }
            }
            else
            {
                await _next(httpContext);
            }
        }
    }
}