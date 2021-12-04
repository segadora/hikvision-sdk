using System.IO;

namespace Segadora.Hikvision.ISAPI
{
    public interface IApiClient
    {
        public string GetDomain();
        public Stream Get(string url);
    }
}
