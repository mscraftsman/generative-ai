using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;
using Mscc.GenerativeAI;
using Mscc.GenerativeAI.Types;
using Shouldly;
using Xunit;

namespace Test.Mscc.GenerativeAI
{
    public class ChatSession_Should
    {
        [Fact]
        public async Task Handle_Automatic_Function_Calling()
        {
            // Arrange
            var functionCalled = false;
            string GetCurrentWeather(string location)
            {
                functionCalled = true;
                return "15 degrees Celsius";
            }

            var tools = new Tools();
            tools.AddFunction("GetCurrentWeather", GetCurrentWeather);

            var handlerMock = new Mock<HttpMessageHandler>();
            
            var response1 = new
            {
                candidates = new[]
                {
                    new
                    {
                        content = new
                        {
                            parts = new[]
                            {
                                new
                                {
                                    functionCall = new
                                    {
                                        name = "GetCurrentWeather",
                                        args = new { location = "London" }
                                    }
                                }
                            },
                            role = "model"
                        },
                        finishReason = "STOP",
                        index = 0
                    }
                }
            };

            var response2 = new
            {
                candidates = new[]
                {
                    new
                    {
                        content = new
                        {
                            parts = new[]
                            {
                                new { text = "The weather in London is 15 degrees Celsius." }
                            },
                            role = "model"
                        },
                        finishReason = "STOP",
                        index = 0
                    }
                }
            };

            var json1 = JsonSerializer.Serialize(response1);
            var json2 = JsonSerializer.Serialize(response2);

            handlerMock
                .Protected()
                .SetupSequence<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(json1)
                })
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(json2)
                });

            var httpClientFactoryMock = new Mock<IHttpClientFactory>();
            httpClientFactoryMock.Setup(x => x.CreateClient(It.IsAny<string>()))
                .Returns(() => new HttpClient(handlerMock.Object) { BaseAddress = new Uri("https://generativelanguage.googleapis.com") });

            var model = new GenerativeModel(apiKey: "AIzaSyDaN098765432109876543210987654321", httpClientFactory: httpClientFactoryMock.Object);

            var chat = model.StartChat(tools: tools, enableAutomaticFunctionCalling: true);

            // Act
            var response = await chat.SendMessage("What is the weather in London?");

            // Assert
            functionCalled.ShouldBeTrue();
            response.Text.ShouldBe("The weather in London is 15 degrees Celsius.");
            
            // Verify History
            // History should contain: 
            // 0: User message ("What is the weather...")
            // 1: Model response (FunctionCall)
            // 2: User/Function response (FunctionResponse)
            // 3: Model response (Text)
            
            chat.History.Count.ShouldBe(4);
            chat.History[1].Parts[0].FunctionCall.ShouldNotBeNull();
            chat.History[1].Parts[0].FunctionCall.Name.ShouldBe("GetCurrentWeather");
            
            chat.History[2].Parts[0].FunctionResponse.ShouldNotBeNull();
            chat.History[2].Parts[0].FunctionResponse.Name.ShouldBe("GetCurrentWeather");
            // chat.History[2].Role.ShouldBe(Role.Function); // Verify this matches implementation
            
            chat.History[3].Text.ShouldBe("The weather in London is 15 degrees Celsius.");
        }
    }
}
