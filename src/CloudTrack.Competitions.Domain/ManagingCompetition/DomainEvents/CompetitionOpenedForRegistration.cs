using CloudTrack.Competitions.Domain.Common;

namespace CloudTrack.Competitions.Domain.ManagingCompetition;

public sealed record CompetitionOpenedForRegistration(
    CompetitionId Id,
    CompetitionPlace Place,
     Distance Distance,
     DateTime StartAt,
     int MaxCompetitors,
     IEnumerable<Checkpoint> Checkpoints)
    : IDomainEvent
{ }
