using CloudTrack.Competitions.Application.Common;
using CloudTrack.Competitions.Domain.ManagingCompetition;
using CloudTrack.Competitions.Domain.Utils;
using MediatR;

namespace CloudTrack.Competitions.Application.Competitions.Commands;

internal class CreateCompetitionCommandHandler(
    IUnitOfWork unitOfWork,
    ICompetitionRepository competitionRepository) : IRequestHandler<CreateCompetitionCommand, Guid>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly ICompetitionRepository _competitionRepository = competitionRepository;

    public async Task<Guid> Handle(CreateCompetitionCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var competition = new Competition(
                CompetitionId.From(Guid.NewGuid()),
                request.Name,
                DistanceHelper.From(request.Distance.Amount, request.Distance.Unit),
                request.StartAt,
                request.MaxCompetitors,
                new CompetitionPlace(request.Place.City, new Geolocalization(request.Place.Latitude, request.Place.Longitute)));

            await _competitionRepository.CreateAsync(competition);
            await _unitOfWork.CommitAsync();

            return competition.Id.Value;
        }
        catch (CompetitionPlaceCityInvalidException)
        {
            throw new Common.Exceptions.ValidationException("City name", "Must me not empty and lenght must be less than 100 characters");
        }
        catch (GeolocalizationLatitudeInvalidException)
        {
            throw new Common.Exceptions.ValidationException("Latitude must be between -90.0000000 and 90.0000000");
        }
        catch (GeolocalizationLongitudeInvalidException)
        {
            throw new Common.Exceptions.ValidationException("Longitude must be between -180.0000000 and 180.0000000");
        }
        catch (DistanceAmountInvalidException)
        {
            throw new Common.Exceptions.ValidationException("Distance amount is invalid");
        }
    }
}
