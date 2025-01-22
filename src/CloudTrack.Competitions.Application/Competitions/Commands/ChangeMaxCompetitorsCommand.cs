using CloudTrack.Competitions.Application.Common;
using MediatR;

namespace CloudTrack.Competitions.Application.Competitions.Commands;

public sealed record ChangeMaxCompetitorsCommand(
    Guid Id,
    int MaxCompetitors) : IRequest, ICommand { }
