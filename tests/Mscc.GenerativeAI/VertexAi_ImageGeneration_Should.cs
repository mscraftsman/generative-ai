using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Mscc.GenerativeAI;
using Neovolve.Logging.Xunit;
using System.Collections;
using Xunit;
using Xunit.Abstractions;

namespace Test.Mscc.GenerativeAI
{
    [Collection(nameof(ConfigurationFixture))]
    public class VertexAiImageGenerationShould : LoggingTestsBase
    {
        private readonly string _model = Model.Imagen4Fast;
        private readonly ITestOutputHelper _output;
        private readonly ConfigurationFixture _fixture;
        private readonly VertexAI _vertexAi;

        public VertexAiImageGenerationShould(ITestOutputHelper output, ConfigurationFixture fixture)
            : base(output, LogLevel.Trace)
        {
            _output = output;
            _fixture = fixture;
            _vertexAi = new(projectId: fixture.ProjectId,
                region: fixture.Region,
                logger: Logger);
        }

        [Fact]
        public void Initialize_VertexAI()
        {
            // Arrange

            // Act
            var vertexAi = new VertexAI(projectId: _fixture.ProjectId, region: _fixture.Region, accessToken: _fixture.AccessToken);

            // Assert
            vertexAi.Should().NotBeNull();
        }

        [Fact]
        public void Initialize_Using_VertexAI()
        {
            // Arrange
            var expected = Environment.GetEnvironmentVariable("GOOGLE_AI_MODEL") ?? Model.ImageGeneration;
            var vertexAi = new VertexAI(projectId: _fixture.ProjectId, region: _fixture.Region, accessToken: _fixture.AccessToken);

            // Act
            var model = vertexAi.ImageGenerationModel();

            // Assert
            model.Should().NotBeNull();
            model.Name.Should().Be($"{expected.SanitizeModelName()}");
        }

        [Fact]
        public void Initialize_Default_Model()
        {
            // Arrange
            var vertexAi = new VertexAI(projectId: _fixture.ProjectId, region: _fixture.Region, accessToken: _fixture.AccessToken);

            // Act
            var model = vertexAi.ImageGenerationModel();

            // Assert
            model.Should().NotBeNull();
            model.Name.Should().Be($"{Model.ImageGeneration.SanitizeModelName()}");
        }

        [Fact]
        public void Initialize_Model()
        {
            // Arrange
            var vertexAi = new VertexAI(projectId: _fixture.ProjectId, region: _fixture.Region, accessToken: _fixture.AccessToken);

            // Act
            var model = vertexAi.ImageGenerationModel(model: _model);

            // Assert
            model.Should().NotBeNull();
            model.Name.Should().Be($"{Model.Imagen3.SanitizeModelName()}");
        }

        [Theory]
        [ClassData(typeof(ImagePrompts))]
        public async Task Generate_Content(string prompt, string aspectRatio)
        {
            // Arrange
            var vertexAi = new VertexAI(projectId: _fixture.ProjectId, region: _fixture.Region, accessToken: _fixture.AccessToken);
            var model = vertexAi.ImageGenerationModel(model: _model);

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
                _output.WriteLine($"Wrote image to {fileName}");
            }
        }

        [Theory]
        [ClassData(typeof(ImagePrompts))]
        public async Task Generate_Images(string prompt, ImageAspectRatio? aspectRatio)
        {
            // Arrange
            var vertexAi = new VertexAI(projectId: _fixture.ProjectId, region: _fixture.Region, accessToken: _fixture.AccessToken);
            var model = vertexAi.ImageGenerationModel(model: _model);

            // Act
            var response = await model.GenerateImages(prompt, aspectRatio: aspectRatio);

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
                _output.WriteLine($"Wrote image to {fileName}");
            }
        }

        [Theory]
        [InlineData("Remove the snow completely and place the cat [1] on a sidewalk, zoomed out. In the background, people and cabs.", "cat.jpg")]
        public async Task Edit_Images(string prompt, string filename)
        {
            // Arrange
            var modelName = Model.Imagen3Capability;
            // var vertexAi = new VertexAI(projectId: _fixture.ProjectId, region: _fixture.Region, accessToken: _fixture.AccessToken, logger: Logger);
            var model = _vertexAi.GenerativeModel(model: modelName);
            var imageToEdit = new SubjectReferenceImage()
            {
                ReferenceId = 1,
                Image = new Image()
                {
                    BytesBase64Encoded = Convert.ToBase64String(
                        await File.ReadAllBytesAsync(Path.Combine(Environment.CurrentDirectory, "payload",
                            filename)))
                },
                SubjectImageConfig = new ()
                {
                    SubjectDescription = "a cat in the snow",
                    SubjectType = SubjectReferenceType.SubjectTypeAnimal
                }
            };

            // Act
            var response = await model.EditImage(modelName, prompt, [imageToEdit]);

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
                _output.WriteLine($"Wrote image to {fileName}");
            }
        }

        [Theory]
        [InlineData("cat.jpg", UpscaleFactor.X2)]
        [InlineData("cat.jpg", UpscaleFactor.X4)]
        public async Task Upscale_Image(string filename, UpscaleFactor upscaleFactor)
        {
            // Arrange
            // var vertexAi = new VertexAI(projectId: _fixture.ProjectId, region: _fixture.Region, accessToken: _fixture.AccessToken, logger: Logger);
            var model = _vertexAi.GenerativeModel(model: _model);
            var imageToScale = new Image()
            {
                BytesBase64Encoded = Convert.ToBase64String(
                    await File.ReadAllBytesAsync(Path.Combine(Environment.CurrentDirectory, "payload", filename)))
            };

            // Act
            var response = await model.UpscaleImage(_model, imageToScale, upscaleFactor);

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
                _output.WriteLine($"Wrote image to {fileName}");
            }
        }
    }

    internal class ImagePrompts : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[]
            {
                "A French cafe with the Golden Gate Bridge in the background.", 
                ImageAspectRatio.Ratio16x9
            };
            yield return new object[]
            {
                "A screenshot of a third-person open world exploration game. The player is an adventurer exploring a forest. There is a house with a red door on the left, and a house with a blue door on the right. The camera is placed directly behind the player. #photorealistic #immersive",
                ImageAspectRatio.Ratio16x9
            };
            yield return new object[]
            {
                "An image of a computer game showing a scene from inside a rough hewn stone cave or mine. The viewer's position is a 3rd person camera based above a player avatar looking down towards the avatar. The player avatar is a knight with a sword. In front of the knight avatar there are x3 stone arched doorways and the knight chooses to go through any one of these doors. Beyond the first and inside we can see strange green plants with glowing flowers lining that tunnel. Inside and beyond the second doorway there is a corridor of spiked iron plates riveted to the cave walls leading towards an ominous glow further along. Through the third door we can see a set of rough hewn stone steps ascending to a mysterious destination.",
                ImageAspectRatio.Ratio16x9
            };
            yield return new object[]
            {
                "Screenshot of a world with a warrior protagonist", 
                ImageAspectRatio.Ratio16x9
            };
            yield return new object[]
            {
                "Photorealistic shot in the style of DSLR camera of the northern lights dancing across the Arctic sky, stars twinkling, snow-covered landscape",
                ImageAspectRatio.Ratio16x9
            };
            yield return new object[]
            {
                "A panning shot of a serene mountain landscape, the camera slowly revealing snow-capped peaks, granite rocks and a crystal-clear lake reflecting the sky",
                ImageAspectRatio.Ratio16x9
            };
            yield return new object[]
            {
                "Hyperrealistic portrait of a person dressed in 1920s flapper fashion, vintage style, black and white, photograph, elegant pose, 8k",
                ImageAspectRatio.Ratio1x1
            };
            yield return new object[]
            {
                "Imagine a close-up of a vintage watch. Generate a realistic depiction with a weathered strap and detailed mechanism",
                ImageAspectRatio.Ratio1x1
            };
            yield return new object[]
            {
                "Impressionistic landscape painting of a sunset over a field of sunflowers, vibrant colors, thick brushstrokes, inspired by Monet",
                ImageAspectRatio.Ratio1x1
            };
            yield return new object[]
            {
                "A surreal dreamscape featuring a giant tortoise with a lush forest growing on its back, floating through a starry sky, glowing mushrooms, bioluminescent plants, ethereal atmosphere",
                ImageAspectRatio.Ratio1x1
            };
            yield return new object[]
            {
                "Lifestyle image of a freshly roasted coffee beans spilling out of a burlap sack onto a rustic wooden table, steam rising from a nearby cup of coffee, 'Awaken Your Senses' is written on the cup in cursive, warm and inviting atmosphere, morning sunlight, product photography",
                ImageAspectRatio.Ratio1x1
            };
            yield return new object[]
            {
                "Hyperrealistic portrait of a woman with piercing blue eyes, laughing, freckles, dramatic lighting, detailed skin texture, 8k",
                ImageAspectRatio.Ratio1x1
            };
            yield return new object[]
            {
                "A panoramic view of a majestic mountain range at dawn",
                ImageAspectRatio.Ratio16x9
            };
            yield return new object[]
            {
                "Show a scene from a game where the player needs to find a specific object by looking into drawers of a messy desk",
                ImageAspectRatio.Ratio1x1
            };
            yield return new object[]
            {
                "A cityscape painted in the style of Van Gogh with swirling brushstrokes and vibrant colors",
                ImageAspectRatio.Ratio16x9
            };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}