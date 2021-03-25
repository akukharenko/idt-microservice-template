using IDT.Boss.ServiceName.Common.Configuration;
using Microsoft.Extensions.Configuration;

namespace IDT.Boss.ServiceName.Common.Extensions
{
    public static class ConfigurationExtensions
    {
        public static HealthCheckOptions GetHealthCheckConfiguration(this IConfiguration configuration)
        {
            return configuration.GetSection(nameof(HealthCheckOptions)).Get<HealthCheckOptions>();
        }
    }
}