using CloudTrack.Competitions.Domain.ManagingCompetition;

namespace CloudTrack.Competitions.Application.Competitions;

public sealed record CompetitionPlaceDto(
    string City,
    decimal Latitude, 
    decimal Longitute)
{
    public static CompetitionPlaceDto FromCompetitionPlace(CompetitionPlace competitionPlace) =>
        new(
            competitionPlace.City,
            competitionPlace.Localization.Latitude,
            competitionPlace.Localization.Longitude
        );
}
