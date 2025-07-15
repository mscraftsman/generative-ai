#if NET472_OR_GREATER || NETSTANDARD2_0
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
#endif
#if NET9_0
using System.Threading.Tasks;
#endif
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Mscc.GenerativeAI;
using Neovolve.Logging.Xunit;
using Xunit;
using Xunit.Abstractions;

namespace Test.Mscc.GenerativeAI
{
    [Collection(nameof(ConfigurationFixture))]
    public class GoogleAiEmbeddingShould : LoggingTestsBase
    {
        private readonly string _model = Model.GeminiEmbedding;
        private readonly ITestOutputHelper _output;
        private readonly ConfigurationFixture _fixture;
        private readonly GoogleAI _googleAi;

        public GoogleAiEmbeddingShould(ITestOutputHelper output, ConfigurationFixture fixture)
            : base(output, LogLevel.Trace)
        {
            _output = output;
            _fixture = fixture;
            _googleAi = new(apiKey: fixture.ApiKey, logger: Logger);
        }

        [Fact]
        public void Initialize_Model()
        {
            // Arrange
            var expected = Model.GeminiEmbedding.SanitizeModelName();
            var googleAi = new GoogleAI(apiKey: _fixture.ApiKey);

            // Act
            var model = _googleAi.GenerativeModel(model: _model);

            // Assert
            model.Should().NotBeNull();
            model.Name.Should().Be(expected);
        }

        [Fact]
        public async Task Embed_Content()
        {
            // Arrange
            var prompt = "Write a story about a magic backpack.";
            IGenerativeAI genAi = new GoogleAI(apiKey: _fixture.ApiKey);
            var model = _googleAi.GenerativeModel(model: _model);

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

        [Fact]
        public async Task Embed_Content_Embedding()
        {
            // Arrange
            var prompt = "Write a story about a magic backpack.";
            IGenerativeAI genAi = new GoogleAI(apiKey: _fixture.ApiKey);
            var model = _googleAi.GenerativeModel(model: _model);

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

        [Fact]
        public async Task Embed_Content_GeminiEmbedding()
        {
            // Arrange
            var prompt = "Write a story about a magic backpack.";
            IGenerativeAI genAi = new GoogleAI(apiKey: _fixture.ApiKey);
            var model = _googleAi.GenerativeModel(model: Model.GeminiEmbedding);

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

        [Fact]
        public async Task Embed_Content_TextEmbedding()
        {
            // Arrange
            var prompt = "Write a story about a magic backpack.";
            IGenerativeAI genAi = new GoogleAI(apiKey: _fixture.ApiKey);
            var model = _googleAi.GenerativeModel(model: Model.TextEmbedding);

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

        [Fact]
        public async Task Embed_Content_Batches()
        {
            // Arrange
            var prompts = new[] {
                "What is the meaning of life?",
                "How much wood would a woodchuck chuck?",
                "How does the brain work?"};
            IGenerativeAI genAi = new GoogleAI(apiKey: _fixture.ApiKey);
            var model = _googleAi.GenerativeModel(model: _model);

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
                _output.WriteLine(x.ToString());
            });
        }

        [Fact]
        public async Task Use_EmbeddingsEndpoint()
        {
            // Arrange
            IGenerativeAI genAi = new GoogleAI(apiKey: _fixture.ApiKey);
            var model = new EmbeddingsModel()
            {
                ApiKey = _fixture.ApiKey, 
                AccessToken = _fixture.ApiKey is null ? _fixture.AccessToken : null
            };
            var request = new GenerateEmbeddingsRequest()
            {
                Model = _model,
                Input = new[] {
                    "What is the meaning of life?",
                    "How much wood would a woodchuck chuck?",
                    "How does the brain work?"}
            };
            
            // Act
            var response = await model.Embeddings(request);
            
            // Assert
            response.Should().NotBeNull();
        }
    }
}
