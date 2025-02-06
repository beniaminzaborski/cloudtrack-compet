using CloudTrack.Competitions.Domain.ManagingCompetition;
using CloudTrack.Competitions.Messaging;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CloudTrack.Competitions.Application.Competitions;

public class CompetitionRegistrationCompletedHandler(
    ILogger<CompetitionRegistrationCompletedHandler> logger,
    IPublishEndpoint publishEndpoint) : INotificationHandler<CompetitionRegistrationCompleted>
{
    public async Task Handle(CompetitionRegistrationCompleted domainEvent, CancellationToken cancellationToken)
    {
        logger.LogInformation("<Application Layer> Competition registration completed!");

        await publishEndpoint.Publish(new CompetitionRegistrationCompletedIntegrationEvent(domainEvent.Id.Value));
    }
}
