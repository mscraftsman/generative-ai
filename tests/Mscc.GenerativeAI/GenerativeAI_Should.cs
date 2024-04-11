#if NET472_OR_GREATER || NETSTANDARD2_0
#endif
using FluentAssertions;
using Mscc.GenerativeAI;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Test.Mscc.GenerativeAI
{
    /// <summary>
    /// Tests are based on the AIPlatform.Samples repository
    /// https://github.com/GoogleCloudPlatform/dotnet-docs-samples/tree/main/aiplatform/api/AIPlatform.Samples
    /// </summary>
    [Collection(nameof(ConfigurationFixture))]
    public class GenerativeAiShould
    {
        private readonly ITestOutputHelper _output;
        private readonly ConfigurationFixture _fixture;
        private readonly string _model = Model.Gemini10ProVision;

        public GenerativeAiShould(ITestOutputHelper output, ConfigurationFixture fixture)
        {
            _output = output;
            _fixture = fixture;
        }

        [Fact]
        public void Initialize_Interface_GoogleAI()
        {
            // Arrange
            IGenerativeAI genAi;
            genAi = new GoogleAI(apiKey: _fixture.ApiKey);
            var expected = _model.SanitizeModelName();
            
            // Act
            var model = genAi.GenerativeModel(model: _model);

            // Assert
            genAi.Should().NotBeNull();
            model.Should().NotBeNull();
            model.Name.Should().Be($"{expected}");
        }

        [Fact]
        public async Task GetModel()
        {
            // Arrange
            IGenerativeAI genAi;
            genAi = new GoogleAI(apiKey: _fixture.ApiKey);
            var expected = Model.Embedding.SanitizeModelName();

            // Act
            var model = genAi.GenerativeModel(model: Model.Embedding);
            var getModel = await model.GetModel();

            // Assert
            getModel.Name.SanitizeModelName().Should().Be(expected);
        }

        [Fact]
        public void Initialize_Interface_VertexAI()
        {
            // Arrange
            IGenerativeAI genAi;
            genAi = new VertexAI(projectId: _fixture.ProjectId, region: _fixture.Region);
            var expected = _model.SanitizeModelName();
            
            // Act
            var model = genAi.GenerativeModel(model: _model);
            
            // Assert
            genAi.Should().NotBeNull();
            model.Should().NotBeNull();
            model.Name.Should().Be($"{expected}");
        }
    }
}