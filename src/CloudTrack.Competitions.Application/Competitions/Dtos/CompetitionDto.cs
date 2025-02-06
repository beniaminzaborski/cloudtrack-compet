using CloudTrack.Competitions.Domain.ManagingCompetition;

namespace CloudTrack.Competitions.Application.Competitions;

public sealed record CompetitionDto(
    Guid Id,                
    string Name,            
    DateTime StartAt,       
    DistanceDto Distance,   
    CompetitionPlaceDto Place,
    int MaxCompetitors,     
    string Status,          
    IEnumerable<CheckpointDto> Checkpoints)
{
    public static CompetitionDto FromCompetition(Competition competition) =>
        new(
            competition.Id.Value,
            competition.Name,
            competition.StartAt,
            DistanceDto.FromDistance(competition.Distance),
            CompetitionPlaceDto.FromCompetitionPlace(competition.Place),
            competition.MaxCompetitors,
            competition.Status.ToString(),
            competition.Checkpoints.Select(c => CheckpointDto.FromCheckpoint(c))
        );
}
