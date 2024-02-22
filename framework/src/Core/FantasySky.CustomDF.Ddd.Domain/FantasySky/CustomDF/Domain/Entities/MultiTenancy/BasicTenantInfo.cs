namespace FantasySky.CustomDF.Domain.Entities.MultiTenancy;

public class BasicTenantInfo
{
    public BasicTenantInfo(Guid? tenantId, string? name = null)
    {
        this.TenantId = tenantId;
        this.Name = name;
    }

    /// <summary>
    /// Name of the tenant if <see cref="TenantId"/> is not null.
    /// </summary>
    public string? Name { get; }

    /// <summary>
    /// Null indicates the host.
    /// Not null value for a tenant.
    /// </summary>
    public Guid? TenantId { get; }
}
