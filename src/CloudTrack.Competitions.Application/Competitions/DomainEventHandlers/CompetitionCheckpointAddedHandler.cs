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
    public async Task Handle(CompetitionCheckpointAdded domainEvent, CancellationToken cancellationToken)
    {
        logger.LogInformation($"<Application Layer> new checkpoint {domainEvent.CheckpointId} to competition {domainEvent.CompetitionId} added!");

        await publishEndpoint.Publish(
            new CompetitionCheckpointAddedIntegrationEvent(
                domainEvent.CompetitionId.Value,
                domainEvent.CheckpointId.Value,
                domainEvent.TrackPoint.Amount,
                domainEvent.TrackPoint.Unit.ToString()));
    }
}
