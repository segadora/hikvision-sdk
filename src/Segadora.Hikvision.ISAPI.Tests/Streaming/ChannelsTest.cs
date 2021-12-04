using System.IO;
using Moq;
using NUnit.Framework;
using Segadora.Hikvision.ISAPI.Streaming;
using SixLabors.ImageSharp;

namespace Segadora.Hikvision.ISAPI.Tests.Streaming
{
    public class ChannelsTest
    {
        [Test]
        public void PictureTest()
        {
            var client = new Mock<IApiClient>();

            client.Setup(x => x.Get("/ISAPI/Streaming/channels/99/picture?snapShotImageType=JPEG"))
                .Returns(new MemoryStream(File.ReadAllBytes("sample.jpg")));

            var actualPicture = new Channels(client.Object, "99").Picture();
            var actualStream = new MemoryStream();
            actualPicture.SaveAsJpeg(actualStream);
            actualPicture.Dispose();

            var expectedPicture = Image.Load(new MemoryStream(File.ReadAllBytes("sample.jpg")));
            var expectedStream = new MemoryStream();
            expectedPicture.SaveAsJpeg(expectedStream);
            expectedPicture.Dispose();

            Assert.That(actualStream.Length, Is.EqualTo(expectedStream.Length));
        }
    }
}
