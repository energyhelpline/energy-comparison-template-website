using System.Threading.Tasks;
using BareboneUi.Common;
using NUnit.Framework;
using System.Collections.Generic;
using System.Net.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using NSubstitute;

namespace BareboneUi.Tests.Common
{
    [TestFixture]
    public class TokenTests
    {
        private IHttpClientWrapper _httpClient;
        private Token _token;
        private Dictionary<string, string> _headers;

        [SetUp]
        public void SetUp()
        {
            _httpClient = Substitute.For<IHttpClientWrapper>();

            _httpClient.Send(
                    Arg.Any<string>(),
                    Arg.Any<HttpMethod>(),
                    Arg.Any<HttpContent>(),
                    Arg.Any<IDictionary<string, string>>())
                .Returns(TokenResponseFromApi("token-from-api"));

            var configuration = Substitute.For<IConfiguration>();
            configuration.GetValue<string>("ApiSecretKey").Returns("test-auth-token");

            _token = new Token(_httpClient, configuration);
            _headers = new Dictionary<string, string>();
        }

        [Test]
        public async Task Adds_authentication_token_from_api_into_headers()
        {
            await _token.RenewToken(_headers, "api-authentication-uri");

            Assert.That(_headers.ContainsKey("Authorization"), "Authorization header not added");
            Assert.That(_headers["Authorization"], Is.EqualTo("Bearer token-from-api"));
        }

        [Test]
        public async Task Renews_token_using_authentication_uri()
        {
            await _token.RenewToken(_headers, "api-authentication-uri");

            await _httpClient
                .Received(1)
                .Send(
                    "api-authentication-uri",
                    Arg.Any<HttpMethod>(),
                    Arg.Any<HttpContent>(),
                    Arg.Any<IDictionary<string, string>>());
        }

        [Test]
        public async Task Renews_token_by_posting_to_api()
        {
            await _token.RenewToken(_headers, "api-authentication-uri");

            await _httpClient
                .Received(1)
                .Send(
                    Arg.Any<string>(),
                    Arg.Is<HttpMethod>(x => x.Equals(HttpMethod.Post)),
                    Arg.Any<HttpContent>(),
                    Arg.Any<IDictionary<string, string>>());
        }

        [Test]
        public async Task Renews_token_by_sending_form_data()
        {
            await _token.RenewToken(_headers, "api-authentication-uri");

            await _httpClient
                .Received(1)
                .Send(
                    Arg.Any<string>(),
                    Arg.Any<HttpMethod>(),
                    Arg.Is<HttpContent>(x => x.ReadAsStringAsync().Result == "grant_type=client_credentials&scope=Domestic"),
                    Arg.Any<IDictionary<string, string>>());
        }

        [Test]
        public async Task Authorization_header_sent_with_token_request()
        {
            await _token.RenewToken(_headers, "api-authentication-uri");

            await _httpClient
                .Received(1)
                .Send(
                    Arg.Any<string>(),
                    Arg.Any<HttpMethod>(),
                    Arg.Any<HttpContent>(),
                    Arg.Is<IDictionary<string, string>>(
                        x =>
                            x["Authorization"] == "Basic test-auth-token"));
        }

        private HttpResponseMessage TokenResponseFromApi(string accessToken)
        {
            return new HttpResponseMessage
            {
                Content = Json(
                    new
                    {
                        access_token = accessToken
                    }
                )
            };
        }

        private StringContent Json(object value) => new StringContent(JsonConvert.SerializeObject(value));
    }
}