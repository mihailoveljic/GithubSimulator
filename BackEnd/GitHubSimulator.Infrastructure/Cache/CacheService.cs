using GitHubSimulator.Core.Interfaces;
using GitHubSimulator.Infrastructure.Configuration;
using Microsoft.Extensions.Options;
using StackExchange.Redis;
using System.Text.Json;

namespace GitHubSimulator.Infrastructure.Cache;

public class CacheService : ICacheService
{
    private readonly IDatabase _cacheDb;

    public CacheService(IOptions<RedisSettings> redisSettings)
    {
        var configurationOptions = new ConfigurationOptions
        {
            AbortOnConnectFail = false,
            EndPoints = { $"{redisSettings.Value.URL}" },
            Password = redisSettings.Value.Password,
            // Add other configuration options as needed
        };
        var redis = ConnectionMultiplexer.Connect(configurationOptions);
        _cacheDb = redis.GetDatabase();
    }
    public T GetData<T>(string key)
    {
        var value = _cacheDb.StringGet(key);
        if(!string.IsNullOrEmpty(value))
        {
            return JsonSerializer.Deserialize<T>(value);
        }
        return default;
    }

    public object RemoveData(string key)
    {
        var exist = _cacheDb.KeyExists(key);

        if(exist)
        {
            return _cacheDb.KeyDelete(key);
        }
        return false;
    }

    public bool SetData<T>(string key, T value, DateTimeOffset expirationTime)
    {
        var expTime = expirationTime.DateTime.Subtract(DateTime.Now);
        return _cacheDb.StringSet(key, JsonSerializer.Serialize(value), expTime);
    }
}
