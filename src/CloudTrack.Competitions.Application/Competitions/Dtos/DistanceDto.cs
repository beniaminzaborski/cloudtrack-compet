using CloudTrack.Competitions.Domain.ManagingCompetition;

namespace CloudTrack.Competitions.Application.Competitions;

public sealed record DistanceDto(
    decimal Amount,
    string Unit)
{
    public static DistanceDto FromDistance(Distance competitionDistance) =>
        new(
            competitionDistance.Amount,
            competitionDistance.Unit.ToString()
        );
}
