namespace CloudTrack.Competitions.Application.Competitions;

public sealed record AddCheckpointRequestDto(decimal TrackPointAmount, string TrackPointUnit)
{
}
