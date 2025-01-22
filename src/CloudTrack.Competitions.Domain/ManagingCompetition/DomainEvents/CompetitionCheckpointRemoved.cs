using CloudTrack.Competitions.Domain.Common;

namespace CloudTrack.Competitions.Domain.ManagingCompetition;

public sealed record CompetitionCheckpointRemoved(
    CompetitionId CompetitionId,
    CheckpointId CheckpointId,
    Distance TrackPoint) : IDomainEvent
{ }
