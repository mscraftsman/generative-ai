using System.Collections.Generic;
using System.Threading.Tasks;
using Shouldly;
using Mscc.GenerativeAI;
using System;
using System.Text;
using Xunit;
using Xunit.Abstractions;

namespace Test.Mscc.GenerativeAI
{
    /// <summary>
    /// Tests are based on the AIPlatform.Samples repository
    /// https://github.com/GoogleCloudPlatform/dotnet-docs-samples/tree/main/aiplatform/api/AIPlatform.Samples
    /// </summary>
    [Collection(nameof(ConfigurationFixture))]
    public class AiPlatformSamplesShould(ITestOutputHelper output, ConfigurationFixture fixture)
    {
        private readonly string _model = Model.GeminiPro;

        [Fact]
        public async Task Gemini_QuickStart()
        {
            // Arrange
            var vertex = new VertexAI(projectId: fixture.ProjectId, region: fixture.Region, accessToken: fixture.AccessToken);
            var model = vertex.GenerativeModel(model: _model);
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
            responseStream.ShouldNotBeNull();
            await foreach (var response in responseStream)
            {
                response.ShouldNotBeNull();
                response.Candidates.ShouldNotBeNull();
                response.Candidates.Count.ShouldBe(1);
                response.Text.ShouldNotBeEmpty();
                fullText.Append(response?.Text);
            }
            fullText.ToString().ShouldContain("blueberry");
            output.WriteLine(fullText.ToString());
        }

        [Fact]
        public async Task Multimodal_Multi_Image()
        {
            // Arrange
            var vertex = new VertexAI(projectId: fixture.ProjectId, region: fixture.Region, accessToken: fixture.AccessToken);
            var model = vertex.GenerativeModel(model: _model);

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
            var options = new RequestOptions(timeout: TimeSpan.FromMinutes(5));
            //var options = new RequestOptions(TimeSpan.FromSeconds(2));

            // Act
            var responseStream = model.GenerateContentStream(request, options);
            
            // Assert
            responseStream.ShouldNotBeNull();
            await foreach (var response in responseStream)
            {
                response.ShouldNotBeNull();
                response.Candidates.ShouldNotBeNull();
                response.Candidates.Count.ShouldBe(1);
                response.Text.ShouldNotBeEmpty();
                fullText.Append(response?.Text);
            }
            fullText.ToString().ShouldContain("redeemer");
            output.WriteLine(fullText.ToString());


        }

        [Fact]
        public async Task Multimodal_Video_Input()
        {
            // Arrange
            var vertex = new VertexAI(projectId: fixture.ProjectId, region: fixture.Region, accessToken: fixture.AccessToken);
            var model = vertex.GenerativeModel(model: _model);
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
            responseStream.ShouldNotBeNull();
            await foreach (var response in responseStream)
            {
                response.ShouldNotBeNull();
                response.Candidates.ShouldNotBeNull();
                response.Candidates.Count.ShouldBe(1);
                response.Text.ShouldNotBeEmpty();
                fullText.Append(response?.Text);
            }
            fullText.ToString().ShouldContain("Zootopia");
            output.WriteLine(fullText.ToString());


        }

        [Fact]
        public async Task With_SafetySettings()
        {
            // Arrange
            var vertex = new VertexAI(projectId: fixture.ProjectId, region: fixture.Region, accessToken: fixture.AccessToken);
            var model = vertex.GenerativeModel(model: _model);
            var generationConfig = new GenerateContentConfig
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
            responseStream.ShouldNotBeNull();
            await foreach (var response in responseStream)
            {
                response.ShouldNotBeNull();
                response.Candidates.ShouldNotBeNull();
                response.Candidates.Count.ShouldBe(1);
                response.Text.ShouldNotBeEmpty();
                fullText.Append(response?.Text);
            }
            fullText.ToString().ShouldContain("assist");
            output.WriteLine(fullText.ToString());


        }
    }
}
