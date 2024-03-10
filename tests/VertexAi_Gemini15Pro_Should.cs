#if NET472_OR_GREATER || NETSTANDARD2_0
using System;
using System.Collections.Generic;
using System.Linq;
#endif
using FluentAssertions;
using Mscc.GenerativeAI;
using Xunit;
using Xunit.Abstractions;

namespace Test.Mscc.GenerativeAI
{
    [Collection(nameof(ConfigurationFixture))]
    public class VertexAi_Gemini15Pro_Should
    {
        private readonly ITestOutputHelper output;
        private readonly ConfigurationFixture fixture;
        private readonly string model = Model.Gemini15Pro;

        public VertexAi_Gemini15Pro_Should(ITestOutputHelper output, ConfigurationFixture fixture)
        {
            this.output = output;
            this.fixture = fixture;
        }

        [Fact]
        public void Initialize_Vertex()
        {
            // Arrange

            // Act
            var vertex = new VertexAI(projectId: fixture.ProjectId, region: fixture.Region);

            // Assert
            vertex.Should().NotBeNull();
        }

        [Fact]
        public void Initialize_Default_Model()
        {
            // Arrange
            var vertex = new VertexAI(projectId: fixture.ProjectId, region: fixture.Region);

            // Act
            var model = vertex.GenerativeModel();

            // Assert
            model.Should().NotBeNull();
            model.Name().Should().Be(Model.Gemini10Pro);
        }

        [Fact]
        public void Initialize_Model()
        {
            // Arrange
            var vertex = new VertexAI(projectId: fixture.ProjectId, region: fixture.Region);

            // Act
            var model = vertex.GenerativeModel(model: this.model);

            // Assert
            model.Should().NotBeNull();
            model.Name().Should().Be(Model.Gemini15Pro);
        }

        [Fact]
        public async void List_Models()
        {
            // Arrange
            var vertex = new VertexAI(projectId: fixture.ProjectId, region: fixture.Region);
            var model = vertex.GenerativeModel(model: this.model);
            model.AccessToken = fixture.AccessToken;

            // Act & Assert
            await Assert.ThrowsAsync<NotSupportedException>(() => model.ListModels());
        }

        [Theory]
        [InlineData(Model.GeminiPro)]
        [InlineData(Model.GeminiProVision)]
        [InlineData(Model.BisonText)]
        [InlineData(Model.BisonChat)]
        public async void Get_Model_Information(string modelName)
        {
            // Arrange
            var vertex = new VertexAI(projectId: fixture.ProjectId, region: fixture.Region);
            var model = vertex.GenerativeModel(model: this.model);
            model.AccessToken = fixture.AccessToken;

            // Act & Assert
            await Assert.ThrowsAsync<NotSupportedException>(() => model.GetModel(model: modelName));
        }

        [Fact]
        public async void Generate_Content()
        {
            // Arrange
            var prompt = "Write a story about a magic backpack.";
            var vertex = new VertexAI(projectId: fixture.ProjectId, region: fixture.Region);
            var model = vertex.GenerativeModel(model: this.model);
            model.AccessToken = fixture.AccessToken;

            // Act
            var response = await model.GenerateContent(prompt);

            // Assert
            response.Should().NotBeNull();
            response.Candidates.Should().NotBeNull().And.HaveCount(1);
            response.Text.Should().NotBeEmpty();
            output.WriteLine(response?.Text);
        }

        [Fact]
        public async void Generate_Content_MultiplePrompt()
        {
            // Arrange
            var vertex = new VertexAI(projectId: fixture.ProjectId, region: fixture.Region);
            var model = vertex.GenerativeModel(model: this.model);
            model.AccessToken = fixture.AccessToken;
            var parts = new List<IPart>
            {
                new TextData { Text = "What is x multiplied by 2?" },
                new TextData { Text = "x = 42" }
            };

            // Act
            var response = await model.GenerateContent(parts);

            // Assert
            response.Should().NotBeNull();
            response.Candidates.Should().NotBeNull().And.HaveCount(1);
            response.Text.Should().Be("84");
            output.WriteLine($"Result: {response?.Text}");
        }

        [Fact]
        public async void Generate_Content_Request()
        {
            // Arrange
            var prompt = "Write a story about a magic backpack.";
            var vertex = new VertexAI(projectId: fixture.ProjectId, region: fixture.Region);
            var model = vertex.GenerativeModel(model: this.model);
            model.AccessToken = fixture.AccessToken;
            var request = new GenerateContentRequest { Contents = new List<Content>() };
            request.Contents.Add(new Content
            {
                Role = Role.User,
                Parts = new List<IPart> { new TextData { Text = prompt } }
            });

            // Act
            var response = await model.GenerateContent(request);

            // Assert
            response.Should().NotBeNull();
            response.Candidates.Should().NotBeNull().And.HaveCount(1);
            response.Text.Should().NotBeEmpty();
            output.WriteLine(response?.Text);
        }

        [Fact]
        public async void Generate_Content_Stream()
        {
            // Arrange
            var prompt = "How are you doing today?";
            var vertex = new VertexAI(projectId: fixture.ProjectId, region: fixture.Region);
            var model = vertex.GenerativeModel(model: this.model);
            model.AccessToken = fixture.AccessToken;

            // Act
            var response = model.GenerateContentStream(prompt);

            // Assert
            // response.Should().NotBeNull().And.HaveCountGreaterThanOrEqualTo(1);
            // response.FirstOrDefault().Should().NotBeNull();
            // response.ForEach(x => output.WriteLine(x.Text));
            // response.LastOrDefault().UsageMetadata.Should().NotBeNull();
            // output.WriteLine($"PromptTokenCount: {response.LastOrDefault().UsageMetadata.PromptTokenCount}");
            // output.WriteLine($"CandidatesTokenCount: {response.LastOrDefault().UsageMetadata.CandidatesTokenCount}");
            // output.WriteLine($"TotalTokenCount: {response.LastOrDefault().UsageMetadata.TotalTokenCount}");
        }

        [Fact]
        public async void Generate_Content_Stream_Request()
        {
            // Arrange
            var prompt = "How are you doing today?";
            var vertex = new VertexAI(projectId: fixture.ProjectId, region: fixture.Region);
            var model = vertex.GenerativeModel(model: this.model);
            model.AccessToken = fixture.AccessToken;
            var request = new GenerateContentRequest { Contents = new List<Content>() };
            request.Contents.Add(new Content
            {
                Role = Role.User,
                Parts = new List<IPart> { new TextData { Text = prompt } }
            });

            // Act
            var response = model.GenerateContentStream(request);

            // Assert
            // response.Should().NotBeNull().And.HaveCountGreaterThanOrEqualTo(1);
            // response.FirstOrDefault().Should().NotBeNull();
            // response.ForEach(x => output.WriteLine(x.Text));
            // response.LastOrDefault().UsageMetadata.Should().NotBeNull();
            // output.WriteLine($"PromptTokenCount: {response.LastOrDefault().UsageMetadata.PromptTokenCount}");
            // output.WriteLine($"CandidatesTokenCount: {response.LastOrDefault().UsageMetadata.CandidatesTokenCount}");
            // output.WriteLine($"TotalTokenCount: {response.LastOrDefault().UsageMetadata.TotalTokenCount}");
        }

        [Theory]
        [InlineData("How are you doing today?", 6)]
        [InlineData("What kind of fish is this?", 7)]
        [InlineData("Write a story about a magic backpack.", 8)]
        [InlineData("Write an extended story about a magic backpack.", 9)]
        public async void Count_Tokens(string prompt, int expected)
        {
            // Arrange
            var vertex = new VertexAI(projectId: fixture.ProjectId, region: fixture.Region);
            var model = vertex.GenerativeModel(model: this.model);
            model.AccessToken = fixture.AccessToken;
            var request = new GenerateContentRequest { Contents = new List<Content>() };
            request.Contents.Add(new Content
            {
                Role = Role.User,
                Parts = new List<IPart> { new TextData { Text = prompt } }
            });

            // Act
            var response = await model.CountTokens(prompt);

            // Assert
            response.Should().NotBeNull();
            response.TotalTokens.Should().Be(expected);
            output.WriteLine($"Tokens: {response?.TotalTokens}");
        }

        [Theory]
        [InlineData("How are you doing today?", 6)]
        [InlineData("What kind of fish is this?", 7)]
        [InlineData("Write a story about a magic backpack.", 8)]
        [InlineData("Write an extended story about a magic backpack.", 9)]
        public async void Count_Tokens_Request(string prompt, int expected)
        {
            // Arrange
            var vertex = new VertexAI(projectId: fixture.ProjectId, region: fixture.Region);
            var model = vertex.GenerativeModel(model: this.model);
            model.AccessToken = fixture.AccessToken;
            var request = new GenerateContentRequest { Contents = new List<Content>() };
            request.Contents.Add(new Content
            {
                Role = Role.User,
                Parts = new List<IPart> { new TextData { Text = prompt } }
            });

            // Act
            var response = await model.CountTokens(request);

            // Assert
            response.Should().NotBeNull();
            response.TotalTokens.Should().Be(expected);
            output.WriteLine($"Tokens: {response?.TotalTokens}");
        }

        [Fact]
        public async void Start_Chat_Streaming()
        {
            // Arrange
            var vertex = new VertexAI(projectId: fixture.ProjectId, region: fixture.Region);
            var model = vertex.GenerativeModel(model: this.model);
            model.AccessToken = fixture.AccessToken;
            var chat = model.StartChat();
            var chatInput1 = "How can I learn more about C#?";

            // Act
            //var response = await chat.SendMessageStream(chatInput1);

            //// Assert
            //response.Should().NotBeNull();
        }

        [Fact]
        public async void Function_Calling_Chat()
        {
            // Arrange
            var vertex = new VertexAI(projectId: fixture.ProjectId, region: fixture.Region);
            var model = vertex.GenerativeModel(model: this.model);
            model.AccessToken = fixture.AccessToken;
            var chat = model.StartChat(tools: new List<Tool>());
            var chatInput1 = "What is the weather in Boston?";

            // Act
            //var result1 = await chat.SendMessageStream(chatInput1);
            //var response1 = await result1.Response;
            //var result2 = await chat.SendMessageStream(new List<IPart> { new FunctionResponse() });
            //var response2 = await result2.Response;

            //// Assert
            //response1.Should().NotBeNull();
            //response.Candidates.Should().NotBeNull().And.HaveCount(1);
            //response.Text.Should().NotBeEmpty();
        }

        [Fact]
        public async void Function_Calling_ContentStream()
        {
            // Arrange
            var vertex = new VertexAI(projectId: fixture.ProjectId, region: fixture.Region);
            var model = vertex.GenerativeModel(model: this.model);
            model.AccessToken = fixture.AccessToken;
            var request = new GenerateContentRequest
            {
                Contents = new List<Content>(),
                Tools = new List<Tool> { }
            };
            request.Contents.Add(new Content
            {
                Role = Role.User,
                Parts = new List<IPart> { new TextData { Text = "What is the weather in Boston?" } }
            });
            request.Contents.Add(new Content
            {
                Role = Role.Model,
                Parts = new List<IPart> { new FunctionCall { Name = "get_current_weather", Args = new { location = "Boston" } } }
            });
            request.Contents.Add(new Content
            {
                Role = Role.Function,
                Parts = new List<IPart> { new FunctionResponse() }
            });

            // Act
            var response = model.GenerateContentStream(request);

            // Assert
            response.Should().NotBeNull();
            //response.Candidates.Should().NotBeNull().And.HaveCount(1);
            //response.Text.Should().NotBeEmpty();
        }
    }
}
