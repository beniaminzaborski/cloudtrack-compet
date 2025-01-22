namespace CloudTrack.Competitions.Application.Competitions;

public sealed record CreateCompetitionDto(
    string Name,
    DateTime StartAt,
    DistanceDto Distance,
    CompetitionPlaceDto Place,
    int MaxCompetitors)
{
}
