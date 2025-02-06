using CloudTrack.Competitions.Application.Common.Exceptions;
using CloudTrack.Competitions.Application.Common;
using CloudTrack.Competitions.Domain.ManagingCompetition;
using MediatR;

namespace CloudTrack.Competitions.Application.Competitions.Commands;

internal class CompleteRegistrationCommandHandler(
    IUnitOfWork unitOfWork,
    ICompetitionRepository competitionRepository) : IRequestHandler<CompleteRegistrationCommand>
{
    public async Task Handle(CompleteRegistrationCommand request, CancellationToken cancellationToken)
    {
        var competition = await competitionRepository.GetAsync(CompetitionId.From(request.Id), i => i.Checkpoints) ?? throw new NotFoundException();
        
        try
        {
            competition.CompleteRegistration();
        }
        catch (CannotCompleteRegistrationException)
        {
            throw new Common.Exceptions.ValidationException("Cannot complete registration");
        }

        await competitionRepository.UpdateAsync(competition);
        await unitOfWork.CommitAsync();
    }
}
