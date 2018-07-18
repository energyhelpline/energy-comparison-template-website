using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace BareboneUi.Common
{
    public interface IHttpClientWrapper
    {
        Task<HttpResponseMessage> Get(string uri, IDictionary<string, string> headers);
        Task<HttpResponseMessage> Send(string uri, HttpMethod method, HttpContent data, IDictionary<string, string> headers);
    }
}