using System.Collections.Generic;
using System.Net.Http;
using Moq;
using NUnit.Framework;
using Segadora.Hikvision.ISAPI.Event;
using Segadora.Hikvision.ISAPI.ResponseTypes;

namespace Segadora.Hikvision.ISAPI.Tests.Event
{
    public class NotificationTest
    {
        [Test(Description = "Tests that the parser is able to handle multiple streamed events")]
        public void AlertStreamTest()
        {
            var content = @"<EventNotificationAlert version=""2.0"" xmlns=""http://www.isapi.com/ver20/XMLSchema"">
<ipAddress>10.17.133.46</ipAddress>
<portNo>80</portNo>
<protocol>HTTP</protocol>
<macAddress>44:19:b6:6d:24:85</macAddress>
<channelID>1</channelID>
<dateTime>2017-05-04T11:20:02+08:00</dateTime>
<activePostCount>0</activePostCount>
<eventType>videoloss</eventType>
<eventState>inactive</eventState>
<eventDescription>videoloss alarm<eventDescription>
</EventNotificationAlert>

<EventNotificationAlert version=""2.0"" xmlns=""http://www.isapi.com/ver20/XMLSchema"">
<ipAddress>10.17.133.46</ipAddress>
<portNo>80</portNo>
<protocol>HTTP</protocol>
<macAddress>44:19:b6:6d:24:85</macAddress>
<channelID>2</channelID>
<dateTime>2019-05-04T11:20:02+08:00</dateTime>
<activePostCount>0</activePostCount>
<eventType>videoloss</eventType>
<eventState>inactive</eventState>
<eventDescription>videoloss alarm<eventDescription>
</EventNotificationAlert>";

            var client = new Mock<IApiClient>();

            client.Setup(x => x.Get("/ISAPI/Event/notification/alertStream")).Returns(new StringContent(content).ReadAsStream());

            var list = new List<EventNotificationAlert>();

            var action = new Notification(client.Object);
            foreach (var eventNotificationAlert in action.AlertStream())
            {
                list.Add(eventNotificationAlert);
            }

            client.VerifyAll();

            Assert.AreEqual(2, list.Count);

            Assert.IsInstanceOf<EventNotificationAlert>(list[0]);
            Assert.AreEqual("10.17.133.46", list[0].IpAddress);
            Assert.AreEqual("80", list[0].PortNo);
            Assert.AreEqual("HTTP", list[0].Protocol);
            Assert.AreEqual("44:19:b6:6d:24:85", list[0].MacAddress);
            Assert.AreEqual("1", list[0].ChannelId);
            Assert.AreEqual("2017-05-04T11:20:02+08:00", list[0].DateTime);
            Assert.AreEqual("0", list[0].ActivePostCount);
            Assert.AreEqual("videoloss", list[0].EventType);
            Assert.AreEqual("inactive", list[0].EventState);
            Assert.AreEqual("videoloss alarm", list[0].EventDescription);

            Assert.IsInstanceOf<EventNotificationAlert>(list[1]);
            Assert.AreEqual("10.17.133.46", list[1].IpAddress);
            Assert.AreEqual("80", list[1].PortNo);
            Assert.AreEqual("HTTP", list[1].Protocol);
            Assert.AreEqual("44:19:b6:6d:24:85", list[1].MacAddress);
            Assert.AreEqual("2", list[1].ChannelId);
            Assert.AreEqual("2019-05-04T11:20:02+08:00", list[1].DateTime);
            Assert.AreEqual("0", list[1].ActivePostCount);
            Assert.AreEqual("videoloss", list[1].EventType);
            Assert.AreEqual("inactive", list[1].EventState);
            Assert.AreEqual("videoloss alarm", list[1].EventDescription);
        }
    }
}
