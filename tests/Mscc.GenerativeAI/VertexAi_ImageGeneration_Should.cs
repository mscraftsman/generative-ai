#if NET472_OR_GREATER || NETSTANDARD2_0
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
#endif
#if NET9_0
using System;
using System.IO;
using System.Threading.Tasks;
#endif
using FluentAssertions;
using Mscc.GenerativeAI;
using System.Collections;
using System.Collections.Generic;
using Xunit;
using Xunit.Abstractions;

namespace Test.Mscc.GenerativeAI
{
    [Collection(nameof(ConfigurationFixture))]
    public class VertexAiImageGenerationShould(ITestOutputHelper output, ConfigurationFixture fixture)
    {
        private readonly string _model = Model.Imagen3;

        [Fact]
        public void Initialize_VertexAI()
        {
            // Arrange

            // Act
            var vertexAi = new VertexAI(projectId: fixture.ProjectId, region: fixture.Region);

            // Assert
            vertexAi.Should().NotBeNull();
        }

        [Fact]
        public void Initialize_Using_VertexAI()
        {
            // Arrange
            var expected = Environment.GetEnvironmentVariable("GOOGLE_AI_MODEL") ?? Model.ImageGeneration;
            var vertexAi = new VertexAI(projectId: fixture.ProjectId, region: fixture.Region);

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
            var vertexAi = new VertexAI(projectId: fixture.ProjectId, region: fixture.Region);

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
            var vertexAi = new VertexAI(projectId: fixture.ProjectId, region: fixture.Region);

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
            var vertexAi = new VertexAI(projectId: fixture.ProjectId, region: fixture.Region);
            var model = vertexAi.ImageGenerationModel(model: _model);
            model.AccessToken = fixture.AccessToken;

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

        [Theory]
        [ClassData(typeof(ImagePrompts))]
        public async Task Generate_Images(string prompt, string aspectRatio)
        {
            // Arrange
            var vertexAi = new VertexAI(projectId: fixture.ProjectId, region: fixture.Region);
            var model = vertexAi.ImageGenerationModel(model: _model);
            model.AccessToken = fixture.AccessToken;

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
                output.WriteLine($"Wrote image to {fileName}");
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
                "16:9"
            };
            yield return new object[]
            {
                "A screenshot of a third-person open world exploration game. The player is an adventurer exploring a forest. There is a house with a red door on the left, and a house with a blue door on the right. The camera is placed directly behind the player. #photorealistic #immersive",
                "16:9"
            };
            yield return new object[]
            {
                "An image of a computer game showing a scene from inside a rough hewn stone cave or mine. The viewer's position is a 3rd person camera based above a player avatar looking down towards the avatar. The player avatar is a knight with a sword. In front of the knight avatar there are x3 stone arched doorways and the knight chooses to go through any one of these doors. Beyond the first and inside we can see strange green plants with glowing flowers lining that tunnel. Inside and beyond the second doorway there is a corridor of spiked iron plates riveted to the cave walls leading towards an ominous glow further along. Through the third door we can see a set of rough hewn stone steps ascending to a mysterious destination.",
                "16:9"
            };
            yield return new object[]
            {
                "Screenshot of a world with a warrior protagonist", "16:9"
            };
            yield return new object[]
            {
                "Photorealistic shot in the style of DSLR camera of the northern lights dancing across the Arctic sky, stars twinkling, snow-covered landscape",
                "16:9"
            };
            yield return new object[]
            {
                "A panning shot of a serene mountain landscape, the camera slowly revealing snow-capped peaks, granite rocks and a crystal-clear lake reflecting the sky",
                "16:9"
            };
            yield return new object[]
            {
                "Group of people looking happy, natural light, 8k",
                "1:1"
            };
            yield return new object[]
            {
                "Hyperrealistic portrait of a person dressed in 1920s flapper fashion, vintage style, black and white, photograph, elegant pose, 8k",
                "1:1"
            };
            yield return new object[]
            {
                "Imagine a close-up of a vintage watch. Generate a realistic depiction with a weathered strap and detailed mechanism",
                "1:1"
            };
            yield return new object[]
            {
                "Impressionistic landscape painting of a sunset over a field of sunflowers, vibrant colors, thick brushstrokes, inspired by Monet",
                "1:1"
            };
            yield return new object[]
            {
                "A surreal dreamscape featuring a giant tortoise with a lush forest growing on its back, floating through a starry sky, glowing mushrooms, bioluminescent plants, ethereal atmosphere",
                "1:1"
            };
            yield return new object[]
            {
                "Lifestyle image of a freshly roasted coffee beans spilling out of a burlap sack onto a rustic wooden table, steam rising from a nearby cup of coffee, 'Awaken Your Senses' is written on the cup in cursive, warm and inviting atmosphere, morning sunlight, product photography",
                "1:1"
            };
            yield return new object[]
            {
                "Hyperrealistic portrait of a woman with piercing blue eyes, laughing, freckles, dramatic lighting, detailed skin texture, 8k",
                "1:1"
            };
            yield return new object[]
            {
                "A panoramic view of a majestic mountain range at dawn",
                "16:9"
            };
            yield return new object[]
            {
                "Show a scene from a game where the player needs to find a specific object by looking into drawers of a messy desk",
                "1:1"
            };
            yield return new object[]
            {
                "A cityscape painted in the style of Van Gogh with swirling brushstrokes and vibrant colors",
                "16:9"
            };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
