#if NET472_OR_GREATER || NETSTANDARD2_0
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
#endif
#if NET9_0
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
#endif
using FluentAssertions;
using Mscc.GenerativeAI;
using Xunit;
using Xunit.Abstractions;

namespace Test.Mscc.GenerativeAI
{
    [Collection(nameof(ConfigurationFixture))]
    public class VertexAiGeminiProVisionShould(ITestOutputHelper output, ConfigurationFixture fixture)
    {
        private readonly string _model = Model.Gemini10ProVision;

        [Fact]
        public void Initialize()
        {
            // Arrange

            // Act
            var vertexAi = new VertexAI(projectId: fixture.ProjectId, region: fixture.Region);

            // Assert
            vertexAi.Should().NotBeNull();
        }

        [Fact]
        public void Return_GenerateModel_GeminiProVision()
        {
            // Arrange
            var vertexAi = new VertexAI(projectId: fixture.ProjectId, region: fixture.Region);

            // Act
            var model = vertexAi.GenerativeModel(model: _model);

            // Assert
            model.Should().NotBeNull();
            model.Name.Should().Be(Model.Gemini10ProVision.SanitizeModelName());
        }

        [Fact]
        public async Task Generate_Content()
        {
            // Arrange
            var vertexAi = new VertexAI(projectId: fixture.ProjectId, region: fixture.Region);
            var model = vertexAi.GenerativeModel(model: _model);
            var request = new GenerateContentRequest { Contents = new List<Content>() };
            request.Contents.Add(new Content
            {
                Role = Role.User,
                Parts = new List<IPart> { new TextData { Text = "How are you doing today?" } }
            });

            // Act
            var response = await model.GenerateContent(request);

            // Assert
            response.Should().NotBeNull();
            response.Candidates.Should().NotBeNull().And.HaveCount(1);
            response.Candidates.FirstOrDefault().Content.Should().NotBeNull();
            response.Candidates.FirstOrDefault().Content.Parts.Should().NotBeNull().And.HaveCountGreaterThanOrEqualTo(1);
            response.Text.Should().Contain("feeling");
            output.WriteLine(response?.Text);
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
            var vertexAi = new VertexAI(projectId: fixture.ProjectId, region: fixture.Region);
            var model = vertexAi.GenerativeModel(model: _model, generationConfig, safetySettings);
            model.AccessToken = fixture.AccessToken;

            // Act
            var response = await model.GenerateContent(prompt);

            // Assert
            response.Should().NotBeNull();
            response.Candidates.Should().NotBeNull().And.HaveCount(1);
            response.Candidates[0].FinishReason.Should().Be(FinishReason.Safety);
            response.Text.Should().BeEmpty();
            output.WriteLine($"{response.Text}");
        }

        [Fact]
        public Task Generate_Streaming_Content()
        {
            // Arrange
            var vertexAi = new VertexAI(projectId: fixture.ProjectId, region: fixture.Region);
            var model = vertexAi.GenerativeModel(model: _model);
            var request = new GenerateContentRequest { Contents = new List<Content>() };
            var parts = new List<IPart>
            {
                new TextData { Text = "How are you doing today?" }
            };
            request.Contents.Add(new Content { Role = Role.User, Parts = parts });

            // Act
            var response = model.GenerateContentStream(request);

            // Assert
            response.Should().NotBeNull();
            return Task.CompletedTask;
        }

        [Fact]
        public async Task Analyze_Image_From_Uri()
        {
            // Arrange
            var vertexAi = new VertexAI(projectId: fixture.ProjectId, region: fixture.Region);
            var model = vertexAi.GenerativeModel(model: _model);
            var image = new Part().FromUri("gs://cloud-samples-data/ai-platform/flowers/daisy/10559679065_50d2b16f6d.jpg", "image/jpeg");
            var parts = new List<IPart>
            {
                new TextData { Text = "what is this image?" },
                image
            };

            // Act
            var response = await model.GenerateContent(parts);

            // Assert
            response.Should().NotBeNull();
            response.Candidates.Should().NotBeNull().And.HaveCount(1);
            response.Candidates.FirstOrDefault().Content.Should().NotBeNull();
            response.Candidates.FirstOrDefault().Content.Parts.Should().NotBeNull().And.HaveCountGreaterThanOrEqualTo(1);
            response.Text.Should().Contain("daisy");
            output.WriteLine(response?.Text);
        }

        [Theory]
        [InlineData("gs://cloud-samples-data/generative-ai/image/320px-Felis_catus-cat_on_snow.jpg")]
        [InlineData("gs://cloud-samples-data/ai-platform/flowers/daisy/10559679065_50d2b16f6d.jpg")]
        public Task Analyze_Image_From_Cloud_Storage(string uri)
        {
            // Arrange
            var vertexAi = new VertexAI(projectId: fixture.ProjectId, region: fixture.Region);
            var model = vertexAi.GenerativeModel(model: _model);
            var request = new GenerateContentRequest { Contents = new List<Content>() };
            var parts = new List<IPart>
            {
                new TextData { Text = "what is this image?" },
                new FileData { MimeType = "image/jpeg", FileUri = uri }
            };
            request.Contents.Add(new Content { Role = Role.User, Parts = parts });

            // Act
            var response = model.GenerateContentStream(request);

            // Assert
            response.Should().NotBeNull();
            return Task.CompletedTask;
            //response.Candidates.Should().NotBeNull().And.HaveCount(1);
            //response.Candidates.FirstOrDefault().Content.Should().NotBeNull();
            //response.Candidates.FirstOrDefault().Content.Parts.Should().NotBeNull().And.HaveCountGreaterThanOrEqualTo(1);
            //response.Text.Should().Contain("Yes");

            // // Act
            // var responseStream = model.GenerateContentStream(request);
            //
            // // Assert
            // responseStream.Should().NotBeNull();
            // await foreach (var response in responseStream)
            // {
            //     response.Should().NotBeNull();
            //     response.Candidates.Should().NotBeNull().And.HaveCount(1);
            //     response.Text.Should().NotBeEmpty();
            //     output.WriteLine(response?.Text);
            //     // response.UsageMetadata.Should().NotBeNull();
            //     // output.WriteLine($"PromptTokenCount: {response?.UsageMetadata?.PromptTokenCount}");
            //     // output.WriteLine($"CandidatesTokenCount: {response?.UsageMetadata?.CandidatesTokenCount}");
            //     // output.WriteLine($"TotalTokenCount: {response?.UsageMetadata?.TotalTokenCount}");
            // }
        }

        [Fact]
        public Task Generate_Text_From_Cloud_Storage()
        {
            // Arrange
            var vertexAi = new VertexAI(projectId: fixture.ProjectId, region: fixture.Region);
            var model = vertexAi.GenerativeModel(model: _model);
            var request = new GenerateContentRequest { Contents = new List<Content>() };
            var parts = new List<IPart>
            {
                new TextData { Text = "What is this picture about?" },
                new FileData { MimeType = "image/jpeg", FileUri = "gs://generativeai-downloads/images/scones.jpg" }
            };
            request.Contents.Add(new Content { Role = Role.User, Parts = parts });

            // Act
            var response = model.GenerateContentStream(request);

            // Assert
            response.Should().NotBeNull();
            return Task.CompletedTask;
        }

        [Fact]
        public Task Provide_Image_Description()
        {
            // Arrange
            var vertexAi = new VertexAI(projectId: fixture.ProjectId, region: fixture.Region);
            var model = vertexAi.GenerativeModel(model: _model);
            var request = new GenerateContentRequest { Contents = new List<Content>() };
            var base64Image = "iVBORw0KGgoAAAANSUhEUgAAAAEAAAABCAYAAAAfFcSJAAAADUlEQVR42mP8z8BQDwAEhQGAhKmMIQAAAABJRU5ErkJggg==";
            var parts = new List<IPart>
            {
                new InlineData { MimeType = "image/jpeg", Data = base64Image },
                new TextData { Text = "What is this picture about?" }
            };
            request.Contents.Add(new Content { Role = Role.User, Parts = parts });

            // Act
            var response = model.GenerateContentStream(request);

            // Assert
            response.Should().NotBeNull();
            return Task.CompletedTask;
            //response.Candidates.Should().NotBeNull().And.HaveCount(1);
            //response.Candidates.FirstOrDefault().Content.Should().NotBeNull();
            //response.Candidates.FirstOrDefault().Content.Parts.Should().NotBeNull().And.HaveCountGreaterThanOrEqualTo(1);
            //response.Text.Should().Contain("red");
        }

        [Fact]
        public async Task Analyze_Video_From_Cloud_Storage()
        {
            // Arrange
            var prompt = "What's in the video?";
            var videoUrl = "gs://cloud-samples-data/video/animals.mp4";
            var vertexAi = new VertexAI(projectId: fixture.ProjectId, region: fixture.Region);
            var model = vertexAi.GenerativeModel(model: _model);
            var request = new GenerateContentRequest(prompt);
            await request.AddMedia(videoUrl, useOnline: true);

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
                output.WriteLine($"PromptTokenCount: {response?.UsageMetadata?.PromptTokenCount}");
                output.WriteLine($"CandidatesTokenCount: {response?.UsageMetadata?.CandidatesTokenCount}");
                output.WriteLine($"TotalTokenCount: {response?.UsageMetadata?.TotalTokenCount}");
            }
        }

        [Fact]
        public async Task Start_Chat_Streaming()
        {
            // Arrange
            var vertexAi = new VertexAI(projectId: fixture.ProjectId, region: fixture.Region);
            var model = vertexAi.GenerativeModel(model: _model);
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
                output.WriteLine($"{response.Text}");
                // response.UsageMetadata.Should().NotBeNull();
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
        public async Task Count_Tokens(string prompt, int expected)
        {
            // Arrange
            var vertexAi = new VertexAI(projectId: fixture.ProjectId, region: fixture.Region);
            var model = vertexAi.GenerativeModel(model: _model);
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
