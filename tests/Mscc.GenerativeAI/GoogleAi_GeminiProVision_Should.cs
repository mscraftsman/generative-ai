#if NET472_OR_GREATER || NETSTANDARD2_0
using System;
using System.Collections.Generic;
using System.IO;
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
    public class GoogleAiGeminiProVisionShould
    {
        private readonly ITestOutputHelper _output;
        private readonly ConfigurationFixture _fixture;
        private readonly string _model = Model.GeminiProVision;

        public GoogleAiGeminiProVisionShould(ITestOutputHelper output, ConfigurationFixture fixture)
        {
            _output = output;
            _fixture = fixture;
        }

        [Fact]
        public void Initialize_GeminiProVision()
        {
            // Arrange

            // Act
            var model = new GenerativeModel(apiKey: _fixture.ApiKey, model: _model);

            // Assert
            model.Should().NotBeNull();
            model.Name.Should().Be(Model.GeminiProVision);
        }

        [Fact]
        public async Task Generate_Text_From_Image()
        {
            // Arrange
            var model = new GenerativeModel(apiKey: _fixture.ApiKey, model: _model);
            var request = new GenerateContentRequest { Contents = new List<Content>() };
            var base64Image = "iVBORw0KGgoAAAANSUhEUgAAAAEAAAABCAYAAAAfFcSJAAAADUlEQVR42mP8z8BQDwAEhQGAhKmMIQAAAABJRU5ErkJggg==";
            var parts = new List<IPart>
            {
                new TextData { Text = "What is this picture about?" },
                new InlineData { MimeType = "image/jpeg", Data = base64Image }
            };
            request.Contents.Add(new Content { Role = Role.User, Parts = parts });

            // Act
            var response = await model.GenerateContent(request);

            // Assert
            response.Should().NotBeNull();
            response.Candidates.Should().NotBeNull().And.HaveCount(1);
            response.Candidates.FirstOrDefault().Content.Should().NotBeNull();
            response.Candidates.FirstOrDefault().Content.Parts.Should().NotBeNull().And.HaveCountGreaterThanOrEqualTo(1);
            response.Text.Should().Contain("red");
            _output.WriteLine(response?.Text);
        }

        [Fact]
        public async Task Describe_Image_From_InlineData()
        {
            // Arrange
            var prompt = "Parse the time and city from the airport board shown in this image into a list, in Markdown";
            var model = new GenerativeModel(apiKey: _fixture.ApiKey, model: _model);
            // Images
            var board = await TestExtensions.ReadImageFileBase64Async("https://raw.githubusercontent.com/mscraftsman/generative-ai/refs/heads/main/tests/Mscc.GenerativeAI/payload/timetable.png");
            var request = new GenerateContentRequest(prompt);
            request.Contents[0].Parts.Add(
                new InlineData { MimeType = "image/png", Data = board }
            );

            // Act
            var response = await model.GenerateContent(request);

            // Assert
            response.Should().NotBeNull();
            response.Candidates.Should().NotBeNull().And.HaveCount(1);
            response.Candidates.FirstOrDefault().Content.Should().NotBeNull();
            response.Candidates.FirstOrDefault().Content.Parts.Should().NotBeNull().And.HaveCountGreaterThanOrEqualTo(1);
            _output.WriteLine(response?.Text);
        }

        [Theory]
        [InlineData("scones.jpg", "image/jpeg", "What is this picture?", "blueberries")]
        [InlineData("cat.jpg", "image/jpeg", "Describe this image", "snow")]
        [InlineData("cat.jpg", "image/jpeg", "Is it a cat?", "Yes")]
        //[InlineData("animals.mp4", "video/mp4", "What's in the video?", "Zootopia")]
        public async Task Generate_Text_From_ImageFile(string filename, string mimetype, string prompt, string expected)
        {
            // Arrange
            var model = new GenerativeModel(apiKey: _fixture.ApiKey, model: _model);
            var base64Image = Convert.ToBase64String(File.ReadAllBytes(Path.Combine(Environment.CurrentDirectory, "payload", filename)));
            var parts = new List<IPart>
            {
                new TextData { Text = prompt },
                new InlineData { MimeType = mimetype, Data = base64Image }
            };
            var generationConfig = new GenerationConfig()
            {
                Temperature = 0.4f, TopP = 1, TopK = 32, MaxOutputTokens = 1024
            };

            // Act
            var response = await model.GenerateContent(parts, generationConfig);

            // Assert
            response.Should().NotBeNull();
            response.Candidates.Should().NotBeNull().And.HaveCount(1);
            response.Candidates.FirstOrDefault().Content.Should().NotBeNull();
            response.Candidates.FirstOrDefault().Content.Parts.Should().NotBeNull().And.HaveCountGreaterThanOrEqualTo(1);
            response.Text.Should().Contain(expected);
            _output.WriteLine(response?.Text);
        }

        [Theory]
        [InlineData("scones.jpg", "What is this picture?", "blueberries")]
        [InlineData("cat.jpg", "Describe this image", "snow")]
        [InlineData("cat.jpg", "Is it a cat?", "Yes")]
        //[InlineData("animals.mp4", "video/mp4", "What's in the video?", "Zootopia")]
        public async Task Describe_AddMedia_From_ImageFile(string filename, string prompt, string expected)
        {
            // Arrange
            var model = new GenerativeModel(apiKey: _fixture.ApiKey, model: _model);
            var request = new GenerateContentRequest(prompt)
            {
                GenerationConfig = new GenerationConfig()
                {
                    Temperature = 0.4f, TopP = 1, TopK = 32, MaxOutputTokens = 1024
                }
            };
            await request.AddMedia(Path.Combine(Environment.CurrentDirectory, "payload", filename));

            // Act
            var response = await model.GenerateContent(request);

            // Assert
            response.Should().NotBeNull();
            response.Candidates.Should().NotBeNull().And.HaveCount(1);
            response.Candidates.FirstOrDefault().Content.Should().NotBeNull();
            response.Candidates.FirstOrDefault().Content.Parts.Should().NotBeNull().And.HaveCountGreaterThanOrEqualTo(1);
            response.Text.Should().Contain(expected);
            _output.WriteLine(response?.Text);
        }

        [Fact]
        public async Task Describe_AddMedia_From_Url_Markdown()
        {
            // Arrange
            var prompt = "Parse the time and city from the airport board shown in this image into a list, in Markdown table";
            var model = new GenerativeModel(apiKey: _fixture.ApiKey, model: _model);
            var request = new GenerateContentRequest(prompt);
            await request.AddMedia("https://raw.githubusercontent.com/mscraftsman/generative-ai/refs/heads/main/tests/Mscc.GenerativeAI/payload/timetable.png");

            // Act
            var response = await model.GenerateContent(request);

            // Assert
            response.Should().NotBeNull();
            response.Candidates.Should().NotBeNull().And.HaveCount(1);
            response.Candidates.FirstOrDefault().Content.Should().NotBeNull();
            response.Candidates.FirstOrDefault().Content.Parts.Should().NotBeNull().And.HaveCountGreaterThanOrEqualTo(1);
            _output.WriteLine(response?.Text);
        }

        [Fact]
        public async Task Describe_AddMedia_From_Url_JSON()
        {
            // Arrange
            var prompt = "Parse the time and city from the airport board shown in this image into a list, in JSON";
            var model = new GenerativeModel(apiKey: _fixture.ApiKey, model: _model);
            var request = new GenerateContentRequest(prompt);
            await request.AddMedia("https://raw.githubusercontent.com/mscraftsman/generative-ai/refs/heads/main/tests/Mscc.GenerativeAI/payload/timetable.png");

            // Act
            var response = await model.GenerateContent(request);

            // Assert
            response.Should().NotBeNull();
            response.Candidates.Should().NotBeNull().And.HaveCount(1);
            response.Candidates.FirstOrDefault().Content.Should().NotBeNull();
            response.Candidates.FirstOrDefault().Content.Parts.Should().NotBeNull().And.HaveCountGreaterThanOrEqualTo(1);
            _output.WriteLine(response?.Text);
        }

        [Fact(Skip = "Bad Request due to FileData part")]
        public async Task Describe_AddMedia_From_UrlRemote()
        {
            // Arrange
            var prompt = "Parse the time and city from the airport board shown in this image into a list, in Markdown";
            var model = new GenerativeModel(apiKey: _fixture.ApiKey, model: _model);
            var request = new GenerateContentRequest(prompt);
            await request.AddMedia("https://raw.githubusercontent.com/mscraftsman/generative-ai/refs/heads/main/tests/Mscc.GenerativeAI/payload/timetable.png", useOnline: true);

            // Act
            var response = await model.GenerateContent(request);

            // Assert
            response.Should().NotBeNull();
            response.Candidates.Should().NotBeNull().And.HaveCount(1);
            response.Candidates.FirstOrDefault().Content.Should().NotBeNull();
            response.Candidates.FirstOrDefault().Content.Parts.Should().NotBeNull().And.HaveCountGreaterThanOrEqualTo(1);
            _output.WriteLine(response?.Text);
        }

        [Fact(Skip = "Bad Request due to FileData part")]
        public async Task Describe_Image_From_FileData()
        {
            // Arrange
            var prompt = "Parse the time and city from the airport board shown in this image into a list, in Markdown";
            var model = new GenerativeModel(apiKey: _fixture.ApiKey, model: _model);
            var request = new GenerateContentRequest(prompt);
            request.Contents[0].Parts.Add(new FileData
            {
                FileUri = "https://raw.githubusercontent.com/mscraftsman/generative-ai/refs/heads/main/tests/Mscc.GenerativeAI/payload/timetable.png",
                MimeType = "image/png"
            });

            // Act
            var response = await model.GenerateContent(request);

            // Assert
            response.Should().NotBeNull();
            response.Candidates.Should().NotBeNull().And.HaveCount(1);
            response.Candidates.FirstOrDefault().Content.Should().NotBeNull();
            response.Candidates.FirstOrDefault().Content.Parts.Should().NotBeNull().And.HaveCountGreaterThanOrEqualTo(1);
            _output.WriteLine(response?.Text);
        }

        [Fact(Skip = "URL scheme not supported")]
        public async Task Multimodal_Video_Input()
        {
            // Arrange
            var prompt = "What's in the video?";
            var videoUrl = "gs://cloud-samples-data/video/animals.mp4";
            var model = new GenerativeModel(apiKey: _fixture.ApiKey, model: _model);
            // var video = await TestExtensions.ReadImageFileBase64Async(videoUrl);
            var request = new GenerateContentRequest(prompt);
            // request.Contents[0].Parts.Add(new InlineData { MimeType = "video/mp4", Data = video });
            request.Contents[0].Parts.Add(new FileData { MimeType = "video/mp4", FileUri = videoUrl });

            // Act
            var response = await model.GenerateContent(request);

            // Assert
            response.Should().NotBeNull();
            response.Candidates.Should().NotBeNull().And.HaveCount(1);
            response.Candidates.FirstOrDefault().Content.Should().NotBeNull();
            response.Candidates.FirstOrDefault().Content.Parts.Should().NotBeNull().And.HaveCountGreaterThanOrEqualTo(1);
            response.Text.Should().Contain("Zootopia");
            _output.WriteLine(response?.Text);
        }
    }
}
