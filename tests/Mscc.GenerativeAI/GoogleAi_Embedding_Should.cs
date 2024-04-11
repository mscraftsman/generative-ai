using FluentAssertions;
using Mscc.GenerativeAI;
using Xunit;
using Xunit.Abstractions;

namespace Test.Mscc.GenerativeAI
{
    [Collection(nameof(ConfigurationFixture))]
    public class GoogleAiEmbeddingShould
    {
        private readonly ITestOutputHelper _output;
        private readonly ConfigurationFixture _fixture;
        private readonly string _model = Model.Embedding;

        public GoogleAiEmbeddingShould(ITestOutputHelper output, ConfigurationFixture fixture)
        {
            _output = output;
            _fixture = fixture;
        }

        [Fact]
        public void Initialize_Model()
        {
            // Arrange
            var expected = Model.Embedding.SanitizeModelName();

            // Act
            var model = new GenerativeModel(apiKey: _fixture.ApiKey, model: _model);

            // Assert
            model.Should().NotBeNull();
            model.Name.Should().Be(expected);
        }

        [Fact]
        public async void Embed_Content()
        {
            // Arrange
            var prompt = "Write a story about a magic backpack.";
            var model = new GenerativeModel(apiKey: _fixture.ApiKey, model: _model);

            // Act
            var response = await model.EmbedContent(prompt);

            // Assert
            response.Should().NotBeNull();
            response.Embedding.Should().NotBeNull();
            response.Embedding.Values.Should().NotBeNull().And.HaveCountGreaterThanOrEqualTo(1);
            response.Embedding.Values.ForEach(x =>
            {
                _output.WriteLine(x.ToString());
            });
        }
    }
}
