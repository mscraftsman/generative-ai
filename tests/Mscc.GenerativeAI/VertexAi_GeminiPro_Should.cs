#if NET472_OR_GREATER || NETSTANDARD2_0
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
#endif
using FluentAssertions;
using Mscc.GenerativeAI;
using Xunit;
using Xunit.Abstractions;

namespace Test.Mscc.GenerativeAI
{
    [Collection(nameof(ConfigurationFixture))]
    public class VertexAi_GeminiPro_Should
    {
        private readonly ITestOutputHelper output;
        private readonly ConfigurationFixture fixture;
        private readonly string model = Model.Gemini10Pro;

        public VertexAi_GeminiPro_Should(ITestOutputHelper output, ConfigurationFixture fixture)
        {
            this.output = output;
            this.fixture = fixture;
        }
        
        [Fact]
        public void Initialize_VertexAI()
        {
            // Arrange

            // Act
            var vertexai = new VertexAI(projectId: fixture.ProjectId, region: fixture.Region);

            // Assert
            vertexai.Should().NotBeNull();
        }

        [Fact]
        public void Initialize_Using_VertexAI()
        {
            // Arrange
            var expected = Environment.GetEnvironmentVariable("GOOGLE_AI_MODEL") ?? Model.Gemini10Pro;
            var vertexai = new VertexAI(projectId: fixture.ProjectId, region: fixture.Region);

            // Act
            var model = vertexai.GenerativeModel();
            
            // Assert
            model.Should().NotBeNull();
            model.Name.Should().Be($"{expected.SanitizeModelName()}");
        }

        [Fact]
        public void Initialize_Default_Model()
        {
            // Arrange
            var vertexai = new VertexAI(projectId: fixture.ProjectId, region: fixture.Region);

            // Act
            var model = vertexai.GenerativeModel();

            // Assert
            model.Should().NotBeNull();
            model.Name.Should().Be($"{Model.Gemini10Pro.SanitizeModelName()}");
        }

        [Fact]
        public void Initialize_Model()
        {
            // Arrange
            var vertexai = new VertexAI(projectId: fixture.ProjectId, region: fixture.Region);

            // Act
            var model = vertexai.GenerativeModel(model: this.model);

            // Assert
            model.Should().NotBeNull();
            model.Name.Should().Be($"{Model.Gemini10Pro.SanitizeModelName()}");
        }

        [Fact]
        public async void List_Models()
        {
            // Arrange
            var vertexai = new VertexAI(projectId: fixture.ProjectId, region: fixture.Region);
            var model = vertexai.GenerativeModel(model: this.model);
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
            var vertexai = new VertexAI(projectId: fixture.ProjectId, region: fixture.Region);
            var model = vertexai.GenerativeModel(model: this.model);
            model.AccessToken = fixture.AccessToken;

            // Act & Assert
            await Assert.ThrowsAsync<NotSupportedException>(() => model.GetModel(model: modelName));
        }

        [Fact]
        public async void Generate_Content()
        {
            // Arrange
            var prompt = "Write a story about a magic backpack.";
            var vertexai = new VertexAI(projectId: fixture.ProjectId, region: fixture.Region);
            var model = vertexai.GenerativeModel(model: this.model);
            model.AccessToken = fixture.AccessToken;

            // Act
            var response = await model.GenerateContent(prompt);

            // Assert
            response.Should().NotBeNull();
            response.Candidates.Should().NotBeNull().And.HaveCount(1);
            response.Text.Should().NotBeEmpty();
            output.WriteLine(response?.Text);
            response.UsageMetadata.Should().NotBeNull();
            output.WriteLine($"PromptTokenCount: {response?.UsageMetadata?.PromptTokenCount}");
            output.WriteLine($"CandidatesTokenCount: {response?.UsageMetadata?.CandidatesTokenCount}");
            output.WriteLine($"TotalTokenCount: {response?.UsageMetadata?.TotalTokenCount}");
        }

        [Fact]
        public async void Generate_Content_With_SafetySettings()
        {
            // Arrange
            var prompt = "Tell me something dangerous.";
            var safetySettings = new List<SafetySetting>()
            {
                new()
                {
                    Category = HarmCategory.HarmCategoryDangerousContent,
                    Threshold = HarmBlockThreshold.BlockLowAndAbove
                }
            };
            var generationConfig = new GenerationConfig() 
                { MaxOutputTokens = 256 };
            var vertexai = new VertexAI(projectId: fixture.ProjectId, region: fixture.Region);
            var model = vertexai.GenerativeModel(model: this.model, generationConfig, safetySettings);
            model.AccessToken = fixture.AccessToken;

            // Act
            var response = await model.GenerateContent(prompt);

            // Assert
            response.Should().NotBeNull();
            response.Candidates.Should().NotBeNull().And.HaveCount(1);
            response.Candidates[0].FinishReason.Should().Be(FinishReason.Safety);
            response.Text.Should().BeNull();
            output.WriteLine("This response stream terminated due to safety concerns.");
        }

        [Fact]
        public async void Generate_Content_MultiplePrompt()
        {
            // Arrange
            var vertexai = new VertexAI(projectId: fixture.ProjectId, region: fixture.Region);
            var model = vertexai.GenerativeModel(model: this.model);
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
            var vertexai = new VertexAI(projectId: fixture.ProjectId, region: fixture.Region);
            var model = vertexai.GenerativeModel(model: this.model);
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
        public async void GenerateContent_WithRequest_MultipleCandidates_ThrowsHttpRequestException()
        {
            // Arrange
            var prompt = "Write a short poem about koi fish.";
            var vertexai = new VertexAI(projectId: fixture.ProjectId, region: fixture.Region);
            var model = vertexai.GenerativeModel(model: this.model);
            model.AccessToken = fixture.AccessToken;
            var request = new GenerateContentRequest
            {
                Contents = new List<Content>(), 
                GenerationConfig = new GenerationConfig()
                {
                    CandidateCount = 3
                }
            };
            request.Contents.Add(new Content
            {
                Role = Role.User,
                Parts = new List<IPart> { new TextData { Text = prompt } }
            });

            // Act & Assert
            await Assert.ThrowsAsync<HttpRequestException>(() => model.GenerateContent(request));
        }

        [Fact]
        public async void Generate_Content_Stream()
        {
            // Arrange
            var prompt = "How are you doing today?";
            var vertexai = new VertexAI(projectId: fixture.ProjectId, region: fixture.Region);
            var model = vertexai.GenerativeModel(model: this.model);
            model.AccessToken = fixture.AccessToken;

            // Act
            var responseStream = model.GenerateContentStream(prompt);

            // Assert
            responseStream.Should().NotBeNull();
            await foreach (var response in responseStream)
            {
                response.Should().NotBeNull();
                response.Candidates.Should().NotBeNull().And.HaveCount(1);
                response.Text.Should().NotBeEmpty();
                output.WriteLine(response?.Text);
                // response.UsageMetadata.Should().NotBeNull();
                // output.WriteLine($"PromptTokenCount: {response?.UsageMetadata?.PromptTokenCount}");
                // output.WriteLine($"CandidatesTokenCount: {response?.UsageMetadata?.CandidatesTokenCount}");
                // output.WriteLine($"TotalTokenCount: {response?.UsageMetadata?.TotalTokenCount}");
            }
        }

        [Fact]
        public async void Generate_Content_Stream_With_SafetySettings()
        {
            // Arrange
            var prompt = "Tell me something dangerous.";
            var safetySettings = new List<SafetySetting>()
            {
                new()
                {
                    Category = HarmCategory.HarmCategoryDangerousContent,
                    Threshold = HarmBlockThreshold.BlockLowAndAbove
                }
            };
            var generationConfig = new GenerationConfig() 
                { MaxOutputTokens = 256 };
            var vertexai = new VertexAI(projectId: fixture.ProjectId, region: fixture.Region);
            var model = vertexai.GenerativeModel(model: this.model, generationConfig, safetySettings);
            model.AccessToken = fixture.AccessToken;

            // Act
            var responseStream = model.GenerateContentStream(prompt);

            // Assert
            responseStream.Should().NotBeNull();
            await foreach (var response in responseStream)
            {
                response.Should().NotBeNull();
                response.Candidates.Should().NotBeNull().And.HaveCount(1);
                response.Candidates[0].FinishReason.Should().BeOneOf(FinishReason.Safety);
                if (response.Candidates[0].FinishReason == FinishReason.Safety)
                {
                    response.Text.Should().BeNull();
                    output.WriteLine("This response stream terminated due to safety concerns.");
                }
                else
                {
                    response.Text.Should().NotBeEmpty();
                    output.WriteLine(response?.Text);
                }
            }
        }

        [Fact]
        public async void Generate_Content_Stream_Request()
        {
            // Arrange
            var prompt = "How are you doing today?";
            var vertexai = new VertexAI(projectId: fixture.ProjectId, region: fixture.Region);
            var model = vertexai.GenerativeModel(model: this.model);
            model.AccessToken = fixture.AccessToken;
            var request = new GenerateContentRequest { Contents = new List<Content>() };
            request.Contents.Add(new Content
            {
                Role = Role.User,
                Parts = new List<IPart> { new TextData { Text = prompt } }
            });

            // Act
            var responseStream = model.GenerateContentStream(request);

            // Assert
            responseStream.Should().NotBeNull();
            await foreach (var response in responseStream)
            {
                response.Should().NotBeNull();
                response.Candidates.Should().NotBeNull().And.HaveCount(1);
                response.Text.Should().NotBeEmpty();
                output.WriteLine(response?.Text);
                // response.UsageMetadata.Should().NotBeNull();
                // output.WriteLine($"PromptTokenCount: {response?.UsageMetadata?.PromptTokenCount}");
                // output.WriteLine($"CandidatesTokenCount: {response?.UsageMetadata?.CandidatesTokenCount}");
                // output.WriteLine($"TotalTokenCount: {response?.UsageMetadata?.TotalTokenCount}");
            }
        }

        [Theory]
        [InlineData("How are you doing today?", 6)]
        [InlineData("What kind of fish is this?", 7)]
        [InlineData("Write a story about a magic backpack.", 8)]
        [InlineData("Write an extended story about a magic backpack.", 9)]
        public async void Count_Tokens(string prompt, int expected)
        {
            // Arrange
            var vertexai = new VertexAI(projectId: fixture.ProjectId, region: fixture.Region);
            var model = vertexai.GenerativeModel(model: this.model);
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
            var vertexai = new VertexAI(projectId: fixture.ProjectId, region: fixture.Region);
            var model = vertexai.GenerativeModel(model: this.model);
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
        public async void Start_Chat()
        {
            // Arrange
            var vertexai = new VertexAI(projectId: fixture.ProjectId, region: fixture.Region);
            var model = vertexai.GenerativeModel(model: this.model);
            model.AccessToken = fixture.AccessToken;
            var chat = model.StartChat();
            var prompt = "How can I learn more about C#?";

            // Act
            var response = await chat.SendMessage(prompt);

            // Assert
            response.Should().NotBeNull();
            response.Candidates.Should().NotBeNull().And.HaveCount(1);
            response.Text.Should().NotBeEmpty();
            output.WriteLine(response?.Text);
        }

        [Fact]
        // Refs:
        // https://cloud.google.com/vertexai-ai/generative-ai/docs/multimodal/send-chat-prompts-gemini
        public async void Start_Chat_Multiple_Prompts()
        {
            // Arrange
            var vertexai = new VertexAI(projectId: fixture.ProjectId, region: fixture.Region);
            var model = vertexai.GenerativeModel(model: this.model);
            model.AccessToken = fixture.AccessToken;
            var chat = model.StartChat();

            // Act
            var prompt = "Hello.";
            var response = await chat.SendMessage(prompt);
            output.WriteLine(prompt);
            output.WriteLine(response?.Text);
            prompt = "What are all the colors in a rainbow?";
            response = await chat.SendMessage(prompt);
            output.WriteLine(prompt);
            output.WriteLine(response?.Text);
            prompt = "Why does it appear when it rains?";
            response = await chat.SendMessage(prompt);
            output.WriteLine(prompt);
            output.WriteLine(response?.Text);

            // Assert
            response.Should().NotBeNull();
            response.Candidates.Should().NotBeNull().And.HaveCount(1);
            response.Text.Should().NotBeEmpty();
        }

        [Fact]
        public async void Start_Chat_Streaming()
        {
            // Arrange
            var vertexai = new VertexAI(projectId: fixture.ProjectId, region: fixture.Region);
            var model = vertexai.GenerativeModel(model: this.model);
            model.AccessToken = fixture.AccessToken;
            var chat = model.StartChat();
            var prompt = "How can I learn more about C#?";

            // Act
            var responseStream = chat.SendMessageStream(prompt);

            // Assert
            responseStream.Should().NotBeNull();
            await foreach (var response in responseStream)
            {
                response.Should().NotBeNull();
                response.Candidates.Should().NotBeNull().And.HaveCount(1);
                response.Text.Should().NotBeEmpty();
                output.WriteLine(response?.Text);
                // response.UsageMetadata.Should().NotBeNull();
                // output.WriteLine($"PromptTokenCount: {response?.UsageMetadata?.PromptTokenCount}");
                // output.WriteLine($"CandidatesTokenCount: {response?.UsageMetadata?.CandidatesTokenCount}");
                // output.WriteLine($"TotalTokenCount: {response?.UsageMetadata?.TotalTokenCount}");
            }
        }

        [Fact]
        public async void Function_Calling_Chat()
        {
            // Arrange
            var prompt = "What is the weather in Boston?";
            var functionDeclarations = new List<FunctionDeclaration>() 
            { new() {
                Name = "get_current_weather",
                Description = "get weather in a given location",
                Parameters = new() {
                    Type = ParameterType.Object,
                    Properties = new {
                        Location = new { Type = ParameterType.String }, 
                        Unit = new {
                            Type = ParameterType.String, 
                            Enum = (string[])["celsius", "fahrenheit"]
                        }
                    },
                    Required = ["location"]
                }
            }};
            var functionResponses = new List<Part>()
            {
                new() {
                FunctionResponse = new()
                {
                    Name = "get_current_weather",
                    Response = new { Name = "get_current_weather", Content = new { Weather = "super nice" }}
                }
            }};
            var vertexai = new VertexAI(projectId: fixture.ProjectId, region: fixture.Region);
            var model = vertexai.GenerativeModel(model: this.model);
            model.AccessToken = fixture.AccessToken;
            var chat = model.StartChat(tools: new()
            {
                new Tool() {FunctionDeclarations = functionDeclarations}
            });

            // Act & Assert
            output.WriteLine(prompt);
            var result1 = chat.SendMessageStream(prompt);
            await foreach (var response in result1)
            {
                response.Should().NotBeNull();
                response.Candidates.Should().NotBeNull().And.HaveCount(1);
                response.Candidates[0].Content.Parts[0].FunctionCall.Should().NotBeNull();
                response?.Candidates?[0]?.Content?.Parts[0]?.FunctionCall?.Should().NotBeNull();
                output.WriteLine(response?.Candidates?[0]?.Content?.Parts[0]?.FunctionCall?.Name);
                output.WriteLine(response?.Candidates?[0]?.Content?.Parts[0]?.FunctionCall?.Args?.ToString());
                output.WriteLine($"PromptTokenCount: {response?.UsageMetadata?.PromptTokenCount}");
                output.WriteLine($"CandidatesTokenCount: {response?.UsageMetadata?.CandidatesTokenCount}");
                output.WriteLine($"TotalTokenCount: {response?.UsageMetadata?.TotalTokenCount}");
            }

            var result2 = chat.SendMessageStream(functionResponses);
            await foreach (var response in result2)
            {
                response.Should().NotBeNull();
                response.Candidates.Should().NotBeNull().And.HaveCount(1);
                response.Text.Should().NotBeEmpty();
                output.WriteLine(response?.Text);
                output.WriteLine($"PromptTokenCount: {response?.UsageMetadata?.PromptTokenCount}");
                output.WriteLine($"CandidatesTokenCount: {response?.UsageMetadata?.CandidatesTokenCount}");
                output.WriteLine($"TotalTokenCount: {response?.UsageMetadata?.TotalTokenCount}");
            }
        }

        [Fact]
        public async void Function_Calling_ContentStream()
        {
            // Arrange
            var vertexai = new VertexAI(projectId: fixture.ProjectId, region: fixture.Region);
            var model = vertexai.GenerativeModel(model: this.model);
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
                Parts = new List<IPart> { new FunctionCall { Name = "get_current_weather", Args = new { Location = "Boston" } } }
            });
            request.Contents.Add(new Content
            {
                Role = Role.Function,
                Parts = new List<IPart> { new FunctionResponse() { 
                    Name = "get_current_weather",
                    Response = new { Name = "get_current_weather", Content = new { Weather = "slightly cloudy" }}
                }}
            });

            // Act
            var responseStream = model.GenerateContentStream(request);

            // Assert
            responseStream.Should().NotBeNull();
            await foreach (var response in responseStream)
            {
                response.Should().NotBeNull();
                response.Candidates.Should().NotBeNull().And.HaveCount(1);
                response.Text.Should().NotBeEmpty();
                output.WriteLine(response?.Text);
                output.WriteLine($"PromptTokenCount: {response?.UsageMetadata?.PromptTokenCount}");
                output.WriteLine($"CandidatesTokenCount: {response?.UsageMetadata?.CandidatesTokenCount}");
                output.WriteLine($"TotalTokenCount: {response?.UsageMetadata?.TotalTokenCount}");
            }
        }
    }
}
