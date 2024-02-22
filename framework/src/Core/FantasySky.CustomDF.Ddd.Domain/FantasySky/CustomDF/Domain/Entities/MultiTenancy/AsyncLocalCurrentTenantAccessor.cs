namespace FantasySky.CustomDF.Domain.Entities.MultiTenancy;

public class AsyncLocalCurrentTenantAccessor : ICurrentTenantAccessor
{
    private readonly AsyncLocal<BasicTenantInfo> _currentScope;

    private AsyncLocalCurrentTenantAccessor()
    {
        _currentScope = new AsyncLocal<BasicTenantInfo>();
    }

    public static AsyncLocalCurrentTenantAccessor Instance { get; } = new();

    public BasicTenantInfo Current
    {
        get => _currentScope.Value;
        set => _currentScope.Value = value;
    }
}
