#if NET472_OR_GREATER || NETSTANDARD2_0
#endif
using FluentAssertions;
using Mscc.GenerativeAI;
using Xunit;
using Xunit.Abstractions;

namespace Test.Mscc.GenerativeAI
{
    /// <summary>
    /// Tests are based on the AIPlatform.Samples repository
    /// https://github.com/GoogleCloudPlatform/dotnet-docs-samples/tree/main/aiplatform/api/AIPlatform.Samples
    /// </summary>
    [Collection(nameof(ConfigurationFixture))]
    public class GenerativeAI_Should
    {
        private readonly ITestOutputHelper output;
        private readonly ConfigurationFixture fixture;
        private readonly string _model = Model.Gemini10ProVision;

        public GenerativeAI_Should(ITestOutputHelper output, ConfigurationFixture fixture)
        {
            this.output = output;
            this.fixture = fixture;
        }

        [Fact]
        public void Initialize_Interface_GoogleAI()
        {
            // Arrange
            IGenerativeAI genAi;
            genAi = new GoogleAI(apiKey: fixture.ApiKey);
            var expected = _model.SanitizeModelName();
            
            // Act
            var model = genAi.GenerativeModel(model: _model);

            // Assert
            genAi.Should().NotBeNull();
            model.Should().NotBeNull();
            model.Name.Should().Be($"{expected}");
        }

        [Fact]
        public void Initialize_Interface_VertexAI()
        {
            // Arrange
            IGenerativeAI genAi;
            genAi = new VertexAI(projectId: fixture.ProjectId, region: fixture.Region);
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