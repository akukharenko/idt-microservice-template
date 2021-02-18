using System;
using AppOptics.Instrumentation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace IDT.Boss.ServiceName.Api.Infrastructure.HealthCheck
{
    public static class AppOpticsHealthCheckExtensions
    {
        private const string LicenseKeyEnvName = "APPOPTICS_SERVICE_KEY";
        private const string HealthCheckName = "appOptics";

        private static bool HasLicenceKey => !string.IsNullOrEmpty(Environment.GetEnvironmentVariable(LicenseKeyEnvName));

        public static IHealthChecksBuilder AddAppOptics(this IHealthChecksBuilder builder)
        {
            return builder.AddCheck(HealthCheckName, () =>
            {
                if (!Trace.IsAgentAvailable())
                {
                    return HealthCheckResult.Unhealthy("AppOptics agent is unavailable");
                }

                return HasLicenceKey
                    ? HealthCheckResult.Healthy()
                    : HealthCheckResult.Unhealthy($"{LicenseKeyEnvName} env variable is not provided");
            });
        }
    }
}