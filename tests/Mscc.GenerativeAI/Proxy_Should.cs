using Microsoft.Extensions.Logging;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using Moq.Protected;
using Mscc.GenerativeAI;
using Neovolve.Logging.Xunit;
using Xunit;
using Xunit.Abstractions;

namespace Test.Mscc.GenerativeAI
{
    [Collection(nameof(ConfigurationFixture))]
    public class Proxy_Should : LoggingTestsBase
    {
        private readonly string _model = Model.Gemini25Pro;
        private readonly ITestOutputHelper _output;
        private readonly ConfigurationFixture _fixture;
        private readonly GoogleAI _googleAi;

        public Proxy_Should(ITestOutputHelper output, ConfigurationFixture fixture)
            : base(output, LogLevel.Trace)
        {
            _output = output;
            _fixture = fixture;
            _googleAi = new(apiKey: fixture.ApiKey, logger: Logger);
        }

        [Fact]
        public async Task Use_Proxy_When_Making_Request()
        {
            // Arrange
            var proxy = new WebProxy("http://localhost:8888");
            var recordingHandler = new RecordingHandler();
            var options = new RequestOptions(proxy: proxy);
            var googleAi = new GoogleAI(apiKey: _fixture.ApiKey,
                requestOptions: options,
                httpClientFactory: new TestHttpClientFactory(recordingHandler));
            var model = googleAi.GenerativeModel(model: _model);

            // Act
            try
            {
                await model.GenerateContent("test");
            }
            catch (HttpRequestException)
            {
                // We expect an exception because we are not returning a valid response.
            }

            // Assert
            Assert.NotNull(recordingHandler.Request);
        }
        
        [Fact]
        public async Task Use_Proxy_When_Making_Mock_Request()
        {
            // Arrange
            var proxy = new WebProxy("http://localhost:8888");
            var mockMessageHandler = new Mock<HttpMessageHandler>();
            var requestReceivedByProxy = false;
            var options = new RequestOptions(proxy: proxy);

            mockMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .Callback<HttpRequestMessage, CancellationToken>((req, ct) =>
                {
                    if (req.RequestUri.ToString().Contains("localhost:8888"))
                    {
                        requestReceivedByProxy = true;
                    }
                })
                .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent("{ \"candidates\": [ { \"content\": { \"parts\": [ { \"text\": \"Hello, world!\" } ] } } ] }")
                });

            var googleAi = new GoogleAI(apiKey: _fixture.ApiKey,
                requestOptions: options,
                httpClientFactory: new TestHttpClientFactory(mockMessageHandler.Object));
            var model = googleAi.GenerativeModel(model: _model);

            // Act
            await model.GenerateContent("test");

            // Assert
            Assert.True(requestReceivedByProxy);
        }
    }

    public class RecordingHandler : DelegatingHandler
    {
        public HttpRequestMessage Request { get; private set; }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            Request = request;
            throw new HttpRequestException("Simulating a network error.");
        }
    }

    public class TestHttpClientFactory : IHttpClientFactory
    {
        private readonly HttpMessageHandler _messageHandler;

        public TestHttpClientFactory(HttpMessageHandler messageHandler)
        {
            _messageHandler = messageHandler;
        }

        public HttpClient CreateClient(string name)
        {
            return new HttpClient(_messageHandler);
        }
    }
}