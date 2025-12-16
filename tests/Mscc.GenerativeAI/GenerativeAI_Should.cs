using System.Threading.Tasks;
using Shouldly;
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
    public class GenerativeAiShould(ITestOutputHelper output, ConfigurationFixture fixture)
    {
        private readonly string _model = Model.Gemini25Flash;

        [Fact]
        public void Initialize_Interface_GoogleAI()
        {
            // Arrange
            IGenerativeAI genAi = new GoogleAI(apiKey: fixture.ApiKey);
            var expected = _model.SanitizeModelName();
            
            // Act
            var model = genAi.GenerativeModel(model: _model);

            // Assert
            genAi.ShouldNotBeNull();
            model.ShouldNotBeNull();
            model.Name.ShouldBe($"{expected}");
        }

        [Fact]
        public void Initialize_Interface_VertexAI()
        {
            // Arrange
            IGenerativeAI genAi = new VertexAI(projectId: fixture.ProjectId, region: fixture.Region, accessToken: fixture.AccessToken);
            var expected = _model.SanitizeModelName();
            
            // Act
            var model = genAi.GenerativeModel(model: _model);
            
            // Assert
            genAi.ShouldNotBeNull();
            model.ShouldNotBeNull();
            model.Name.ShouldBe($"{expected}");
        }

        [Fact]
        public async Task GoogleAI_Should_Not_Load_Credentials()
        {
            // Arrange
            IGenerativeAI genAi = new GoogleAI(apiKey: fixture.ApiKey);
            var model = genAi.GenerativeModel(model: _model);
            var prompt = "I love Taipei. Give me a 15 words summary about it.";
            
            // Act
            var response = await model.GenerateContent(prompt);
            
            // Assert
            genAi.ShouldNotBeNull();
            model.ShouldNotBeNull();
            output.WriteLine($"{response.Text}");
        }

        [Fact]
        public async Task GetModel()
        {
            // Arrange
            IGenerativeAI genAi = new GoogleAI(apiKey: fixture.ApiKey);
            var expected = Model.Embedding.SanitizeModelName();

            // Act
            var model = genAi.GenerativeModel(model: Model.Embedding);
            var getModel = await model.GetModel();

            // Assert
            genAi.ShouldNotBeNull();
            model.ShouldNotBeNull();
            getModel.ShouldNotBeNull();
            getModel.Name.ShouldNotBeNull();
            getModel.Name!.SanitizeModelName().ShouldBe(expected);
        }
    }
}