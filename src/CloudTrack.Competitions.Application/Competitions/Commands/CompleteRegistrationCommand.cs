using CloudTrack.Competitions.Application.Common;
using MediatR;

namespace CloudTrack.Competitions.Application.Competitions.Commands;

public sealed record CompleteRegistrationCommand(
    Guid Id) : IRequest, ICommand { }
