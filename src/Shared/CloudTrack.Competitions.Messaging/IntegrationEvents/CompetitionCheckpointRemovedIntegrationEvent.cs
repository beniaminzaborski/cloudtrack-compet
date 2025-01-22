namespace CloudTrack.Competitions.Messaging;

public sealed record CompetitionCheckpointRemovedIntegrationEvent(
    Guid CompetitionId,
    Guid CheckpointId,
    decimal TrackPointDistance,
    string TrackPointUnit) { }