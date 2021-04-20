﻿using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace IDT.Boss.ServiceName.Api.Infrastructure.HealthCheck
{
    /// <summary>
    /// Extension to add additional custom health checks.
    /// </summary>
    public static class HealthCheckExtensions
    {
        public static IHealthChecksBuilder AddMemoryHealthCheck(this IHealthChecksBuilder builder, HealthStatus? failureStatus = null,
            IEnumerable<string> tags = default, long? thresholdInBytes = null)
        {
            // Register a check of type GCInfo.
            builder.AddCheck<MemoryHealthCheck>(MemoryHealthCheck.Name, failureStatus ?? HealthStatus.Degraded, tags);

            // Configure named options to pass the threshold into the check.
            if (thresholdInBytes.HasValue)
            {
                builder.Services.Configure<MemoryCheckOptions>(MemoryHealthCheck.Name, options => { options.Threshold = thresholdInBytes.Value; });
            }

            return builder;
        }
    }
}