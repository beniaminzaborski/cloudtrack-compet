using CloudTrack.Competitions.Application.Common;
using MediatR;

namespace CloudTrack.Competitions.Application.Competitions.Commands;

public sealed record CreateCompetitionCommand(
    string Name,
    DateTime StartAt,
    DistanceDto Distance,
    CompetitionPlaceDto Place,
    int MaxCompetitors) : IRequest<Guid>, ICommand { }
