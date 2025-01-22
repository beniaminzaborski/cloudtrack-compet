using MediatR;

namespace CloudTrack.Competitions.Application.Competitions.Queries;

public sealed record GetCompetitionQuery(
    Guid Id) : IRequest<CompetitionDto> { }
