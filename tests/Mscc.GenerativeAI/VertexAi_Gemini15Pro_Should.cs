﻿#if NET472_OR_GREATER || NETSTANDARD2_0
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
#endif
#if NET9_0
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
#endif
using FluentAssertions;
using Mscc.GenerativeAI;
using Xunit;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace Test.Mscc.GenerativeAI
{
    [Collection(nameof(ConfigurationFixture))]
    public class VertexAiGemini15ProShould(ITestOutputHelper output, ConfigurationFixture fixture)
    {
        private readonly string _model = Model.Gemini20FlashLitePreview0205;

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
            model.Name.Should().Be(Model.Gemini15Pro.SanitizeModelName());
        }

        [Fact]
        public void Initialize_Model()
        {
            // Arrange
            var vertex = new VertexAI(projectId: fixture.ProjectId, region: fixture.Region);

            // Act
            var model = vertex.GenerativeModel(model: _model);

            // Assert
            model.Should().NotBeNull();
            model.Name.Should().Be(_model.SanitizeModelName());
        }

        [Fact]
        public async Task List_Models()
        {
            // Arrange
            var vertex = new VertexAI(projectId: fixture.ProjectId, region: fixture.Region);
            var model = vertex.GenerativeModel(model: _model);
            model.AccessToken = fixture.AccessToken;

            // Act & Assert
            await Assert.ThrowsAsync<NotSupportedException>(() => model.ListModels());
        }

        [Theory]
        [InlineData(Model.Gemini15ProLatest)]
        [InlineData(Model.GeminiProVision)]
        [InlineData(Model.BisonText)]
        [InlineData(Model.BisonChat)]
        public async Task Get_Model_Information(string modelName)
        {
            // Arrange
            var vertex = new VertexAI(projectId: fixture.ProjectId, region: fixture.Region);
            var model = vertex.GenerativeModel(model: _model);
            model.AccessToken = fixture.AccessToken;

            // Act & Assert
            await Assert.ThrowsAsync<NotSupportedException>(() => model.GetModel(model: modelName));
        }

        [Fact]
        public async Task Generate_Content()
        {
            // Arrange
            var prompt = "Write a story about a magic backpack.";
            var vertex = new VertexAI(projectId: fixture.ProjectId, region: fixture.Region);
            var model = vertex.GenerativeModel(model: _model);
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
        public async Task Generate_Content_MultiplePrompt()
        {
            // Arrange
            var vertex = new VertexAI(projectId: fixture.ProjectId, region: fixture.Region);
            var model = vertex.GenerativeModel(model: _model);
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
        public async Task Generate_Content_Request()
        {
            // Arrange
            var prompt = "Write a story about a magic backpack.";
            var vertex = new VertexAI(projectId: fixture.ProjectId, region: fixture.Region);
            var model = vertex.GenerativeModel(model: _model);
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
        public async Task Generate_Content_Stream()
        {
            // Arrange
            var prompt = "How are you doing today?";
            var vertex = new VertexAI(projectId: fixture.ProjectId, region: fixture.Region);
            var model = vertex.GenerativeModel(model: _model);
            model.AccessToken = fixture.AccessToken;

            // Act
            var responseStream = model.GenerateContentStream(prompt);

            // Assert
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

        [Fact]
        public async Task Generate_Content_Stream_Request()
        {
            // Arrange
            var prompt = "How are you doing today?";
            var vertex = new VertexAI(projectId: fixture.ProjectId, region: fixture.Region);
            var model = vertex.GenerativeModel(model: _model);
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

        [Fact]
        public async Task Generate_Content_With_SafetySettings()
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
            var vertex = new VertexAI(projectId: fixture.ProjectId, region: fixture.Region);
            var model = vertex.GenerativeModel(model: _model, generationConfig, safetySettings);
            model.AccessToken = fixture.AccessToken;

            // Act
            var response = await model.GenerateContent(prompt);

            // Assert
            response.Should().NotBeNull();
            response.Candidates.Should().NotBeNull().And.HaveCount(1);
            response.Candidates[0].FinishReason.Should().Be(FinishReason.Safety);
            response.Text.Should().BeNull();
            // output.WriteLine(response?.Text);
        }

        [Fact]
        public async Task Generate_Content_SystemInstruction()
        {
            // Load a example model with system instructions
            // Arrange
            var systemInstruction =
                new Content(
                    "You are a friendly pirate. Speak like one. Your mission is to translate text in English to French.");
            var prompt = @"User input: I like bagels.
Answer:";
            var generationConfig = new GenerationConfig() 
                { 
                    Temperature = 0.9f,
                    TopP = 1.0f,
                    TopK = 32,
                    CandidateCount = 1,
                    MaxOutputTokens = 8192
                };
            var safetySettings = new List<SafetySetting>()
            {
                new() { Category = HarmCategory.HarmCategoryHarassment, Threshold = HarmBlockThreshold.BlockLowAndAbove },
                new() { Category = HarmCategory.HarmCategoryHateSpeech, Threshold = HarmBlockThreshold.BlockLowAndAbove },
                new() { Category = HarmCategory.HarmCategorySexuallyExplicit, Threshold = HarmBlockThreshold.BlockLowAndAbove },
                new() { Category = HarmCategory.HarmCategoryDangerousContent, Threshold = HarmBlockThreshold.BlockLowAndAbove }
            };
            var vertex = new VertexAI(projectId: fixture.ProjectId, region: fixture.Region);
            var model = vertex.GenerativeModel(model: _model, systemInstruction: systemInstruction);
            model.AccessToken = fixture.AccessToken;
            var request = new GenerateContentRequest(prompt, generationConfig, safetySettings);

            // Act
            var response = await model.GenerateContent(request);

            // Assert
            response.Should().NotBeNull();
            response.Candidates.Should().NotBeNull().And.HaveCount(1);
            response.Text.Should().NotBeNull();
            output.WriteLine($"{prompt} {response?.Text}");
            output.WriteLine($"Usage metadata: {response.UsageMetadata.TotalTokenCount}");
            output.WriteLine($"Finish reason: {response.Candidates[0].FinishReason}");
            output.WriteLine($"Safety settings: {response.Candidates[0].SafetyRatings}");
        }

        [Theory]
        [InlineData("How are you doing today?", 6)]
        [InlineData("What kind of fish is this?", 7)]
        [InlineData("Write a story about a magic backpack.", 8)]
        [InlineData("Write an extended story about a magic backpack.", 9)]
        public async Task Count_Tokens(string prompt, int expected)
        {
            // Arrange
            var vertex = new VertexAI(projectId: fixture.ProjectId, region: fixture.Region);
            var model = vertex.GenerativeModel(model: _model);
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
        public async Task Count_Tokens_Request(string prompt, int expected)
        {
            // Arrange
            var vertex = new VertexAI(projectId: fixture.ProjectId, region: fixture.Region);
            var model = vertex.GenerativeModel(model: _model);
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
        public Task Start_Chat_Streaming()
        {
            // Arrange
            var vertex = new VertexAI(projectId: fixture.ProjectId, region: fixture.Region);
            var model = vertex.GenerativeModel(model: _model);
            model.AccessToken = fixture.AccessToken;
            var chat = model.StartChat();
            var chatInput1 = "How can I learn more about C#?";

            // Act
            //var response = await chat.SendMessageStream(chatInput1);

            //// Assert
            //response.Should().NotBeNull();
            return Task.CompletedTask;
        }

        [Fact]
        public Task Function_Calling_Chat()
        {
            // Arrange
            var vertex = new VertexAI(projectId: fixture.ProjectId, region: fixture.Region);
            var model = vertex.GenerativeModel(model: _model);
            model.AccessToken = fixture.AccessToken;
            var chat = model.StartChat(tools: new Tools());
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
            return Task.CompletedTask;
        }

        [Fact]
        public Task Function_Calling_ContentStream()
        {
            // Arrange
            var vertex = new VertexAI(projectId: fixture.ProjectId, region: fixture.Region);
            var model = vertex.GenerativeModel(model: _model);
            model.AccessToken = fixture.AccessToken;
            var request = new GenerateContentRequest
            {
                Contents = new List<Content>(),
                Tools = new Tools { }
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
            return Task.CompletedTask;
            //response.Candidates.Should().NotBeNull().And.HaveCount(1);
            //response.Text.Should().NotBeEmpty();
        }

        [Fact]
        public void Initialize_Vertex_ExpressMode()
        {
            // Arrange

            // Act
            var vertex = new VertexAI(apiKey: fixture.ApiKey);

            // Assert
            vertex.Should().NotBeNull();
        }

        [Fact]
        public void Initialize_Default_Model_ExpressMode()
        {
            // Arrange
            var vertex = new VertexAI(apiKey: fixture.ApiKey);

            // Act
            var model = vertex.GenerativeModel();

            // Assert
            model.Should().NotBeNull();
            model.Name.Should().Be(Model.Gemini15Pro.SanitizeModelName());
        }

        [Fact]
        public void Initialize_Model_ExpressMode()
        {
            // Arrange
            var vertex = new VertexAI(apiKey: fixture.ApiKey);

            // Act
            var model = vertex.GenerativeModel(model: _model);

            // Assert
            model.Should().NotBeNull();
            model.Name.Should().Be(_model.SanitizeModelName());
        }

        [Fact]
        public async Task Generate_Content_ExpressMode()
        {
            // Arrange
            var prompt = "Explain bubble sort to me.";
            var vertex = new VertexAI(apiKey: fixture.ApiKey);
            var model = vertex.GenerativeModel(model: _model);

            // Act
            var response = await model.GenerateContent(prompt);

            // Assert
            response.Should().NotBeNull();
            response.Candidates.Should().NotBeNull().And.HaveCount(1);
            response.Text.Should().NotBeEmpty();
            output.WriteLine(response?.Text);
        }

        [Fact]
        public async Task Generate_Content_Stream_ExpressMode()
        {
            // Arrange
            var prompt = "How are you doing today?";
            var vertex = new VertexAI(apiKey: fixture.ApiKey);
            var model = vertex.GenerativeModel(model: _model);

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
                output.WriteLine($"PromptTokenCount: {response?.UsageMetadata?.PromptTokenCount}");
                output.WriteLine($"CandidatesTokenCount: {response?.UsageMetadata?.CandidatesTokenCount}");
                output.WriteLine($"TotalTokenCount: {response?.UsageMetadata?.TotalTokenCount}");
            }
        }

        [Theory]
        [InlineData("How are you doing today?", 6)]
        [InlineData("What kind of fish is this?", 7)]
        [InlineData("Write a story about a magic backpack.", 8)]
        [InlineData("Write an extended story about a magic backpack.", 9)]
        public async Task Count_Tokens_Request_ExpressMode(string prompt, int expected)
        {
            // Arrange
            var vertex = new VertexAI(apiKey: fixture.ApiKey);
            var model = vertex.GenerativeModel(model: _model);
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
    }
}
