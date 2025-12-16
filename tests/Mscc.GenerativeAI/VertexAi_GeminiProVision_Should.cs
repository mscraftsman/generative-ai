using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shouldly;
using Mscc.GenerativeAI;
using Mscc.GenerativeAI.Types;
using Xunit;
using Xunit.Abstractions;

namespace Test.Mscc.GenerativeAI
{
    [Collection(nameof(ConfigurationFixture))]
    public class VertexAiGeminiProVisionShould(ITestOutputHelper output, ConfigurationFixture fixture)
    {
        private readonly string _model = Model.GeminiPro;

        [Fact]
        public void Initialize()
        {
            // Arrange

            // Act
            var vertexAi = new VertexAI(projectId: fixture.ProjectId, region: fixture.Region, accessToken: fixture.AccessToken);

            // Assert
            vertexAi.ShouldNotBeNull();
        }

        [Fact]
        public void Return_GenerateModel_GeminiProVision()
        {
            // Arrange
            var vertexAi = new VertexAI(projectId: fixture.ProjectId, region: fixture.Region, accessToken: fixture.AccessToken);

            // Act
            var model = vertexAi.GenerativeModel(model: _model);

            // Assert
            model.ShouldNotBeNull();
            model.Name.ShouldBe(Model.GeminiPro.SanitizeModelName());
        }

        [Fact]
        public async Task Generate_Content()
        {
            // Arrange
            var vertexAi = new VertexAI(projectId: fixture.ProjectId, region: fixture.Region, accessToken: fixture.AccessToken);
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
            response.ShouldNotBeNull();
            response.Candidates.ShouldNotBeNull();
            response.Candidates.Count.ShouldBe(1);
            response.Candidates.FirstOrDefault().Content.ShouldNotBeNull();
            response.Candidates.FirstOrDefault().Content.Parts.ShouldNotBeNull();
            response.Candidates.FirstOrDefault().Content.Parts.Count.ShouldBeGreaterThanOrEqualTo(1);
            response.Text.ShouldContain("feeling");
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
            var vertexAi = new VertexAI(projectId: fixture.ProjectId, region: fixture.Region, accessToken: fixture.AccessToken);
            var model = vertexAi.GenerativeModel(model: _model, generationConfig, safetySettings);

            // Act
            var response = await model.GenerateContent(prompt);

            // Assert
            response.ShouldNotBeNull();
            response.Candidates.ShouldNotBeNull();
            response.Candidates.Count.ShouldBe(1);
            response.Candidates[0].FinishReason.ShouldBe(FinishReason.Safety);
            response.Text.ShouldBeEmpty();
            output.WriteLine($"{response.Text}");
        }

        [Fact]
        public Task Generate_Streaming_Content()
        {
            // Arrange
            var vertexAi = new VertexAI(projectId: fixture.ProjectId, region: fixture.Region, accessToken: fixture.AccessToken);
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
            response.ShouldNotBeNull();
            return Task.CompletedTask;
        }

        [Fact]
        public async Task Analyze_Image_From_Uri()
        {
            // Arrange
            var vertexAi = new VertexAI(projectId: fixture.ProjectId, region: fixture.Region, accessToken: fixture.AccessToken);
            var model = vertexAi.GenerativeModel(model: _model);
            var image = Part.FromUri("gs://cloud-samples-data/ai-platform/flowers/daisy/10559679065_50d2b16f6d.jpg", "image/jpeg");
            var parts = new List<IPart>
            {
                new TextData { Text = "what is this image?" },
                image
            };

            // Act
            var response = await model.GenerateContent(parts);

            // Assert
            response.ShouldNotBeNull();
            response.Candidates.ShouldNotBeNull();
            response.Candidates.Count.ShouldBe(1);
            response.Candidates.FirstOrDefault().Content.ShouldNotBeNull();
            response.Candidates.FirstOrDefault().Content.Parts.ShouldNotBeNull();
            response.Candidates.FirstOrDefault().Content.Parts.Count.ShouldBeGreaterThanOrEqualTo(1);
            response.Text.ShouldContain("daisy");
            output.WriteLine(response?.Text);
        }

        [Theory]
        [InlineData("gs://cloud-samples-data/generative-ai/image/320px-Felis_catus-cat_on_snow.jpg")]
        [InlineData("gs://cloud-samples-data/ai-platform/flowers/daisy/10559679065_50d2b16f6d.jpg")]
        public Task Analyze_Image_From_Cloud_Storage(string uri)
        {
            // Arrange
            var vertexAi = new VertexAI(projectId: fixture.ProjectId, region: fixture.Region, accessToken: fixture.AccessToken);
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
            response.ShouldNotBeNull();
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
            //     response.ShouldNotBeNull();
            //     response.Candidates.ShouldNotBeNull();
            //     response.Candidates.Count.ShouldBe(1);
            //     response.Text.ShouldNotBeEmpty();
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
            var vertexAi = new VertexAI(projectId: fixture.ProjectId, region: fixture.Region, accessToken: fixture.AccessToken);
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
            response.ShouldNotBeNull();
            return Task.CompletedTask;
        }

        [Fact]
        public Task Provide_Image_Description()
        {
            // Arrange
            var vertexAi = new VertexAI(projectId: fixture.ProjectId, region: fixture.Region, accessToken: fixture.AccessToken);
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
            response.ShouldNotBeNull();
            return Task.CompletedTask;
            //response.Candidates.ShouldNotBeNull();
            //response.Candidates.Count.ShouldBe(1);
            //response.Candidates.FirstOrDefault().Content.ShouldNotBeNull();
            //response.Candidates.FirstOrDefault().Content.Parts.ShouldNotBeNull();
            //response.Candidates.FirstOrDefault().Content.Parts.Count.ShouldBeGreaterThanOrEqualTo(1);
            //response.Text.ShouldContain("red");
        }

        [Fact]
        public async Task Analyze_Video_From_Cloud_Storage()
        {
            // Arrange
            var prompt = "What's in the video?";
            var videoUrl = "gs://cloud-samples-data/video/animals.mp4";
            var vertexAi = new VertexAI(projectId: fixture.ProjectId, region: fixture.Region, accessToken: fixture.AccessToken);
            var model = vertexAi.GenerativeModel(model: _model);
            var request = new GenerateContentRequest(prompt);
            await request.AddMedia(videoUrl, useOnline: true);

            // Act
            var responseStream = model.GenerateContentStream(request);

            // Assert
            responseStream.ShouldNotBeNull();
            await foreach (var response in responseStream)
            {
                response.ShouldNotBeNull();
                response.Candidates.ShouldNotBeNull();
                response.Candidates.Count.ShouldBe(1);
                response.Text.ShouldNotBeEmpty();
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
            var vertexAi = new VertexAI(projectId: fixture.ProjectId, region: fixture.Region, accessToken: fixture.AccessToken);
            var model = vertexAi.GenerativeModel(model: _model);
            var chat = model.StartChat();
            var prompt = "How can I learn more about C#?";

            // Act
            var responseStream = chat.SendMessageStream(prompt);

            // Assert
            responseStream.ShouldNotBeNull();
            await foreach (var response in responseStream)
            {
                response.ShouldNotBeNull();
                response.Candidates.ShouldNotBeNull();
                response.Candidates.Count.ShouldBe(1);
                response.Text.ShouldNotBeEmpty();
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
            var vertexAi = new VertexAI(projectId: fixture.ProjectId, region: fixture.Region, accessToken: fixture.AccessToken);
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
            response.ShouldNotBeNull();
            response.TotalTokens.ShouldBe(expected);
            output.WriteLine($"Tokens: {response?.TotalTokens}");
        }
    }
}
