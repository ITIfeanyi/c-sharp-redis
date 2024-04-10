using System.Text.Json;
using redisAPI.Models;
using StackExchange.Redis;

namespace redisAPI.Data;

public class RedisPlatformRepo : IPlatformRepo
{
    private readonly IConnectionMultiplexer _redis;
    
    public RedisPlatformRepo(IConnectionMultiplexer redis)
    {
        _redis = redis;
    }
    public void CreatePlatform(Platform platform)
    {
        if (platform == null)
        {
            throw new ArgumentOutOfRangeException(nameof(platform));
        }

        var db = _redis.GetDatabase();
        var serializePlat = JsonSerializer.Serialize(platform);
      //  db.StringSet(platform.Id, serializePlat);
       // db.SetAdd("PlatformSet", serializePlat);
       db.HashSet("hashplatform", new HashEntry[]
           { new HashEntry(platform.Id, serializePlat) }
       );


    }

    public Platform? GetPlatformById(string id)
    {
        var db = _redis.GetDatabase();
        // var platform = db.StringGet(id);

        var platform = db.HashGet("hashplatform", id);
        if (platform.IsNullOrEmpty) return null;

        return JsonSerializer.Deserialize<Platform>(platform);
    }

    public IEnumerable<Platform?>? GetAllPlatforms()
    {
        var db = _redis.GetDatabase();
        //    var completeSet = db.SetMembers("PlatformSet");
        var completeHash = db.HashGetAll("hashplatform");
        if (completeHash.Length > 0)
        {
            // return Array.ConvertAll(completeHash, val => JsonSerializer.Deserialize<Platform>(val)).ToList();
            return Array.ConvertAll(completeHash, val => JsonSerializer.Deserialize<Platform>(val.Value)).ToList();
        }

        return null;
    }
}