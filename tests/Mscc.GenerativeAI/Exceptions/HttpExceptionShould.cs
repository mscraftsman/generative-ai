using Moq;
using Moq.Protected;
using Mscc.GenerativeAI.Exceptions;
using System;
using Test.Mscc.GenerativeAI;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Mscc.GenerativeAI.Tests.Exceptions
{
    [Collection("SERIAL_TESTS")]
    public class HttpExceptionShould : IClassFixture<ConfigurationFixture>
    {
        private readonly ITestOutputHelper _output;

        public HttpExceptionShould(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public async Task Handle_Http429_Error_With_RetryInfo()
        {
            // Arrange
            var mockHandler = new Mock<HttpMessageHandler>();
            var response429 = new HttpResponseMessage
            {
                StatusCode = (HttpStatusCode)429,
                Content = new StringContent(
                    @"{
                ""error"": {
                    ""code"": 429,
                    ""message"": ""You exceeded your current quota."",
                    ""status"": ""RESOURCE_EXHAUSTED"",
                    ""details"": [
                        {
                            ""@type"": ""type.googleapis.com/google.rpc.RetryInfo"",
                            ""retryDelay"": ""3s""
                        }
                    ]
                }
            }")
            };
            var response200 = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(
                    @"{
                  ""candidates"": [
                    {
                      ""content"": {
                        ""parts"": [
                          {
                            ""text"": ""Hello there!""
                          }
                        ],
                        ""role"": ""model""
                      }
                    }
                  ]
                }")
            };
            mockHandler.Protected()
                .SetupSequence<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(response429)
                .ReturnsAsync(response200);

            var model = new GenerativeModel(mockHandler.Object);
            var request = new GenerateContentRequest { Contents = new List<Content> { new Content { Parts = new List<Part> { new Part { Text = "test" } } } } };
            var stopwatch = new Stopwatch();

            // Act
            stopwatch.Start();
            var result = await model.GenerateContent(request);
            stopwatch.Stop();

            // Assert
            Assert.NotNull(result);
            Assert.True(stopwatch.Elapsed > System.TimeSpan.FromSeconds(3));
        }
    }
}
