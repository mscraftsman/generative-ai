using Microsoft.Extensions.Logging;
using Mscc.GenerativeAI;
using Neovolve.Logging.Xunit;
using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Test.Mscc.GenerativeAI
{
    public class DelayingHandler : HttpMessageHandler
    {
        private readonly TimeSpan _delay;
        private readonly HttpStatusCode _statusCodeToReturn;
        private readonly string _responseContent;

        public DelayingHandler(TimeSpan delay, HttpStatusCode statusCodeToReturn = HttpStatusCode.OK, string? responseContent = null)
        {
            _delay = delay;
            _statusCodeToReturn = statusCodeToReturn;
            _responseContent = responseContent ?? "{\"candidates\": [{\"content\": {\"parts\": [{\"text\": \"mock response\"}]}}]}";
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            try
            {
                await Task.Delay(_delay, cancellationToken);
            }
            catch (TaskCanceledException)
            {
                // If Task.Delay is cancelled by the CancellationToken (e.g. from RequestOptions.Timeout or HttpClient.Timeout),
                // rethrow to simulate the behavior of HttpClient's SendAsync when its own CancellationToken is triggered.
                throw;
            }

            return new HttpResponseMessage(_statusCodeToReturn)
            {
                Content = new StringContent(_responseContent, System.Text.Encoding.UTF8, "application/json")
            };
        }
    }

    public class Timeout_Tests : LoggingTestsBase, IDisposable
    {
        private GenerativeModel? _model;
        private readonly ITestOutputHelper _output;
        private readonly ConfigurationFixture _fixture;
        private readonly GoogleAI _googleAi;

        public Timeout_Tests(ITestOutputHelper output, ConfigurationFixture fixture)
            : base(output, LogLevel.Trace)
        {
            _output = output;
            _fixture = fixture;
            _googleAi = new(apiKey: fixture.ApiKey, logger: Logger);
        }

        public void Dispose()
        {
            _model?.Dispose();
            GC.SuppressFinalize(this);
        }

        [Fact]
        public async Task BaseModel_Timeout_Property_Should_Cancel_When_Exceeded()
        {
            var clientTimeout = TimeSpan.FromMilliseconds(50);
            var delayTime = TimeSpan.FromMilliseconds(200); // Longer than clientTimeout

            var delayingHandler = new DelayingHandler(delayTime);
            _model = new GenerativeModel(delayingHandler); // Using the new internal constructor
            _model.Timeout = clientTimeout;

            // Expect TaskCanceledException directly from HttpClient's timeout mechanism
            await Assert.ThrowsAsync<TaskCanceledException>(() =>
                _model.GenerateContent("test"));
        }

        [Fact]
        public async Task RequestOptions_Timeout_Should_Cancel_When_Exceeded_And_Succeed_When_Not()
        {
            var requestTimeoutShort = TimeSpan.FromMilliseconds(50);
            var requestTimeoutLong = TimeSpan.FromMilliseconds(300);
            var delayTime = TimeSpan.FromMilliseconds(150); // Between short and long timeouts

            var delayingHandler = new DelayingHandler(delayTime);
            _model = new GenerativeModel(delayingHandler);

            // Test cancellation
            var requestOptionsShort = new RequestOptions { Timeout = requestTimeoutShort };
            // This should throw TaskCanceledException because RequestOptions.Timeout (50ms) < delayTime (150ms)
            // The exception is thrown by Task.Delay in the handler, then rethrown.
            await Assert.ThrowsAsync<TaskCanceledException>(() =>
                _model.GenerateContent("test prompt", requestOptions: requestOptionsShort));

            // Test success
            var requestOptionsLong = new RequestOptions { Timeout = requestTimeoutLong };
            // This should succeed because RequestOptions.Timeout (300ms) > delayTime (150ms)
            var response = await _model.GenerateContent("test prompt", requestOptions: requestOptionsLong);
            Assert.NotNull(response);
            Assert.Equal("mock response", response.Text);
        }

        [Fact]
        public async Task RequestOptions_Timeout_Should_Take_Precedence_Over_BaseModel_Timeout()
        {
            var clientTimeout = TimeSpan.FromSeconds(5); // Long BaseModel timeout
            var requestOptionsTimeout = TimeSpan.FromMilliseconds(100); // Short RequestOptions timeout
            var delayTime = TimeSpan.FromMilliseconds(300); // Longer than RequestOptions.Timeout

            var delayingHandler = new DelayingHandler(delayTime);
            _model = new GenerativeModel(delayingHandler);
            _model.Timeout = clientTimeout;

            var requestOptions = new RequestOptions { Timeout = requestOptionsTimeout };
            // Expect TaskCanceledException due to RequestOptions.Timeout being shorter
            // The exception is thrown by Task.Delay in the handler, then rethrown.
            await Assert.ThrowsAsync<TaskCanceledException>(() =>
                _model.GenerateContent("test prompt", requestOptions: requestOptions));
        }

        [Fact]
        public async Task Setting_BaseModel_Timeout_MultipleTimes_Should_Not_Throw_InvalidOperationException()
        {
            // Use a handler that responds immediately
            var immediateHandler = new DelayingHandler(TimeSpan.Zero);
            _model = new GenerativeModel(immediateHandler);

            _model.Timeout = TimeSpan.FromSeconds(1);
            var response1 = await _model.GenerateContent("test1");
            Assert.NotNull(response1);
            Assert.Equal("mock response", response1.Text);

            // Setting Timeout again after a request has been sent.
            // HttpClient allows this; it applies to subsequent requests.
            _model.Timeout = TimeSpan.FromSeconds(2);
            var response2 = await _model.GenerateContent("test2");
            Assert.NotNull(response2);
            Assert.Equal("mock response", response2.Text);
            // The main assertion is that no InvalidOperationException was thrown
        }
    }
}
