namespace CloudTrack.Competitions.Messaging;

public sealed record CompetitionPlaceDto(
    string City,
    decimal Latitude,
    decimal Longitute) { }
