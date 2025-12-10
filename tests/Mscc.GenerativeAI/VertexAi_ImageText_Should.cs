using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Mscc.GenerativeAI;
using Xunit;
using Xunit.Abstractions;

namespace Test.Mscc.GenerativeAI
{
    [Collection(nameof(ConfigurationFixture))]
    public class ImageTextShould(ITestOutputHelper output, ConfigurationFixture fixture)
    {
        private readonly string _model = Model.ImageText;

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
            var expected = Environment.GetEnvironmentVariable("GOOGLE_AI_MODEL") ?? Model.ImageText;
            var vertexAi = new VertexAI(projectId: fixture.ProjectId, region: fixture.Region);

            // Act
            var model = vertexAi.ImageTextModel();
            
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
            var model = vertexAi.ImageTextModel();

            // Assert
            model.Should().NotBeNull();
            model.Name.Should().Be($"{Model.ImageText.SanitizeModelName()}");
        }

        [Fact]
        public void Initialize_Model()
        {
            // Arrange
            var vertexAi = new VertexAI(projectId: fixture.ProjectId, region: fixture.Region);

            // Act
            var model = vertexAi.ImageTextModel(model: _model);

            // Assert
            model.Should().NotBeNull();
            model.Name.Should().Be($"{Model.ImageText.SanitizeModelName()}");
        }

        [Theory]
        [InlineData("scones.jpg", "muffin")]
        [InlineData("cat.jpg", "snow")]
        [InlineData("image.jpg", "jetpack")]
        public async Task Get_Image_Captions(string filename, string expected)
        {
            // Arrange
            var vertexAi = new VertexAI(projectId: fixture.ProjectId, region: fixture.Region, accessToken: fixture.AccessToken);
            var model = vertexAi.ImageTextModel(model: _model);
            var base64Image = Convert.ToBase64String(File.ReadAllBytes(Path.Combine(Environment.CurrentDirectory, "payload", filename)));

            // Act
            var response = await model.GetCaptions(base64Image);

            // Assert
            response.Should().NotBeNull();
            response.Predictions.Should().NotBeNull()
                .And.HaveCountGreaterThanOrEqualTo(1)
                .And.HaveCountLessThanOrEqualTo(8);
            response.Predictions[0].Should().Contain(expected);
            foreach (var item in response.Predictions)
            {
                output.WriteLine($"Information: {item}");
            }
        }

        [Theory]
        [InlineData("cat.jpg", "de", "Schnee")]
        [InlineData("cat.jpg", "es", "gato")]
        [InlineData("cat.jpg", "fr", "neige")]
        [InlineData("cat.jpg", "it", "neve")]
        [InlineData("image.jpg", "de", "Jetpack")]
        [InlineData("image.jpg", "es", "cohete")]
        [InlineData("image.jpg", "fr", "jetpack")]
        [InlineData("image.jpg", "it", "jetpack")]
        [InlineData("scones.jpg", "de", "Cupcakes")]
        [InlineData("scones.jpg", "es", "galletas")]
        [InlineData("scones.jpg", "fr", "biscuits")]
        [InlineData("scones.jpg", "it", "biscotti")]
        public async Task Get_Image_Captions_Language(string filename, string language, string expected)
        {
            // Arrange
            var vertexAi = new VertexAI(projectId: fixture.ProjectId, region: fixture.Region, accessToken: fixture.AccessToken);
            var model = vertexAi.ImageTextModel(model: _model);
            var base64Image = Convert.ToBase64String(File.ReadAllBytes(Path.Combine(Environment.CurrentDirectory, "payload", filename)));

            // Act
            var response = await model.GetCaptions(base64Image, language: language);

            // Assert
            response.Should().NotBeNull();
            response.Predictions.Should().NotBeNull()
                .And.HaveCountGreaterThanOrEqualTo(1)
                .And.HaveCountLessThanOrEqualTo(8);
            response.Predictions[0].Should().Contain(expected);
            foreach (var item in response.Predictions)
            {
                output.WriteLine($"Information: {item}");
            }
        }

        [Fact]
        public async Task Ask_Questions()
        {
            // Arrange
            var prompt = "Is this in the mountains?";
            var filename = "cat.jpg";
            var vertexAi = new VertexAI(projectId: fixture.ProjectId, region: fixture.Region, accessToken: fixture.AccessToken);
            var model = vertexAi.ImageTextModel(model: _model);
            var base64Image = Convert.ToBase64String(File.ReadAllBytes(Path.Combine(Environment.CurrentDirectory, "payload", filename)));

            // Act
            var response = await model.AskQuestion(base64Image, prompt, 2);

            // Assert
            response.Should().NotBeNull();
            response.Predictions.Should().NotBeNull()
                .And.HaveCountGreaterThanOrEqualTo(1)
                .And.HaveCountLessThanOrEqualTo(8);
            foreach (var item in response.Predictions)
            {
                output.WriteLine($"Answer: {item}");
            }
        }
    }
}
