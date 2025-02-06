using CloudTrack.Competitions.Application.Common.Exceptions;
using CloudTrack.Competitions.Application.Common;
using CloudTrack.Competitions.Domain.ManagingCompetition;
using MediatR;

namespace CloudTrack.Competitions.Application.Competitions.Commands;

internal class ChangeMaxCompetitorsCommandHandler(
    IUnitOfWork unitOfWork,
    ICompetitionRepository competitionRepository) : IRequestHandler<ChangeMaxCompetitorsCommand>
{
    public async Task Handle(ChangeMaxCompetitorsCommand request, CancellationToken cancellationToken)
    {
        var competition = await competitionRepository.GetAsync(CompetitionId.From(request.Id)) ?? throw new NotFoundException();
        
        try
        {
            competition.ChangeMaxCompetitors(request.MaxCompetitors);
        }
        catch (CompetitionMaxCompetitorsChangeNotAllowedException)
        {
            throw new Common.Exceptions.ValidationException("Changing maximum numbers of competitors is not allowed");
        }

        await competitionRepository.UpdateAsync(competition);
        await unitOfWork.CommitAsync();
    }
}