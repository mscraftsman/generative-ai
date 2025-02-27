#if NET472_OR_GREATER || NETSTANDARD2_0
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
#endif
#if NET9_0
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
#endif
using FluentAssertions;
using Mscc.GenerativeAI;
using System.IO;
using Xunit;
using Xunit.Abstractions;
using GoogleAI = Mscc.GenerativeAI.GoogleAI;

namespace Test.Mscc.GenerativeAI
{
    [Collection(nameof(ConfigurationFixture))]
    public class GoogleAiImageGenerationShould(ITestOutputHelper output, ConfigurationFixture fixture)
    {
        private readonly string _model = Model.Imagen3;

        [Fact]
        public async Task Generate_Content()
        {
            // Arrange
            var prompt = "A French cafe with the Golden Gate Bridge in the background.";
            var genAi = new GoogleAI(apiKey: fixture.ApiKey);
            var model = genAi.ImageGenerationModel(model: _model);

            // Act
            var response = await model.GenerateContent(prompt);

            // Assert
            response.Should().NotBeNull();
            response.Predictions.Should().NotBeNull()
                .And.HaveCountGreaterThanOrEqualTo(1)
                .And.HaveCountLessThanOrEqualTo(8);
            foreach (var image in response.Predictions)
            {
                var fileName = Path.Combine(Environment.CurrentDirectory, "payload",
                    Path.ChangeExtension($"{Guid.NewGuid():D}",
                        image.MimeType.Replace("image/", "")));
                File.WriteAllBytes(fileName, Convert.FromBase64String(image.BytesBase64Encoded));
                output.WriteLine($"Wrote image to {fileName}");
            }
        }

        [Fact]
        public async Task Generate_Images_Request()
        {
            // Arrange
            var prompt = "Fuzzy bunnies in my kitchen";
            var genAi = new GoogleAI(apiKey: fixture.ApiKey);
            var model = genAi.ImageGenerationModel(model: _model);
            var request = new ImageGenerationRequest(prompt);

            // Act
            var response = await model.GenerateImages(request);

            // Assert
            response.Should().NotBeNull();
            response.Predictions.Should().NotBeNull()
                .And.HaveCountGreaterThanOrEqualTo(1)
                .And.HaveCountLessThanOrEqualTo(8);
            foreach (var image in response.Predictions)
            {
                var fileName = Path.Combine(Environment.CurrentDirectory, "payload",
                    Path.ChangeExtension($"{Guid.NewGuid():D}",
                        image.MimeType.Replace("image/", "")));
                File.WriteAllBytes(fileName, Convert.FromBase64String(image.BytesBase64Encoded));
                output.WriteLine($"Wrote image to {fileName}");
            }
        }

        [Fact]
        public async Task Generate_Images_Parameters()
        {
            // Arrange
            var genAi = new GoogleAI(apiKey: fixture.ApiKey);
            var model = genAi.ImageGenerationModel(model: _model);

            // Act
            var response = await model.GenerateImages(
                prompt: "Fuzzy bunnies in my kitchen",
                numberOfImages: 4,
                safetyFilterLevel: "block_only_high",
                personGeneration: "allow_adult",
                aspectRatio: "3:4"
//                negativePrompt: "Outside"
            );

            // Assert
            response.Should().NotBeNull();
            response.Predictions.Should().NotBeNull()
                .And.HaveCountGreaterThanOrEqualTo(1)
                .And.HaveCountLessThanOrEqualTo(8);
            foreach (var image in response.Predictions)
            {
                var fileName = Path.Combine(Environment.CurrentDirectory, "payload",
                    Path.ChangeExtension($"{Guid.NewGuid():D}",
                        image.MimeType.Replace("image/", "")));
                File.WriteAllBytes(fileName, Convert.FromBase64String(image.BytesBase64Encoded));
                output.WriteLine($"Wrote image to {fileName}");
            }
        }

        [Fact]
        public async Task Generate_Images()
        {
            // Arrange
            var prompt =
                "A panning shot of a serene mountain landscape, the camera slowly revealing snow-capped peaks, granite rocks and a crystal-clear lake reflecting the sky";
            var genAi = new GoogleAI(apiKey: fixture.ApiKey);
            var model = genAi.GenerativeModel(model: _model);
            var request = new GenerateImagesRequest(prompt);
            
            // Act
            var response = await model.GenerateImages(request);
            
            // Assert
            response.Should().NotBeNull();
            response.Predictions.Should().NotBeNull()
                .And.HaveCountGreaterThanOrEqualTo(1)
                .And.HaveCountLessThanOrEqualTo(8);
            foreach (var image in response.Images)
            {
                var fileName = Path.Combine(Environment.CurrentDirectory, "payload",
                    Path.ChangeExtension($"{Guid.NewGuid():D}",
                        image.MimeType.Replace("image/", "")));
                File.WriteAllBytes(fileName, Convert.FromBase64String(image.BytesBase64Encoded));
                output.WriteLine($"Wrote image to {fileName}");
            }
        }

        [Fact]
        public async Task Generate_Images_Config()
        {
            // Arrange
            var genAi = new GoogleAI(apiKey: fixture.ApiKey);
            var model = genAi.GenerativeModel();

            // Act
            // Ref: https://developers.googleblog.com/en/imagen-3-arrives-in-the-gemini-api/
            var response = await model.GenerateImages(
                model: _model,
                prompt: "a portrait of a sheepadoodle wearing cape",
                config: new GenerateImagesConfig()
                {
                    NumberOfImages = 1
                }
            );

            // Assert
            response.Should().NotBeNull();
            response.Predictions.Should().NotBeNull()
                .And.HaveCountGreaterThanOrEqualTo(1)
                .And.HaveCountLessThanOrEqualTo(8);
            foreach (var image in response.Images)
            {
                var fileName = Path.Combine(Environment.CurrentDirectory, "payload",
                    Path.ChangeExtension($"{Guid.NewGuid():D}",
                        image.MimeType.Replace("image/", "")));
                File.WriteAllBytes(fileName, Convert.FromBase64String(image.BytesBase64Encoded));
                output.WriteLine($"Wrote image to {fileName}");
            }
        }

        [Theory]
        [ClassData(typeof(ImagePrompts))]
        public async Task Generate_Images_Samples(string prompt, string aspectRatio)
        {
            // Arrange
            var genAi = new GoogleAI(apiKey: fixture.ApiKey);
            var model = genAi.GenerativeModel();

            // Act
            // Ref: https://developers.googleblog.com/en/imagen-3-arrives-in-the-gemini-api/
            var response = await model.GenerateImages(
                model: _model,
                prompt: prompt,
                config: new GenerateImagesConfig()
                {
                    NumberOfImages = 1,
                    AspectRatio = aspectRatio
                }
            );

            // Assert
            response.Should().NotBeNull();
            response.Predictions.Should().NotBeNull()
                .And.HaveCountGreaterThanOrEqualTo(1)
                .And.HaveCountLessThanOrEqualTo(8);
            foreach (var image in response.Predictions)
            {
                var fileName = Path.Combine(Environment.CurrentDirectory, "payload",
                    Path.ChangeExtension($"{Guid.NewGuid():D}",
                        image.MimeType.Replace("image/", "")));
                File.WriteAllBytes(fileName, Convert.FromBase64String(image.BytesBase64Encoded));
                output.WriteLine($"Wrote image to {fileName}");
            }
        }
    }
}