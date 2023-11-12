using FSMS.Service.Services.CacheServices;
using FSMS.WebAPI.Configurations;
using StackExchange.Redis;

namespace FSMS.WebAPI.Installers
{
    public class CacheInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            var redisConfiguration = new RedisConfiguration();
            configuration.GetSection("RedisConfiguration").Bind(redisConfiguration);
            services.AddSingleton(redisConfiguration);

            if (!redisConfiguration.Enable)
            {
                // Optional: Log that Redis is disabled
                return;
            }

            var connectionMultiplexer = CreateConnectionMultiplexer(redisConfiguration.ConnectionString);
            if (connectionMultiplexer == null)
            {
                // Optional: Log that connection multiplexer creation failed
                return;
            }

            services.AddSingleton<IConnectionMultiplexer>(connectionMultiplexer);

            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = $"{redisConfiguration.ConnectionString},abortConnect=false";
                options.InstanceName = "Interactive";
            });

            services.AddSingleton<IResponseCacheService, ResponseCacheService>();
        }

        private IConnectionMultiplexer CreateConnectionMultiplexer(string connectionString)
        {
            try
            {
                var config = ConfigurationOptions.Parse(connectionString);
                config.AbortOnConnectFail = false;
                config.ConnectRetry = 3; // Number of retries
                return ConnectionMultiplexer.Connect(config);
            }
            catch (Exception)
            {
                // Log the exception or handle it as needed
                // Optional: Log exception details for debugging
                return null;
            }
        }
    }
}
