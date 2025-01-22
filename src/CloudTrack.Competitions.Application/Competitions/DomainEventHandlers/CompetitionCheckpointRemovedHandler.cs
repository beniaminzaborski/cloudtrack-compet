using CloudTrack.Competitions.Domain.ManagingCompetition;
using CloudTrack.Competitions.Messaging;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CloudTrack.Competitions.Application.Competitions;

internal class CompetitionCheckpointRemovedHandler(
    ILogger<CompetitionCheckpointRemovedHandler> logger,
    IPublishEndpoint publishEndpoint) : INotificationHandler<CompetitionCheckpointRemoved>
{
    private readonly ILogger<CompetitionCheckpointRemovedHandler> _logger = logger;
    private readonly IPublishEndpoint _publishEndpoint = publishEndpoint;

    public async Task Handle(CompetitionCheckpointRemoved domainEvent, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"<Application Layer> checkpoint {domainEvent.CheckpointId} from competition {domainEvent.CompetitionId} removed!");

        await _publishEndpoint.Publish(
            new CompetitionCheckpointRemovedIntegrationEvent(
                domainEvent.CompetitionId.Value,
                domainEvent.CheckpointId.Value,
                domainEvent.TrackPoint.Amount,
                domainEvent.TrackPoint.Unit.ToString()));
    }
}
