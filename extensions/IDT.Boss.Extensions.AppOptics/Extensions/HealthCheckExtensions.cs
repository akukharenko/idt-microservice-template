using System.Collections.Generic;
using IDT.Boss.Extensions.AppOptics.HealthCheck;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace IDT.Boss.Extensions.AppOptics.Extensions
{
    public static class HealthCheckExtensions
    {
        public static IHealthChecksBuilder AddAppOptics(this IHealthChecksBuilder builder, HealthStatus failureStatus,
            IEnumerable<string> tags = default)
        {
            return builder.Add(new HealthCheckRegistration(AppOpticsHealthCheck.Name,
                new AppOpticsHealthCheck(), failureStatus, tags));
        }
    }
}