using CloudTrack.Competitions.Domain.ManagingCompetition;
using MediatR;
using System.Linq.Expressions;

namespace CloudTrack.Competitions.Application.Competitions.Queries;

internal class GetCompetitionListQueryHandler(
    ICompetitionRepository competitionRepository) : IRequestHandler<GetCompetitionListQuery, IEnumerable<CompetitionDto>>
{
    public async Task<IEnumerable<CompetitionDto>> Handle(GetCompetitionListQuery request, CancellationToken cancellationToken)
    {
        var competitions = !string.IsNullOrEmpty(request.Search)
            ? await competitionRepository.GetFilteredAsync(SearchByNameFilter(request.Search))
            : await competitionRepository.GetAllAsync(i => i.Checkpoints);
        return competitions.Select(c => CompetitionDto.FromCompetition(c)).ToList();

        static Expression<Func<Competition, bool>> SearchByNameFilter(string search) =>
            c => c.Name.ToLower().Contains(search.ToLower());
    }
}