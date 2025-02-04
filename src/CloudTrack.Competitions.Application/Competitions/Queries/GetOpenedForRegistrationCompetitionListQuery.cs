using MediatR;

namespace CloudTrack.Competitions.Application.Competitions.Queries;

public sealed record GetOpenedForRegistrationCompetitionListQuery() 
    : IRequest<IEnumerable<CompetitionDto>>
{ }
