#if NET472_OR_GREATER || NETSTANDARD2_0
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
#endif
#if NET9_0
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
#endif
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Mscc.GenerativeAI;
using Neovolve.Logging.Xunit;
using System.ComponentModel;
using System.Dynamic;
using System.Runtime.Serialization;
using System.Text.Json;
using Xunit;
using Xunit.Abstractions;

namespace Test.Mscc.GenerativeAI
{
    [Collection(nameof(ConfigurationFixture))]
    public class GoogleAiGeminiShould : LoggingTestsBase
    {
        private readonly string _model = Model.Gemini25ProExperimental;
        private readonly ITestOutputHelper _output;
        private readonly ConfigurationFixture _fixture;
        private readonly GoogleAI _googleAi;

        public GoogleAiGeminiShould(ITestOutputHelper output, ConfigurationFixture fixture)
            : base(output, LogLevel.Trace)
        {
            _output = output;
            _fixture = fixture;
            _googleAi = new(apiKey: fixture.ApiKey, logger: Logger);
        }

        [Fact]
        public void Initialize_Gemini15Pro()
        {
            // Arrange

            // Act
            var googleAi = new GoogleAI(apiKey: _fixture.ApiKey);
            var model = _googleAi.GenerativeModel(model: _model);

            // Assert
            model.Should().NotBeNull();
            model.Name.Should().Be(Model.Gemini25ProExperimental.SanitizeModelName());
        }

        [Fact]
        public void Initialize_GoogleAI()
        {
            // Arrange

            // Act
            var googleAi = new GoogleAI(apiKey: _fixture.ApiKey, logger: Logger);

            // Assert
            _googleAi.Should().NotBeNull();
        }

        [Fact]
        public void Initialize_Using_GoogleAI()
        {
            // Arrange
            var expected = Environment.GetEnvironmentVariable("GOOGLE_AI_MODEL") ?? Model.Gemini15Pro;
            var googleAi = new GoogleAI(apiKey: _fixture.ApiKey);

            // Act
            var model = _googleAi.GenerativeModel();

            // Assert
            model.Should().NotBeNull();
            model.Name.Should().Be($"{expected.SanitizeModelName()}");
        }

        [Fact]
        public void Initialize_EnvVars()
        {
            // Arrange
            Environment.SetEnvironmentVariable("GOOGLE_API_KEY", _fixture.ApiKey);
            var expected = Environment.GetEnvironmentVariable("GOOGLE_AI_MODEL") ?? Model.Gemini15Pro;
            var googleAI = new GoogleAI(accessToken: _fixture.AccessToken);

            // Act
            var model = _googleAi.GenerativeModel();

            // Assert
            model.Should().NotBeNull();
            model.Name.Should().Be($"{expected.SanitizeModelName()}");
        }

        [Fact]
        public void Initialize_Default_Model()
        {
            // Arrange
            var expected = Environment.GetEnvironmentVariable("GOOGLE_AI_MODEL") ?? Model.Gemini15Pro;
            var googleAi = new GoogleAI(apiKey: _fixture.ApiKey);

            // Act
            var model = _googleAi.GenerativeModel();

            // Assert
            model.Should().NotBeNull();
            model.Name.Should().Be($"{expected.SanitizeModelName()}");
        }

        [Fact]
        public void Initialize_Model()
        {
            // Arrange
            var expected = _model;

            // Act
            var googleAi = new GoogleAI(apiKey: _fixture.ApiKey);
            var model = _googleAi.GenerativeModel(model: _model);

            // Assert
            model.Should().NotBeNull();
            model.Name.Should().Be($"{expected.SanitizeModelName()}");
        }

        [Fact]
        public async Task List_Models()
        {
            // Arrange
            var googleAi = new GoogleAI(apiKey: _fixture.ApiKey);
            var model = _googleAi.GenerativeModel();

            // Act
            var sut = await model.ListModels();

            // Assert
            sut.Should().NotBeNull();
            sut.Should().NotBeNull().And.HaveCountGreaterThanOrEqualTo(1);
            sut.ForEach(x =>
            {
                _output.WriteLine($"Model: {x.DisplayName} ({x.Name})");
                x.SupportedGenerationMethods.ForEach(m => _output.WriteLine($"  Method: {m}"));
            });
        }

        [Fact]
        public async Task List_Models_Using_OAuth()
        {
            // Arrange
            var model = new GenerativeModel { AccessToken = _fixture.AccessToken };

            // Act
            var sut = await model.ListModels();

            // Assert
            sut.Should().NotBeNull();
            sut.Should().NotBeNull().And.HaveCountGreaterThanOrEqualTo(1);
            sut.ForEach(x =>
            {
                _output.WriteLine($"Model: {x.DisplayName} ({x.Name})");
                x.SupportedGenerationMethods.ForEach(m => _output.WriteLine($"  Method: {m}"));
            });
        }

        [Fact]
        public async Task List_Tuned_Models()
        {
            // Arrange
            var model = new GenerativeModel { AccessToken = _fixture.AccessToken };

            // Act
            var sut = await model.ListModels(true);
            // var sut = await model.ListTunedModels();

            // Assert
            sut.Should().NotBeNull();
            sut.Should().NotBeNull().And.HaveCountGreaterThanOrEqualTo(1);
            sut.ForEach(x =>
            {
                _output.WriteLine($"Model: {x.DisplayName} ({x.Name})");
                x.TuningTask.Snapshots.ForEach(m => _output.WriteLine($"  Snapshot: {m}"));
            });
        }

        [Theory]
        [InlineData(Model.GeminiProVision)]
        [InlineData(Model.BisonText)]
        [InlineData(Model.BisonChat)]
        [InlineData("tunedModels/number-generator-model-psx3d3gljyko")]
        [InlineData(Model.Gemini25Pro)]
        public async Task Get_Model_Information(string modelName)
        {
            // Arrange
            var googleAi = new GoogleAI(apiKey: _fixture.ApiKey);
            var model = _googleAi.GenerativeModel();

            // Act
            var sut = await model.GetModel(model: modelName);

            // Assert
            sut.Should().NotBeNull();
            // sut.Name.Should().Be($"{modelName.SanitizeModelName()}");
            _output.WriteLine($"Model: {sut.DisplayName} ({sut.Name})");
            sut.SupportedGenerationMethods.ForEach(m => _output.WriteLine($"  Method: {m}"));
        }

        [Theory]
        [InlineData("tunedModels/number-generator-model-psx3d3gljyko")]
        public async Task Get_TunedModel_Information_Using_ApiKey(string modelName)
        {
            // Arrange
            var googleAi = new GoogleAI(apiKey: _fixture.ApiKey);
            var model = _googleAi.GenerativeModel();

            // Act & Assert
            await Assert.ThrowsAsync<NotSupportedException>(() => model.GetModel(model: modelName));
        }

        [Theory]
        [InlineData(Model.GeminiProVision)]
        [InlineData(Model.BisonText)]
        [InlineData(Model.BisonChat)]
        [InlineData("tunedModels/number-generator-model-psx3d3gljyko")]
        public async Task Get_Model_Information_Using_OAuth(string modelName)
        {
            // Arrange
            var model = new GenerativeModel { AccessToken = _fixture.AccessToken };
            var expected = modelName;
            if (!expected.Contains("/"))
                expected = $"{expected.SanitizeModelName()}";

            // Act
            var sut = await model.GetModel(model: modelName);

            // Assert
            sut.Should().NotBeNull();
            sut.Name.Should().Be(expected);
            _output.WriteLine($"Model: {sut.DisplayName} ({sut.Name})");
            if (sut.State is null)
            {
                sut?.SupportedGenerationMethods?.ForEach(m => _output.WriteLine($"  Method: {m}"));
            }
            else
            {
                _output.WriteLine($"State: {sut.State}");
            }
        }

        [Fact]
        public async Task Generate_Content()
        {
            // Arrange
            var prompt = "Write a story about a magic backpack.";
            var googleAi = new GoogleAI(apiKey: _fixture.ApiKey);
            var model = _googleAi.GenerativeModel(model: _model);

            // Act
            var response = await model.GenerateContent(prompt);

            // Assert
            response.Should().NotBeNull();
            response.Candidates.Should().NotBeNull().And.HaveCount(1);
            response.Text.Should().NotBeEmpty();
            _output.WriteLine(response?.Text);
        }

        [Fact]
        public async Task Generate_Content_WithEmptyPrompt_ThrowsArgumentNullException()
        {
            // Arrange
            var googleAi = new GoogleAI(apiKey: _fixture.ApiKey);
            var model = _googleAi.GenerativeModel(model: _model);
            string prompt = null;

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => model.GenerateContent(prompt));
        }

        [Fact]
        public async Task Generate_Content_WithInvalidAPIKey_ChangingBeforeRequest()
        {
            // Arrange
            var prompt = "Tell me 4 things about Taipei. Be short.";
            var googleAi = new GoogleAI(apiKey: "WRONG_API_KEY");
            var model = _googleAi.GenerativeModel(model: _model);
            model.ApiKey = _fixture.ApiKey;

            // Act
            var response = await model.GenerateContent(prompt);

            // Assert
            response.Should().NotBeNull();
            response.Candidates.Should().NotBeNull().And.HaveCount(1);
            response.Text.Should().NotBeEmpty();
            _output.WriteLine(response?.Text);
        }

        [Fact]
        public async Task Generate_Content_WithInvalidAPIKey_ChangingAfterRequest()
        {
            // Arrange
            var prompt = "Tell me 4 things about Taipei. Be short.";
            var googleAi = new GoogleAI(apiKey: "AIzaTESTkJmQDe5tghndp6UvqPX0HAA9XpBNGWY");
            var model = _googleAi.GenerativeModel(model: _model);
            await Assert.ThrowsAsync<HttpRequestException>(() => model.GenerateContent(prompt));

            // Act
            model.ApiKey = _fixture.ApiKey;
            var response = await model.GenerateContent(prompt);

            // Assert
            response.Should().NotBeNull();
            response.Candidates.Should().NotBeNull().And.HaveCount(1);
            response.Text.Should().NotBeEmpty();
            _output.WriteLine(response?.Text);
        }

        [Fact]
        public async Task Generate_Content_MaxTokens()
        {
            // Arrange
            var prompt = "Write a story about a magic backpack.";
            var googleAi = new GoogleAI(apiKey: _fixture.ApiKey);
            var model = _googleAi.GenerativeModel(model: _model);
            var generationConfig = new GenerationConfig() { MaxOutputTokens = 20 };

            // Act
            var response = await model.GenerateContent(prompt, generationConfig);

            // Assert
            response.Should().NotBeNull();
            response.Candidates.Should().NotBeNull().And.HaveCount(1);
            response.Text.Should().BeEmpty();
            _output.WriteLine(response?.Text);
        }

        [Fact]
        public async Task Generate_Content_Logprobs()
        {
            // Arrange
            var prompt = "Write a story about a magic backpack.";
            var googleAi = new GoogleAI(apiKey: _fixture.ApiKey);
            var model = _googleAi.GenerativeModel(model: _model);
            var generationConfig = new GenerationConfig() { ResponseLogprobs = true };

            // Act
            var response = await model.GenerateContent(prompt, generationConfig);

            // Assert
            response.Should().NotBeNull();
            response.Candidates.Should().NotBeNull().And.HaveCount(1);
            response.Text.Should().BeEmpty();
            _output.WriteLine(response?.Text);
        }

        [Fact]
        public async Task Generate_Content_Stream_MaxTokens()
        {
            // Arrange
            var prompt = "Write a story about a magic backpack.";
            var googleAi = new GoogleAI(apiKey: _fixture.ApiKey);
            var model = _googleAi.GenerativeModel(model: _model);
            var generationConfig = new GenerationConfig() { MaxOutputTokens = 20 };

            // Act
            var responseStream = model.GenerateContentStream(prompt, generationConfig);

            // Assert
            responseStream.Should().NotBeNull();
            await foreach (var response in responseStream)
            {
                response.Should().NotBeNull();
                response.Candidates.Should().NotBeNull().And.HaveCount(1);
                response.Text.Should().NotBeEmpty();
                if (response.Candidates[0].FinishReason is FinishReason.MaxTokens)
                    _output.WriteLine("...");
                else
                    _output.WriteLine(response?.Text);
            }
        }

        [Fact]
        public async Task Generate_Content_MultiplePrompt()
        {
            // Arrange
            var googleAi = new GoogleAI(apiKey: _fixture.ApiKey);
            var model = _googleAi.GenerativeModel(model: _model);
            var parts = new List<IPart>
            {
                new TextData { Text = "What is x multiplied by 2?" }, new TextData { Text = "x = 42" }
            };

            // Act
            var response = await model.GenerateContent(parts);

            // Assert
            response.Should().NotBeNull();
            response.Candidates.Should().NotBeNull().And.HaveCount(1);
            _output.WriteLine(response?.Text);
            response.Text.Should().Contain("84");
        }

        [Fact]
        public async Task Generate_Content_Request()
        {
            // Arrange
            var prompt = "Write a story about a magic backpack.";
            var googleAi = new GoogleAI(apiKey: _fixture.ApiKey);
            var model = _googleAi.GenerativeModel(model: _model);
            var request = new GenerateContentRequest { Contents = new List<Content>() };
            request.Contents.Add(new Content
            {
                Role = Role.User, Parts = new List<IPart> { new TextData { Text = prompt } }
            });

            // Act
            var response = await model.GenerateContent(request);

            // Assert
            response.Should().NotBeNull();
            response.Candidates.Should().NotBeNull().And.HaveCount(1);
            response.Text.Should().NotBeEmpty();
            _output.WriteLine(response?.Text);
        }

        [Fact]
        public async Task Generate_Content_WithRequest_MultipleCandidates_ThrowsHttpRequestException()
        {
            // Arrange
            var prompt = "Write a short poem about koi fish.";
            var googleAi = new GoogleAI(apiKey: _fixture.ApiKey);
            var model = _googleAi.GenerativeModel(model: _model);
            var request = new GenerateContentRequest(prompt: prompt,
                generationConfig: new GenerationConfig() { CandidateCount = 3 });

            // Act & Assert
            // await Assert.ThrowsAsync<HttpRequestException>(() => model.GenerateContent(request));
            // Act
            var response = await model.GenerateContent(request);

            // Assert
            response.Should().NotBeNull();
            response.Candidates.Should().NotBeNull().And.HaveCountGreaterOrEqualTo(1);
            response.Text.Should().NotBeEmpty();
            _output.WriteLine(response?.Text);
        }

        [Fact]
        public async Task Generate_Content_WithNullRequest_ThrowsArgumentNullException()
        {
            // Arrange
            var googleAi = new GoogleAI(apiKey: _fixture.ApiKey);
            var model = _googleAi.GenerativeModel(model: _model);
            GenerateContentRequest request = null;

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => model.GenerateContent(request));
        }

        [Fact]
        public async Task Generate_Content_RequestConstructor()
        {
            // Arrange
            var prompt = "Write a story about a magic backpack.";
            var googleAi = new GoogleAI(apiKey: _fixture.ApiKey);
            var model = _googleAi.GenerativeModel(model: _model);
            var request = new GenerateContentRequest(prompt);
            request.Contents[0].Role = Role.User;

            // Act
            var response = await model.GenerateContent(request);

            // Assert
            response.Should().NotBeNull();
            response.Candidates.Should().NotBeNull().And.HaveCount(1);
            response.Text.Should().NotBeEmpty();
            _output.WriteLine(response?.Text);
        }

        [Fact]
        public async Task Generate_Content_WithRequest_UseServerSentEvents()
        {
            // Arrange
            var prompt = "Write a story about a magic backpack.";
            var googleAi = new GoogleAI(apiKey: _fixture.ApiKey);
            var model = _googleAi.GenerativeModel(model: _model);
            model.UseServerSentEventsFormat = true;
            var request = new GenerateContentRequest(prompt);

            // Act
            var response = await model.GenerateContent(request);

            // Assert
            response.Should().NotBeNull();
            response.Candidates.Should().NotBeNull().And.HaveCount(1);
            response.Text.Should().NotBeEmpty();
            _output.WriteLine(response?.Text);
        }

        [Fact]
        public async Task Generate_Content_Stream_WithRequest_ServerSentEvents()
        {
            // Arrange
            var prompt = "Write a story about a magic backpack.";
            var googleAi = new GoogleAI(apiKey: _fixture.ApiKey);
            var model = _googleAi.GenerativeModel(model: _model);
            model.UseServerSentEventsFormat = true;
            var request = new GenerateContentRequest(prompt);

            // Act
            var responseEvents = model.GenerateContentStream(request);

            // Assert
            responseEvents.Should().NotBeNull();
            await foreach (var response in responseEvents)
            {
                _output.WriteLine($"{response.Text}");
            }
        }

        [Fact]
        public async Task Generate_Content_with_Modalities()
        {
            // Arrange
            var prompt =
                "Hi, can you create a 3d rendered image of a pig with wings and a top hat flying over a happy futuristic scifi city with lots of greenery?";
            var googleAi = new GoogleAI(apiKey: _fixture.ApiKey);
            var model = _googleAi.GenerativeModel(model: _model);
            var request = new GenerateContentRequest(prompt,
                generationConfig: new() { ResponseModalities = [ResponseModality.Text, ResponseModality.Image] });

            // Act
            var response = await model.GenerateContent(request);

            // Assert
            response.Should().NotBeNull();
            response.Candidates.Should().NotBeNull().And.HaveCount(1);
            response.Candidates![0].Content!.Parts.ForEach(part =>
            {
                _output.WriteLine($"{part.Text}");
                var fileName = Path.Combine(Environment.CurrentDirectory, "payload",
                    Path.ChangeExtension($"{Guid.NewGuid():D}",
                        part.InlineData.MimeType.Replace("image/", "")));
                File.WriteAllBytes(fileName, Convert.FromBase64String(part.InlineData.Data));
                _output.WriteLine($"Wrote image to {fileName}");
            });
        }

        [Fact]
        public async Task Generate_Content_with_Modalities_multiple_Images()
        {
            // Arrange
            var prompt = "Show me how to bake a macaron with images.";
            var googleAi = new GoogleAI(apiKey: _fixture.ApiKey);
            var model = _googleAi.GenerativeModel(model: Model.Gemini20FlashImageGeneration);
            var request = new GenerateContentRequest(prompt,
                generationConfig: new() { ResponseModalities = [ResponseModality.Text, ResponseModality.Image] });

            // Act
            var response = await model.GenerateContent(request);

            // Assert
            response.Should().NotBeNull();
            response.Candidates.Should().NotBeNull().And.HaveCount(1);
            response.Candidates![0].Content!.Parts.ForEach(part =>
            {
                if (!string.IsNullOrEmpty(part.Text))
                    _output.WriteLine($"{part.Text}");
                if (part.InlineData is not null)
                {
                    var fileName = Path.Combine(Environment.CurrentDirectory, "payload",
                        Path.ChangeExtension($"{Guid.NewGuid():D}",
                            part.InlineData.MimeType.Replace("image/", "")));
                    File.WriteAllBytes(fileName, Convert.FromBase64String(part.InlineData.Data));
                    _output.WriteLine($"Wrote image to {fileName}");
                }
            });
        }

        [Fact]
        public async Task Generate_Content_Stream()
        {
            // Arrange
            var prompt = "How are you doing today?";
            var googleAi = new GoogleAI(apiKey: _fixture.ApiKey);
            var model = _googleAi.GenerativeModel(model: _model);

            // Act
            var responseStream = model.GenerateContentStream(prompt);

            // Assert
            responseStream.Should().NotBeNull();
            await foreach (var response in responseStream)
            {
                response.Should().NotBeNull();
                response.Candidates.Should().NotBeNull().And.HaveCount(1);
                response.Text.Should().NotBeEmpty();
                _output.WriteLine($"{response.Text}");
                // response.UsageMetadata.Should().NotBeNull();
                // output.WriteLine($"PromptTokenCount: {response?.UsageMetadata?.PromptTokenCount}");
                // output.WriteLine($"CandidatesTokenCount: {response?.UsageMetadata?.CandidatesTokenCount}");
                // output.WriteLine($"TotalTokenCount: {response?.UsageMetadata?.TotalTokenCount}");
            }
        }

        [Fact]
        public async Task Generate_Content_Stream_Request()
        {
            // Arrange
            var prompt = "How are you doing today?";
            var googleAi = new GoogleAI(apiKey: _fixture.ApiKey);
            var model = _googleAi.GenerativeModel(model: _model);
            var request = new GenerateContentRequest { Contents = new List<Content>() };
            request.Contents.Add(new Content
            {
                Role = Role.User, Parts = new List<IPart> { new TextData { Text = prompt } }
            });

            // Act
            var responseStream = model.GenerateContentStream(request);

            // Assert
            responseStream.Should().NotBeNull();
            await foreach (var response in responseStream)
            {
                response.Should().NotBeNull();
                response.Candidates.Should().NotBeNull().And.HaveCount(1);
                response.Text.Should().NotBeEmpty();
                _output.WriteLine($"{response.Text}");
                // response.UsageMetadata.Should().NotBeNull();
                // output.WriteLine($"PromptTokenCount: {response?.UsageMetadata?.PromptTokenCount}");
                // output.WriteLine($"CandidatesTokenCount: {response?.UsageMetadata?.CandidatesTokenCount}");
                // output.WriteLine($"TotalTokenCount: {response?.UsageMetadata?.TotalTokenCount}");
            }
        }

        [Fact]
        public async Task GenerateAnswer_WithValidRequest_ReturnsAnswerResponse()
        {
            // Arrange
            var googleAi = new GoogleAI(apiKey: _fixture.ApiKey);
            var model = _googleAi.GenerativeModel(model: Model.AttributedQuestionAnswering);
            var request = new GenerateAnswerRequest("What is the capital of France?", AnswerStyle.Abstractive);

            // Act
            var response = await model.GenerateAnswer(request);

            // Assert
            response.Should().NotBeNull();
            response.Answer.Should().NotBeNull();
            response.Text.Should().Be("Paris");
        }

        [Theory]
        [InlineData("How are you doing today?", 6)]
        [InlineData("What kind of fish is this?", 7)]
        [InlineData("Write a story about a magic backpack.", 8)]
        [InlineData("Write an extended story about a magic backpack.", 9)]
        public async Task Count_Tokens(string prompt, int expected)
        {
            // Arrange
            var googleAi = new GoogleAI(apiKey: _fixture.ApiKey);
            var model = _googleAi.GenerativeModel(model: _model);

            // Act
            var response = await model.CountTokens(prompt);

            // Assert
            response.Should().NotBeNull();
            response.TotalTokens.Should().BeGreaterThanOrEqualTo(expected);
            _output.WriteLine($"Tokens: {response?.TotalTokens}");
        }

        [Theory]
        [InlineData("How are you doing today?", 7)]
        [InlineData("What kind of fish is this?", 8)]
        [InlineData("Write a story about a magic backpack.", 9)]
        [InlineData("Write an extended story about a magic backpack.", 10)]
        public async Task Count_Tokens_Request(string prompt, int expected)
        {
            // Arrange
            var googleAi = new GoogleAI(apiKey: _fixture.ApiKey);
            var model = _googleAi.GenerativeModel(model: _model);
            var request = new GenerateContentRequest { Contents = new List<Content>() };
            request.Contents.Add(new Content
            {
                Role = Role.User, Parts = new List<IPart> { new TextData { Text = prompt } }
            });

            // Act
            var response = await model.CountTokens(request);

            // Assert
            response.Should().NotBeNull();
            response.TotalTokens.Should().BeGreaterOrEqualTo(expected);
            _output.WriteLine($"Tokens: {response?.TotalTokens}");
        }

        [Fact]
        public async Task Start_Chat()
        {
            // Arrange
            var googleAi = new GoogleAI(apiKey: _fixture.ApiKey);
            var model = _googleAi.GenerativeModel(model: _model);
            var chat = model.StartChat();
            var prompt = "How can I learn more about C#?";

            // Act
            var response = await chat.SendMessage(prompt);

            // Assert
            response.Should().NotBeNull();
            response.Candidates.Should().NotBeNull().And.HaveCount(1);
            response.Text.Should().NotBeEmpty();
            _output.WriteLine(response?.Text);
        }

        [Fact]
        public async Task Start_Chat_With_History()
        {
            // Arrange
            var googleAi = new GoogleAI(apiKey: _fixture.ApiKey);
            var model = _googleAi.GenerativeModel(model: _model);
            var history = new List<ContentResponse>
            {
                new() { Role = Role.User, Text = "Hello" },
                new() { Role = Role.Model, Text = "Hello! How can I assist you today?" }
            };
            var chat = model.StartChat(history);
            var prompt = "How does electricity work?";

            // Act
            var response = await chat.SendMessage(prompt);

            // Assert
            response.Should().NotBeNull();
            response.Candidates.Should().NotBeNull().And.HaveCount(1);
            response.Text.Should().NotBeEmpty();
            _output.WriteLine(prompt);
            _output.WriteLine(response?.Text);
            //output.WriteLine(response?.PromptFeedback);
        }

        [Fact]
        // Refs:
        // https://cloud.google.com/vertex-ai/generative-ai/docs/multimodal/send-chat-prompts-gemini
        public async Task Start_Chat_Multiple_Prompts()
        {
            // Arrange
            var googleAi = new GoogleAI(apiKey: _fixture.ApiKey);
            var model = _googleAi.GenerativeModel(model: _model);
            var chat = model.StartChat();

            // Act
            var prompt = "Hello, let's talk a bit about nature.";
            var response = await chat.SendMessage(prompt);
            _output.WriteLine(prompt);
            _output.WriteLine(response?.Text);
            prompt = "What are all the colors in a rainbow?";
            response = await chat.SendMessage(prompt);
            _output.WriteLine(prompt);
            _output.WriteLine(response?.Text);
            prompt = "Why does it appear when it rains?";
            response = await chat.SendMessage(prompt);
            _output.WriteLine(prompt);
            _output.WriteLine(response?.Text);

            // Assert
            response.Should().NotBeNull();
            response.Candidates.Should().NotBeNull().And.HaveCount(1);
            response.Text.Should().NotBeEmpty();
        }

        [Fact]
        // Refs:
        // https://ai.google.dev/tutorials/python_quickstart#chat_conversations
        public async Task Start_Chat_Conversations()
        {
            // Arrange
            var googleAi = new GoogleAI(apiKey: _fixture.ApiKey);
            var model = _googleAi.GenerativeModel(model: _model);
            var chat = model.StartChat();

            // Act
            _ = await chat.SendMessage("Hello, fancy brainstorming about IT?");
            _ = await chat.SendMessage("In one sentence, explain how a computer works to a young child.");
            _ = await chat.SendMessage("Okay, how about a more detailed explanation to a high schooler?");
            _ = await chat.SendMessage("Lastly, give a thorough definition for a CS graduate.");

            // Assert
            chat.History.ForEach(c =>
            {
                _output.WriteLine($"{new string('-', 20)}");
                _output.WriteLine($"{c.Role}: {c.Text}");
            });
        }

        [Fact]
        // Refs:
        // https://ai.google.dev/tutorials/python_quickstart#chat_conversations
        public async Task Start_Chat_Rewind_Conversation()
        {
            // Arrange
            var googleAi = new GoogleAI(apiKey: _fixture.ApiKey);
            var model = _googleAi.GenerativeModel(model: _model);
            var chat = model.StartChat();
            _ = await chat.SendMessage("Hello, fancy brainstorming about IT?");
            _ = await chat.SendMessage("In one sentence, explain how a computer works to a young child.");
            _ = await chat.SendMessage("Okay, how about a more detailed explanation to a high school kid?");
            _ = await chat.SendMessage("Lastly, give a thorough definition for a CS graduate.");

            // Act
            var entries = chat.Rewind();

            // Assert
            entries.Should().NotBeNull();
            entries.Sent.Should().NotBeNull();
            entries.Received.Should().NotBeNull();
            _output.WriteLine("------ Rewind ------");
            _output.WriteLine($"{entries.Sent.Role}: {entries.Sent.Text}");
            _output.WriteLine($"{new string('-', 20)}");
            _output.WriteLine($"{entries.Received.Role}: {entries.Received.Text}");
            _output.WriteLine($"{new string('-', 20)}");

            chat.History.Count.Should().Be(6);
            _output.WriteLine("------ History -----");
            chat.History.ForEach(c =>
            {
                _output.WriteLine($"{new string('-', 20)}");
                _output.WriteLine($"{c.Role}: {c.Text}");
            });
        }

        [Fact]
        // Refs:
        // https://ai.google.dev/tutorials/python_quickstart#chat_conversations
        public async Task Start_Chat_Conversations_Get_Last()
        {
            // Arrange
            var googleAi = new GoogleAI(apiKey: _fixture.ApiKey);
            var model = _googleAi.GenerativeModel(model: _model);
            var chat = model.StartChat();
            _ = await chat.SendMessage("Hello, fancy brainstorming about IT?");
            _ = await chat.SendMessage("In one sentence, explain how a computer works to a young child.");
            _ = await chat.SendMessage("Okay, how about a more detailed explanation to a high school kid?");
            _ = await chat.SendMessage("Lastly, give a thorough definition for a CS graduate.");

            // Act
            var sut = chat.Last;

            // Assert
            sut.Should().NotBeNull();
            _output.WriteLine($"{sut.Role}: {sut.Text}");
        }

        [Fact]
        public async Task Start_Chat_With_Multimodal_Content()
        {
            // Arrange
            var systemInstruction = new Content("You are an expert analyzing transcripts.");
            var genAi = new GoogleAI(apiKey: _fixture.ApiKey);
            var model = _googleAi.GenerativeModel(_model, systemInstruction: systemInstruction);
            var chat = model.StartChat();
            var filePath = Path.Combine(Environment.CurrentDirectory, "payload", "a11.txt");
            var document = await _googleAi.UploadFile(filePath, "Apollo 11 Flight Report");
            _output.WriteLine(
                $"Display Name: {document.File.DisplayName} ({Enum.GetName(typeof(StateFileResource), document.File.State)})");

            var request = new GenerateContentRequest("Hi, could you summarize this transcript?");
            request.AddMedia(document.File);

            // Act
            var response = await chat.SendMessage(request);
            _output.WriteLine($"model: {response.Text}");
            _output.WriteLine("----------");
            response = await chat.SendMessage("Okay, could you tell me more about the trans-lunar injection");
            _output.WriteLine($"model: {response.Text}");

            // Assert
            model.Should().NotBeNull();
            chat.History.Count.Should().Be(4);
            response.Should().NotBeNull();
            response.Candidates.Should().NotBeNull().And.HaveCount(1);
            response.Text.Should().NotBeNull();
            _output.WriteLine($"model: {response.Text}");
        }

        [Fact]
        public async Task Start_Chat_Streaming()
        {
            // Arrange
            var googleAi = new GoogleAI(apiKey: _fixture.ApiKey);
            var model = _googleAi.GenerativeModel(model: _model);
            var chat = model.StartChat();
            var prompt = "How can I learn more about C#?";

            // Act
            var responseStream = chat.SendMessageStream(prompt);

            // Assert
            responseStream.Should().NotBeNull();
            await foreach (var response in responseStream)
            {
                response.Should().NotBeNull();
                response.Candidates.Should().NotBeNull().And.HaveCount(1);
                response.Text.Should().NotBeEmpty();
                _output.WriteLine($"{response.Text}");
                // response.UsageMetadata.Should().NotBeNull();
                // output.WriteLine($"PromptTokenCount: {response?.UsageMetadata?.PromptTokenCount}");
                // output.WriteLine($"CandidatesTokenCount: {response?.UsageMetadata?.CandidatesTokenCount}");
                // output.WriteLine($"TotalTokenCount: {response?.UsageMetadata?.TotalTokenCount}");
            }

            chat.History.Count.Should().Be(2);
            _output.WriteLine($"{new string('-', 20)}");
            _output.WriteLine("------ History -----");
            chat.History.ForEach(c =>
            {
                _output.WriteLine($"{new string('-', 20)}");
                _output.WriteLine($"{c.Role}: {c.Text}");
            });
        }

        [Fact]
        // Ref: https://ai.google.dev/api/generate-content#code-execution
        public async Task Generate_Content_Code_Execution()
        {
            // Arrange
            var prompt = "What is the sum of the first 50 prime numbers?";
            var googleAi = new GoogleAI(_fixture.ApiKey);
            var model = _googleAi.GenerativeModel(model: _model,
                tools: [new Tool { CodeExecution = new() }]);

            // Act
            var response = await model.GenerateContent(prompt);

            // Assert
            response.Should().NotBeNull();
            response.Candidates.Should().NotBeNull().And.HaveCount(1);
            _output.WriteLine(string.Join(Environment.NewLine,
                response.Candidates[0].Content.Parts.Select(x =>
                        x.Text)
//                    .Where(t => !string.IsNullOrEmpty(t))
                    .ToArray()));
        }

        [Theory]
        [InlineData(Model.Gemma3)]
        [InlineData(Model.Gemini15Flash)]
        [InlineData(Model.Gemini20Flash)]
        [InlineData(Model.Gemini20FlashLite)]
        [InlineData(Model.Gemini20FlashThinking)]
        [InlineData(Model.Gemini20Pro)]
        [InlineData(Model.Gemini25Pro)]
        // Ref: https://ai.google.dev/api/generate-content#code-execution
        public async Task Generate_Content_Code_Execution_using_FileAPI(string modelName)
        {
            // Arrange
            var prompt = "Calculate the sum of the column 'effect'.";
            var genAi = new GoogleAI(_fixture.ApiKey);
            var model = _googleAi.GenerativeModel(modelName);
            var filePath = Path.Combine(Environment.CurrentDirectory, "payload", "sampledata.csv");
            var file = await ((GoogleAI)genAi).UploadFile(filePath);
            _output.WriteLine($"File: {file.File.Name}\tName: '{file.File.DisplayName}'");
            var request = new GenerateContentRequest(prompt)
            {
                GenerationConfig = new GenerationConfig()
                {
                    Temperature = 1f,
                    TopK = 40,
                    TopP = 0.95f,
                    MaxOutputTokens = 8192,
                    ResponseMimeType = "text/plain"
                },
                Tools = [new Tool { CodeExecution = new() }]
            };
            request.AddMedia(file.File);

            // Act
            var response = await model.GenerateContent(request);

            // Assert
            response.Should().NotBeNull();
            response.Candidates.Should().NotBeNull().And.HaveCount(1);
            _output.WriteLine(string.Join(Environment.NewLine,
                response.Candidates[0].Content.Parts.Select(x =>
                        x.Text)
//                    .Where(t => !string.IsNullOrEmpty(t))
                    .ToArray()));
        }

        [Fact]
        // Ref: https://ai.google.dev/gemini-api/docs/grounding
        public async Task Generate_Content_Grounding_Search()
        {
            // Arrange
            var prompt = "What is the current Google (GOOG) stock price?";
            var genAi = new GoogleAI(_fixture.ApiKey, logger: Logger);
            var model = _googleAi.GenerativeModel(model: _model);
            model.UseGrounding = true;

            // Act
            var response = await model.GenerateContent(prompt);

            // Assert
            response.Should().NotBeNull();
            response.Candidates.Should().NotBeNull().And.HaveCount(1);
            response.Candidates![0].GroundingMetadata.Should().NotBeNull();
            response.Candidates![0].GroundingMetadata!.SearchEntryPoint.Should().NotBeNull();
            response.Candidates![0].GroundingMetadata!.WebSearchQueries.Should().NotBeNull();
            _output.WriteLine(string.Join(Environment.NewLine,
                response.Candidates![0].Content!.Parts
                    .Select(x => x.Text)
//                    .Where(t => !string.IsNullOrEmpty(t))
                    .ToArray()));
            response.Candidates![0].GroundingMetadata!.GroundingChunks?
                .ForEach(c =>
                    _output.WriteLine($"{c!.Web!.Title} - {c!.Web!.Uri}"));
            _output.WriteLine(string.Join(Environment.NewLine,
                response.Candidates![0].GroundingMetadata!.WebSearchQueries!
                    .Select(w => w)
                    .ToArray()));
            _output.WriteLine(response.Candidates![0].GroundingMetadata!.SearchEntryPoint!.RenderedContent);
        }

        [Fact]
        // Ref: https://ai.google.dev/gemini-api/docs/grounding
        public async Task Generate_Content_Grounding_Search_by_String()
        {
            // Arrange
            var prompt = "Who won Wimbledon this year?";
            var genAi = new GoogleAI(_fixture.ApiKey);
            var model = _googleAi.GenerativeModel("gemini-1.5-pro-002",
                tools: [new Tool { GoogleSearchRetrieval = new() }]);

            // Act
            var response = await model.GenerateContent(prompt);

            // Assert
            response.Should().NotBeNull();
            response.Candidates.Should().NotBeNull().And.HaveCount(1);
            response.Candidates![0].GroundingMetadata.Should().NotBeNull();
            response.Candidates![0].GroundingMetadata!.SearchEntryPoint.Should().NotBeNull();
            response.Candidates![0].GroundingMetadata!.WebSearchQueries.Should().NotBeNull();
            _output.WriteLine(string.Join(Environment.NewLine,
                response.Candidates![0].Content!.Parts
                    .Select(x => x.Text)
//                    .Where(t => !string.IsNullOrEmpty(t))
                    .ToArray()));
            response.Candidates![0].GroundingMetadata!.GroundingChunks?
                .ForEach(c =>
                    _output.WriteLine($"{c!.Web!.Title} - {c!.Web!.Uri}"));
            _output.WriteLine(string.Join(Environment.NewLine,
                response.Candidates![0].GroundingMetadata!.WebSearchQueries!
                    .Select(w => w)
                    .ToArray()));
            _output.WriteLine(response.Candidates![0].GroundingMetadata!.SearchEntryPoint!.RenderedContent);
        }

        [Fact]
        // Ref: https://ai.google.dev/gemini-api/docs/grounding
        public async Task Generate_Content_Grounding_Search_by_Dictionary()
        {
            // Arrange
            var prompt = "Who won Wimbledon this year?";
            var genAi = new GoogleAI(_fixture.ApiKey);
            var model = _googleAi.GenerativeModel("gemini-1.5-pro-002",
                tools:
                [
                    new Tool
                    {
                        GoogleSearchRetrieval =
                            new(DynamicRetrievalConfigMode.ModeUnspecified, 0.06f)
                    }
                ]);
            model.UseGrounding = true;

            // Act
            var response = await model.GenerateContent(prompt);

            // Assert
            response.Should().NotBeNull();
            response.Candidates.Should().NotBeNull().And.HaveCount(1);
            _output.WriteLine(string.Join(Environment.NewLine,
                response.Candidates![0].Content!.Parts
                    .Select(x => x.Text)
//                    .Where(t => !string.IsNullOrEmpty(t))
                    .ToArray()));
            response.Candidates![0].GroundingMetadata!.GroundingChunks?
                .ForEach(c =>
                    _output.WriteLine($"{c!.Web!.Title} - {c!.Web!.Uri}"));
            _output.WriteLine(string.Join(Environment.NewLine,
                response.Candidates![0].GroundingMetadata!.WebSearchQueries!
                    .Select(w => w)
                    .ToArray()));
            _output.WriteLine(response.Candidates![0].GroundingMetadata!.SearchEntryPoint!.RenderedContent);
        }

        [Theory]
        [InlineData(Model.Gemini15Pro002)]
        [InlineData(Model.Gemini15Flash)]
        [InlineData(Model.Gemini20Flash)]
        [InlineData(Model.Gemini20Flash001)]
        [InlineData(Model.Gemini20FlashLite)]
        [InlineData(Model.Gemini20ProExperimental)]
        [InlineData(Model.Gemini25Pro)]
        // Ref: https://ai.google.dev/gemini-api/docs/grounding
        public async Task Generate_Content_Grounding_Search_Default(string modelName)
        {
            // Arrange
            var prompt = "When and where does F1 start this year?";
            var genAi = new GoogleAI(_fixture.ApiKey);
            var model = _googleAi.GenerativeModel(modelName,
                tools:
                [
                    new Tool
                    {
                        GoogleSearchRetrieval = new() { DynamicRetrievalConfig = new() { DynamicThreshold = 0.6f } }
                    }
                ]);

            // Act
            var response = await model.GenerateContent(prompt);

            // Assert
            response.Should().NotBeNull();
            response.Candidates.Should().NotBeNull().And.HaveCount(1);
            _output.WriteLine(string.Join(Environment.NewLine,
                response.Candidates![0].Content!.Parts
                    .Select(x => x.Text)
//                    .Where(t => !string.IsNullOrEmpty(t))
                    .ToArray()));
            response.Candidates![0].GroundingMetadata!.GroundingChunks?
                .ForEach(c =>
                    _output.WriteLine($"{c!.Web!.Title} - {c!.Web!.Uri}"));
            _output.WriteLine(string.Join(Environment.NewLine,
                response.Candidates![0].GroundingMetadata!.WebSearchQueries!
                    .Select(w => w)
                    .ToArray()));
            _output.WriteLine(response.Candidates![0].GroundingMetadata!.SearchEntryPoint!.RenderedContent);
        }

        [Fact]
        // Ref: https://ai.google.dev/gemini-api/docs/models/gemini-v2#search-tool
        public async Task Generate_Content_with_Google_Search()
        {
            // Arrange
            var prompt = "When is the next total solar eclipse in Mauritius?";
            var genAi = new GoogleAI(_fixture.ApiKey);
            var model = _googleAi.GenerativeModel(model: _model);
            model.UseGoogleSearch = true;

            // Act
            var response = await model.GenerateContent(prompt);

            // Assert
            response.Should().NotBeNull();
            response.Candidates.Should().NotBeNull().And.HaveCount(1);
            response.Candidates![0].GroundingMetadata.Should().NotBeNull();
            response.Candidates![0].GroundingMetadata!.SearchEntryPoint.Should().NotBeNull();
            response.Candidates![0].GroundingMetadata!.WebSearchQueries.Should().NotBeNull();
            _output.WriteLine(string.Join(Environment.NewLine,
                response.Candidates![0].Content!.Parts
                    .Select(x => x.Text)
//                    .Where(t => !string.IsNullOrEmpty(t))
                    .ToArray()));
            response.Candidates![0].GroundingMetadata!.GroundingChunks?
                .ForEach(c =>
                    _output.WriteLine($"{c!.Web!.Title} - {c!.Web!.Uri}"));
            _output.WriteLine(string.Join(Environment.NewLine,
                response.Candidates![0].GroundingMetadata!.WebSearchQueries!
                    .Select(w => w)
                    .ToArray()));
            _output.WriteLine(response.Candidates![0].GroundingMetadata!.SearchEntryPoint!.RenderedContent);
        }

        [Fact]
        // Ref: https://ai.google.dev/gemini-api/docs/models/gemini-v2#search-tool
        public async Task Generate_Content_with_GoogleSearch_ResponseModalities()
        {
            // Arrange
            var prompt = "When is the next total solar eclipse in Mauritius?";
            var genAi = new GoogleAI(_fixture.ApiKey);
            var model = _googleAi.GenerativeModel(model: _model);
            model.UseGoogleSearch = true;

            // Act
            var response = await model.GenerateContent(prompt,
                generationConfig: new GenerationConfig() { ResponseModalities = [ResponseModality.Text] });

            // Assert
            response.Should().NotBeNull();
            response.Candidates.Should().NotBeNull().And.HaveCount(1);
            response.Candidates![0].GroundingMetadata.Should().NotBeNull();
            response.Candidates![0].GroundingMetadata!.SearchEntryPoint.Should().NotBeNull();
            response.Candidates![0].GroundingMetadata!.WebSearchQueries.Should().NotBeNull();
            _output.WriteLine(string.Join(Environment.NewLine,
                response.Candidates![0].Content!.Parts
                    .Select(x => x.Text)
//                    .Where(t => !string.IsNullOrEmpty(t))
                    .ToArray()));
            response.Candidates![0].GroundingMetadata!.GroundingChunks?
                .ForEach(c =>
                    _output.WriteLine($"{c!.Web!.Title} - {c!.Web!.Uri}"));
            _output.WriteLine(string.Join(Environment.NewLine,
                response.Candidates![0].GroundingMetadata!.WebSearchQueries!
                    .Select(w => w)
                    .ToArray()));
            _output.WriteLine(response.Candidates![0].GroundingMetadata!.SearchEntryPoint!.RenderedContent);
        }

        [Fact]
        // Ref: https://ai.google.dev/docs/function_calling
        public async Task Function_Calling()
        {
            // Arrange
            var prompt = "Which theaters in Mountain View show Barbie movie?";
            var googleAi = new GoogleAI(apiKey: _fixture.ApiKey);
            var model = _googleAi.GenerativeModel(model: _model);
            List<Tool> tools =
            [
                new Tool()
                {
                    FunctionDeclarations =
                    [
                        new()
                        {
                            Name = "find_movies",
                            Description =
                                "find movie titles currently playing in theaters based on any description, genre, title words, etc.",
                            Parameters = new()
                            {
                                Type = ParameterType.Object,
                                Properties = new
                                {
                                    Location = new
                                    {
                                        Type = ParameterType.String,
                                        Description =
                                            "The city and state, e.g. San Francisco, CA or a zip code e.g. 95616"
                                    },
                                    Description = new
                                    {
                                        Type = ParameterType.String,
                                        Description =
                                            "Any kind of description including category or genre, title words, attributes, etc."
                                    }
                                },
                                Required = ["description"]
                            }
                        },


                        new()
                        {
                            Name = "find_theaters",
                            Description =
                                "find theaters based on location and optionally movie title which are is currently playing in theaters",
                            Parameters = new()
                            {
                                Type = ParameterType.Object,
                                Properties = new
                                {
                                    Location = new
                                    {
                                        Type = ParameterType.String,
                                        Description =
                                            "The city and state, e.g. San Francisco, CA or a zip code e.g. 95616"
                                    },
                                    Movie = new { Type = ParameterType.String, Description = "Any movie title" }
                                },
                                Required = ["location"]
                            }
                        },


                        new()
                        {
                            Name = "get_showtimes",
                            Description = "Find the start times for movies playing in a specific theater",
                            Parameters = new()
                            {
                                Type = ParameterType.Object,
                                Properties = new
                                {
                                    Location = new
                                    {
                                        Type = ParameterType.String,
                                        Description =
                                            "The city and state, e.g. San Francisco, CA or a zip code e.g. 95616"
                                    },
                                    Movie = new { Type = ParameterType.String, Description = "Any movie title" },
                                    Theater = new { Type = ParameterType.String, Description = "Name of the theater" },
                                    Date = new
                                    {
                                        Type = ParameterType.String, Description = "Date for requested showtime"
                                    }
                                },
                                Required = ["location", "movie", "theater", "date"]
                            }
                        }
                    ]
                }
            ];

            // Act
            var response = await model.GenerateContent(prompt, tools: tools);

            // Assert
            response.Should().NotBeNull();
            response.Candidates.Should().NotBeNull().And.HaveCount(1);
            response?.Candidates?[0]?.Content?.Parts[0]?.FunctionCall?.Should().NotBeNull();
            _output.WriteLine(response?.Candidates?[0]?.Content?.Parts[0]?.FunctionCall?.Name);
            _output.WriteLine(response?.Candidates?[0]?.Content?.Parts[0]?.FunctionCall?.Args?.ToString());
        }

        protected string SearchEvent(string name)
        {
            return name.ToLower() switch
            {
                "player move" => "on move",
                _ => "not found"
            };
        }

        [Fact]
        public async Task Function_Calling_Issue74()
        {
            // Arrange
            var prompt = "Is there an event when a player moves?";
            var model = _googleAi.GenerativeModel(model: _model);
            List<Tool> tools =
            [
                new Tool()
                {
                    FunctionDeclarations =
                    [
                        new()
                        {
                            Name = "find_event",
                            Description = "search for an event in the documentation",
                            Parameters = new()
                            {
                                Type = ParameterType.Object,
                                Properties = new
                                {
                                    Name = new
                                    {
                                        Type = ParameterType.String,
                                        Description = "The name of the event to search for"
                                    },
                                },
                                Required = ["name"]
                            }
                        },
                    ]
                }
            ];
            var chatSession = new ChatSession(model, tools: tools);
            var response = await chatSession.SendMessage(prompt);
            var functionArgs = response?.Candidates?[0]?.Content?.Parts[0]?.FunctionCall?.Args as JsonElement?;
            string? name = functionArgs?.GetProperty("name").GetString();
            string result = SearchEvent(name);
            var functionResponsePart = new Part
            {
                FunctionResponse = new FunctionResponse { Name = "find_event", Response = new { content = result } }
            };

            // Act
            var followUpResponse = await chatSession.SendMessage([functionResponsePart]);

            // Assert
            followUpResponse.Should().NotBeNull();
            _output.WriteLine(followUpResponse.Text);
        }

        [Fact]
        // Ref: https://ai.google.dev/docs/function_calling#function-calling-one-and-a-half-turn-curl-sample
        public async Task Function_Calling_MultiTurn()
        {
            // Arrange
            var prompt = "Which theaters in Mountain View show Barbie movie?";
            var googleAi = new GoogleAI(apiKey: _fixture.ApiKey);
            var model = _googleAi.GenerativeModel(model: _model);
            List<Tool> tools =
            [
                new Tool()
                {
                    FunctionDeclarations =
                    [
                        new()
                        {
                            Name = "find_movies",
                            Description =
                                "find movie titles currently playing in theaters based on any description, genre, title words, etc.",
                            Parameters = new()
                            {
                                Type = ParameterType.Object,
                                Properties = new
                                {
                                    Location = new
                                    {
                                        Type = ParameterType.String,
                                        Description =
                                            "The city and state, e.g. San Francisco, CA or a zip code e.g. 95616"
                                    },
                                    Description = new
                                    {
                                        Type = ParameterType.String,
                                        Description =
                                            "Any kind of description including category or genre, title words, attributes, etc."
                                    }
                                },
                                Required = ["description"]
                            }
                        },


                        new()
                        {
                            Name = "find_theaters",
                            Description =
                                "find theaters based on location and optionally movie title which are is currently playing in theaters",
                            Parameters = new()
                            {
                                Type = ParameterType.Object,
                                Properties = new
                                {
                                    Location = new
                                    {
                                        Type = ParameterType.String,
                                        Description =
                                            "The city and state, e.g. San Francisco, CA or a zip code e.g. 95616"
                                    },
                                    Movie = new { Type = ParameterType.String, Description = "Any movie title" }
                                },
                                Required = ["location"]
                            }
                        },


                        new()
                        {
                            Name = "get_showtimes",
                            Description = "Find the start times for movies playing in a specific theater",
                            Parameters = new()
                            {
                                Type = ParameterType.Object,
                                Properties = new
                                {
                                    Location = new
                                    {
                                        Type = ParameterType.String,
                                        Description =
                                            "The city and state, e.g. San Francisco, CA or a zip code e.g. 95616"
                                    },
                                    Movie = new { Type = ParameterType.String, Description = "Any movie title" },
                                    Theater = new { Type = ParameterType.String, Description = "Name of the theater" },
                                    Date = new
                                    {
                                        Type = ParameterType.String, Description = "Date for requested showtime"
                                    }
                                },
                                Required = ["location", "movie", "theater", "date"]
                            }
                        }
                    ]
                }
            ];
            var request = new GenerateContentRequest(prompt, tools: tools);
            request.Contents[0].Role = Role.User;
            request.Contents.Add(new Content()
            {
                Role = Role.Model,
                Parts = new()
                {
                    new FunctionCall()
                    {
                        Name = "find_theaters",
                        Args = new { Location = "Mountain View, CA", Movie = "Barbie" }
                    }
                }
            });
            request.Contents.Add(new Content()
            {
                Role = Role.Function,
                Parts = new()
                {
                    new FunctionResponse()
                    {
                        Name = "find_theaters",
                        Response = new
                        {
                            Name = "find_theaters",
                            Content = new
                            {
                                Movie = "Barbie",
                                Theaters = new dynamic[]
                                {
                                    new
                                    {
                                        Name = "AMC Mountain View 16",
                                        Address =
                                            "2000 W El Camino Real, Mountain View, CA 94040"
                                    },
                                    new
                                    {
                                        Name = "Regal Edwards 14",
                                        Address = "245 Castro St, Mountain View, CA 94040"
                                    }
                                }
                            }
                        }
                    }
                }
            });

            // Act
            var response = await model.GenerateContent(request);

            // Assert
            response.Should().NotBeNull();
            response.Candidates.Should().NotBeNull().And.HaveCount(1);
            response.Text.Should().NotBeEmpty();
            _output.WriteLine(response?.Text);
        }

        [Fact]
        // Ref: https://ai.google.dev/docs/function_calling#multi-turn-example-2
        public async Task Function_Calling_MultiTurn_Multiple()
        {
            // Arrange
            var prompt = "Which theaters in Mountain View show Barbie movie?";
            var googleAi = new GoogleAI(apiKey: _fixture.ApiKey);
            var model = _googleAi.GenerativeModel(model: _model);
            List<Tool> tools =
            [
                new Tool()
                {
                    FunctionDeclarations =
                    [
                        new()
                        {
                            Name = "find_movies",
                            Description =
                                "find movie titles currently playing in theaters based on any description, genre, title words, etc.",
                            Parameters = new()
                            {
                                Type = ParameterType.Object,
                                Properties = new
                                {
                                    Location = new
                                    {
                                        Type = ParameterType.String,
                                        Description =
                                            "The city and state, e.g. San Francisco, CA or a zip code e.g. 95616"
                                    },
                                    Description = new
                                    {
                                        Type = ParameterType.String,
                                        Description =
                                            "Any kind of description including category or genre, title words, attributes, etc."
                                    }
                                },
                                Required = ["description"]
                            }
                        },


                        new()
                        {
                            Name = "find_theaters",
                            Description =
                                "find theaters based on location and optionally movie title which are is currently playing in theaters",
                            Parameters = new()
                            {
                                Type = ParameterType.Object,
                                Properties = new
                                {
                                    Location = new
                                    {
                                        Type = ParameterType.String,
                                        Description =
                                            "The city and state, e.g. San Francisco, CA or a zip code e.g. 95616"
                                    },
                                    Movie = new { Type = ParameterType.String, Description = "Any movie title" }
                                },
                                Required = ["location"]
                            }
                        },


                        new()
                        {
                            Name = "get_showtimes",
                            Description = "Find the start times for movies playing in a specific theater",
                            Parameters = new()
                            {
                                Type = ParameterType.Object,
                                Properties = new
                                {
                                    Location = new
                                    {
                                        Type = ParameterType.String,
                                        Description =
                                            "The city and state, e.g. San Francisco, CA or a zip code e.g. 95616"
                                    },
                                    Movie = new { Type = ParameterType.String, Description = "Any movie title" },
                                    Theater = new { Type = ParameterType.String, Description = "Name of the theater" },
                                    Date = new
                                    {
                                        Type = ParameterType.String, Description = "Date for requested showtime"
                                    }
                                },
                                Required = ["location", "movie", "theater", "date"]
                            }
                        }
                    ]
                }
            ];
            var request = new GenerateContentRequest(prompt, tools: tools);
            request.Contents[0].Role = Role.User;
            request.Contents.Add(new Content()
            {
                Role = Role.Model,
                Parts = new()
                {
                    new FunctionCall()
                    {
                        Name = "find_theaters",
                        Args = new { Location = "Mountain View, CA", Movie = "Barbie" }
                    }
                }
            });
            request.Contents.Add(new Content()
            {
                Role = Role.Function,
                Parts = new()
                {
                    new FunctionResponse()
                    {
                        Name = "find_theaters",
                        Response = new
                        {
                            Name = "find_theaters",
                            Content = new
                            {
                                Movie = "Barbie",
                                Theaters = new dynamic[]
                                {
                                    new
                                    {
                                        Name = "AMC Mountain View 16",
                                        Address =
                                            "2000 W El Camino Real, Mountain View, CA 94040"
                                    },
                                    new
                                    {
                                        Name = "Regal Edwards 14",
                                        Address = "245 Castro St, Mountain View, CA 94040"
                                    }
                                }
                            }
                        }
                    }
                }
            });
            request.Contents.Add(new Content()
            {
                Role = Role.Model,
                Parts = new()
                {
                    new TextData()
                    {
                        Text =
                            "OK. I found two theaters in Mountain View showing Barbie: AMC Mountain View 16 and Regal Edwards 14."
                    }
                }
            });
            request.Contents.Add(new Content()
            {
                Role = Role.User,
                Parts = new()
                {
                    new TextData() { Text = "Can we recommend some comedy movies on show in Mountain View?" }
                }
            });

            // Act
            var response = await model.GenerateContent(request);

            // Assert
            response.Should().NotBeNull();
            response.Candidates.Should().NotBeNull().And.HaveCount(1);
            response?.Candidates?[0]?.Content?.Parts[0]?.FunctionCall?.Should().NotBeNull();
            _output.WriteLine(response?.Candidates?[0]?.Content?.Parts[0]?.FunctionCall?.Name);
            _output.WriteLine(response?.Candidates?[0]?.Content?.Parts[0]?.FunctionCall?.Args?.ToString());
        }

        [Fact(Skip = "Work in progress")]
        public Task Function_Calling_Chat()
        {
            // Arrange
            var googleAi = new GoogleAI(apiKey: _fixture.ApiKey);
            var model = _googleAi.GenerativeModel(model: _model);
            var chat = model.StartChat(tools: new List<Tool>());
            var chatInput1 = "What is the weather in Boston?";

            // Act
            //var result1 = await chat.SendMessageStream(prompt);
            //var response1 = await result1.Response;
            //var result2 = await chat.SendMessageStream(new List<IPart> { new FunctionResponse() });
            //var response2 = await result2.Response;

            //// Assert
            //response1.Should().NotBeNull();
            //response.Candidates.Should().NotBeNull().And.HaveCount(1);
            //response.Text.Should().NotBeEmpty();
            //output.WriteLine(response?.Text);
            return Task.FromResult(Task.CompletedTask);
        }

        [Fact(Skip = "Work in progress")]
        public Task Function_Calling_ContentStream()
        {
            // Arrange
            var googleAi = new GoogleAI(apiKey: _fixture.ApiKey);
            var model = _googleAi.GenerativeModel(model: _model);
            var request = new GenerateContentRequest { Contents = new List<Content>(), Tools = new List<Tool> { } };
            request.Contents.Add(new Content
            {
                Role = Role.User,
                Parts = new List<IPart> { new TextData { Text = "What is the weather in Boston?" } }
            });
            request.Contents.Add(new Content
            {
                Role = Role.Model,
                Parts = new List<IPart>
                {
                    new FunctionCall { Name = "get_current_weather", Args = new { location = "Boston" } }
                }
            });
            request.Contents.Add(new Content
            {
                Role = Role.Function, Parts = new List<IPart> { new FunctionResponse() }
            });

            // Act
            var response = model.GenerateContentStream(request);

            // Assert
            // response.Should().NotBeNull().And.HaveCountGreaterThanOrEqualTo(1);
            // response.FirstOrDefault().Should().NotBeNull();
            // response.ForEach(x => output.WriteLine(x.Text));
            // response.LastOrDefault().UsageMetadata.Should().NotBeNull();
            // output.WriteLine($"PromptTokenCount: {response.LastOrDefault().UsageMetadata.PromptTokenCount}");
            // output.WriteLine($"CandidatesTokenCount: {response.LastOrDefault().UsageMetadata.CandidatesTokenCount}");
            // output.WriteLine($"TotalTokenCount: {response.LastOrDefault().UsageMetadata.TotalTokenCount}");
            return Task.FromResult(Task.CompletedTask);
        }

        [Fact]
        public async Task Create_Tuned_Model()
        {
            // Arrange
            var googleAI = new GoogleAI(accessToken: _fixture.AccessToken);
            var model = googleAI.GenerativeModel(model: Model.GeminiPro);
            model.ProjectId = _fixture.ProjectId;
            var request = new CreateTunedModelRequest()
            {
                BaseModel = $"{Model.GeminiPro.SanitizeModelName()}",
                DisplayName = "Autogenerated Test model",
                TuningTask = new()
                {
                    Hyperparameters = new() { BatchSize = 2, LearningRate = 0.001f, EpochCount = 3 },
                    TrainingData = new()
                    {
                        Examples = new()
                        {
                            Examples = new()
                            {
                                new TuningExample() { TextInput = "1", Output = "2" },
                                new TuningExample() { TextInput = "3", Output = "4" },
                                new TuningExample() { TextInput = "-3", Output = "-2" },
                                new TuningExample() { TextInput = "twenty two", Output = "twenty three" },
                                new TuningExample() { TextInput = "two hundred", Output = "two hundred one" },
                                new TuningExample() { TextInput = "ninety nine", Output = "one hundred" },
                                new TuningExample() { TextInput = "8", Output = "9" },
                                new TuningExample() { TextInput = "-98", Output = "-97" },
                                new TuningExample() { TextInput = "1,000", Output = "1,001" },
                                new TuningExample() { TextInput = "thirteen", Output = "fourteen" },
                                new TuningExample() { TextInput = "seven", Output = "eight" },
                            }
                        }
                    }
                }
            };

            // Act
            var response = await model.CreateTunedModel(request);

            // Assert
            response.Should().NotBeNull();
            response.Name.Should().NotBeNull();
            response.Metadata.Should().NotBeNull();
            _output.WriteLine($"Name: {response.Name}");
            _output.WriteLine($"Model: {response.Metadata.TunedModel} (Steps: {response.Metadata.TotalSteps})");
        }

        [Fact]
        public async Task Create_Tuned_Model_Simply()
        {
            // Arrange
            var googleAI = new GoogleAI(accessToken: _fixture.AccessToken);
            var model = googleAI.GenerativeModel(model: Model.GeminiPro);
            model.ProjectId = _fixture.ProjectId;
            var parameters = new HyperParameters() { BatchSize = 2, LearningRate = 0.001f, EpochCount = 3 };
            var dataset = new List<TuningExample>
            {
                new() { TextInput = "1", Output = "2" },
                new() { TextInput = "3", Output = "4" },
                new() { TextInput = "-3", Output = "-2" },
                new() { TextInput = "twenty two", Output = "twenty three" },
                new() { TextInput = "two hundred", Output = "two hundred one" },
                new() { TextInput = "ninety nine", Output = "one hundred" },
                new() { TextInput = "8", Output = "9" },
                new() { TextInput = "-98", Output = "-97" },
                new() { TextInput = "1,000", Output = "1,001" },
                new() { TextInput = "thirteen", Output = "fourteen" },
                new() { TextInput = "seven", Output = "eight" },
            };
            var request = new CreateTunedModelRequest(Model.GeminiPro,
                "Simply autogenerated Test model",
                dataset,
                parameters);

            // Act
            var response = await model.CreateTunedModel(request);

            // Assert
            response.Should().NotBeNull();
            response.Name.Should().NotBeNull();
            response.Metadata.Should().NotBeNull();
            _output.WriteLine($"Name: {response.Name}");
            _output.WriteLine($"Model: {response.Metadata.TunedModel} (Steps: {response.Metadata.TotalSteps})");
        }

        [Fact]
        public async Task Delete_Tuned_Model()
        {
            // Arrange
            var modelName =
                "tunedModels/number-generator-model-psx3d3gljyko"; // see List_Tuned_Models for available options.
            var googleAI = new GoogleAI(accessToken: _fixture.AccessToken);
            var model = googleAI.GenerativeModel();
            model.ProjectId = _fixture.ProjectId;

            // Act
            var response = await model.DeleteTunedModel(modelName);

            // Assert
            response.Should().NotBeNull();
            _output.WriteLine(response);
        }

        [Theory]
        [InlineData("255", "256")]
        [InlineData("41", "42")]
        // [InlineData("five", "six")]
        // [InlineData("Six hundred thirty nine", "Six hundred forty")]
        public async Task Generate_Content_TunedModel(string prompt, string expected)
        {
            // Arrange
            var googleAI = new GoogleAI(accessToken: _fixture.AccessToken);
            var model = googleAI.GenerativeModel(model: "tunedModels/autogenerated-test-model-48gob9c9v54p");
            model.ProjectId = _fixture.ProjectId;

            // Act
            var response = await model.GenerateContent(prompt);

            // Assert
            response.Should().NotBeNull();
            response.Candidates.Should().NotBeNull().And.HaveCount(1);
            response.Text.Should().NotBeEmpty();
            _output.WriteLine(response?.Text);
            response?.Text.Should().Be(expected);
        }

        [Fact]
        public async Task Generate_Content_Using_JsonMode()
        {
            // Arrange
            var prompt = "List a few popular cookie recipes.";
            var googleAi = new GoogleAI(apiKey: _fixture.ApiKey);
            var model = _googleAi.GenerativeModel(model: _model);
            model.UseJsonMode = true;

            // Act
            var response = await model.GenerateContent(prompt);

            // Assert
            response.Should().NotBeNull();
            response.Candidates.Should().NotBeNull().And.HaveCount(1);
            response.Text.Should().NotBeEmpty();
            _output.WriteLine(response?.Text);
        }

        [Fact]
        public async Task Generate_Content_Using_JsonMode_SchemaPrompt()
        {
            // Arrange
            var prompt =
                "List a few popular cookie recipes using this JSON schema: {'type': 'object', 'properties': { 'recipe_name': {'type': 'string'}}}";
            var googleAi = new GoogleAI(apiKey: _fixture.ApiKey);
            var model = _googleAi.GenerativeModel(model: _model);
            model.UseJsonMode = true;

            // Act
            var response = await model.GenerateContent(prompt);

            // Assert
            response.Should().NotBeNull();
            response.Candidates.Should().NotBeNull().And.HaveCount(1);
            response.Text.Should().NotBeEmpty();
            _output.WriteLine(response?.Text);
        }

        class Recipe
        {
            public string Name { get; set; }
#if NET9_0
            public required string RecipeName { get; set; }
#endif
            public List<string> Ingredients { get; set; }
        }

        [Fact]
        public async Task Generate_Content_Using_ResponseSchema_with_List()
        {
            // Arrange
            var prompt = "List a few popular cookie recipes.";
            var googleAi = new GoogleAI(apiKey: _fixture.ApiKey);
            var model = _googleAi.GenerativeModel(model: _model);
            var generationConfig = new GenerationConfig()
            {
                ResponseMimeType = "application/json", ResponseSchema = new List<Recipe>()
                ResponseMimeType = "application/json", 
                ResponseSchema = new List<Recipe>()
            };

            // Act
            var response = await model.GenerateContent(prompt,
                generationConfig: generationConfig);

            // Assert
            response.Should().NotBeNull();
            response.Candidates.Should().NotBeNull().And.HaveCount(1);
            response.Text.Should().NotBeEmpty();
            _output.WriteLine(response?.Text);
        }

        class FlightSchedule
        {
            public string Time { get; set; }
            public string Destination { get; set; }
        }

        [Fact]
        public async Task Generate_Content_Using_ResponseSchema_with_List_From_InlineData()
        {
            // Arrange
            var prompt = "Parse the time and city from the airport board shown in this image into a list.";
            var googleAi = new GoogleAI(apiKey: _fixture.ApiKey);
            var model = _googleAi.GenerativeModel(model: _model);
            // Images
            var board = await TestExtensions.ReadImageFileBase64Async(
                "https://raw.githubusercontent.com/mscraftsman/generative-ai/refs/heads/main/tests/Mscc.GenerativeAI/payload/timetable.png");
            var request = new GenerateContentRequest(prompt);
            request.Contents[0].Parts.Add(
                new InlineData { MimeType = "image/png", Data = board }
            );
            var generationConfig = new GenerationConfig()
            {
                ResponseMimeType = "application/json", ResponseSchema = new List<FlightSchedule>()
            };

            // Act
            var response = await model.GenerateContent(prompt,
                generationConfig: generationConfig);

            // Assert
            response.Should().NotBeNull();
            response.Candidates.Should().NotBeNull().And.HaveCount(1);
            response.Candidates.FirstOrDefault().Content.Should().NotBeNull();
            response.Candidates.FirstOrDefault().Content.Parts.Should().NotBeNull().And
                .HaveCountGreaterThanOrEqualTo(1);
            _output.WriteLine(response?.Text);
        }

        // Define the Instrument enum
        public enum Instrument
        {
            [EnumMember(Value = "Percussion")] Percussion,
            [EnumMember(Value = "String")] String,
            [EnumMember(Value = "Woodwind")] Woodwind,
            [EnumMember(Value = "Brass")] Brass,
            [EnumMember(Value = "Keyboard")] Keyboard
        }

        [Theory]
        [InlineData("What type of instrument is an oboe?")]
        [InlineData("What type of instrument is an grand piano?")]
        [InlineData("What type of instrument is an guitar?")]
        public async Task Generate_Content_Using_ResponseSchema_with_Enumeration(string prompt)
        {
            // Arrange
            var googleAi = new GoogleAI(apiKey: _fixture.ApiKey);
            var model = _googleAi.GenerativeModel(model: _model);

            var generationConfig = new GenerationConfig
            {
                ResponseMimeType = "text/x.enum", // Important for enum handling
                ResponseSchema = typeof(Instrument) // Provide the enum type
            };

            // Act
            var response = await model.GenerateContent(prompt, generationConfig: generationConfig);

            // Assert
            response.Should().NotBeNull();
            response.Candidates.Should().NotBeNull().And.HaveCount(1);
            response.Text.Should().NotBeEmpty();
            _output.WriteLine($"Response: {response.Text}");

            // Parse the enum (more robust error handling)
            if (Enum.TryParse(response.Text, out Instrument instrument))
            {
                _output.WriteLine($"Parsed Instrument: {instrument}");
            }
            else
            {
                _output.WriteLine($"Could not parse '{response.Text}' as a valid Instrument enum.");
            }
        }

        [Theory]
        [InlineData("https://storage.googleapis.com/generativeai-downloads/images/instrument.jpg")]
        public async Task Generate_Content_Using_ResponseSchema_with_Enumeration_and_Image(string uri)
        {
            // Arrange
            var googleAi = new GoogleAI(apiKey: _fixture.ApiKey);
            var model = _googleAi.GenerativeModel(model: _model);

            var generationConfig = new GenerationConfig
            {
                ResponseMimeType = "text/x.enum", // Important for enum handling
                ResponseSchema = typeof(Instrument) // Provide the enum type
            };
            var request = new GenerateContentRequest(prompt: "what category of instrument is this?",
                generationConfig: generationConfig);
            await request.AddMedia(uri);

            // Act
            var response = await model.GenerateContent(request);

            // Assert
            response.Should().NotBeNull();
            response.Candidates.Should().NotBeNull().And.HaveCount(1);
            response.Text.Should().NotBeEmpty();
            _output.WriteLine($"Response: {response.Text}");

            // Parse the enum (more robust error handling)
            if (Enum.TryParse(response.Text, out Instrument instrument))
            {
                _output.WriteLine($"Parsed Instrument: {instrument}");
            }
            else
            {
                _output.WriteLine($"Could not parse '{response.Text}' as a valid Instrument enum.");
            }
        }

        [Fact]
        public async Task Generate_Content_Using_ResponseSchema_with_Anonymous()
        {
            // Arrange
            var prompt = "List a few popular cookie recipes.";
            var googleAi = new GoogleAI(apiKey: _fixture.ApiKey);
            var model = _googleAi.GenerativeModel(model: _model);
            var generationConfig = new GenerationConfig()
            {
                ResponseMimeType = "application/json",
                ResponseSchema = new
                {
                    type = "array",
                    items = new { type = "object", properties = new { name = new { type = "string" } } }
                }
            };

            // Act
            var response = await model.GenerateContent(prompt,
                generationConfig: generationConfig);

            // Assert
            response.Should().NotBeNull();
            response.Candidates.Should().NotBeNull().And.HaveCount(1);
            response.Text.Should().NotBeEmpty();
            _output.WriteLine(response?.Text);
        }

        [Fact]
        public async Task Generate_Content_Using_ResponseSchema_with_String()
        {
            // Arrange
            var prompt = "List a few popular cookie recipes.";
            var googleAi = new GoogleAI(apiKey: _fixture.ApiKey);
            var model = _googleAi.GenerativeModel(model: _model);
            var schema = """
                         {
                           "type": "object",
                           "properties": {
                             "menus": {
                               "type": "array",
                               "description": "A list of menus, each representing a specific day.",
                               "items": {
                                 "type": "object",
                                 "properties": {
                                   "date": {
                                     "type": "string",
                                     "description": "The date of the menu in YYYY-MM-DD format."
                                   },
                                   "meals": {
                                     "type": "array",
                                     "description": "A list of meals available on this day.",
                                     "items": {
                                       "type": "object",
                                       "properties": {
                                         "type": {
                                           "type": "string",
                                           "enum": ["regular", "diet"],
                                           "description": "Indicates whether the meal is a regular option or a dietary option."
                                         },
                                         "name": {
                                           "type": "string",
                                           "description": "The name of the meal."
                                         },
                                         "weight": {
                                           "type": "number",
                                           "description": "The weight of the meal in grams."
                                         },
                                         "selected": {
                                           "type": "boolean",
                                           "description": "Indicates whether the meal was selected by the user."
                                         }
                                       },
                                       "required": ["type", "name", "selected"]
                                     }
                                   }
                                 },
                                 "required": ["date", "meals"]
                               }
                             }
                           },
                           "required": ["menus"]
                         }
                         """;
            var generationConfig = new GenerationConfig()
            {
                ResponseMimeType = "application/json", 
                ResponseSchema = schema
            };

            // Act
            var response = await model.GenerateContent(prompt,
                generationConfig: generationConfig);

            // Assert
            response.Should().NotBeNull();
            response.Candidates.Should().NotBeNull().And.HaveCount(1);
            response.Text.Should().NotBeEmpty();
            _output.WriteLine(response?.Text);
        }

        public class AiWeaponModel
        {
            public string WeaponName { get; set; }
            public string WeaponDescription { get; set; }
        }
        
        [Fact]
        public async Task Generate_Content_Using_ResponseSchema_Issue77()
        {
            // Arrange
            var prompt = "List a few popular arny weapons with name and summary";
            var googleAi = new GoogleAI(apiKey: _fixture.ApiKey);
            var model = _googleAi.GenerativeModel(model: _model);
            var generationConfig = new GenerationConfig()
            {
                ResponseMimeType = "application/json", 
                ResponseSchema = new List<AiWeaponModel>()
            };

            // Act
            var response = await model.GenerateContent(prompt,
                generationConfig: generationConfig);

            // Assert
            response.Should().NotBeNull();
            response.Candidates.Should().NotBeNull().And.HaveCount(1);
            response.Text.Should().NotBeEmpty();
            _output.WriteLine(response?.Text);
        }
        
        [Fact]
        public async Task Generate_Content_Using_ResponseSchema_Issue80()
        {
            // Arrange
            var prompt = "List a few popular arny weapons with name and summary";
            var googleAi = new GoogleAI(apiKey: _fixture.ApiKey);
            var model = _googleAi.GenerativeModel(model: _model);
            var generationConfig = new GenerationConfig()
            {
                ResponseSchema = """{"$schema":"http://json-schema.org/draft-07/schema#","type":"object","properties":{"type":{"type":"string"},"topic":{"type":["string","null"]},"iptc":{"type":"object","additionalProperties":{"type":"number","minimum":0.0,"maximum":1.0}}...""",
                ResponseMimeType = "application/json"
            };

            // Act
            var response = await model.GenerateContent(prompt,
                generationConfig: generationConfig);

            // Assert
            response.Should().NotBeNull();
            response.Candidates.Should().NotBeNull().And.HaveCount(1);
            response.Text.Should().NotBeEmpty();
            _output.WriteLine(response?.Text);
        }

#if NET9_0
        public record Root([Description("A list of menus, each representing a specific day.")] List<Menu> Menus);
        public record Menu(DateOnly Date, List<Meal> Meals);
        public record Meal(string Type, string Name, double? Weight, bool Selected)
        {
            public string FullName => $"{Weight}g {Name}";
        }
        
        // [Fact(Skip = "ReadOnly declaration not accepted.")]
        [Fact]
        public async Task Generate_Content_Using_ResponseSchema_with_Record()
        {
            // Arrange
            var prompt = "List a few popular cookie recipes.";
            var googleAi = new GoogleAI(apiKey: _fixture.ApiKey);
            var model = _googleAi.GenerativeModel(model: _model);
            var generationConfig = new GenerationConfig()
            {
                ResponseMimeType = "application/json", 
                ResponseSchema = new Root([])
            };

            // Act
            var response = await model.GenerateContent(prompt,
                generationConfig: generationConfig);

            // Assert
            response.Should().NotBeNull();
            response.Candidates.Should().NotBeNull().And.HaveCount(1);
            response.Text.Should().NotBeEmpty();
            _output.WriteLine(response?.Text);
        }
#endif
        
        [Fact(Skip = "ReadOnly declaration not accepted.")]
        public async Task Generate_Content_Using_ResponseSchema_with_Dynamic()
        {
            // Arrange
            var prompt = "List a few popular cookie recipes.";
            var googleAi = new GoogleAI(apiKey: _fixture.ApiKey);
            var model = _googleAi.GenerativeModel(model: _model);
            dynamic schema = new ExpandoObject();
            schema.Name = "dynamic";

            var generationConfig = new GenerationConfig()
            {
                ResponseMimeType = "application/json", ResponseSchema = schema
            };

            // Act
            var response = await model.GenerateContent(prompt,
                generationConfig: generationConfig);

            // Assert
            response.Should().NotBeNull();
            response.Candidates.Should().NotBeNull().And.HaveCount(1);
            response.Text.Should().NotBeEmpty();
            _output.WriteLine(response?.Text);
        }

        [Fact]
        public async Task Generate_Text_From_Image()
        {
            // Arrange
            var googleAi = new GoogleAI(apiKey: _fixture.ApiKey);
            var model = _googleAi.GenerativeModel(model: _model);
            var request = new GenerateContentRequest { Contents = new List<Content>() };
            var base64Image =
                "iVBORw0KGgoAAAANSUhEUgAAAAEAAAABCAYAAAAfFcSJAAAADUlEQVR42mP8z8BQDwAEhQGAhKmMIQAAAABJRU5ErkJggg==";
            var parts = new List<IPart>
            {
                new TextData { Text = "What is this picture about?" },
                new InlineData { MimeType = "image/jpeg", Data = base64Image }
            };
            request.Contents.Add(new Content { Role = Role.User, Parts = parts });

            // Act
            var response = await model.GenerateContent(request);

            // Assert
            response.Should().NotBeNull();
            response.Candidates.Should().NotBeNull().And.HaveCount(1);
            response.Candidates.FirstOrDefault().Content.Should().NotBeNull();
            response.Candidates.FirstOrDefault().Content.Parts.Should().NotBeNull().And
                .HaveCountGreaterThanOrEqualTo(1);
            response.Text.Should().Contain("red");
            _output.WriteLine(response?.Text);
        }

        [Fact]
        public async Task Describe_Image_From_InlineData()
        {
            // Arrange
            var prompt = "Parse the time and city from the airport board shown in this image into a list, in Markdown";
            var googleAi = new GoogleAI(apiKey: _fixture.ApiKey);
            var model = _googleAi.GenerativeModel(model: _model);
            // Images
            var board = await TestExtensions.ReadImageFileBase64Async(
                "https://raw.githubusercontent.com/mscraftsman/generative-ai/refs/heads/main/tests/Mscc.GenerativeAI/payload/timetable.png");
            var request = new GenerateContentRequest(prompt);
            request.Contents[0].Parts.Add(
                new InlineData { MimeType = "image/png", Data = board }
            );

            // Act
            var response = await model.GenerateContent(request);

            // Assert
            response.Should().NotBeNull();
            response.Candidates.Should().NotBeNull().And.HaveCount(1);
            response.Candidates.FirstOrDefault().Content.Should().NotBeNull();
            response.Candidates.FirstOrDefault().Content.Parts.Should().NotBeNull().And
                .HaveCountGreaterThanOrEqualTo(1);
            _output.WriteLine(response?.Text);
        }

        [Theory]
        [InlineData("scones.jpg", "image/jpeg", "What is this picture?", "blueberries")]
        [InlineData("cat.jpg", "image/jpeg", "Describe this image", "snow")]
        [InlineData("cat.jpg", "image/jpeg", "Is it a cat?", "Yes")]
        //[InlineData("animals.mp4", "video/mp4", "What's in the video?", "Zootopia")]
        public async Task Generate_Text_From_ImageFile(string filename, string mimetype, string prompt, string expected)
        {
            // Arrange
            var googleAi = new GoogleAI(apiKey: _fixture.ApiKey);
            var model = _googleAi.GenerativeModel(model: _model);
            var base64Image =
                Convert.ToBase64String(
                    File.ReadAllBytes(Path.Combine(Environment.CurrentDirectory, "payload", filename)));
            var parts = new List<IPart>
            {
                new TextData { Text = prompt }, new InlineData { MimeType = mimetype, Data = base64Image }
            };
            var generationConfig = new GenerationConfig()
            {
                Temperature = 0.4f, TopP = 1, TopK = 32, MaxOutputTokens = 1024
            };

            // Act
            var response = await model.GenerateContent(parts, generationConfig);

            // Assert
            response.Should().NotBeNull();
            response.Candidates.Should().NotBeNull().And.HaveCount(1);
            response.Candidates.FirstOrDefault().Content.Should().NotBeNull();
            response.Candidates.FirstOrDefault().Content.Parts.Should().NotBeNull().And
                .HaveCountGreaterThanOrEqualTo(1);
            response.Text.Should().Contain(expected);
            _output.WriteLine(response?.Text);
        }

        [Theory]
        [InlineData("scones.jpg", "What is this picture?", "blueberries")]
        [InlineData("cat.jpg", "Describe this image", "snow")]
        [InlineData("cat.jpg", "Is it a feline?", "Yes")]
        [InlineData("organ.jpg", "Tell me about this instrument", "pipe organ")]
        //[InlineData("animals.mp4", "video/mp4", "What's in the video?", "Zootopia")]
        public async Task Describe_AddMedia_From_ImageFile(string filename, string prompt, string expected)
        {
            // Arrange
            var googleAi = new GoogleAI(apiKey: _fixture.ApiKey);
            var model = _googleAi.GenerativeModel(model: _model);
            var request = new GenerateContentRequest(prompt)
            {
                GenerationConfig = new GenerationConfig()
                {
                    Temperature = 0.4f, TopP = 1, TopK = 32, MaxOutputTokens = 1024
                }
            };
            await request.AddMedia(uri: Path.Combine(Environment.CurrentDirectory, "payload", filename));

            // Act
            var response = await model.GenerateContent(request);

            // Assert
            response.Should().NotBeNull();
            response.Candidates.Should().NotBeNull().And.HaveCount(1);
            response.Candidates.FirstOrDefault().Content.Should().NotBeNull();
            response.Candidates.FirstOrDefault().Content.Parts.Should().NotBeNull().And
                .HaveCountGreaterThanOrEqualTo(1);
            response.Text.Should().Contain(expected);
            _output.WriteLine(response?.Text);
        }

        [Theory]
        [InlineData(
            "https://raw.githubusercontent.com/mscraftsman/generative-ai/refs/heads/main/tests/Mscc.GenerativeAI/payload/timetable.png",
            "Parse the time and city from the airport board shown in this image into a list, in Markdown")]
        [InlineData("http://groups.di.unipi.it/~occhiuto/sintassi.pdf", "Generate 5 exercise for a student")]
        public async Task Describe_AddMedia_From_Url(string uri, string prompt)
        {
            // Arrange
            var googleAi = new GoogleAI(apiKey: _fixture.ApiKey, apiVersion: ApiVersion.V1Alpha);
            var model = _googleAi.GenerativeModel(model: _model);
            var request = new GenerateContentRequest(prompt);
            await request.AddMedia(uri);

            // Act
            var response = await model.GenerateContent(request);

            // Assert
            response.Should().NotBeNull();
            response.Candidates.Should().NotBeNull().And.HaveCount(1);
            response.Candidates.FirstOrDefault().Content.Should().NotBeNull();
            response.Candidates.FirstOrDefault().Content.Parts.Should().NotBeNull().And
                .HaveCountGreaterThanOrEqualTo(1);
            _output.WriteLine(response?.Text);
        }

        [Fact(Skip = "Bad Request due to FileData part")]
        public async Task Describe_AddMedia_From_UrlRemote()
        {
            // Arrange
            var prompt = "Parse the time and city from the airport board shown in this image into a list, in Markdown";
            var googleAi = new GoogleAI(apiKey: _fixture.ApiKey);
            var model = _googleAi.GenerativeModel(model: _model);
            var request = new GenerateContentRequest(prompt);
            await request.AddMedia(
                "https://raw.githubusercontent.com/mscraftsman/generative-ai/refs/heads/main/tests/Mscc.GenerativeAI/payload/timetable.png",
                useOnline: true);

            // Act
            var response = await model.GenerateContent(request);

            // Assert
            response.Should().NotBeNull();
            response.Candidates.Should().NotBeNull().And.HaveCount(1);
            response.Candidates.FirstOrDefault().Content.Should().NotBeNull();
            response.Candidates.FirstOrDefault().Content.Parts.Should().NotBeNull().And
                .HaveCountGreaterThanOrEqualTo(1);
            _output.WriteLine(response?.Text);
        }

        [Theory]
        [InlineData("scones.jpg", "Set of blueberry scones")]
        [InlineData("cat.jpg", "Wildcat on snow")]
        [InlineData("cat.jpg", "Cat in the snow")]
        [InlineData("image.jpg", "Sample drawing")]
        [InlineData("animals.mp4", "Zootopia in da house")]
        [InlineData("sample.mp3", "State_of_the_Union_Address_30_January_1961")]
        [InlineData("pixel.mp3", "Pixel Feature Drops: March 2023")]
        [InlineData("gemini.pdf",
            "Gemini 1.5: Unlocking multimodal understanding across millions of tokens of context")]
        [InlineData("Big_Buck_Bunny.mp4", "Video clip (CC BY 3.0) from https://peach.blender.org/download/")]
        [InlineData("GeminiDocumentProcessing.rtf", "Sample document in Rich Text Format")]
        public async Task Upload_File_Using_FileAPI(string filename, string displayName)
        {
            // Arrange
            var filePath = Path.Combine(Environment.CurrentDirectory, "payload", filename);
            IGenerativeAI genAi = new GoogleAI(_fixture.ApiKey);
            var model = _googleAi.GenerativeModel(_model);

            // Act
            var response = await ((GoogleAI)genAi).UploadFile(filePath, displayName);

            // Assert
            response.Should().NotBeNull();
            response.File.Should().NotBeNull();
            response.File.Name.Should().NotBeNull();
            response.File.DisplayName.Should().Be(displayName);
            // response.File.MimeType.Should().Be("image/jpeg");
            // response.File.CreateTime.Should().BeGreaterThan(DateTime.Now.Add(TimeSpan.FromHours(48)));
            // response.File.ExpirationTime.Should().NotBeNull();
            // response.File.UpdateTime.Should().NotBeNull();
            response.File.SizeBytes.Should().BeGreaterThan(0);
            response.File.Sha256Hash.Should().NotBeNull();
            response.File.Uri.Should().NotBeNull();
            _output.WriteLine($"Uploaded file '{response?.File.DisplayName}' as: {response?.File.Uri}");
        }

        [Fact]
        public async Task Upload_File_TooLarge_ThrowsMaxUploadFileSizeException()
        {
            // Arrange
            IGenerativeAI genAi = new GoogleAI(_fixture.ApiKey);
            var model = _googleAi.GenerativeModel(_model);
            var filePath = Path.Combine(Environment.CurrentDirectory, "payload", "toolarge.jpg");
            var displayName = "Too Large File";
            using var fs = new FileStream(filePath, FileMode.CreateNew);
            fs.Seek(2048L * 1024 * 1024, SeekOrigin.Begin);
            fs.WriteByte(0);
            fs.Close();

            // Act & Assert
            // await Assert.ThrowsAsync<MaxUploadFileSizeException>(() => ((GoogleAI)genAi).UploadFile(filePath, displayName));
            // Act
            Func<Task> sut = async () =>
            {
                await ((GoogleAI)genAi).UploadFile(filePath, displayName);
            };

            // Assert
            await sut.Should().ThrowAsync<MaxUploadFileSizeException>();

            // House keeping
            File.Delete(filePath);
        }

        [Fact]
        public async Task Upload_File_WithResume_Using_FileAPI()
        {
            // Arrange
            IGenerativeAI genAi = new GoogleAI(_fixture.ApiKey);
            var model = _googleAi.GenerativeModel(_model);
            model.Timeout = TimeSpan.FromMinutes(5);
            var filePath = Path.Combine(Environment.CurrentDirectory, "payload", "resume.jpg");
            var displayName = "Resumable File";
            if (!File.Exists(filePath))
            {
                using var fs = new FileStream(filePath, FileMode.CreateNew);
                fs.Seek(10L * 1024 * 1024, SeekOrigin.Begin);
                fs.WriteByte(0);
                fs.Close();
            }

            // Act
            var response = await ((GoogleAI)genAi).UploadFile(filePath, displayName, resumable: true);

            // Assert
            response.Should().NotBeNull();
            response.File.Should().NotBeNull();
            response.File.Name.Should().NotBeNull();
            response.File.DisplayName.Should().Be(displayName);
            // response.File.MimeType.Should().Be("image/jpeg");
            // response.File.CreateTime.Should().BeGreaterThan(DateTime.Now.Add(TimeSpan.FromHours(48)));
            // response.File.ExpirationTime.Should().NotBeNull();
            // response.File.UpdateTime.Should().NotBeNull();
            response.File.SizeBytes.Should().BeGreaterThan(0);
            response.File.Sha256Hash.Should().NotBeNull();
            response.File.Uri.Should().NotBeNull();
            _output.WriteLine($"Uploaded file '{response?.File.DisplayName}' as: {response?.File.Uri}");

            // House keeping
            File.Delete(filePath);
        }

        [Theory]
        [InlineData("scones.jpg", "Set of blueberry scones", "image/jpeg")]
        [InlineData("cat.jpg", "Wildcat on snow", "image/jpeg")]
        [InlineData("cat.jpg", "Cat in the snow", "image/jpeg")]
        [InlineData("image.jpg", "Sample drawing", "image/jpeg")]
        [InlineData("animals.mp4", "Zootopia in da house", "video/mp4")]
        [InlineData("sample.mp3", "State_of_the_Union_Address_30_January_1961", "audio/mp3")]
        [InlineData("pixel.mp3", "Pixel Feature Drops: March 2023", "audio/mp3")]
        [InlineData("gemini.pdf",
            "Gemini 1.5: Unlocking multimodal understanding across millions of tokens of context", "application/pdf")]
        public async Task Upload_Stream_Using_FileAPI(string filename, string displayName, string mimeType)
        {
            // Arrange
            var filePath = Path.Combine(Environment.CurrentDirectory, "payload", filename);
            IGenerativeAI genAi = new GoogleAI(_fixture.ApiKey);
            var model = _googleAi.GenerativeModel(_model);

            // Act
            using (var fs = new FileStream(filePath, FileMode.Open))
            {
                var response = await ((GoogleAI)genAi).UploadFile(fs, displayName, mimeType);

                // Assert
                response.Should().NotBeNull();
                response.File.Should().NotBeNull();
                response.File.Name.Should().NotBeNull();
                response.File.DisplayName.Should().Be(displayName);
                // response.File.MimeType.Should().Be("image/jpeg");
                // response.File.CreateTime.Should().BeGreaterThan(DateTime.Now.Add(TimeSpan.FromHours(48)));
                // response.File.ExpirationTime.Should().NotBeNull();
                // response.File.UpdateTime.Should().NotBeNull();
                response.File.SizeBytes.Should().BeGreaterThan(0);
                response.File.Sha256Hash.Should().NotBeNull();
                response.File.Uri.Should().NotBeNull();
                _output.WriteLine($"Uploaded file '{response?.File.DisplayName}' as: {response?.File.Uri}");
            }
        }

        [Fact]
        public async Task List_Files()
        {
            // Arrange
            IGenerativeAI genAi = new GoogleAI(_fixture.ApiKey);
            var model = _googleAi.GenerativeModel(_model);

            // Act
            var sut = await ((GoogleAI)genAi).ListFiles();

            // Assert
            sut.Should().NotBeNull();
            sut.Files.Should().NotBeNull().And.HaveCountGreaterThanOrEqualTo(1);
            sut.Files.ForEach(x =>
            {
                _output.WriteLine(
                    $"Display Name: {x.DisplayName} ({Enum.GetName(typeof(StateFileResource), x.State)})");
                _output.WriteLine(
                    $"File: {x.Name} (MimeType: {x.MimeType}, Size: {x.SizeBytes} bytes, Created: {x.CreateTime} UTC, Updated: {x.UpdateTime} UTC)");
                _output.WriteLine($"Uri: {x.Uri}");
            });
        }

        [Fact]
        public async Task Get_File()
        {
            // Arrange
            IGenerativeAI genAi = new GoogleAI(_fixture.ApiKey);
            var model = _googleAi.GenerativeModel(_model);
            var files = await _googleAi.ListFiles();
            var fileName = files.Files.FirstOrDefault().Name;

            // Act
            var sut = await model.GetFile(fileName);

            // Assert
            sut.Should().NotBeNull();
            _output.WriteLine($"Retrieved file '{sut.DisplayName}'");
            _output.WriteLine(
                $"File: {sut.Name} (MimeType: {sut.MimeType}, Size: {sut.SizeBytes} bytes, Created: {sut.CreateTime} UTC, Updated: {sut.UpdateTime} UTC)");
            _output.WriteLine(($"Uri: {sut.Uri}"));
        }

        [Fact(Skip = "Check source: File API or Generated File.")]
        public async Task Download_File()
        {
            // Arrange
            IGenerativeAI googleAi = new GoogleAI(_fixture.ApiKey);
            var model = _googleAi.GenerativeModel(_model);
            var files = await _googleAi.ListFiles();
            var fileName = files.Files.FirstOrDefault().Name;

            // Act
            var sut = await ((GoogleAI)googleAi).DownloadFile(fileName);

            // Assert
            sut.Should().NotBeNull();
//            output.WriteLine($"Retrieved file '{sut.DisplayName}'");
//            output.WriteLine(
//                $"File: {sut.Name} (MimeType: {sut.MimeType}, Size: {sut.SizeBytes} bytes, Created: {sut.CreateTime} UTC, Updated: {sut.UpdateTime} UTC)");
//            output.WriteLine(($"Uri: {sut.Uri}"));
        }

        [Fact]
        public async Task Delete_File()
        {
            // Arrange
            IGenerativeAI genAi = new GoogleAI(_fixture.ApiKey);
            var model = _googleAi.GenerativeModel(_model);
            var files = await _googleAi.ListFiles();
            var fileName = files.Files.FirstOrDefault().Name;
            _output.WriteLine($"File: {fileName}");

            // Act
            var response = await model.DeleteFile(fileName);

            // Assert
            response.Should().NotBeNull();
            _output.WriteLine(response);
        }

        [Fact]
        public async Task Describe_Single_Media_From_FileAPI()
        {
            // Arrange
            var prompt = "Describe the image with a creative description";
            IGenerativeAI genAi = new GoogleAI(_fixture.ApiKey);
            var model = _googleAi.GenerativeModel(_model);
            var request = new GenerateContentRequest(prompt);
            var files = await ((GoogleAI)genAi).ListFiles();
            var file = files.Files.Where(x => x.MimeType.StartsWith("image/")).FirstOrDefault();
            _output.WriteLine($"File: {file.Name}\tName: '{file.DisplayName}'");
            request.AddMedia(file);

            // Act
            var response = await model.GenerateContent(request);

            // Assert
            response.Should().NotBeNull();
            response.Candidates.Should().NotBeNull().And.HaveCount(1);
            response.Candidates.FirstOrDefault().Content.Should().NotBeNull();
            response.Candidates.FirstOrDefault().Content.Parts.Should().NotBeNull().And
                .HaveCountGreaterThanOrEqualTo(1);
            _output.WriteLine(response?.Text);
        }

        [Fact]
        public async Task Describe_Images_From_FileAPI()
        {
            // Arrange
            var prompt = "Make a short story from the media resources. The media resources are:";
            IGenerativeAI genAi = new GoogleAI(_fixture.ApiKey);
            var model = _googleAi.GenerativeModel(_model);
            var request = new GenerateContentRequest(prompt);
            var files = await ((GoogleAI)genAi).ListFiles();
            foreach (var file in files.Files.Where(x => x.MimeType.StartsWith("image/")))
            {
                _output.WriteLine($"File: {file.Name}\tName: '{file.DisplayName}'");
                request.AddMedia(file);
            }

            // Act
            var response = await model.GenerateContent(request);

            // Assert
            response.Should().NotBeNull();
            response.Candidates.Should().NotBeNull().And.HaveCount(1);
            response.Candidates.FirstOrDefault().Content.Should().NotBeNull();
            response.Candidates.FirstOrDefault().Content.Parts.Should().NotBeNull().And
                .HaveCountGreaterThanOrEqualTo(1);
            _output.WriteLine(response?.Text);
        }

        [Fact]
        public async Task Describe_Audio_From_FileAPI()
        {
            // Arrange
            var prompt = "Listen carefully to the following audio file. Provide a brief summary.";
            IGenerativeAI genAi = new GoogleAI(_fixture.ApiKey);
            var model = _googleAi.GenerativeModel(_model);
            var request = new GenerateContentRequest(prompt);
            var files = await ((GoogleAI)genAi).ListFiles();
            foreach (var file in files.Files.Where(x => x.MimeType.StartsWith("audio/")))
            {
                _output.WriteLine($"File: {file.Name}\tName: '{file.DisplayName}'");
                request.AddMedia(file);
            }

            // Act
            var response = await model.GenerateContent(request);

            // Assert
            response.Should().NotBeNull();
            response.Candidates.Should().NotBeNull().And.HaveCount(1);
            response.Candidates.FirstOrDefault().Content.Should().NotBeNull();
            response.Candidates.FirstOrDefault().Content.Parts.Should().NotBeNull().And
                .HaveCountGreaterThanOrEqualTo(1);
            _output.WriteLine(response?.Text);
        }

        [Theory]
        [InlineData(Model.Gemini20Flash)]
        [InlineData(Model.Gemini20FlashLite)]
        [InlineData(Model.Gemini20FlashThinking)]
        [InlineData(Model.Gemini20Pro)]
        [InlineData(Model.Gemini25Pro)]
        public async Task Describe_Audio_with_Timestamps(string modelName)
        {
            // Arrange
            var prompt =
                @"Transcribe this audio into english texts. Break the text into small logical segments. Include punctuation where appropriate. Timestamps should have milli-second level accuracy.

Output the segments in the SRT format:

subtitle_id
start_time → end_time
content";
            IGenerativeAI genAi = new GoogleAI(_fixture.ApiKey);
            var model = _googleAi.GenerativeModel(modelName);
            var request = new GenerateContentRequest(prompt);
            var filePath = Path.Combine(Environment.CurrentDirectory, "payload", "out.mp3");
            var file = await ((GoogleAI)genAi).UploadFile(filePath);
            _output.WriteLine($"File: {file.File.Name}\tName: '{file.File.DisplayName}'");
            request.AddMedia(file.File);

            // Act
            var response = await model.GenerateContent(request);

            // Assert
            response.Should().NotBeNull();
            response.Candidates.Should().NotBeNull().And.HaveCount(1);
            response.Candidates.FirstOrDefault().Content.Should().NotBeNull();
            response.Candidates.FirstOrDefault().Content.Parts.Should().NotBeNull().And
                .HaveCountGreaterThanOrEqualTo(1);
            _output.WriteLine(response?.Text);
        }

        [Fact]
        public async Task Summarize_Audio_From_FileAPI()
        {
            // Arrange
            var prompt = @"Please provide a summary for the audio.
Provide chapter titles with timestamps, be concise and short, no need to provide chapter summaries.
Do not make up any information that is not part of the audio and do not be verbose.
";
            IGenerativeAI genAi = new GoogleAI(_fixture.ApiKey);
            var model = _googleAi.GenerativeModel(_model);
            var request = new GenerateContentRequest(prompt);
            var files = await ((GoogleAI)genAi).ListFiles();
            var file = files.Files.Where(x => x.MimeType.StartsWith("audio/")).FirstOrDefault();
            _output.WriteLine($"File: {file.Name}\tName: '{file.DisplayName}'");
            request.AddMedia(file);

            // Act
            var response = await model.GenerateContent(request);

            // Assert
            response.Should().NotBeNull();
            response.Candidates.Should().NotBeNull().And.HaveCount(1);
            response.Candidates.FirstOrDefault().Content.Should().NotBeNull();
            response.Candidates.FirstOrDefault().Content.Parts.Should().NotBeNull().And
                .HaveCountGreaterThanOrEqualTo(1);
            _output.WriteLine(response?.Text);
        }

        [Fact]
        public async Task Analyze_Document_PDF_From_FileAPI()
        {
            // Arrange
            var prompt =
                @"Your are a very professional document summarization specialist. Please summarize the given document.";
            IGenerativeAI genAi = new GoogleAI(_fixture.ApiKey);
            var model = _googleAi.GenerativeModel(_model);
            var request = new GenerateContentRequest(prompt);
            var files = await ((GoogleAI)genAi).ListFiles();
            var file = files.Files.Where(x => x.MimeType.StartsWith("application/pdf")).FirstOrDefault();
            _output.WriteLine($"File: {file.Name}\tName: '{file.DisplayName}'");
            request.AddMedia(file);

            // Act
            var response = await model.GenerateContent(request);

            // Assert
            response.Should().NotBeNull();
            response.Candidates.Should().NotBeNull().And.HaveCount(1);
            response.Candidates.FirstOrDefault().Content.Should().NotBeNull();
            response.Candidates.FirstOrDefault().Content.Parts.Should().NotBeNull().And
                .HaveCountGreaterThanOrEqualTo(1);
            _output.WriteLine(response?.Text);
        }

        [Theory]
        [InlineData("application/rtf")]
        [InlineData("text/rtf")]
        public async Task Analyze_Document_RTF_From_FileAPI(string mimetype)
        {
            // Arrange
            var prompt =
                @"Your are a very professional document summarization specialist. Please summarize the given document.";
            IGenerativeAI genAi = new GoogleAI(_fixture.ApiKey);
            var model = _googleAi.GenerativeModel(_model);
            var request = new GenerateContentRequest(prompt);
            var files = await ((GoogleAI)genAi).ListFiles();
            var file = files.Files.Where(x => x.MimeType.StartsWith(mimetype)).FirstOrDefault();
            _output.WriteLine($"File: {file.Name}\tName: '{file.DisplayName}'");
            request.AddMedia(file);

            // Act
            var response = await model.GenerateContent(request);

            // Assert
            response.Should().NotBeNull();
            response.Candidates.Should().NotBeNull().And.HaveCount(1);
            response.Candidates.FirstOrDefault().Content.Should().NotBeNull();
            response.Candidates.FirstOrDefault().Content.Parts.Should().NotBeNull().And
                .HaveCountGreaterThanOrEqualTo(1);
            _output.WriteLine(response?.Text);
        }

        [Fact]
        public async Task TranscribeStream_Audio_From_FileAPI()
        {
            // Arrange
            var prompt = @"Can you transcribe this interview, in the format of timecode, speaker, caption.
Use speaker A, speaker B, etc. to identify the speakers.
";
            IGenerativeAI genAi = new GoogleAI(_fixture.ApiKey);
            var model = _googleAi.GenerativeModel(_model);
            var request = new GenerateContentRequest(prompt);
            var files = await ((GoogleAI)genAi).ListFiles();
            var file = files.Files.Where(x => x.MimeType.StartsWith("audio/")).FirstOrDefault();
            _output.WriteLine($"File: {file.Name}\tName: '{file.DisplayName}'");
            request.AddMedia(file);

            // Act
            var responseStream = model.GenerateContentStream(request);

            // Assert
            responseStream.Should().NotBeNull();
            await foreach (var response in responseStream)
            {
                response.Should().NotBeNull();
                response.Candidates.Should().NotBeNull().And.HaveCount(1);
                // response.Text.Should().NotBeEmpty();
                _output.WriteLine(response?.Text);
                // response.UsageMetadata.Should().NotBeNull();
                // output.WriteLine($"PromptTokenCount: {response?.UsageMetadata?.PromptTokenCount}");
                // output.WriteLine($"CandidatesTokenCount: {response?.UsageMetadata?.CandidatesTokenCount}");
                // output.WriteLine($"TotalTokenCount: {response?.UsageMetadata?.TotalTokenCount}");
            }
        }

        [Fact]
        public async Task TranscribeStream_Audio_From_FileAPI_UsingSSEFormat()
        {
            // Arrange
            var prompt = @"Can you transcribe this interview, in the format of timecode, speaker, caption.
Use speaker A, speaker B, etc. to identify the speakers.
";
            IGenerativeAI genAi = new GoogleAI(_fixture.ApiKey);
            var model = _googleAi.GenerativeModel(_model);
            model.UseServerSentEventsFormat = true;
            var request = new GenerateContentRequest(prompt);
            var files = await ((GoogleAI)genAi).ListFiles();
            var file = files.Files.Where(x => x.MimeType.StartsWith("audio/")).FirstOrDefault();
            _output.WriteLine($"File: {file.Name}\tName: '{file.DisplayName}'");
            request.AddMedia(file);

            // Act
            var responseStream = model.GenerateContentStream(request);

            // Assert
            responseStream.Should().NotBeNull();
            await foreach (var response in responseStream)
            {
                response.Should().NotBeNull();
                response.Candidates.Should().NotBeNull().And.HaveCount(1);
                _output.WriteLine(response?.Text);
            }
        }

        [Fact]
        public async Task Describe_Videos_From_FileAPI()
        {
            // Arrange
            var prompt = "Describe this video clip.";
            IGenerativeAI genAi = new GoogleAI(_fixture.ApiKey);
            var model = _googleAi.GenerativeModel(_model);
            var request = new GenerateContentRequest(prompt);
            var files = await ((GoogleAI)genAi).ListFiles();
            foreach (var file in files.Files.Where(x => x.MimeType.StartsWith("video/")))
            {
                _output.WriteLine($"File: {file.Name}\tName: '{file.DisplayName}'");
                request.AddMedia(file);
            }

            // Act
            var response = await model.GenerateContent(request);

            // Assert
            response.Should().NotBeNull();
            response.Candidates.Should().NotBeNull().And.HaveCount(1);
            response.Candidates.FirstOrDefault().Content.Should().NotBeNull();
            response.Candidates.FirstOrDefault().Content.Parts.Should().NotBeNull().And
                .HaveCountGreaterThanOrEqualTo(1);
            _output.WriteLine(response?.Text);
        }

        [Fact]
        public async Task Describe_Videos_From_Youtube()
        {
            // Arrange
            var prompt = "Describe this video clip."; // Can you summarize this video?
            IGenerativeAI genAi = new GoogleAI(_fixture.ApiKey);
            var model = _googleAi.GenerativeModel(_model);
            var request = new GenerateContentRequest(prompt);
            await request.AddMedia("https://www.youtube.com/watch?v=1XALhtem2h0", useOnline: true);

            // Act
            var response = await model.GenerateContent(request);

            // Assert
            response.Should().NotBeNull();
            response.Candidates.Should().NotBeNull().And.HaveCount(1);
            response.Candidates.FirstOrDefault().Content.Should().NotBeNull();
            response.Candidates.FirstOrDefault().Content.Parts.Should().NotBeNull().And
                .HaveCountGreaterThanOrEqualTo(1);
            _output.WriteLine(response?.Text);
        }

        [Fact]
        public async Task Make_Story_using_Videos_From_FileAPI()
        {
            // Arrange
            var prompt = "Make a short story from the media resources. The media resources are:";
            IGenerativeAI genAi = new GoogleAI(_fixture.ApiKey);
            var model = _googleAi.GenerativeModel(_model);
            var request = new GenerateContentRequest(prompt);
            var files = await ((GoogleAI)genAi).ListFiles();
            foreach (var file in files.Files.Where(x => x.MimeType.StartsWith("video/")))
            {
                _output.WriteLine($"File: {file.Name}\tName: '{file.DisplayName}'");
                request.AddMedia(file);
            }

            // Act
            var response = await model.GenerateContent(request);

            // Assert
            response.Should().NotBeNull();
            response.Candidates.Should().NotBeNull().And.HaveCount(1);
            response.Candidates.FirstOrDefault().Content.Should().NotBeNull();
            response.Candidates.FirstOrDefault().Content.Parts.Should().NotBeNull().And
                .HaveCountGreaterThanOrEqualTo(1);
            _output.WriteLine(response?.Text);
        }

        [Fact(Skip = "Bad Request due to FileData part")]
        public async Task Describe_Image_From_StorageBucket()
        {
            // Arrange
            var prompt = "Describe the image with a creative description";
            var googleAi = new GoogleAI(apiKey: _fixture.ApiKey);
            var model = _googleAi.GenerativeModel(model: _model);
            var generationConfig = new GenerationConfig
            {
                Temperature = 0.4f, TopP = 1, TopK = 32, MaxOutputTokens = 2048
            };
            // var request = new GenerateContentRequest(prompt, generationConfig);
            var request = new GenerateContentRequest(prompt);
            await request.AddMedia(
                "gs://generativeai-downloads/images/scones.jpg",
                "image/jpeg");

            // Act
            var response = await model.GenerateContent(request);

            // Assert
            response.Should().NotBeNull();
            response.Candidates.Should().NotBeNull().And.HaveCount(1);
            response.Candidates.FirstOrDefault().Content.Should().NotBeNull();
            response.Candidates.FirstOrDefault().Content.Parts.Should().NotBeNull().And
                .HaveCountGreaterThanOrEqualTo(1);
            _output.WriteLine(response?.Text);
        }

        [Fact]
        public async Task Describe_Image_From_URL()
        {
            // Arrange
            var prompt = "Parse the time and city from the airport board shown in this image into a list, in Markdown";
            var googleAi = new GoogleAI(apiKey: _fixture.ApiKey);
            var model = _googleAi.GenerativeModel(model: _model);
            var request = new GenerateContentRequest(prompt);
            await request.AddMedia(
                "https://raw.githubusercontent.com/mscraftsman/generative-ai/refs/heads/main/tests/Mscc.GenerativeAI/payload/timetable.png");

            // Act
            var response = await model.GenerateContent(request);

            // Assert
            response.Should().NotBeNull();
            response.Candidates.Should().NotBeNull().And.HaveCount(1);
            response.Candidates.FirstOrDefault().Content.Should().NotBeNull();
            response.Candidates.FirstOrDefault().Content.Parts.Should().NotBeNull().And
                .HaveCountGreaterThanOrEqualTo(1);
            _output.WriteLine(response?.Text);
        }

        [Fact]
        public async Task Generate_Content_SystemInstruction()
        {
            // Arrange
            var systemInstruction = new Content("You are a friendly pirate. Speak like one.");
            var prompt = "Good morning! How are you?";
            IGenerativeAI genAi = new GoogleAI(_fixture.ApiKey);
            var model = _googleAi.GenerativeModel(_model, systemInstruction: systemInstruction);
            var request = new GenerateContentRequest(prompt);

            // Act
            var response = await model.GenerateContent(request);

            // Assert
            response.Should().NotBeNull();
            response.Candidates.Should().NotBeNull().And.HaveCount(1);
            response.Text.Should().NotBeNull();
            _output.WriteLine($"{response?.Text}");
        }

        [Fact]
        public async Task Generate_Content_SystemInstruction_WithSafetySettings()
        {
            // Arrange
            var systemInstruction =
                new Content(
                    "You are a helpful language translator. Your mission is to translate text in English to French.");
            var prompt = @"User input: I like bagels.
Answer:";
            var generationConfig = new GenerationConfig()
            {
                Temperature = 0.9f,
                TopP = 1.0f,
                TopK = 32,
                CandidateCount = 1,
                MaxOutputTokens = 8192
            };
            var safetySettings = new List<SafetySetting>()
            {
                new()
                {
                    Category = HarmCategory.HarmCategoryHarassment,
                    Threshold = HarmBlockThreshold.BlockLowAndAbove
                },
                new()
                {
                    Category = HarmCategory.HarmCategoryHateSpeech,
                    Threshold = HarmBlockThreshold.BlockLowAndAbove
                },
                new()
                {
                    Category = HarmCategory.HarmCategorySexuallyExplicit,
                    Threshold = HarmBlockThreshold.BlockLowAndAbove
                },
                new()
                {
                    Category = HarmCategory.HarmCategoryDangerousContent,
                    Threshold = HarmBlockThreshold.BlockLowAndAbove
                }
            };
            IGenerativeAI genAi = new GoogleAI(_fixture.ApiKey);
            var model = _googleAi.GenerativeModel(_model, systemInstruction: systemInstruction);
            var request = new GenerateContentRequest(prompt, generationConfig, safetySettings);

            // Act
            var response = await model.GenerateContent(request);

            // Assert
            response.Should().NotBeNull();
            response.Candidates.Should().NotBeNull().And.HaveCount(1);
            response.Text.Should().NotBeNull();
            _output.WriteLine($"{prompt} {response?.Text}");
        }

        [Fact(Skip = "The 'gs' scheme is not supported.")]
        public async Task Multimodal_Video_Input()
        {
            // Arrange
            var googleAi = new GoogleAI(apiKey: _fixture.ApiKey);
            var model = _googleAi.GenerativeModel(model: _model);
            var video = await TestExtensions.ReadImageFileBase64Async("gs://cloud-samples-data/video/animals.mp4");
            var request = new GenerateContentRequest("What's in the video?");
            request.Contents[0].Role = Role.User;
            request.Contents[0].Parts.Add(new InlineData { MimeType = "video/mp4", Data = video });

            // Act
            var response = await model.GenerateContent(request);

            // Assert
            response.Should().NotBeNull();
            response.Candidates.Should().NotBeNull().And.HaveCount(1);
            response.Candidates.FirstOrDefault().Content.Should().NotBeNull();
            response.Candidates.FirstOrDefault().Content.Parts.Should().NotBeNull().And
                .HaveCountGreaterThanOrEqualTo(1);
            response.Text.Should().Contain("Zootopia");
            _output.WriteLine(response?.Text);
        }

        [Theory]
        [InlineData(78330)]
        public async Task Count_Tokens_Audio(int expected)
        {
            // Arrange
            IGenerativeAI genAi = new GoogleAI(_fixture.ApiKey);
            var model = _googleAi.GenerativeModel(_model);
            var request = new GenerateContentRequest { Contents = new List<Content>() };
            var files = await ((GoogleAI)genAi).ListFiles();
            foreach (var file in files.Files.Where(x => x.MimeType.StartsWith("audio/")))
            {
                _output.WriteLine($"File: {file.Name}");
                request.Contents.Add(new Content
                {
                    Role = Role.User,
                    Parts = new List<IPart> { new FileData { FileUri = file.Uri, MimeType = file.MimeType } }
                });
            }

            // Act
            var response = await model.CountTokens(request);

            // Assert
            response.Should().NotBeNull();
            response.TotalTokens.Should().BeGreaterOrEqualTo(expected);
            _output.WriteLine($"Tokens: {response?.TotalTokens}");
        }

        [Fact]
        public async Task Count_Tokens_Audio_FileApi()
        {
            // Arrange
            IGenerativeAI genAi = new GoogleAI(_fixture.ApiKey);
            var model = _googleAi.GenerativeModel(_model);
            var request = new GenerateContentRequest { Contents = new List<Content>() };
            var files = await ((GoogleAI)genAi).ListFiles();
            var file = files.Files.Where(x => x.MimeType.StartsWith("audio/")).FirstOrDefault();

            // Act
            var response = await model.CountTokens(file);

            // Assert
            response.Should().NotBeNull();
            _output.WriteLine($"Tokens: {response?.TotalTokens}");
        }

        [Fact]
        public void Initialize_GeminiProVision()
        {
            // Arrange
            var googleAi = new GoogleAI(apiKey: _fixture.ApiKey);

            // Act
            var model = _googleAi.GenerativeModel(model: _model);

            // Assert
            model.Should().NotBeNull();
            model.Name.Should().Be(Model.Gemini25ProExperimental);
        }

        [Fact]
        public async Task Describe_AddMedia_From_Url_Markdown()
        {
            // Arrange
            var prompt =
                "Parse the time and city from the airport board shown in this image into a list, in Markdown table";
            var googleAi = new GoogleAI(apiKey: _fixture.ApiKey);
            var model = _googleAi.GenerativeModel(model: _model);
            var request = new GenerateContentRequest(prompt);
            await request.AddMedia(
                "https://raw.githubusercontent.com/mscraftsman/generative-ai/refs/heads/main/tests/Mscc.GenerativeAI/payload/timetable.png");

            // Act
            var response = await model.GenerateContent(request);

            // Assert
            response.Should().NotBeNull();
            response.Candidates.Should().NotBeNull().And.HaveCount(1);
            response.Candidates.FirstOrDefault().Content.Should().NotBeNull();
            response.Candidates.FirstOrDefault().Content.Parts.Should().NotBeNull().And
                .HaveCountGreaterThanOrEqualTo(1);
            _output.WriteLine(response?.Text);
        }

        [Fact]
        public async Task Describe_AddMedia_From_Url_JSON()
        {
            // Arrange
            var prompt = "Parse the time and city from the airport board shown in this image into a list, in JSON";
            var googleAi = new GoogleAI(apiKey: _fixture.ApiKey);
            var model = _googleAi.GenerativeModel(model: _model);
            var request = new GenerateContentRequest(prompt);
            await request.AddMedia(
                "https://raw.githubusercontent.com/mscraftsman/generative-ai/refs/heads/main/tests/Mscc.GenerativeAI/payload/timetable.png");

            // Act
            var response = await model.GenerateContent(request);

            // Assert
            response.Should().NotBeNull();
            response.Candidates.Should().NotBeNull().And.HaveCount(1);
            response.Candidates.FirstOrDefault().Content.Should().NotBeNull();
            response.Candidates.FirstOrDefault().Content.Parts.Should().NotBeNull().And
                .HaveCountGreaterThanOrEqualTo(1);
            _output.WriteLine(response?.Text);
        }

        [Fact(Skip = "Bad Request due to FileData part")]
        public async Task Describe_Image_From_FileData()
        {
            // Arrange
            var prompt = "Parse the time and city from the airport board shown in this image into a list, in Markdown";
            var googleAi = new GoogleAI(apiKey: _fixture.ApiKey);
            var model = _googleAi.GenerativeModel(model: _model);
            var request = new GenerateContentRequest(prompt);
            request.Contents[0].Parts.Add(new FileData
            {
                FileUri =
                    "https://raw.githubusercontent.com/mscraftsman/generative-ai/refs/heads/main/tests/Mscc.GenerativeAI/payload/timetable.png",
                MimeType = "image/png"
            });

            // Act
            var response = await model.GenerateContent(request);

            // Assert
            response.Should().NotBeNull();
            response.Candidates.Should().NotBeNull().And.HaveCount(1);
            response.Candidates.FirstOrDefault().Content.Should().NotBeNull();
            response.Candidates.FirstOrDefault().Content.Parts.Should().NotBeNull().And
                .HaveCountGreaterThanOrEqualTo(1);
            _output.WriteLine(response?.Text);
        }
    }
}