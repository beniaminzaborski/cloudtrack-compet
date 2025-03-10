﻿using CloudTrack.Competitions.Messaging;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using CloudTrack.Competitions.Domain.ManagingCompetition;

namespace CloudTrack.Competitions.Application.Competitions;

public class CompetitionOpenedForRegistrationHandler(
    ILogger<CompetitionOpenedForRegistrationHandler> logger,
    IPublishEndpoint publishEndpoint) : INotificationHandler<CompetitionOpenedForRegistration>
{
    public async Task Handle(CompetitionOpenedForRegistration domainEvent, CancellationToken cancellationToken)
    {
        logger.LogInformation("<Application Layer> Competition opened to registration by competitors!");

        await publishEndpoint.Publish(new CompetitionOpenedForRegistrationIntegrationEvent(
            domainEvent.Id.Value,
            new(domainEvent.Place.City, domainEvent.Place.Localization.Latitude, domainEvent.Place.Localization.Longitude),
            new(domainEvent.Distance.Amount, domainEvent.Distance.Unit.ToString()),
            domainEvent.StartAt,
            domainEvent.MaxCompetitors,
            domainEvent.Checkpoints.Select(c => new Messaging.CheckpointDto(c.Id.Value, c.TrackPoint.Amount, c.TrackPoint.Unit.ToString())))
        );
    }
}
