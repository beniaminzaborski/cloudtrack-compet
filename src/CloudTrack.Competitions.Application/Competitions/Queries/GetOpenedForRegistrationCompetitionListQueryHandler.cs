using CloudTrack.Competitions.Domain.ManagingCompetition;
using MediatR;
using System.Linq.Expressions;

namespace CloudTrack.Competitions.Application.Competitions.Queries;

internal class GetOpenedForRegistrationCompetitionListQueryHandler(
    ICompetitionRepository competitionRepository) : IRequestHandler<GetOpenedForRegistrationCompetitionListQuery, IEnumerable<CompetitionDto>>
{
    public async Task<IEnumerable<CompetitionDto>> Handle(GetOpenedForRegistrationCompetitionListQuery request, CancellationToken cancellationToken)
    {
        var competitions = await competitionRepository.GetFilteredAsync(OpenedForRegistrationFilter());
        return competitions.Select(c => CompetitionDto.FromCompetition(c)).ToList();

        static Expression<Func<Competition, bool>> OpenedForRegistrationFilter() =>
            c => c.Status == CompetitionStatus.OpenedForRegistration;
    } 
}