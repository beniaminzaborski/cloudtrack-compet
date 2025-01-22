using CloudTrack.Competitions.Domain.ManagingCompetition;
using CloudTrack.Competitions.Messaging;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CloudTrack.Competitions.Application.Competitions;

public class CompetitionCheckpointAddedHandler(
    ILogger<CompetitionCheckpointAddedHandler> logger,
    IPublishEndpoint publishEndpoint) : INotificationHandler<CompetitionCheckpointAdded>
{
    private readonly ILogger<CompetitionCheckpointAddedHandler> _logger = logger;
    private readonly IPublishEndpoint _publishEndpoint = publishEndpoint;

    public async Task Handle(CompetitionCheckpointAdded domainEvent, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"<Application Layer> new checkpoint {domainEvent.CheckpointId} to competition {domainEvent.CompetitionId} added!");

        await _publishEndpoint.Publish(
            new CompetitionCheckpointAddedIntegrationEvent(
                domainEvent.CompetitionId.Value,
                domainEvent.CheckpointId.Value,
                domainEvent.TrackPoint.Amount,
                domainEvent.TrackPoint.Unit.ToString()));
    }
}
