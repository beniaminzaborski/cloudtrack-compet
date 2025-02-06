using CloudTrack.Competitions.Application.Common.Exceptions;
using CloudTrack.Competitions.Domain.ManagingCompetition;
using MediatR;

namespace CloudTrack.Competitions.Application.Competitions.Queries;

internal class GetCompetitionQueryHandler(
    ICompetitionRepository competitionRepository) : IRequestHandler<GetCompetitionQuery, CompetitionDto>
{
    public async Task<CompetitionDto> Handle(GetCompetitionQuery request, CancellationToken cancellationToken)
    {
        var competition = await competitionRepository.GetAsync(CompetitionId.From(request.Id), i => i.Checkpoints);
        return competition is null ? throw new NotFoundException() : CompetitionDto.FromCompetition(competition);
    }
}
