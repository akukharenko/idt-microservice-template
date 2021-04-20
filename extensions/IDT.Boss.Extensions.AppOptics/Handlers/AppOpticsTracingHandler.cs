using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using AppOptics.Instrumentation;
using Microsoft.Extensions.Logging;

namespace IDT.Boss.Extensions.AppOptics.Handlers
{
    /// <summary>
    /// Custom handler to trace the external services calls with HttpClient from the application.
    /// </summary>
    public sealed class AppOpticsTracingHandler : DelegatingHandler
    {
        private readonly ILogger<AppOpticsTracingHandler> _logger;

        public AppOpticsTracingHandler(ILogger<AppOpticsTracingHandler> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Wrapper of <see cref="HttpClientHandler.SendAsync"/> method.
        /// It collects information from request and response. Also, a trace header will be injected 
        /// into the HttpWebRequest to propagate the tracing to downstream web service. 
        /// </summary>
        /// <param name="request">An instance of <see cref="HttpResponseMessage"/></param>
        /// <param name="cancellationToken">An instance of <see cref="CancellationToken"/></param>
        /// <returns>A Task of <see cref="HttpResponseMessage"/> representing the asynchronous operation</returns>
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            Tracer tracer = null;
            try
            {
                tracer = Tracer.TraceMethod(request.RequestUri.Host,
                    new Dictionary<string, object>()
                    {
                        {"action", "HttpClient request"},
                        {"method", request.Method},
                        {"request_uri", $"{request.RequestUri.Scheme}://{request.RequestUri.Authority}{request.RequestUri.AbsolutePath}"},
                        {"headers", request.Headers},
                        {"properties", JsonSerializer.Serialize(request.Properties)}
                    });

                request.Headers.Add("X-Trace", Trace.GetCurrentTraceId());

                _logger.LogInformation($"Tracing request to the {request.RequestUri.Host} with traceId = {Trace.GetCurrentTraceId()}");

                return await base.SendAsync(request, cancellationToken);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Error handled in the AppOpticsTracingHandler!");
                Trace.ReportException(exception);
                throw;
            }
            finally
            {
                tracer?.Dispose();
            }
        }
    }
}