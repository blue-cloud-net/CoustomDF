namespace FantasySky.CustomDF.Caching.StackExchangeRedis;

internal static class RedisExtensions
{
    public static RedisValue[][] HashMemberGetMany(
        this IDatabase cache,
        RedisKey[] keys,
        params string[] members)
    {
        var tasks = new Task<RedisValue[]>[keys.Length];
        var fields = members.Select(member => (RedisValue)member).ToArray();
        var results = new RedisValue[keys.Length][];

        for (var i = 0; i < keys.Length; i++)
        {
            tasks[i] = cache.HashGetAsync((RedisKey)keys[i], fields);
        }

        for (var i = 0; i < tasks.Length; i++)
        {
            results[i] = cache.Wait(tasks[i]);
        }

        return results;
    }

    public static async Task<RedisValue[][]> HashMemberGetManyAsync(
        this IDatabase cache,
        RedisKey[] keys,
        params string[] members)
    {
        var tasks = new Task<RedisValue[]>[keys.Length];
        var fields = members.Select(member => (RedisValue)member).ToArray();

        for (var i = 0; i < keys.Length; i++)
        {
            tasks[i] = cache.HashGetAsync(keys[i], fields);
        }

        return await Task.WhenAll(tasks);
    }

    internal static RedisValue[] HashMemberGet(this IDatabase cache, RedisKey key, params string[] members)
    {
        // TODO: Error checking?
        return cache.HashGet(key, GetRedisMembers(members));
    }

    internal static async Task<RedisValue[]> HashMemberGetAsync(
        this IDatabase cache,
        RedisKey key,
        params string[] members)
    {
        // TODO: Error checking?
        return await cache.HashGetAsync(key, GetRedisMembers(members)).ConfigureAwait(false);
    }

    private static RedisValue[] GetRedisMembers(params string[] members)
    {
        var redisMembers = new RedisValue[members.Length];
        for (int i = 0; i < members.Length; i++)
        {
            redisMembers[i] = (RedisValue)members[i];
        }

        return redisMembers;
    }
}
