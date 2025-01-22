using CloudTrack.Competitions.Application.Common;
using MediatR;

namespace CloudTrack.Competitions.Application.Competitions.Commands;

public sealed record OpenRegistrationCommand(
    Guid Id) : IRequest, ICommand { }
