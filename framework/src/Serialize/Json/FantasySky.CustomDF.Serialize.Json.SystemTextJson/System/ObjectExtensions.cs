using FantasySky.CustomDF.Serialize.Json;
using FantasySky.CustomDF.Serialize.Json.SystemTextJson;

namespace System;

/// <summary>
/// object对象扩展方法
/// </summary>
public static class ObjectExtensions
{
    public static string ToJson(this object obj, JsonOptions? jsonOptions = null)
    {
        return DefaultJsonSerializer.Instance.Serialize(obj);
    }

    public static string ToJson(this object obj, IJsonSerializer jsonSerializer)
    {
        return jsonSerializer.Serialize(obj);
    }
}
