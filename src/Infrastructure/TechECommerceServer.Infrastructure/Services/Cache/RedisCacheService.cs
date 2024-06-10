using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using StackExchange.Redis;
using TechECommerceServer.Application.Abstractions.Cache;

namespace TechECommerceServer.Infrastructure.Services.Cache
{
    public class RedisCacheService : IRedisCacheService
    {
        private readonly ConnectionMultiplexer _redisConnection;
        private readonly IDatabase _database;
        private readonly RedisCacheSettings _redisCacheSettings;
        public RedisCacheService(IOptions<RedisCacheSettings> options)
        {
            _redisCacheSettings = options.Value;
            ConfigurationOptions configuration = ConfigurationOptions.Parse(_redisCacheSettings.ConnectionString);
            _redisConnection = ConnectionMultiplexer.Connect(configuration);
            _database = _redisConnection.GetDatabase();
        }

        public async Task<T> GetAsync<T>(string key)
        {
            RedisValue redisValue = await _database.StringGetAsync(key);
            if (redisValue.HasValue)
                return JsonConvert.DeserializeObject<T>(redisValue);

            return default;
        }

        public async Task SetAsync<T>(string key, T value, DateTime? expirationTime = null)
        {
            TimeSpan timeUntilExpiration = expirationTime.Value - DateTime.Now;
            await _database.StringSetAsync(key, JsonConvert.SerializeObject(value), timeUntilExpiration);
        }
    }
}
