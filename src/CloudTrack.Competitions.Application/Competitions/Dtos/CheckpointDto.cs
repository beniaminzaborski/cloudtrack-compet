using CloudTrack.Competitions.Domain.ManagingCompetition;

namespace CloudTrack.Competitions.Application.Competitions;

public sealed record CheckpointDto(
    Guid Id,
    decimal TrackPointAmount,
    string TrackPointUnit)
{ 
    public static CheckpointDto FromCheckpoint(Checkpoint checkpoint) =>
        new(
            checkpoint.Id.Value,
            checkpoint.TrackPoint.Amount,
            checkpoint.TrackPoint.Unit.ToString());
}
