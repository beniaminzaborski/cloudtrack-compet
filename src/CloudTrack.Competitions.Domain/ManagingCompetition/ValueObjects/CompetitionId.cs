using CloudTrack.Competitions.Domain.Common;

namespace CloudTrack.Competitions.Domain.ManagingCompetition;

public record CompetitionId(Guid Value) : EntityId<Guid>(Value)
{
    public static CompetitionId From(Guid value) => new CompetitionId(value);
}
