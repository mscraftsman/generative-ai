using FluentAssertions;
using Mscc.GenerativeAI;
using Xunit;
using Xunit.Abstractions;

namespace Test.Mscc.GenerativeAI
{
    [Collection(nameof(ConfigurationFixture))]
    public class GoogleAi_Embedding_Should
    {
        private readonly ITestOutputHelper output;
        private readonly ConfigurationFixture fixture;
        private readonly string model = Model.Embedding;

        public GoogleAi_Embedding_Should(ITestOutputHelper output, ConfigurationFixture fixture)
        {
            this.output = output;
            this.fixture = fixture;
        }

        [Fact]
        public void Initialize_Model()
        {
            // Arrange
            var expected = Model.Embedding;
            
            // Act
            var model = new GenerativeModel(apiKey: fixture.ApiKey, model: this.model);

            // Assert
            model.Should().NotBeNull();
            model.Name().Should().Be(expected);
        }

        [Fact]
        public async void Embed_Content()
        {
            // Arrange
            var prompt = "Write a story about a magic backpack.";
            var model = new GenerativeModel(apiKey: fixture.ApiKey, model: this.model);

            // Act
            var response = await model.EmbedContent(prompt);

            // Assert
            response.Should().NotBeNull();
            response.Embedding.Should().NotBeNull();
            response.Embedding.Values.Should().NotBeNull().And.HaveCountGreaterThanOrEqualTo(1);
            response.Embedding.Values.ForEach(x =>
            {
                output.WriteLine(x.ToString());
            });
        }
    }
}
