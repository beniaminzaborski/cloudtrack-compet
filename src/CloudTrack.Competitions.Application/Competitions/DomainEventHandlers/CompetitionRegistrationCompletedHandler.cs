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
    private readonly ILogger<CompetitionRegistrationCompletedHandler> _logger = logger;
    private readonly IPublishEndpoint _publishEndpoint = publishEndpoint;

    public async Task Handle(CompetitionRegistrationCompleted domainEvent, CancellationToken cancellationToken)
    {
        _logger.LogInformation("<Application Layer> Competition registration completed!");

        await _publishEndpoint.Publish(new CompetitionRegistrationCompletedIntegrationEvent(domainEvent.Id.Value));
    }
}
