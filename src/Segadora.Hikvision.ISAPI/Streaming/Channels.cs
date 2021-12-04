using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;

namespace Segadora.Hikvision.ISAPI.Streaming
{
    public class Channels
    {
        private readonly IApiClient _client;
        private readonly string _channelId;

        public Channels(IApiClient client, string channelId)
        {
            _client = client;
            _channelId = channelId;
        }

        public Image Picture()
        {
            return Image.Load(
                _client.Get($"/ISAPI/Streaming/channels/{_channelId}/picture?snapShotImageType=JPEG"),
                new JpegDecoder()
            );
        }
    }
}
