using FluentAssertions;
using Microsoft.Extensions.AI;
using Mscc.GenerativeAI;
using Mscc.GenerativeAI.Microsoft;
using System.ComponentModel;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;
using mea = Microsoft.Extensions.AI;

namespace Test.Mscc.GenerativeAI.Microsoft
{
    [Collection(nameof(ConfigurationFixture))]
    public class GeminiChatClient_Should
    {
        private readonly ITestOutputHelper _output;
        private readonly ConfigurationFixture _fixture;

        public GeminiChatClient_Should(ITestOutputHelper output, ConfigurationFixture fixture)
        {
            _output = output;
            _fixture = fixture;
        }

        [Fact]
        public async Task Handle_AIFunction_Tool()
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
                new(mea.ChatRole.User, "What is the user's name and age?")
            };

            // Act
            var response = await chatClient.GetResponseAsync(chatHistory, options);

            // Assert
            response.Should().NotBeNull();
            response.Messages.Should().NotBeNull().And.HaveCount(1);
            response.Messages[0].Contents.Should().NotBeNull().And.HaveCount(1);
            var functionCallContent = response.Messages[0].Contents[0] as mea.FunctionCallContent;
            functionCallContent.Should().NotBeNull();
            functionCallContent.Name.Should().Be("get_user_information");
        }

        [Description("Get basic information of the current user")]
        private static string GetUserInformation() => @"{ ""Name"": ""John Doe"", ""Age"": 42 }";
    }
}
