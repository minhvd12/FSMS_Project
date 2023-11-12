using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using StackExchange.Redis;

namespace FSMS.Service.Services.CacheServices
{
    public class ResponseCacheService : IResponseCacheService
    {
        private readonly IDistributedCache _distributedCache;
        private readonly IConnectionMultiplexer _connectionMultiplexer;


        public ResponseCacheService(IDistributedCache distributedCache, IConnectionMultiplexer connectionMultiplexer)
        {
            _distributedCache = distributedCache;
            _connectionMultiplexer = connectionMultiplexer;
        }

        public async Task<string> GetCachedResponseAsync(string cacheKey)
        {
            try
            {
                var cachedResponse = await _distributedCache.GetStringAsync(cacheKey);

                if (!string.IsNullOrWhiteSpace(cachedResponse))
                {
                    return cachedResponse;
                }

                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving cached response for key {cacheKey}: {ex.Message}");
                return null;
            }
        }
        //public async Task SetCacheResponseAsync(string cacheKey, object response, TimeSpan timeOut)
        //{
        //    if (response == null)
        //        return;

        //    try
        //    {
        //        var serialierResponse = JsonConvert.SerializeObject(response, new JsonSerializerSettings()
        //        {
        //            ContractResolver = new CamelCasePropertyNamesContractResolver()
        //        });

        //        await _distributedCache.SetStringAsync(cacheKey, serialierResponse, new DistributedCacheEntryOptions
        //        {
        //            AbsoluteExpirationRelativeToNow = timeOut
        //        });
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine($"Lỗi chuyển đổi đối tượng thành chuỗi JSON cho khóa cache {cacheKey}: {ex.Message}");
        //    }
        //}
        public async Task SetCacheResponseAsync(string cacheKey, object response, TimeSpan timeOut)
        {
            if (response == null)
                return;

            ConnectionMultiplexer connectionMultiplexer = null;

            try
            {
                var serialierResponse = JsonConvert.SerializeObject(response, new JsonSerializerSettings()
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                });

                var redisOptions = new ConfigurationOptions
                {
                    EndPoints = { "redis-14785.c295.ap-southeast-1-1.ec2.cloud.redislabs.com:14785" },
                    Password = "zoGWOT1FaVSKc8OaGuBFJxAOmJebzyJj",
                    ConnectTimeout = 10000,
                    SyncTimeout = 5000
                };

                connectionMultiplexer = ConnectionMultiplexer.Connect(redisOptions);

                var database = connectionMultiplexer.GetDatabase();

                await database.StringSetAsync(cacheKey, serialierResponse, timeOut);
            }
            catch (TimeoutException tex)
            {
                Console.WriteLine($"Timeout connecting to Redis: {tex.Message}");
                // Handle the timeout exception as needed (e.g., log, retry, etc.)
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error converting object to JSON and caching with key {cacheKey}: {ex.Message}");
            }
            finally
            {
                // Ensure proper disposal of resources
                connectionMultiplexer?.Dispose();
            }
        }






    }
}