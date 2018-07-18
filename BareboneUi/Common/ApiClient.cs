using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace BareboneUi.Common
{
    public class ApiClient : IApiClient
    {
        private readonly IHttpClientWrapper _httpClientWrapper;
        private readonly IAuthenticate _authenticate;
        private readonly string _apiMediaType;

        public ApiClient(IHttpClientWrapper httpClientWrapper, IAuthenticate authenticate, IConfiguration configuration)
        {
            _httpClientWrapper = httpClientWrapper;
            _authenticate = authenticate;
            _apiMediaType = configuration.GetValue<string>("ApiMediaType");
        }

        public async Task<T> GetAsync<T>(string url)
        {
            var headers = GetHeaders();
            return await CallApi<T>(headers, () => _httpClientWrapper.Get(url, headers));
        }

        public async Task<string> GetAsync(string url)
        {
            var headers = GetHeaders();
            return await CallApi(headers, () => _httpClientWrapper.Get(url, headers));
        }

        private Dictionary<string, string> GetHeaders()
        {
            return new Dictionary<string, string>
            {
                ["Accept"] = _apiMediaType,
                ["Content-type"] = _apiMediaType
            };
        }

        public async Task<Resource> SendAsync(Resource resource)
        {
            var headers = new Dictionary<string, string>
            {
                ["Content-type"] = _apiMediaType
            };

            var selfLink = resource.GetUriForRel("/rels/self");
            var httpMethodFromDataTemplate = new HttpMethod(resource.DataTemplate.Methods[0]);
            var jsonDataTemplate = new StringContent(JsonConvert.SerializeObject(resource.DataTemplate));

            return await CallApi<Resource>(
                headers,
                () => _httpClientWrapper.Send(selfLink, httpMethodFromDataTemplate, jsonDataTemplate, headers));
        }

        private async Task<T> CallApi<T>(IDictionary<string, string> headers, Func<Task<HttpResponseMessage>> callApi)
        {
            var response = await GetResponse(headers, callApi);
            return await ConvertJsonBody.From(response).To<T>();
        }

        private async Task<string> CallApi(IDictionary<string, string> headers, Func<Task<HttpResponseMessage>> callApi)
        {
            var response = await GetResponse(headers, callApi);
            return await ConvertJsonBody.From(response).ToStringAsync();
        }

        private async Task<HttpResponseMessage> GetResponse(IDictionary<string, string> headers, Func<Task<HttpResponseMessage>> callApi)
        {
            await _authenticate.RenewToken(headers);

            var response = await callApi();

            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                var unauthorizedResponse = await ConvertJsonBody.From(response).To<Resource>();
                var tokenUri = unauthorizedResponse.GetUriForRel("/rels/token");

                await _authenticate.RenewToken(headers, tokenUri);
                response = await callApi();
            }

            return response;
        }
    }
}