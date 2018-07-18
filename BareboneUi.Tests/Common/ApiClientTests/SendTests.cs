using System;
using System.Collections.Generic;
using System.Linq;
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
    public class SendTests
    {
        private ApiClient _sut;
        private IHttpClientWrapper _httpClientWrapper;
        private IAuthenticate _authenticate;
        private ResourceBuilder _resourceBuilder;

        [SetUp]
        public void SetUp()
        {
            _httpClientWrapper = Substitute.For<IHttpClientWrapper>();
            _authenticate = Substitute.For<IAuthenticate>();

            _resourceBuilder = new ResourceBuilder();
            _resourceBuilder
                .WithDataTemplate()
                .WithLink("/rels/a /rels/self /rels/b", "web-address")
                .Build();

            var configuration = Substitute.For<IConfiguration>();
            configuration.GetValue<string>("ApiMediaType").Returns("test-media-value");
            _sut = new ApiClient(_httpClientWrapper, _authenticate, configuration);

            _httpClientWrapper
                .Send(
                    Arg.Any<string>(),
                    Arg.Any<HttpMethod>(),
                    Arg.Any<HttpContent>(),
                    Arg.Any<IDictionary<string, string>>())
                .Returns(NotFoundResponse);
        }

        [TestCase("first-uri")]
        [TestCase("second-uri")]
        public async Task Sends_to_self_rel_when_authenticated(string nextUri)
        {
            _httpClientWrapper
                .Send("web-address", Arg.Any<HttpMethod>(), Arg.Any<HttpContent>() ,Arg.Any<IDictionary<string, string>>())
                .Returns(AuthorizedResponse(nextUri));

            var result = await _sut.SendAsync(_resourceBuilder.Build());

            Assert.That(result.Links, Is.Not.Null);
            Assert.That(result.Links.Single().Uri, Is.EqualTo(nextUri));
        }

        [TestCase("POST")]
        [TestCase("PUT")]
        public async Task Sends_with_http_method_from_data_template_when_authenticated(string httpMethod)
        {
            _httpClientWrapper
                .Send(
                    Arg.Any<string>(),
                    Arg.Is<HttpMethod>(m => m.Method.Equals(httpMethod, StringComparison.OrdinalIgnoreCase)),
                    Arg.Any<HttpContent>(),
                    Arg.Any<IDictionary<string, string>>())
                .Returns(AuthorizedResponse("next-uri"));

            _resourceBuilder
                .WithMethod(httpMethod);

            var result = await _sut.SendAsync(_resourceBuilder.Build());

            Assert.That(result.Links, Is.Not.Null);
            Assert.That(result.Links.Single().Uri, Is.EqualTo("next-uri"));
        }

        [TestCase("first-group", "first-item")]
        [TestCase("second-group", "second-item")]
        public async Task Sends_the_data_template_as_request_body_when_authenticated(string group, string item)
        {
            _httpClientWrapper
                .Send(
                    Arg.Any<string>(),
                    Arg.Any<HttpMethod>(),
                    Arg.Is<HttpContent>(h => DataTemplateContains(h, group, item)),
                    Arg.Any<IDictionary<string, string>>())
                .Returns(AuthorizedResponse("next-uri"));

            _resourceBuilder
                .WithGroup(group)
                .WithItem(item);

            var result = await _sut.SendAsync(_resourceBuilder.Build());

            Assert.That(result.Links, Is.Not.Null);
            Assert.That(result.Links.Single().Uri, Is.EqualTo("next-uri"));
        }

        private bool DataTemplateContains(HttpContent content, string group, string item)
        {
            var stringContent = content.ReadAsStringAsync().Result;
            var resource = JsonConvert.DeserializeObject<DataTemplate>(stringContent);

            return resource.HasItem(group, item);
        }

        [Test]
        public async Task Content_type_header_is_added_to_request_when_authenticated()
        {
            await _sut.SendAsync(_resourceBuilder.Build());

            await _httpClientWrapper
                .Received(1)
                .Send(
                    Arg.Any<string>(),
                    Arg.Any<HttpMethod>(),
                    Arg.Any<HttpContent>(),
                    HeaderIs("Content-type", "test-media-value"));
        }

        [Test]
        public async Task Authentication_header_is_added_to_request_when_authenticated()
        {
            _authenticate
                .When(x => x.RenewToken(Arg.Any<IDictionary<string, string>>()))
                .Do(x => x.Arg<IDictionary<string, string>>().Add("Authorization", "valid-token"));

            await _sut.SendAsync(_resourceBuilder.Build());

            await _httpClientWrapper
                .Received(1)
                .Send(
                    Arg.Any<string>(),
                    Arg.Any<HttpMethod>(),
                    Arg.Any<HttpContent>(),
                    HeaderIs("Authorization", "valid-token"));
        }

        [TestCase("first-uri")]
        [TestCase("second-uri")]
        public async Task Sends_to_self_rel_when_not_authenticated(string nextUri)
        {
            _httpClientWrapper
                .Send("web-address", Arg.Any<HttpMethod>(), Arg.Any<HttpContent>(), Arg.Any<IDictionary<string, string>>())
                .Returns(
                    UnauthorizedResponse("token-uri"),
                    AuthorizedResponse(nextUri));

            var result = await _sut.SendAsync(_resourceBuilder.Build());

            Assert.That(result.Links, Is.Not.Null);
            Assert.That(result.Links.Single().Uri, Is.EqualTo(nextUri));
        }

        [TestCase("POST")]
        [TestCase("PUT")]
        public async Task Sends_with_http_method_from_data_template_when_not_authenticated(string httpMethod)
        {
            _httpClientWrapper
                .Send(
                    Arg.Any<string>(),
                    Arg.Is<HttpMethod>(m => m.Method.Equals(httpMethod, StringComparison.OrdinalIgnoreCase)),
                    Arg.Any<HttpContent>(),
                    Arg.Any<IDictionary<string, string>>())
                .Returns(
                    UnauthorizedResponse("token-uri"),
                    AuthorizedResponse("next-uri"));

            _resourceBuilder
                .WithMethod(httpMethod);

            var result = await _sut.SendAsync(_resourceBuilder.Build());

            Assert.That(result.Links, Is.Not.Null);
            Assert.That(result.Links.Single().Uri, Is.EqualTo("next-uri"));
        }

        [TestCase("first-group", "first-item")]
        [TestCase("second-group", "second-item")]
        public async Task Sends_the_data_template_as_request_body_when_not_authenticated(string group, string item)
        {
            _httpClientWrapper
                .Send(
                    Arg.Any<string>(),
                    Arg.Any<HttpMethod>(),
                    Arg.Is<HttpContent>(h => DataTemplateContains(h, group, item)),
                    Arg.Any<IDictionary<string, string>>())
                .Returns(
                    UnauthorizedResponse("token-uri"),
                    AuthorizedResponse("next-uri"));

            _resourceBuilder
                .WithGroup(group)
                .WithItem(item);

            var result = await _sut.SendAsync(_resourceBuilder.Build());

            Assert.That(result.Links, Is.Not.Null);
            Assert.That(result.Links.Single().Uri, Is.EqualTo("next-uri"));
        }

        [Test]
        public async Task Renews_the_token_using_uri_from_unauthenticated_response()
        {
            _httpClientWrapper
                .Send(
                    Arg.Any<string>(),
                    Arg.Any<HttpMethod>(),
                    Arg.Any<HttpContent>(),
                    Arg.Any<IDictionary<string, string>>())
                .Returns(
                    UnauthorizedResponse("token-uri"),
                    AuthorizedResponse("next-uri"));

            await _sut.SendAsync(_resourceBuilder.Build());

            await _authenticate.Received().RenewToken(Arg.Any<IDictionary<string, string>>(), "token-uri");
        }

        [Test]
        public async Task Authentication_header_is_updated_after_token_renewal()
        {
            _httpClientWrapper
                .Send(
                    Arg.Any<string>(),
                    Arg.Any<HttpMethod>(),
                    Arg.Any<HttpContent>(),
                    HeaderIs("Authorization", "expired-token"))
                .Returns(UnauthorizedResponse("token-uri"));

            _httpClientWrapper
                .Send(
                    Arg.Any<string>(),
                    Arg.Any<HttpMethod>(),
                    Arg.Any<HttpContent>(),
                    HeaderIs("Authorization", "valid-token"))
                .Returns(AuthorizedResponse("next-uri"));

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

            var result = await _sut.SendAsync(_resourceBuilder.Build());

            Assert.That(result.Links, Is.Not.Null);
            Assert.That(result.Links.Single().Uri, Is.EqualTo("next-uri"));
        }

        private IDictionary<string, string> HeaderIs(string name, string value)
        {
            return Arg.Is<IDictionary<string, string>>(hdr => hdr[name] == value);
        }

        private HttpResponseMessage AuthorizedResponse(string nextUri)
        {
            return new HttpResponseMessage
            {
                Content = Json(
                    new Resource
                    {
                        Links = new[]
                        {
                            new Link
                            {
                                Rel = "/rels/next",
                                Uri = nextUri
                            }
                        }
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

        private HttpResponseMessage NotFoundResponse => new HttpResponseMessage(HttpStatusCode.NotFound)
        {
            Content = Json(
                new Resource
                {
                    Links = new[]
                    {
                        new Link
                        {
                            Rel = "/rels/error",
                            Uri = "error-not-found"
                        }
                    }
                })
        };

        private StringContent Json(object value) => new StringContent(JsonConvert.SerializeObject(value));
    }
}