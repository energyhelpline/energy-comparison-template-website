using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace BareboneUi.Common
{
    public class Token : IAuthenticate
    {
        private readonly IHttpClientWrapper _httpClientWrapper;
        private readonly IDictionary<string, string> _tokenHeaders;
        private readonly FormUrlEncodedContent _tokenRequestBody;

        private string _currentToken;
        public Token(IHttpClientWrapper httpClientWrapper, IConfiguration configuration)
        {
            _httpClientWrapper = httpClientWrapper;

            var apiSecretKey = configuration.GetValue<string>("ApiSecretKey");
            _tokenHeaders = new Dictionary<string, string>
            {
                ["Authorization"] = $"Basic {apiSecretKey}",
                ["Content-type"] = "application/x-www-form-urlencoded"
            };

            _tokenRequestBody = new FormUrlEncodedContent(
                new Dictionary<string, string>
                {
                    ["grant_type"] = "client_credentials",
                    ["scope"] = "Domestic"
                });
        }

        public async Task RenewToken(IDictionary<string, string> headers, string tokenUri = null)
        {
            if (string.IsNullOrEmpty(tokenUri))
            {
                AddBearerToken(headers);
            }
            else
            {
                var response = await _httpClientWrapper.Send(tokenUri, HttpMethod.Post, _tokenRequestBody, _tokenHeaders);
                var token = await ConvertJsonBody.From(response).To<TokenResponse>();
                AddBearerToken(headers, token.AccessToken);
            }
        }

        private void AddBearerToken(IDictionary<string, string> headers, string token = null)
        {
            if (!string.IsNullOrEmpty(token))
            {
                _currentToken = token;
            }

            headers["Authorization"] = $"Bearer {_currentToken}";
        }
    }
}