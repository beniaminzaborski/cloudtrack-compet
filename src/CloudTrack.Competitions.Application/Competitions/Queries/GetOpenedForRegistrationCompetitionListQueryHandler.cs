using CloudTrack.Competitions.Domain.ManagingCompetition;
using MediatR;

namespace CloudTrack.Competitions.Application.Competitions.Queries;

internal class GetOpenedForRegistrationCompetitionListQueryHandler(
    ICompetitionRepository competitionRepository) : IRequestHandler<GetOpenedForRegistrationCompetitionListQuery, IEnumerable<CompetitionDto>>
{
    private readonly ICompetitionRepository _competitionRepository = competitionRepository;

    public async Task<IEnumerable<CompetitionDto>> Handle(GetOpenedForRegistrationCompetitionListQuery request, CancellationToken cancellationToken)
    {
        var competitions = await _competitionRepository.GetFilteredAsync(i => i.Status == CompetitionStatus.OpenedForRegistration);
        return competitions.Select(c => CompetitionDto.FromCompetition(c)).ToList();
    }
}