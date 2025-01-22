using CloudTrack.Competitions.Application.Common;
using MediatR;

namespace CloudTrack.Competitions.Application.Competitions.Commands;

public sealed record AddCheckpointRequestCommand(
    Guid CompetitionId,
    decimal TrackPointAmount,
    string TrackPointUnit) : IRequest, ICommand  { }
