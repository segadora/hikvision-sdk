using System;
using System.IO;
using System.Net;
using System.Net.Http;

namespace Segadora.Hikvision.ISAPI
{
    public class ApiClient : IApiClient
    {
        private readonly string _domain;
        private readonly string _username;
        private readonly string _password;

        public ApiClient(string domain, string username, string password)
        {
            _domain = domain;
            
            _username = username;
            _password = password;
        }

        public string GetDomain() => _domain;

        public Stream Get(string url)
        {
            var response = GetHttpClient()
                .Send(
                    new HttpRequestMessage(
                        HttpMethod.Get,
                        $"{_domain}/{url}"
                    ),
                    HttpCompletionOption.ResponseHeadersRead
                );

            response.EnsureSuccessStatusCode();
            
            return response.Content.ReadAsStream();
        }

        private HttpClient GetHttpClient()
        {
            return new (
                new HttpClientHandler
                {
                    Credentials = new CredentialCache
                    {
                        {
                            new Uri(_domain),
                            "Digest",
                            new NetworkCredential(_username, _password)
                        }
                    }
                }
            );
        }
    }
}
