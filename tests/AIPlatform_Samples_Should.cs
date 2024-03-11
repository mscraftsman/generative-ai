#if NET472_OR_GREATER || NETSTANDARD2_0
using System.Collections.Generic;
#endif
using FluentAssertions;
using Mscc.GenerativeAI;
using System.Text;
using Xunit;
using Xunit.Abstractions;

namespace Test.Mscc.GenerativeAI
{
    /// <summary>
    /// Tests are based on the AIPlatform.Samples repository
    /// https://github.com/GoogleCloudPlatform/dotnet-docs-samples/tree/8db139c177a205ed075cccc0c5d5a61647748943/aiplatform/api/AIPlatform.Samples
    /// </summary>
    [Collection(nameof(ConfigurationFixture))]
    public class AIPlatform_Samples_Should
    {
        private readonly ITestOutputHelper output;
        private readonly ConfigurationFixture fixture;
        private readonly string model = Model.Gemini10ProVision;

        public AIPlatform_Samples_Should(ITestOutputHelper output, ConfigurationFixture fixture)
        {
            this.output = output;
            this.fixture = fixture;
        }

        [Fact]
        public async void Gemini_QuickStart()
        {
            // Arrange
            var vertex = new VertexAI(projectId: fixture.ProjectId, region: fixture.Region);
            var model = vertex.GenerativeModel(model: this.model);
            model.AccessToken = fixture.AccessToken;
            var generationConfig = new GenerationConfig
            {
                Temperature = 0.4f,
                TopP = 1,
                TopK = 32,
                MaxOutputTokens = 2048
            };
            var request = new GenerateContentRequest("What's in this photo?", generationConfig);
            request.Contents[0].Role = Role.User;
            request.Contents[0].Parts.Add(new FileData
            {
                FileUri = "gs://generativeai-downloads/images/scones.jpg",
                MimeType = "image/jpeg"
            });
            var fullText = new StringBuilder();

            // Act
            var responseStream = model.GenerateContentStream(request);

            // Assert
            responseStream.Should().NotBeNull();
            await foreach (var response in responseStream)
            {
                response.Should().NotBeNull();
                response.Candidates.Should().NotBeNull().And.HaveCount(1);
                response.Text.Should().NotBeEmpty();
                fullText.Append(response?.Text);
            }
            fullText.ToString().Should().Contain("blueberry");
            output.WriteLine(fullText.ToString());
        }

        [Fact(Skip = "Skipped due to the configured HttpClient.Timeout of 100 seconds elapsing.")]
        public async void Multimodal_Multi_Image()
        {
            // Arrange
            var vertex = new VertexAI(projectId: fixture.ProjectId, region: fixture.Region);
            var model = vertex.GenerativeModel(model: this.model);
            model.AccessToken = fixture.AccessToken;

            // Images
            var colosseum = await TestExtensions.ReadImageFileBase64Async(
                "https://storage.googleapis.com/cloud-samples-data/vertex-ai/llm/prompts/landmark1.png");

            var forbiddenCity = await TestExtensions.ReadImageFileBase64Async(
                "https://storage.googleapis.com/cloud-samples-data/vertex-ai/llm/prompts/landmark2.png");

            var christRedeemer = await TestExtensions.ReadImageFileBase64Async(
                "https://storage.googleapis.com/cloud-samples-data/vertex-ai/llm/prompts/landmark3.png");

            // Initialize request argument(s)
            var parts = new List<IPart>
            {
                new InlineData { MimeType = "image/png", Data = colosseum },
                new TextData { Text = "city: Rome, Landmark: the Colosseum"},
                new InlineData { MimeType = "image/png", Data = forbiddenCity },
                new TextData { Text = "city: Beijing, Landmark: Forbidden City"},
                new InlineData { MimeType = "image/png", Data = christRedeemer }
            };
            var request = new GenerateContentRequest { Contents = new List<Content>() };
            request.Contents.Add(new Content { Role = Role.User, Parts = parts });
            var fullText = new StringBuilder();

            // Act
            var responseStream = model.GenerateContentStream(request);
            
            // Assert
            responseStream.Should().NotBeNull();
            await foreach (var response in responseStream)
            {
                response.Should().NotBeNull();
                response.Candidates.Should().NotBeNull().And.HaveCount(1);
                response.Text.Should().NotBeEmpty();
                fullText.Append(response?.Text);
            }
            fullText.ToString().Should().Contain("redeemer");
            output.WriteLine(fullText.ToString());

            // // Act
            // var response = await model.GenerateContent(request);
            //
            // // Assert
            // response.Should().NotBeNull();
            // response.Candidates.Should().NotBeNull().And.HaveCount(1);
            // //response.Candidates.FirstOrDefault().Content.Should().NotBeNull();
            // //response.Candidates.FirstOrDefault().Content.Parts.Should().NotBeNull().And.HaveCountGreaterThanOrEqualTo(1);
            // //response.Text.Should().Contain("redeemer");
            // output.WriteLine(response?.Text);
            // output.WriteLine(fullText.ToString());
        }

        [Fact]
        public async void Multimodal_Video_Input()
        {
            // Arrange
            var vertex = new VertexAI(projectId: fixture.ProjectId, region: fixture.Region);
            var model = vertex.GenerativeModel(model: this.model);
            model.AccessToken = fixture.AccessToken;
            var request = new GenerateContentRequest("What's in the video?");
            request.Contents[0].Role = Role.User;
            request.Contents[0].Parts.Add(new FileData
            {
                FileUri = "gs://cloud-samples-data/video/animals.mp4",
                MimeType = "video/mp4"
            });
            var fullText = new StringBuilder();

            // Act
            var responseStream = model.GenerateContentStream(request);

            // Assert
            responseStream.Should().NotBeNull();
            await foreach (var response in responseStream)
            {
                response.Should().NotBeNull();
                response.Candidates.Should().NotBeNull().And.HaveCount(1);
                response.Text.Should().NotBeEmpty();
                fullText.Append(response?.Text);
            }
            fullText.ToString().Should().Contain("Zootopia");
            output.WriteLine(fullText.ToString());

            // Act
            // var response = await model.GenerateContent(request);

            // Assert
            // response.Should().NotBeNull();
            // response.Candidates.Should().NotBeNull().And.HaveCount(1);
            // //response.Candidates.FirstOrDefault().Content.Should().NotBeNull();
            // //response.Candidates.FirstOrDefault().Content.Parts.Should().NotBeNull().And.HaveCountGreaterThanOrEqualTo(1);
            // response.Text.Should().Contain("Zootopia");
            // output.WriteLine(response?.Text);
            // output.WriteLine(fullText.ToString());
        }

        [Fact]
        public async void With_SafetySettings()
        {
            // Arrange
            var vertex = new VertexAI(projectId: fixture.ProjectId, region: fixture.Region);
            var model = vertex.GenerativeModel(model: this.model);
            model.AccessToken = fixture.AccessToken;
            var generationConfig = new GenerationConfig
            {
                Temperature = 0.4f,
                TopP = 1,
                TopK = 32,
                MaxOutputTokens = 2048
            };
            var safetySettings = new List<SafetySetting>()
            {
                new SafetySetting()
                {
                    Category = HarmCategory.HarmCategoryHateSpeech,
                    Threshold = HarmBlockThreshold.BlockLowAndAbove
                },
                new SafetySetting()
                {
                    Category = HarmCategory.HarmCategoryDangerousContent,
                    Threshold = HarmBlockThreshold.BlockMediumAndAbove
                }
            };
            var request = new GenerateContentRequest("Hello!", generationConfig, safetySettings);
            request.Contents[0].Role = Role.User;
            var fullText = new StringBuilder();

            // Act
            var responseStream = model.GenerateContentStream(request);

            // Assert
            responseStream.Should().NotBeNull();
            await foreach (var response in responseStream)
            {
                response.Should().NotBeNull();
                response.Candidates.Should().NotBeNull().And.HaveCount(1);
                response.Text.Should().NotBeEmpty();
                fullText.Append(response?.Text);
            }
            fullText.ToString().Should().Contain("assist");
            output.WriteLine(fullText.ToString());

            // Act
            // var response = await model.GenerateContent(request);

            // Assert
            // response.Should().NotBeNull();
            // response.Candidates.Should().NotBeNull().And.HaveCount(1);
            // //response.Candidates.FirstOrDefault().Content.Should().NotBeNull();
            // //response.Candidates.FirstOrDefault().Content.Parts.Should().NotBeNull().And.HaveCountGreaterThanOrEqualTo(1);
            // response.Text.Should().Contain("assist");
            // output.WriteLine(response?.Text);
            // output.WriteLine(fullText.ToString());
        }
    }
}
