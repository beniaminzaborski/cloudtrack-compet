using CloudTrack.Competitions.Domain.ManagingCompetition;
using CloudTrack.Competitions.Messaging;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CloudTrack.Competitions.Application.Competitions;

public class CompetitionMaxCompetitorsIncreasedHandler(
    ILogger<CompetitionMaxCompetitorsIncreasedHandler> logger,
    IPublishEndpoint publishEndpoint) : INotificationHandler<CompetitionMaxCompetitorsIncreased>
{
    public async Task Handle(CompetitionMaxCompetitorsIncreased domainEvent, CancellationToken cancellationToken)
    {
        logger.LogInformation("<Application Layer> Maximum numbers of competitors has been increased!");

        await publishEndpoint.Publish(new CompetitionMaxCompetitorsIncreasedIntegrationEvent(domainEvent.Id.Value, domainEvent.MaxCompetitors));
    }
}
