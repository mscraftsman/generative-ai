#if NET472_OR_GREATER || NETSTANDARD2_0
using System;
using System.Threading.Tasks;
#endif
#if NET9_0
using System.Threading.Tasks;
#endif
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Mscc.GenerativeAI;
using Neovolve.Logging.Xunit;
using System;
using System.IO;
using Xunit;
using Xunit.Abstractions;

namespace Test.Mscc.GenerativeAI
{
    [Collection(nameof(ConfigurationFixture))]
    public class OpenAi_Should(ITestOutputHelper output, ConfigurationFixture fixture)
        : LoggingTestsBase(output, LogLevel.Trace)
    {
        private readonly string _model = Model.Gemini20Flash;

        [Fact]
        public async Task List_Models()
        {
            // Arrange
            var model = new OpenAIModel(Logger) { ApiKey = fixture.ApiKey, Model = _model };

            // Act
            var sut = await model.ListModels();

            // Assert
            sut.Should().NotBeNull();
            sut.Data.Should().NotBeNull().And.HaveCountGreaterThanOrEqualTo(1);
            sut.Data.ForEach(x =>
            {
                output.WriteLine($"Model: {x.Id} ({x.Object} / {x.Created}) by {x.OwnedBy}");
            });
        }

        [Theory]
        [InlineData(Model.Gemini15Pro)]
        [InlineData(Model.Gemini15Flash)]
        [InlineData(Model.Gemini20Flash)]
        public async Task Get_Model_Information(string modelName)
        {
            // Arrange
            var model = new OpenAIModel(Logger) { ApiKey = fixture.ApiKey, Model = modelName };

            // Act
            var sut = await model.GetModel(modelsId: modelName);

            // Assert
            sut.Should().NotBeNull();
            output.WriteLine($"Model: {sut.Id} ({sut.Object} / {sut.Created}) by {sut.OwnedBy}");
        }

        [Fact]
        public async Task ChatCompletions_Using_OpenAiModel()
        {
            // Arrange
            var model = new OpenAIModel(Logger) { ApiKey = fixture.ApiKey, Model = _model };
            var request = new ChatCompletionsRequest()
                {
                    Model = _model, 
                    Messages = [
                        new { Role = Role.User, Content = "Explain to me how AI works"} 
                    ]
                };
            
            // Act
            var response = await model.Completions(request);
            
            // Assert
            response.Should().NotBeNull();
            output.WriteLine($"{response.Choices[0].Message.Content}");
        }

        [Fact]
        public async Task ChatCompletions_Using_ChatModel()
        {
            // Arrange
            var model = new ChatModel(Logger) { ApiKey = fixture.ApiKey, Model = _model };
            var request = new ChatCompletionsRequest()
            {
                Model = _model, 
                Messages = [
                    new { Role = Role.User, Content = "Explain to me how AI works"} 
                ]
            };
            
            // Act
            var response = await model.Completions(request);
            
            // Assert
            response.Should().NotBeNull();
            output.WriteLine($"{response.Choices[0].Message.Content}");
        }

        [Fact]
        public async Task Embed_Using_OpenAiModel()
        {
            // Arrange
            var model = new OpenAIModel(Logger) { ApiKey = fixture.ApiKey, Model = Model.TextEmbedding };
            var request = new GenerateEmbeddingsRequest()
            {
                Model = "text-embedding-004", 
                Input = "Your text string goes here"
            };
            
            // Act
            var response = await model.Embeddings(request);
            
            // Assert
            response.Should().NotBeNull();
            output.WriteLine($"{string.Join(",", response.Data[0].Embedding.ToArray())}");
        }

        [Fact]
        public async Task Embed_Using_EmbeddingsModel()
        {
            // Arrange
            var model = new EmbeddingsModel { ApiKey = fixture.ApiKey, Model = Model.TextEmbedding };
            var request = new GenerateEmbeddingsRequest()
            {
                Model = "text-embedding-004", 
                Input = "Your text string goes here"
            };
            
            // Act
            var response = await model.Embeddings(request);
            
            // Assert
            response.Should().NotBeNull();
            output.WriteLine($"{string.Join(",", response.Data[0].Embedding.ToArray())}");
        }

        [Theory]
        [InlineData(ImageResponseFormat.B64Json, ImageQuality.Standard, ImageSize.Size256X256, ImageStyle.Natural)]
        [InlineData(ImageResponseFormat.B64Json, ImageQuality.Hd, ImageSize.Size256X256, ImageStyle.Natural)]
        [InlineData(ImageResponseFormat.B64Json, ImageQuality.Standard, ImageSize.Size512X512, ImageStyle.Natural)]
        [InlineData(ImageResponseFormat.B64Json, ImageQuality.Hd, ImageSize.Size512X512, ImageStyle.Natural)]
        [InlineData(ImageResponseFormat.B64Json, ImageQuality.Standard, ImageSize.Size1024X1024, ImageStyle.Natural)]
        [InlineData(ImageResponseFormat.B64Json, ImageQuality.Hd, ImageSize.Size1024X1024, ImageStyle.Natural)]
        [InlineData(ImageResponseFormat.B64Json, ImageQuality.Standard, ImageSize.Size1024X1792, ImageStyle.Natural)]
        [InlineData(ImageResponseFormat.B64Json, ImageQuality.Hd, ImageSize.Size1024X1792, ImageStyle.Natural)]
        [InlineData(ImageResponseFormat.B64Json, ImageQuality.Standard, ImageSize.Size1792X1024, ImageStyle.Natural)]
        [InlineData(ImageResponseFormat.B64Json, ImageQuality.Hd, ImageSize.Size1792X1024, ImageStyle.Natural)]
        [InlineData(ImageResponseFormat.B64Json, ImageQuality.Standard, ImageSize.Size256X256, ImageStyle.Vivid)]
        [InlineData(ImageResponseFormat.B64Json, ImageQuality.Hd, ImageSize.Size256X256, ImageStyle.Vivid)]
        [InlineData(ImageResponseFormat.B64Json, ImageQuality.Standard, ImageSize.Size512X512, ImageStyle.Vivid)]
        [InlineData(ImageResponseFormat.B64Json, ImageQuality.Hd, ImageSize.Size512X512, ImageStyle.Vivid)]
        [InlineData(ImageResponseFormat.B64Json, ImageQuality.Standard, ImageSize.Size1024X1024, ImageStyle.Vivid)]
        [InlineData(ImageResponseFormat.B64Json, ImageQuality.Hd, ImageSize.Size1024X1024, ImageStyle.Vivid)]
        [InlineData(ImageResponseFormat.B64Json, ImageQuality.Standard, ImageSize.Size1024X1792, ImageStyle.Vivid)]
        [InlineData(ImageResponseFormat.B64Json, ImageQuality.Hd, ImageSize.Size1024X1792, ImageStyle.Vivid)]
        [InlineData(ImageResponseFormat.B64Json, ImageQuality.Standard, ImageSize.Size1792X1024, ImageStyle.Vivid)]
        [InlineData(ImageResponseFormat.B64Json, ImageQuality.Hd, ImageSize.Size1792X1024, ImageStyle.Vivid)]
        public async Task Images_Using_OpenAiModel(ImageResponseFormat responseFormat, ImageQuality quality, ImageSize size, ImageStyle style)
        {
            // Arrange
            var prompt =
                "Photorealistic shot in the style of DSLR camera of the northern lights dancing across the Arctic sky, stars twinkling, snow-covered landscape";
            var model = new OpenAIModel(Logger) { ApiKey = fixture.ApiKey, Model = Model.Imagen3 };
            var request = new ImagesGenerationsRequest(Model.Imagen3, prompt)
            {
                ResponseFormat = responseFormat,
                N = 1,
                Quality = quality,
                Size = size,
                Style = style,
                User = "Mscc.GenerativeAI - test cases"
            };
            
            // Act
            var response = await model.Images(request);
            
            // Assert
            response.Should().NotBeNull();
            response.Data.Should().NotBeNull().And.HaveCountGreaterThanOrEqualTo(1);
            foreach (var image in response.Data)
            {
                if (!string.IsNullOrEmpty(image.B64Json))
                {
                    var fileName = Path.Combine(Environment.CurrentDirectory, "payload",
                        Path.ChangeExtension($"{Guid.NewGuid():D}", "png"));
                    File.WriteAllBytes(fileName, Convert.FromBase64String(image.B64Json));
                    output.WriteLine($"Wrote image to {fileName}");
                } else if (!string.IsNullOrEmpty(image.Url))
                {
                    Output.WriteLine($"Image URL: {image.Url}");
                }
                else
                {
                    Output.WriteLine("No image generated.");
                }
            }
        }

        [Fact]
        public async Task Images_Using_ImagesModel()
        {
            // Arrange
            var prompt =
                "Photorealistic shot in the style of DSLR camera of the northern lights dancing across the Arctic sky, stars twinkling, snow-covered landscape";
            var model = new ImagesModel(Logger) { ApiKey = fixture.ApiKey, Model = Model.Imagen3 };
            var request = new ImagesGenerationsRequest(Model.Imagen3, prompt)
            {
                ResponseFormat = ImageResponseFormat.B64Json
            };
            
            // Act
            var response = await model.Images(request);
            
            // Assert
            response.Should().NotBeNull();
            response.Data.Should().NotBeNull().And.HaveCountGreaterThanOrEqualTo(1);
            foreach (var image in response.Data)
            {
                if (!string.IsNullOrEmpty(image.B64Json))
                {
                    var fileName = Path.Combine(Environment.CurrentDirectory, "payload",
                        Path.ChangeExtension($"{Guid.NewGuid():D}", "png"));
                    File.WriteAllBytes(fileName, Convert.FromBase64String(image.B64Json));
                    output.WriteLine($"Wrote image to {fileName}");
                } else if (!string.IsNullOrEmpty(image.Url))
                {
                    Output.WriteLine($"Image URL: {image.Url}");
                }
                else
                {
                    Output.WriteLine("No image generated.");
                }
            }
        }
    }
}