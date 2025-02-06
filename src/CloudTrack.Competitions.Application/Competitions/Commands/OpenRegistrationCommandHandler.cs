using CloudTrack.Competitions.Application.Common;
using CloudTrack.Competitions.Application.Common.Exceptions;
using CloudTrack.Competitions.Domain.ManagingCompetition;
using MediatR;

namespace CloudTrack.Competitions.Application.Competitions.Commands;

internal class OpenRegistrationCommandHandler(
    IUnitOfWork unitOfWork,
    ICompetitionRepository competitionRepository) : IRequestHandler<OpenRegistrationCommand>
{
    public async Task Handle(OpenRegistrationCommand request, CancellationToken cancellationToken)
    {
        var competition = await competitionRepository.GetAsync(CompetitionId.From(request.Id), i => i.Checkpoints) ?? throw new NotFoundException();
        
        try
        {
            competition.OpenRegistration();
        }
        catch (CannotOpenRegistrationException)
        {
            throw new Common.Exceptions.ValidationException("Cannot open registration");
        }

        await competitionRepository.UpdateAsync(competition);
        await unitOfWork.CommitAsync();
    }
}
