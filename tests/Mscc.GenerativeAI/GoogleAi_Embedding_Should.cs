#if NET472_OR_GREATER || NETSTANDARD2_0
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
#endif
using FluentAssertions;
using Mscc.GenerativeAI;
using Xunit;
using Xunit.Abstractions;

namespace Test.Mscc.GenerativeAI
{
    [Collection(nameof(ConfigurationFixture))]
    public class GoogleAiEmbeddingShould(ITestOutputHelper output, ConfigurationFixture fixture)
    {
        private readonly string _model = Model.Embedding;

        [Fact]
        public void Initialize_Model()
        {
            // Arrange
            var expected = Model.Embedding.SanitizeModelName();

            // Act
            var model = new GenerativeModel(apiKey: fixture.ApiKey, model: _model);

            // Assert
            model.Should().NotBeNull();
            model.Name.Should().Be(expected);
        }

        [Fact]
        public async Task Embed_Content()
        {
            // Arrange
            var prompt = "Write a story about a magic backpack.";
            IGenerativeAI genAi = new GoogleAI(apiKey: fixture.ApiKey);
            var model = genAi.GenerativeModel(model: _model);

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

        [Fact]
        public async Task Embed_Content_TextEmbedding()
        {
            // Arrange
            var prompt = "Write a story about a magic backpack.";
            IGenerativeAI genAi = new GoogleAI(apiKey: fixture.ApiKey);
            var model = genAi.GenerativeModel(model: Model.TextEmbedding);

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

        [Fact]
        public async Task Embed_Content_Batches()
        {
            // Arrange
            var prompts = new[] {
                "What is the meaning of life?",
                "How much wood would a woodchuck chuck?",
                "How does the brain work?"};
            IGenerativeAI genAi = new GoogleAI(apiKey: fixture.ApiKey);
            var model = genAi.GenerativeModel(model: _model);

            // Act
            var response = await model.EmbedContent(content: prompts, 
                taskType: TaskType.RetrievalDocument,
                title: "Embedding of list of strings");

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
