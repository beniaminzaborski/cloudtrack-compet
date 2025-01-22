using CloudTrack.Competitions.Application.Common;
using CloudTrack.Competitions.Application.Common.Exceptions;
using CloudTrack.Competitions.Domain.ManagingCompetition;
using MediatR;

namespace CloudTrack.Competitions.Application.Competitions.Commands;

public class AddCheckpointRequestCommandHandler(
    IUnitOfWork unitOfWork,
    ICompetitionRepository competitionRepository) : IRequestHandler<AddCheckpointRequestCommand>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly ICompetitionRepository _competitionRepository = competitionRepository;

    public async Task Handle(AddCheckpointRequestCommand request, CancellationToken cancellationToken)
    {
        var competition = await _competitionRepository.GetAsync(CompetitionId.From(request.CompetitionId), x => x.Checkpoints) ?? throw new NotFoundException();

        if (!Enum.TryParse<DistanceUnit>(request.TrackPointUnit, out var distanceUnit)) throw new Common.Exceptions.ValidationException("Distance unit is incorrect");

        try
        {
            competition.AddCheckpoint(
                new Checkpoint(
            CheckpointId.From(Guid.NewGuid()),
                    CompetitionId.From(request.CompetitionId),
                    new Distance(request.TrackPointAmount, distanceUnit)));
        }
        catch (CheckpointAlreadyExistsException)
        {
            throw new Common.Exceptions.ValidationException("Cannot add a checkpoint because checkpoint in this place already exists");
        }
        catch (DistanceAmountInvalidException)
        {
            throw new Common.Exceptions.ValidationException("Cannot add a checkpoint because distance amount is invalid");
        }

        await _competitionRepository.UpdateAsync(competition);
        await _unitOfWork.CommitAsync();
    }
}
