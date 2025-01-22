using CloudTrack.Competitions.Domain.ManagingCompetition;

namespace CloudTrack.Competitions.Application.Competitions;

public sealed record CheckpointDto
{
    public Guid Id { get; init; }
    public decimal TrackPointAmount { get; init; }
    public string TrackPointUnit { get; init; }

    public static CheckpointDto FromCheckpoint(Checkpoint checkpoint)
    {
        return new CheckpointDto()
        {
            Id = checkpoint.Id.Value,
            TrackPointAmount = checkpoint.TrackPoint.Amount,
            TrackPointUnit = checkpoint.TrackPoint.Unit.ToString()
        };
    }
}
