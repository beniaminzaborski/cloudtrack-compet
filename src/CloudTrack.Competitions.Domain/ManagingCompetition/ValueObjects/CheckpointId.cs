using CloudTrack.Competitions.Domain.Common;

namespace CloudTrack.Competitions.Domain.ManagingCompetition;

public record CheckpointId(Guid Value) : EntityId<Guid>(Value)
{
    public static CheckpointId From(Guid value)
    {
        return new CheckpointId(value);
    }
}
