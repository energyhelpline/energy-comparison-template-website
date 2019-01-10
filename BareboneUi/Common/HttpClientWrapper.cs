using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace BareboneUi.Common
{
    public class HttpClientWrapper : IHttpClientWrapper
    {
        public async Task<HttpResponseMessage> Get(string uri, IDictionary<string, string> headers)
        {
            using (var httpClient = new HttpClient(new HttpClientHandler { AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip }))
            {
                foreach (var header in headers)
                {
                    httpClient.DefaultRequestHeaders.TryAddWithoutValidation(header.Key, header.Value);
                }

                return await httpClient.GetAsync(uri);
            }
        }

        public async Task<HttpResponseMessage> Send(string uri, HttpMethod method, HttpContent data, IDictionary<string, string> headers)
        {
            var request = new HttpRequestMessage(method, uri);

            AddContent(data, request);
            AddContentTypeHeader(headers, request);

            using (var httpClient = new HttpClient())
            {
                foreach (var header in headers)
                {
                    httpClient.DefaultRequestHeaders.TryAddWithoutValidation(header.Key, header.Value);
                }

                return await httpClient.SendAsync(request);
            }
        }

        private static void AddContent(HttpContent data, HttpRequestMessage request)
        {
            request.Content = data;
        }

        private static void AddContentTypeHeader(IDictionary<string, string> headers, HttpRequestMessage request)
        {
            var apiContentType = headers["Content-type"].Split(";");
            var contentType = new MediaTypeWithQualityHeaderValue(apiContentType[0]);
            if (apiContentType.Length > 1)
            {
                var version = apiContentType[1].Split("=");
                contentType.Parameters.Add(new NameValueHeaderValue(version[0].Trim(), version[1].Trim()));
            }

            request.Content.Headers.ContentType = contentType;
        }
    }
}