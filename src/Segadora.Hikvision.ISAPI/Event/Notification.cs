using System;
using System.IO;
using Segadora.Hikvision.ISAPI.ResponseTypes;

namespace Segadora.Hikvision.ISAPI.Event
{
    public class Notification
    {
        private const StringComparison StringComparisonConstant = StringComparison.InvariantCulture;

        private readonly IApiClient _client;

        public Notification(IApiClient client)
        {
            _client = client;
        }

        public System.Collections.Generic.IEnumerable<EventNotificationAlert> AlertStream()
        {
            var currentEvent = new EventNotificationAlert();
            var reader = new StreamReader(_client.Get("/ISAPI/Event/notification/alertStream"));
            while (!reader.EndOfStream)
            {
                var content = reader.ReadLine();
                if (content is null || content.IndexOf("<", StringComparisonConstant) < 0)
                {
                    continue;
                }

                if (
                    content.IndexOf(" ", StringComparisonConstant) > -1 &&
                    content.Substring(1, "EventNotificationAlert".Length) == "EventNotificationAlert"
                )
                {
                    currentEvent = new EventNotificationAlert();

                    continue;
                }

                var tagName = content.Substring(1, content.IndexOf(">", StringComparisonConstant) - 1);
                if (tagName == "/EventNotificationAlert")
                {
                    yield return currentEvent;

                    continue;
                }
                    
                var propertyName = tagName switch
                {
                    "ipAddress" => "IpAddress",
                    "portNo" => "PortNo",
                    "protocol" => "Protocol",
                    "macAddress" => "MacAddress",
                    "channelID" => "ChannelId",
                    "dateTime" => "DateTime",
                    "activePostCount" => "ActivePostCount",
                    "eventType" => "EventType",
                    "eventState" => "EventState",
                    "eventDescription" => "EventDescription",
                    "channelName" => "ChannelName",
                    _ => null,
                };

                if (propertyName is not null)
                {
                    currentEvent
                        .GetType()
                        .GetProperty(propertyName)
                        ?.SetValue(
                            currentEvent,
                            content.Substring(
                                content.IndexOf(">", StringComparisonConstant) + 1,
                                content
                                    .Substring(content.IndexOf(">", StringComparisonConstant) + 1)
                                    .IndexOf("<", 0, StringComparisonConstant)
                            ),
                            null
                        );
                }
            }
        }
    }
}
 