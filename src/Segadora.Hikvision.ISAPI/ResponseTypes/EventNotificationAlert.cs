namespace Segadora.Hikvision.ISAPI.ResponseTypes
{
    public class EventNotificationAlert
    {
        public string IpAddress { get; set; }
        public string PortNo { get; set; }
        public string Protocol { get; set; }
        public string MacAddress { get; set; }
        public string ChannelId { get; set; }
        public string DateTime { get; set; }
        public string ActivePostCount { get; set; }
        public string EventType { get; set; }
        public string EventState { get; set; }
        public string EventDescription { get; set; }
        public string ChannelName { get; set; }
    }
}
