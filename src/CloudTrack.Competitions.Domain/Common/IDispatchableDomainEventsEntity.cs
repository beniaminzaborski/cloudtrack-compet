namespace CloudTrack.Competitions.Domain.Common;

public interface IDispatchableDomainEventsEntity
{
    IReadOnlyCollection<IDomainEvent> GetDomainEvents();

    public void ClearDomainEvents();
}
