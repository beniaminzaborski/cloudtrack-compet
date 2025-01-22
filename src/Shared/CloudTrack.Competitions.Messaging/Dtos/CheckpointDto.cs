namespace CloudTrack.Competitions.Messaging;

public sealed record CheckpointDto(
    Guid Id,
    decimal TrackPointAmount,
    string TrackPointUnit) { }
