using Microsoft.AspNetCore.OutputCaching;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace OutputCaching.Sample.ExternalStore
{
    public static class RedisExtension
    {
        public static IServiceCollection AddRedisOutputCache(this IServiceCollection services)
        {
            ArgumentNullException.ThrowIfNull(services);

            // Add required services        
            services.AddOutputCache();

            // Remove default IOutputCacheStore
            services.RemoveAll<IOutputCacheStore>();

            // Add custom IOutputCacheStore
            services.AddSingleton<IOutputCacheStore, RedisOutputCacheStore>();

            return services;
        }
    }
    
}
