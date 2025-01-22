using MediatR;

namespace CloudTrack.Competitions.Application.Competitions.Queries;

public sealed record GetCompetitionListQuery(string? Search) 
    : IRequest<IEnumerable<CompetitionDto>>
{ }
