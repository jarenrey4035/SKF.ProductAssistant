using StackExchange.Redis;

namespace SKF.ProductAssistant.Services
{
    public class RedisService
    {
        private readonly IDatabase _db;

        public RedisService()
        {
            var connection = ConnectionMultiplexer.Connect(
                Environment.GetEnvironmentVariable("REDIS_CONNECTION"));

            _db = connection.GetDatabase();
        }

        public async Task<string> GetAsync(string key)
        {
            return await _db.StringGetAsync(key);
        }

        public async Task SetAsync(string key, string value)
        {
            await _db.StringSetAsync(key, value);
        }
    }
}