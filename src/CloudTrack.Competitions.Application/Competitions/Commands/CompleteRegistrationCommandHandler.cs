using CloudTrack.Competitions.Application.Common.Exceptions;
using CloudTrack.Competitions.Application.Common;
using CloudTrack.Competitions.Domain.ManagingCompetition;
using MediatR;

namespace CloudTrack.Competitions.Application.Competitions.Commands;

internal class CompleteRegistrationCommandHandler(
    IUnitOfWork unitOfWork,
    ICompetitionRepository competitionRepository) : IRequestHandler<CompleteRegistrationCommand>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly ICompetitionRepository _competitionRepository = competitionRepository;

    public async Task Handle(CompleteRegistrationCommand request, CancellationToken cancellationToken)
    {
        var competition = await _competitionRepository.GetAsync(CompetitionId.From(request.Id), i => i.Checkpoints) ?? throw new NotFoundException();
        
        try
        {
            competition.CompleteRegistration();
        }
        catch (CannotCompleteRegistrationException)
        {
            throw new Common.Exceptions.ValidationException("Cannot complete registration");
        }

        await _competitionRepository.UpdateAsync(competition);
        await _unitOfWork.CommitAsync();
    }
}
