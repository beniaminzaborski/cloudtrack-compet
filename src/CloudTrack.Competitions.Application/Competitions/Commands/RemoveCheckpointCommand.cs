using CloudTrack.Competitions.Application.Common;
using MediatR;

namespace CloudTrack.Competitions.Application.Competitions.Commands;

public sealed record RemoveCheckpointCommand(
    Guid CompetitionId,
    Guid CheckpointId) : IRequest, ICommand { }
