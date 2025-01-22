using CloudTrack.Competitions.Domain.Common;

namespace CloudTrack.Competitions.Domain.ManagingCompetition;

public sealed record CompetitionRegistrationCompleted(
    CompetitionId Id)
    : IDomainEvent
{ }
