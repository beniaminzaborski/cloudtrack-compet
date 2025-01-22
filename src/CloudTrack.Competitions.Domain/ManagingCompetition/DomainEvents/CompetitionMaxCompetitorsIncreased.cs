using CloudTrack.Competitions.Domain.Common;

namespace CloudTrack.Competitions.Domain.ManagingCompetition;

public sealed record CompetitionMaxCompetitorsIncreased(
    CompetitionId Id,
    int MaxCompetitors)
    : IDomainEvent
{ }
