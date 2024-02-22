namespace FantasySky.CustomDF.Domain.Entities;

public interface IGeneratesDomainEvents
{
    void ClearDistributedEvents();

    void ClearLocalEvents();

    IEnumerable<DomainEventRecord> GetDistributedEvents();

    IEnumerable<DomainEventRecord> GetLocalEvents();
}
