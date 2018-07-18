using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using BareboneUi.Common;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using NSubstitute;
using NUnit.Framework;

namespace BareboneUi.Tests.Common.ApiClientTests
{
    [TestFixture]
    public class GetTests
    {
        private ApiClient _sut;
        private IHttpClientWrapper _httpClientWrapper;
        private IAuthenticate _authenticate;
        private IConfiguration _configuration;

        [SetUp]
        public void SetUp()
        {
            _httpClientWrapper = Substitute.For<IHttpClientWrapper>();
            _authenticate = Substitute.For<IAuthenticate>();

            _configuration = Substitute.For<IConfiguration>();

            _configuration.GetValue<string>("ApiMediaType").Returns("test-media-string");

            _sut = new ApiClient(_httpClientWrapper, _authenticate, _configuration);
        }

        [TestCase("response")]
        [TestCase("another-response")]
        public async Task Returns_response_from_http_client_when_authenticated(string expectedResponse)
        {
            _httpClientWrapper
                .Get(Arg.Is("web-address"), Arg.Any<IDictionary<string, string>>())
                .Returns(AuthorizedResponse(expectedResponse));

            var result = await _sut.GetAsync<JsonResponse>("web-address");

            Assert.That(result.Value, Is.EqualTo(expectedResponse));
        }

        [Test]
        public async Task Accept_header_is_added_to_request_when_authenticated()
        {
            _httpClientWrapper
                .Get(Arg.Is("web-address"), Arg.Any<IDictionary<string, string>>())
                .Returns(AuthorizedResponse("ok"));

            await _sut.GetAsync<JsonResponse>("web-address");

            await _httpClientWrapper.Received(1).Get(Arg.Any<string>(), HeaderIs("Accept", "test-media-string"));
        }

        [Test]
        public async Task Authentication_header_is_added_to_request_when_authenticated()
        {
            _httpClientWrapper
                .Get(Arg.Is("web-address"), Arg.Any<IDictionary<string, string>>())
                .Returns(AuthorizedResponse("ok"));

            _authenticate
                .When(x => x.RenewToken(Arg.Any<IDictionary<string, string>>()))
                .Do(x => x.Arg<IDictionary<string, string>>().Add("Authorization","valid-token"));

            await _sut.GetAsync<JsonResponse>("web-address");

            await _httpClientWrapper.Received(1).Get(Arg.Any<string>(), HeaderIs("Authorization", "valid-token"));
        }

        [TestCase("response")]
        [TestCase("another-response")]
        public async Task Returns_response_from_http_client_when_not_authenticated(string expectedResponse)
        {
            _httpClientWrapper
                .Get(Arg.Is("web-address"), Arg.Any<IDictionary<string, string>>())
                .Returns(
                    UnauthorizedResponse("token-uri"),
                    AuthorizedResponse(expectedResponse));

            var result = await _sut.GetAsync<JsonResponse>("web-address");

            Assert.That(result.Value, Is.EqualTo(expectedResponse));
        }

        [Test]
        public async Task Renews_the_token_using_uri_from_unauthenticated_response()
        {
            _httpClientWrapper
                .Get(Arg.Is("web-address"), Arg.Any<IDictionary<string, string>>())
                .Returns(
                    UnauthorizedResponse("token-uri"),
                    AuthorizedResponse("ok"));

            await _sut.GetAsync<JsonResponse>("web-address");

            await _authenticate.Received().RenewToken(Arg.Any<IDictionary<string, string>>(), "token-uri");
        }

        [Test]
        public async Task Authentication_header_is_updated_after_token_renewal()
        {
            _httpClientWrapper
                .Get(Arg.Is("web-address"), HeaderIs("Authorization", "expired-token"))
                .Returns(UnauthorizedResponse("token-uri"));

            _httpClientWrapper
                .Get(Arg.Is("web-address"), HeaderIs("Authorization", "valid-token"))
                .Returns(AuthorizedResponse("ok"));

            _authenticate
                .When(x => x.RenewToken(Arg.Any<IDictionary<string, string>>(), Arg.Any<string>()))
                .Do(x =>
                {
                    var headers = x.Arg<IDictionary<string, string>>();

                    if (!headers.ContainsKey("Authorization"))
                    {
                        headers["Authorization"] = "expired-token";
                    }
                    else
                    {
                        headers["Authorization"] = "valid-token";
                    }
                });

            var result = await _sut.GetAsync<JsonResponse>("web-address");

            Assert.That(result.Value, Is.EqualTo("ok"));
        }

        private IDictionary<string, string> HeaderIs(string name, string value)
        {
            return Arg.Is<IDictionary<string, string>>(hdr => hdr[name] == value);
        }

        private HttpResponseMessage AuthorizedResponse(string expectedResponse)
        {
            return new HttpResponseMessage
            {
                Content = Json(
                    new
                    {
                        value = expectedResponse
                    })
            };
        }

        private HttpResponseMessage UnauthorizedResponse(string tokenUri)
        {
            return new HttpResponseMessage(HttpStatusCode.Unauthorized)
            {
                Content = Json(
                    new
                    {
                        links = new[]
                        {
                            new
                            {
                                rel = "/rels/token",
                                uri = tokenUri
                            }
                        }
                    })
            };
        }

        private StringContent Json(object value) => new StringContent(JsonConvert.SerializeObject(value));
    }
}
