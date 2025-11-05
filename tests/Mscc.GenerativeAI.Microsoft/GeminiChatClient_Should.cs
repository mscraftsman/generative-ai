using FluentAssertions;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.Logging;
using Mscc.GenerativeAI;
using Mscc.GenerativeAI.Microsoft;
using Neovolve.Logging.Xunit;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;
using mea = Microsoft.Extensions.AI;

namespace Test.Mscc.GenerativeAI.Microsoft
{
    [Collection(nameof(ConfigurationFixture))]
    public class GeminiChatClient_Should : LoggingTestsBase
    {
        private readonly ITestOutputHelper _output;
        private readonly ConfigurationFixture _fixture;

        public GeminiChatClient_Should(ITestOutputHelper output, ConfigurationFixture fixture)
            : base(output, LogLevel.Trace)
        {
            _output = output;
            _fixture = fixture;
        }

        [Theory]
        [InlineData("What is the user's name and age?")]
        [InlineData("Who am I?")]
        public async Task Handle_AIFunction_Tool(string prompt)
        {
            // Arrange
            var model = Model.Gemini25Pro;
            var gemini = new GeminiChatClient(_fixture.ApiKey, model);
            IChatClient chatClient = new ChatClientBuilder(gemini)
                .UseFunctionInvocation()
                .Build();

            AIFunction getUserInformationTool =
                AIFunctionFactory.Create(GetUserInformation, name: "get_user_information");

            var options = new ChatOptions { Tools = [getUserInformationTool] };

            var chatHistory = new List<mea.ChatMessage> { new(ChatRole.User, prompt) };

            // Act
            var response = await chatClient.GetResponseAsync(chatHistory, options);

            // Assert
            response.Should().NotBeNull();
            response.Messages.Should().NotBeNull().And.HaveCount(3);
            response.Messages[0].Contents.Should().NotBeNull().And.HaveCountGreaterOrEqualTo(1);
            var functionCallContent = response.Messages[0].Contents[0] as FunctionCallContent;
            functionCallContent.Should().NotBeNull();
            functionCallContent?.Name.Should().Be("get_user_information");
            _output.WriteLine(response.Text);
        }

        [Theory]
        [InlineData("What is the user's name and age?")]
        [InlineData("Who am I?")]
        public async Task Handle_AIFunction_Tool_Streaming(string prompt)
        {
            // Arrange
            var model = Model.Gemini25Pro;
            var gemini = new GeminiChatClient(_fixture.ApiKey, model);
            IChatClient chatClient = new ChatClientBuilder(gemini)
                .UseFunctionInvocation()
                .Build();

            AIFunction getUserInformationTool =
                AIFunctionFactory.Create(GetUserInformation, name: "get_user_information");

            var options = new ChatOptions { Tools = [getUserInformationTool] };

            var chatHistory = new List<mea.ChatMessage> { new(ChatRole.User, prompt) };

            // Act
            IAsyncEnumerable<ChatResponseUpdate> responseStream =
                chatClient.GetStreamingResponseAsync(chatHistory, options);

            // Assert
            responseStream.Should().NotBeNull();
            await foreach (ChatResponseUpdate responseUpdate in responseStream)
            {
                responseUpdate.Should().NotBeNull();
                // responseUpdate.Messages.Should().NotBeNull().And.HaveCount(3);
                // responseUpdate.Messages[0].Contents.Should().NotBeNull().And.HaveCountGreaterOrEqualTo(1);
                // var functionCallContent = responseUpdate.Messages[0].Contents[0] as mea.FunctionCallContent;
                // functionCallContent.Should().NotBeNull();
                // functionCallContent?.Name.Should().Be("get_user_information");
                _output.WriteLine(responseUpdate.Text);
            }
        }

        [Fact]
        public async Task Dotnet_Genai_Sample()
        {
            // assuming credentials are set up in environment variables.
            IChatClient chatClient = new GeminiClient(_fixture.ApiKey)
                .AsIChatClient("gemini-2.0-flash")
                .AsBuilder()
                .UseFunctionInvocation()
                .UseOpenTelemetry()
                .Build();

            ChatOptions options = new()
            {
                Tools = [AIFunctionFactory.Create(
                        ([Description("The name of the person whose age is to be retrieved")] string personName) =>
                            personName switch
                            {
                                "Alice" => 30,
                                "Bob" => 25,
                                _ => 35
                            }, "get_person_age", "Gets the age of the specified person")
                ]
            };

            await foreach (var update in chatClient.GetStreamingResponseAsync(
                               "How much older is Alice than Bob?", options))
            {
                _output.WriteLine(update.Text);
            }
        }

        [Description("Get basic information of the current user")]
        private static string GetUserInformation() => @"{ ""Name"": ""John Doe"", ""Age"": 42 }";
    }
}