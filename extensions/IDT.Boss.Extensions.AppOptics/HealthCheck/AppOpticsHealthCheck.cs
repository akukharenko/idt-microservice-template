using System;
using System.Threading;
using System.Threading.Tasks;
using AppOptics.Instrumentation;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace IDT.Boss.Extensions.AppOptics.HealthCheck
{
    /// <summary>
    /// Custom health check for the AppOptics agent.
    /// </summary>
    public sealed class AppOpticsHealthCheck : IHealthCheck
    {
        private const string LicenseKeyEnvName = "APPOPTICS_SERVICE_KEY";
        public const string Name = "appOptics";

        private static bool HasLicenceKey => !string.IsNullOrEmpty(Environment.GetEnvironmentVariable(LicenseKeyEnvName));

        public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = new CancellationToken())
        {
            if (!Trace.IsAgentAvailable())
            {
                return Task.FromResult(HealthCheckResult.Unhealthy("AppOptics agent is unavailable"));
            }

            return HasLicenceKey
                ? Task.FromResult(HealthCheckResult.Healthy())
                : Task.FromResult(HealthCheckResult.Unhealthy($"{LicenseKeyEnvName} env variable is not provided"));
        }
    }
}