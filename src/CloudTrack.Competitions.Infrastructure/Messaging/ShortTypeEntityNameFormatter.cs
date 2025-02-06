using MassTransit;
using System.Text.RegularExpressions;

namespace CloudTrack.Competitions.Infrastructure.Messaging;

internal class ShortTypeEntityNameFormatter : IEntityNameFormatter
{
    private const string IntegrationEventSuffix = "IntegrationEvent";

    public string FormatEntityName<T>()
    {
        var messageType = typeof(T).Name;
        if (messageType.EndsWith(IntegrationEventSuffix, StringComparison.OrdinalIgnoreCase))
        {
            messageType = messageType.Replace(IntegrationEventSuffix, string.Empty);
        }
        return Regex.Replace(messageType, "([a-z])([A-Z])", "$1-$2").ToLower();
    }
}
