namespace FantasySky.CustomDF.Domain.Entities.Auditing;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Property)]
public class DisableAuditingAttribute : Attribute
{
}
