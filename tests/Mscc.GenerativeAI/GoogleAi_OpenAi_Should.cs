#if NET472_OR_GREATER || NETSTANDARD2_0
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
#endif
#if NET9_0
using System.Threading.Tasks;
#endif
using FluentAssertions;
using Mscc.GenerativeAI;
using Xunit;
using Xunit.Abstractions;

namespace Test.Mscc.GenerativeAI
{
    [Collection(nameof(ConfigurationFixture))]
    public class GoogleAi_OpenAi_Should(ITestOutputHelper output, ConfigurationFixture fixture)
    {
        private readonly string _model = Model.Gemini15Flash;

        [Fact]
        public async Task ChatCompletions_Using_OpenAiModel()
        {
            // Arrange
            var genAi = new GoogleAI(apiKey: fixture.ApiKey);
            var model = new OpenAIModel { ApiKey = fixture.ApiKey, Model = _model };
            var request = new ChatCompletionsRequest()
                {
                    Model = _model, 
                    Messages = [
                        new { Role = Role.User, Content = "Explain to me how AI works"} 
                    ]
                };
            
            // Act
            var response = await model.Completions(request);
            
            // Assert
            response.Should().NotBeNull();
            output.WriteLine($"{response.Choices[0].Message.Content}");
        }

        [Fact]
        public async Task ChatCompletions_Using_ChatModel()
        {
            // Arrange
            var genAi = new GoogleAI(apiKey: fixture.ApiKey);
            var model = new ChatModel { ApiKey = fixture.ApiKey, Model = _model };
            var request = new ChatCompletionsRequest()
            {
                Model = _model, 
                Messages = [
                    new { Role = Role.User, Content = "Explain to me how AI works"} 
                ]
            };
            
            // Act
            var response = await model.Completions(request);
            
            // Assert
            response.Should().NotBeNull();
            output.WriteLine($"{response.Choices[0].Message.Content}");
        }

        [Fact]
        public async Task Embed_Using_OpenAiModel()
        {
            // Arrange
            var genAi = new GoogleAI(apiKey: fixture.ApiKey);
            var model = new OpenAIModel { ApiKey = fixture.ApiKey, Model = "text-embedding-004" };
            var request = new GenerateEmbeddingsRequest()
            {
                Model = "text-embedding-004", 
                Input = "Your text string goes here"
            };
            
            // Act
            var response = await model.Embeddings(request);
            
            // Assert
            response.Should().NotBeNull();
            output.WriteLine($"{string.Join(",", response.Data[0].Embedding.ToArray())}");
        }

        [Fact]
        public async Task Embed_Using_EmbeddingsModel()
        {
            // Arrange
            var genAi = new GoogleAI(apiKey: fixture.ApiKey);
            var model = new EmbeddingsModel { ApiKey = fixture.ApiKey, Model = "text-embedding-004" };
            var request = new GenerateEmbeddingsRequest()
            {
                Model = "text-embedding-004", 
                Input = "Your text string goes here"
            };
            
            // Act
            var response = await model.Embeddings(request);
            
            // Assert
            response.Should().NotBeNull();
            output.WriteLine($"{string.Join(",", response.Data[0].Embedding.ToArray())}");
        }
    }
}