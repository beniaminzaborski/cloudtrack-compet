using CloudTrack.Competitions.Application.Common;
using CloudTrack.Competitions.Application.Common.Exceptions;
using CloudTrack.Competitions.Domain.ManagingCompetition;
using MediatR;

namespace CloudTrack.Competitions.Application.Competitions.Commands;

public class RemoveCheckpointCommandHandler(
    IUnitOfWork unitOfWork,
    ICompetitionRepository competitionRepository) : IRequestHandler<RemoveCheckpointCommand>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly ICompetitionRepository _competitionRepository = competitionRepository;

    public async Task Handle(RemoveCheckpointCommand request, CancellationToken cancellationToken)
    {
        var competition = await _competitionRepository.GetAsync(CompetitionId.From(request.CompetitionId), x => x.Checkpoints) ?? throw new NotFoundException();

        try
        {
            competition.RemoveCheckpoint(CheckpointId.From(request.CheckpointId));
        }
        catch (CheckpointNotExistsException)
        {
            throw new Common.Exceptions.ValidationException("Cannot remove a checkpoint because checkpoint does not exist");
        }

        await _competitionRepository.UpdateAsync(competition);
        await _unitOfWork.CommitAsync();
    }
}
