namespace CloudTrack.Competitions.Messaging;

public sealed record CompetitionOpenedForRegistrationIntegrationEvent(
    Guid Id,
    CompetitionPlaceDto Place,
    DistanceDto Distance,
    DateTime StartAt,
    int MaxCompetitors,
    IEnumerable<CheckpointDto> Checkpoints) { }
