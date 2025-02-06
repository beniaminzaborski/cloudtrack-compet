using CloudTrack.Competitions.Domain.ManagingCompetition;
using MediatR;

namespace CloudTrack.Competitions.Application.Competitions.Queries;

internal class GetOpenedForRegistrationCompetitionListQueryHandler(
    ICompetitionRepository competitionRepository) : IRequestHandler<GetOpenedForRegistrationCompetitionListQuery, IEnumerable<CompetitionDto>>
{
    public async Task<IEnumerable<CompetitionDto>> Handle(GetOpenedForRegistrationCompetitionListQuery request, CancellationToken cancellationToken)
    {
        var competitions = await competitionRepository.GetFilteredAsync(i => i.Status == CompetitionStatus.OpenedForRegistration);
        return competitions.Select(c => CompetitionDto.FromCompetition(c)).ToList();
    }
}