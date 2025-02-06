namespace CloudTrack.Competitions.Domain.Common;

public abstract class Entity<TId> : IDispatchableDomainEventsEntity
{
    public TId Id { get; protected set; }

    public override int GetHashCode() => Id.GetHashCode();

    public override bool Equals(object? obj) =>
        obj is Entity<TId> entity && entity.Id != null && entity.Id.Equals(Id);
    
    private readonly IList<IDomainEvent> _domainEvents = new List<IDomainEvent>();

    protected void QueueDomainEvent(IDomainEvent @event) => _domainEvents.Add(@event);

    public IReadOnlyCollection<IDomainEvent> GetDomainEvents() => _domainEvents.AsReadOnly();

    public void ClearDomainEvents() => _domainEvents.Clear();
}
