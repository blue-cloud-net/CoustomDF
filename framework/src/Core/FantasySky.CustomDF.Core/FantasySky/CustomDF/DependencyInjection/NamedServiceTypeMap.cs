using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;

namespace FantasySky.CustomDF.DependencyInjection;

public class NamedServiceTypeMap : ConcurrentDictionary<string, Type>, INamedServiceTypeMap
{
    public bool AddNamedService<InterfaceT, ImplT>(string name)
        => this.AddNamedService(typeof(InterfaceT), typeof(ImplT), name);

    public bool AddNamedService(Type interfaceType, Type implType, string name)
    {
        Check.NotNullOrWhiteSpace(name, nameof(name));

        var key = this.NormalizeKey(interfaceType, name);

        return this.TryAdd(key, implType);
    }

    public bool TryGetType<InterfaceT>(string name, [MaybeNullWhen(false)] out Type type)
        => this.TryGetType(typeof(InterfaceT), name, out type);

    public bool TryGetType(Type interfaceType, string name, [MaybeNullWhen(false)] out Type type)
    {
        var key = this.NormalizeKey(interfaceType, name);

        return this.TryGetValue(key, out type);
    }

    private string NormalizeKey(Type interfaceType, string name)
        => $"{interfaceType.Name}_{name}";
}

public interface INamedServiceTypeMap
{
    bool AddNamedService<InterfaceT, ImplT>(string name);

    bool AddNamedService(Type interfaceType, Type implType, string name);

    bool TryGetType<InterfaceT>(string name, [MaybeNullWhen(false)] out Type type);

    bool TryGetType(Type interfaceType, string name, [MaybeNullWhen(false)] out Type type);
}
