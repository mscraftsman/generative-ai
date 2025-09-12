using FluentAssertions;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.Logging;
using Mscc.GenerativeAI;
using Mscc.GenerativeAI.Microsoft;
using Neovolve.Logging.Xunit;
using System.Collections.Generic;
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
            mea.IChatClient chatClient = new mea.ChatClientBuilder(gemini)
                .UseFunctionInvocation()
                .Build();

            mea.AIFunction getUserInformationTool = mea.AIFunctionFactory.Create(GetUserInformation, name: "get_user_information");

            var options = new mea.ChatOptions
            {
                Tools = [getUserInformationTool]
            };

            var chatHistory = new System.Collections.Generic.List<mea.ChatMessage>
            {
                new(mea.ChatRole.User, prompt)
            };

            // Act
            var response = await chatClient.GetResponseAsync(chatHistory, options);

            // Assert
            response.Should().NotBeNull();
            response.Messages.Should().NotBeNull().And.HaveCount(3);
            response.Messages[0].Contents.Should().NotBeNull().And.HaveCountGreaterOrEqualTo(1);
            var functionCallContent = response.Messages[0].Contents[0] as mea.FunctionCallContent;
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
            mea.IChatClient chatClient = new mea.ChatClientBuilder(gemini)
                .UseFunctionInvocation()
                .Build();

            mea.AIFunction getUserInformationTool = mea.AIFunctionFactory.Create(GetUserInformation, name: "get_user_information");

            var options = new mea.ChatOptions
            {
                Tools = [getUserInformationTool]
            };

            var chatHistory = new System.Collections.Generic.List<mea.ChatMessage>
            {
                new(mea.ChatRole.User, prompt)
            };

            // Act
            IAsyncEnumerable<ChatResponseUpdate> responseStream = chatClient.GetStreamingResponseAsync(chatHistory, options);
	
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

        [System.ComponentModel.Description("Get basic information of the current user")]
        private static string GetUserInformation() => @"{ ""Name"": ""John Doe"", ""Age"": 42 }";
    }
}
