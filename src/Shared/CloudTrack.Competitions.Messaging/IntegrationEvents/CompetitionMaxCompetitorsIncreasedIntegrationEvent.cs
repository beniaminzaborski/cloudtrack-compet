namespace CloudTrack.Competitions.Messaging;

public sealed record CompetitionMaxCompetitorsIncreasedIntegrationEvent(
    Guid Id, 
    int MaxCompetitors) { }
