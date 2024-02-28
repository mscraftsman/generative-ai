using FluentAssertions;
using Mscc.GenerativeAI;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Xunit.Abstractions;

namespace Test.Mscc.GenerativeAI
{
    [Collection("Configuration")]
    public class VertexAi_GeminiProVision_Should
    {
        private readonly ITestOutputHelper output;
        private readonly ConfigurationFixture fixture;
        private readonly string projectId;
        private readonly string region;
        private readonly string model = Model.Gemini10ProVision;

        public VertexAi_GeminiProVision_Should(ITestOutputHelper output, ConfigurationFixture fixture)
        {
            this.output = output;
            this.fixture = fixture;
            projectId = fixture.ProjectId;
            region = fixture.Region;
        }

        [Fact]
        public void Initialize()
        {
            // Arrange

            // Act
            var vertex = new VertexAI(projectId: projectId, region: region);

            // Assert
            vertex.Should().NotBeNull();
        }

        [Fact]
        public void Return_GenerateModel_GeminiProVision()
        {
            // Arrange
            var vertex = new VertexAI(projectId: projectId, region: region);

            // Act
            var model = vertex.GenerativeModel(model: this.model);

            // Assert
            model.Should().NotBeNull();
            model.Name().Should().Be(Model.Gemini10ProVision);
        }

        [Fact]
        public async void Generate_Content()
        {
            // Arrange
            var vertex = new VertexAI(projectId: projectId, region: region);
            var model = vertex.GenerativeModel(model: this.model);
            var request = new GeminiRequest { Contents = new List<Content>() };
            request.Contents.Add(new Content
            {
                Role = "user",
                Parts = new List<IPart> { new TextData { Text = "How are you doing today?" } }
            });

            // Act
            var response = await model.GenerateContent(request);

            // Assert
            response.Should().NotBeNull();
            response.Candidates.Should().NotBeNull().And.HaveCount(1);
            response.Candidates.FirstOrDefault().Content.Should().NotBeNull();
            response.Candidates.FirstOrDefault().Content.Parts.Should().NotBeNull().And.HaveCountGreaterThanOrEqualTo(1);
            response.Text.Should().Contain("good");
            output.WriteLine(response?.Text);
        }

        [Fact]
        public async void Generate_Streaming_Content()
        {
            // Arrange
            var vertex = new VertexAI(projectId: projectId, region: region);
            var model = vertex.GenerativeModel(model: this.model);
            var request = new GeminiRequest { Contents = new List<Content>() };
            var parts = new List<IPart>
            {
                new TextData { Text = "How are you doing today?" }
            };
            request.Contents.Add(new Content { Role = "user", Parts = parts });

            // Act
            var response = await model.GenerateContentStream(request);

            // Assert
            response.Should().NotBeNull();
        }

        [Fact]
        public async void Analyze_Image_From_Uri()
        {
            // Arrange
            var vertex = new VertexAI(projectId: projectId, region: region);
            var model = vertex.GenerativeModel(model: this.model);
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

        [Fact]
        public async void Analyze_Image_From_Cloud_Storage()
        {
            // Arrange
            var vertex = new VertexAI(projectId: projectId, region: region);
            var model = vertex.GenerativeModel(model: this.model);
            var request = new GeminiRequest { Contents = new List<Content>() };
            var parts = new List<IPart>
            {
                new TextData { Text = "Is it a cat?" },
                new FileData { MimeType = "image/jpeg", FileUri = "gs://cloud-samples-data/generative-ai/image/320px-Felis_catus-cat_on_snow.jpg" }
            };
            request.Contents.Add(new Content { Role = "user", Parts = parts });

            // Act
            var response = await model.GenerateContentStream(request);

            // Assert
            response.Should().NotBeNull();
            //response.Candidates.Should().NotBeNull().And.HaveCount(1);
            //response.Candidates.FirstOrDefault().Content.Should().NotBeNull();
            //response.Candidates.FirstOrDefault().Content.Parts.Should().NotBeNull().And.HaveCountGreaterThanOrEqualTo(1);
            //response.Text.Should().Contain("Yes");
        }

        [Fact]
        public async void Generate_Text_From_Cloud_Storage()
        {
            // Arrange
            var vertex = new VertexAI(projectId: projectId, region: region);
            var model = vertex.GenerativeModel(model: this.model);
            var request = new GeminiRequest { Contents = new List<Content>() };
            var parts = new List<IPart>
            {
                new TextData { Text = "What is this picture about?" },
                new FileData { MimeType = "image/jpeg", FileUri = "gs://generativeai-downloads/images/scones.jpg" }
            };
            request.Contents.Add(new Content { Role = "user", Parts = parts });

            // Act
            var response = await model.GenerateContentStream(request);

            // Assert
            response.Should().NotBeNull();
        }

        [Fact]
        public async void Provide_Image_Description()
        {
            // Arrange
            var vertex = new VertexAI(projectId: projectId, region: region);
            var model = vertex.GenerativeModel(model: this.model);
            var request = new GeminiRequest { Contents = new List<Content>() };
            var base64image = "iVBORw0KGgoAAAANSUhEUgAAAAEAAAABCAYAAAAfFcSJAAAADUlEQVR42mP8z8BQDwAEhQGAhKmMIQAAAABJRU5ErkJggg==";
            var parts = new List<IPart>
            {
                new InlineData { MimeType = "image/jpeg", Data = base64image },
                new TextData { Text = "What is this picture about?" }
            };
            request.Contents.Add(new Content { Role = "user", Parts = parts });

            // Act
            var response = await model.GenerateContentStream(request);

            // Assert
            response.Should().NotBeNull();
            //response.Candidates.Should().NotBeNull().And.HaveCount(1);
            //response.Candidates.FirstOrDefault().Content.Should().NotBeNull();
            //response.Candidates.FirstOrDefault().Content.Parts.Should().NotBeNull().And.HaveCountGreaterThanOrEqualTo(1);
            //response.Text.Should().Contain("red");
        }

        [Fact]
        public async void Analyze_Video_From_Cloud_Storage()
        {
            // Arrange
            var vertex = new VertexAI(projectId: projectId, region: region);
            var model = vertex.GenerativeModel(model: this.model);
            var request = new GeminiRequest { Contents = new List<Content>() };
            var parts = new List<IPart>
            {
                new FileData { FileUri = "gs://cloud-samples-data/video/animals.mp4", MimeType = "video/mp4" },
                new TextData { Text = "What is in the video?" }
            };
            request.Contents.Add(new Content { Role = "user", Parts = parts });

            // Act
            var response = await model.GenerateContentStream(request);

            // Assert
            response.Should().NotBeNull();
        }

        [Fact]
        public async void Start_Chat_Streaming()
        {
            // Arrange
            var vertex = new VertexAI(projectId: projectId, region: region);
            var model = vertex.GenerativeModel(model: this.model);
            var chat = model.StartChat();
            var chatInput1 = "How can I learn more about C#?";

            // Act
            //var response = await chat.SendMessageStream(chatInput1);

            //// Assert
            //response.Should().NotBeNull();
        }

        [Theory]
        [InlineData("How are you doing today?", 7)]
        [InlineData("What kind of fish is this?", 7)]
        [InlineData("Write a story about a magic backpack.", 8)]
        [InlineData("Write an extended story about a magic backpack.", 9)]
        public async void Count_Tokens(string prompt, int expected)
        {
            // Arrange
            var vertex = new VertexAI(projectId: projectId, region: region);
            var model = vertex.GenerativeModel(model: this.model);
            var request = new GeminiRequest { Contents = new List<Content>() };
            request.Contents.Add(new Content
            {
                Role = "user",
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
