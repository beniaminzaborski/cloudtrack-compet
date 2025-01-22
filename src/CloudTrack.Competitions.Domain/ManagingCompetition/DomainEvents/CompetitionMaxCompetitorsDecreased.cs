using CloudTrack.Competitions.Domain.Common;

namespace CloudTrack.Competitions.Domain.ManagingCompetition;

public sealed record CompetitionMaxCompetitorsDecreased(
    CompetitionId Id,
    int MaxCompetitors)
    : IDomainEvent
{ }
