using System.Drawing;

namespace Segadora.Hikvision.ISAPI.Streaming
{
    public class Channels
    {
        private readonly ApiClient _client;
        private readonly string _channelId;

        public Channels(ApiClient client, string channelId)
        {
            _client = client;
            _channelId = channelId;
        }

        public Bitmap Picture()
        {
            return new (_client.Get($"/ISAPI/Streaming/channels/{_channelId}/picture?snapShotImageType=JPEG"));
        }
    }
}
