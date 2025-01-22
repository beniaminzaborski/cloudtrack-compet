using CloudTrack.Competitions.Domain.Common;

namespace CloudTrack.Competitions.Domain.ManagingCompetition;

public record CheckpointId : EntityId<Guid>
{
    public CheckpointId(Guid value) : base(value) { }

    public static CheckpointId From(Guid value)
    {
        return new CheckpointId(value);
    }
}
