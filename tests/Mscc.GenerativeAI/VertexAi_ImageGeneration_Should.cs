#if NET472_OR_GREATER || NETSTANDARD2_0
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
#endif
using FluentAssertions;
using Mscc.GenerativeAI;
using Xunit;
using Xunit.Abstractions;

namespace Test.Mscc.GenerativeAI
{
    [Collection(nameof(ConfigurationFixture))]
    public class ImageGenerationShould
    {
        private readonly ITestOutputHelper _output;
        private readonly ConfigurationFixture _fixture;
        private readonly string _model = Model.ImageGeneration;

        public ImageGenerationShould(ITestOutputHelper output, ConfigurationFixture fixture)
        {
            _output = output;
            _fixture = fixture;
        }
        
        [Fact]
        public void Initialize_VertexAI()
        {
            // Arrange

            // Act
            var vertexAi = new VertexAI(projectId: _fixture.ProjectId, region: _fixture.Region);

            // Assert
            vertexAi.Should().NotBeNull();
        }

        [Fact]
        public void Initialize_Using_VertexAI()
        {
            // Arrange
            var expected = Environment.GetEnvironmentVariable("GOOGLE_AI_MODEL") ?? Model.ImageGeneration;
            var vertexAi = new VertexAI(projectId: _fixture.ProjectId, region: _fixture.Region);

            // Act
            var model = vertexAi.ImageGenerationModel();
            
            // Assert
            model.Should().NotBeNull();
            model.Name.Should().Be($"{expected}");
        }

        [Fact]
        public void Initialize_Default_Model()
        {
            // Arrange
            var vertexAi = new VertexAI(projectId: _fixture.ProjectId, region: _fixture.Region);

            // Act
            var model = vertexAi.ImageGenerationModel();

            // Assert
            model.Should().NotBeNull();
            model.Name.Should().Be($"{Model.ImageGeneration}");
        }

        [Fact]
        public void Initialize_Model()
        {
            // Arrange
            var vertexAi = new VertexAI(projectId: _fixture.ProjectId, region: _fixture.Region);

            // Act
            var model = vertexAi.ImageGenerationModel(model: _model);

            // Assert
            model.Should().NotBeNull();
            model.Name.Should().Be($"{Model.ImageGeneration}");
        }

        [Fact]
        public async void Generate_Content()
        {
            // Arrange
            var prompt = "A French cafe with the Golden Gate Bridge in the background.";
            var vertexAi = new VertexAI(projectId: _fixture.ProjectId, region: _fixture.Region);
            var model = vertexAi.ImageGenerationModel(model: _model);
            model.AccessToken = _fixture.AccessToken;

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
    }
}
