#if NET472_OR_GREATER || NETSTANDARD2_0
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
#endif
#if NET9_0
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
#endif
using FluentAssertions;
using Mscc.GenerativeAI;
using System.ComponentModel;
using System.IO;
using Xunit;
using Xunit.Abstractions;

namespace Test.Mscc.GenerativeAI
{
    [Collection(nameof(ConfigurationFixture))]
    public class GoogleAiGeminiProShould(ITestOutputHelper output, ConfigurationFixture fixture)
    {
        private readonly string _model = Model.Gemini15Pro;

        [Fact]
        public void Initialize_GoogleAI()
        {
            // Arrange

            // Act
            var googleAi = new GoogleAI(apiKey: fixture.ApiKey);

            // Assert
            googleAi.Should().NotBeNull();
        }

        [Fact]
        public void Initialize_Using_GoogleAI()
        {
            // Arrange
            var expected = Environment.GetEnvironmentVariable("GOOGLE_AI_MODEL") ?? Model.Gemini15Pro;
            var googleAi = new GoogleAI(apiKey: fixture.ApiKey);

            // Act
            var model = googleAi.GenerativeModel();

            // Assert
            model.Should().NotBeNull();
            model.Name.Should().Be($"{expected.SanitizeModelName()}");
        }

        [Fact]
        public void Initialize_EnvVars()
        {
            // Arrange
            Environment.SetEnvironmentVariable("GOOGLE_API_KEY", fixture.ApiKey);
            var expected = Environment.GetEnvironmentVariable("GOOGLE_AI_MODEL") ?? Model.Gemini15Pro;
            var googleAI = new GoogleAI(accessToken: fixture.AccessToken);

            // Act
            var model = googleAI.GenerativeModel();

            // Assert
            model.Should().NotBeNull();
            model.Name.Should().Be($"{expected.SanitizeModelName()}");
        }

        [Fact]
        public void Initialize_Default_Model()
        {
            // Arrange
            var expected = Environment.GetEnvironmentVariable("GOOGLE_AI_MODEL") ?? Model.Gemini15Pro;
            var googleAi = new GoogleAI(apiKey: fixture.ApiKey);

            // Act
            var model = googleAi.GenerativeModel();

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
            var googleAi = new GoogleAI(apiKey: fixture.ApiKey);
            var model = googleAi.GenerativeModel(model: _model);

            // Assert
            model.Should().NotBeNull();
            model.Name.Should().Be($"{expected.SanitizeModelName()}");
        }

        [Fact]
        public async Task List_Models()
        {
            // Arrange
            var googleAi = new GoogleAI(apiKey: fixture.ApiKey);
            var model = googleAi.GenerativeModel();

            // Act
            var sut = await model.ListModels();

            // Assert
            sut.Should().NotBeNull();
            sut.Should().NotBeNull().And.HaveCountGreaterThanOrEqualTo(1);
            sut.ForEach(x =>
            {
                output.WriteLine($"Model: {x.DisplayName} ({x.Name})");
                x.SupportedGenerationMethods.ForEach(m => output.WriteLine($"  Method: {m}"));
            });
        }

        [Fact]
        public async Task List_Models_Using_OAuth()
        {
            // Arrange
            var model = new GenerativeModel { AccessToken = fixture.AccessToken };

            // Act
            var sut = await model.ListModels();

            // Assert
            sut.Should().NotBeNull();
            sut.Should().NotBeNull().And.HaveCountGreaterThanOrEqualTo(1);
            sut.ForEach(x =>
            {
                output.WriteLine($"Model: {x.DisplayName} ({x.Name})");
                x.SupportedGenerationMethods.ForEach(m => output.WriteLine($"  Method: {m}"));
            });
        }

        [Fact]
        public async Task List_Tuned_Models()
        {
            // Arrange
            var model = new GenerativeModel { AccessToken = fixture.AccessToken };

            // Act
            var sut = await model.ListModels(true);
            // var sut = await model.ListTunedModels();

            // Assert
            sut.Should().NotBeNull();
            sut.Should().NotBeNull().And.HaveCountGreaterThanOrEqualTo(1);
            sut.ForEach(x =>
            {
                output.WriteLine($"Model: {x.DisplayName} ({x.Name})");
                x.TuningTask.Snapshots.ForEach(m => output.WriteLine($"  Snapshot: {m}"));
            });
        }

        [Theory]
        [InlineData(Model.Gemini10Pro001)]
        [InlineData(Model.GeminiProVision)]
        [InlineData(Model.BisonText)]
        [InlineData(Model.BisonChat)]
        [InlineData("tunedModels/number-generator-model-psx3d3gljyko")]
        public async Task Get_Model_Information(string modelName)
        {
            // Arrange
            var googleAi = new GoogleAI(apiKey: fixture.ApiKey);
            var model = googleAi.GenerativeModel();

            // Act
            var sut = await model.GetModel(model: modelName);

            // Assert
            sut.Should().NotBeNull();
            // sut.Name.Should().Be($"{modelName.SanitizeModelName()}");
            output.WriteLine($"Model: {sut.DisplayName} ({sut.Name})");
            sut.SupportedGenerationMethods.ForEach(m => output.WriteLine($"  Method: {m}"));
        }

        [Theory]
        [InlineData("tunedModels/number-generator-model-psx3d3gljyko")]
        public async Task Get_TunedModel_Information_Using_ApiKey(string modelName)
        {
            // Arrange
            var googleAi = new GoogleAI(apiKey: fixture.ApiKey);
            var model = googleAi.GenerativeModel();

            // Act & Assert
            await Assert.ThrowsAsync<NotSupportedException>(() => model.GetModel(model: modelName));
        }

        [Theory]
        [InlineData(Model.Gemini10Pro001)]
        [InlineData(Model.GeminiProVision)]
        [InlineData(Model.BisonText)]
        [InlineData(Model.BisonChat)]
        [InlineData("tunedModels/number-generator-model-psx3d3gljyko")]
        public async Task Get_Model_Information_Using_OAuth(string modelName)
        {
            // Arrange
            var model = new GenerativeModel { AccessToken = fixture.AccessToken };
            var expected = modelName;
            if (!expected.Contains("/"))
                expected = $"{expected.SanitizeModelName()}";

            // Act
            var sut = await model.GetModel(model: modelName);

            // Assert
            sut.Should().NotBeNull();
            sut.Name.Should().Be(expected);
            output.WriteLine($"Model: {sut.DisplayName} ({sut.Name})");
            if (sut.State is null)
            {
                sut?.SupportedGenerationMethods?.ForEach(m => output.WriteLine($"  Method: {m}"));
            }
            else
            {
                output.WriteLine($"State: {sut.State}");
            }
        }

        [Fact]
        public async Task Generate_Content()
        {
            // Arrange
            var prompt = "Write a story about a magic backpack.";
            var googleAi = new GoogleAI(apiKey: fixture.ApiKey);
            var model = googleAi.GenerativeModel(model: _model);

            // Act
            var response = await model.GenerateContent(prompt);

            // Assert
            response.Should().NotBeNull();
            response.Candidates.Should().NotBeNull().And.HaveCount(1);
            response.Text.Should().NotBeEmpty();
            output.WriteLine(response?.Text);
        }

        [Fact]
        public async Task GenerateContent_WithEmptyPrompt_ThrowsArgumentNullException()
        {
            // Arrange
            var googleAi = new GoogleAI(apiKey: fixture.ApiKey);
            var model = googleAi.GenerativeModel(model: _model);
            string prompt = null;

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => model.GenerateContent(prompt));
        }

        [Fact]
        public async Task GenerateContent_WithInvalidAPIKey_ChangingBeforeRequest()
        {
            // Arrange
            var prompt = "Tell me 4 things about Taipei. Be short.";
            var googleAi = new GoogleAI(apiKey: "WRONG_API_KEY");
            var model = googleAi.GenerativeModel(model: Model.Gemini10Pro001);
            model.ApiKey = fixture.ApiKey;

            // Act
            var response = await model.GenerateContent(prompt);

            // Assert
            response.Should().NotBeNull();
            response.Candidates.Should().NotBeNull().And.HaveCount(1);
            response.Text.Should().NotBeEmpty();
            output.WriteLine(response?.Text);
        }

        [Fact]
        public async Task GenerateContent_WithInvalidAPIKey_ChangingAfterRequest()
        {
            // Arrange
            var prompt = "Tell me 4 things about Taipei. Be short.";
            var googleAi = new GoogleAI(apiKey: "WRONG_API_KEY");
            var model = googleAi.GenerativeModel(model: Model.Gemini10Pro001);
            await Assert.ThrowsAsync<HttpRequestException>(() => model.GenerateContent(prompt));

            // Act
            model.ApiKey = fixture.ApiKey;
            var response = await model.GenerateContent(prompt);

            // Assert
            response.Should().NotBeNull();
            response.Candidates.Should().NotBeNull().And.HaveCount(1);
            response.Text.Should().NotBeEmpty();
            output.WriteLine(response?.Text);
        }

        [Fact]
        public async Task Generate_Content_MaxTokens()
        {
            // Arrange
            var prompt = "Write a story about a magic backpack.";
            var googleAi = new GoogleAI(apiKey: fixture.ApiKey);
            var model = googleAi.GenerativeModel(model: Model.Gemini10Pro001);
            var generationConfig = new GenerationConfig() { MaxOutputTokens = 20 };

            // Act
            var response = await model.GenerateContent(prompt, generationConfig);

            // Assert
            response.Should().NotBeNull();
            response.Candidates.Should().NotBeNull().And.HaveCount(1);
            response.Text.Should().BeEmpty();
            // output.WriteLine(response?.Text);
        }

        [Fact]
        public async Task Generate_Content_Stream_MaxTokens()
        {
            // Arrange
            var prompt = "Write a story about a magic backpack.";
            var googleAi = new GoogleAI(apiKey: fixture.ApiKey);
            var model = googleAi.GenerativeModel(model: Model.Gemini10Pro001);
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
                    output.WriteLine("...");
                else
                    output.WriteLine(response?.Text);
            }
        }

        [Fact]
        public async Task Generate_Content_MultiplePrompt()
        {
            // Arrange
            var googleAi = new GoogleAI(apiKey: fixture.ApiKey);
            var model = googleAi.GenerativeModel(model: _model);
            var parts = new List<IPart>
            {
                new TextData { Text = "What is x multiplied by 2?" }, new TextData { Text = "x = 42" }
            };

            // Act
            var response = await model.GenerateContent(parts);

            // Assert
            response.Should().NotBeNull();
            response.Candidates.Should().NotBeNull().And.HaveCount(1);
            output.WriteLine(response?.Text);
            response.Text.Should().Be("84");
        }

        [Fact]
        public async Task Generate_Content_Request()
        {
            // Arrange
            var prompt = "Write a story about a magic backpack.";
            var googleAi = new GoogleAI(apiKey: fixture.ApiKey);
            var model = googleAi.GenerativeModel(model: _model);
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
            output.WriteLine(response?.Text);
        }

        [Fact]
        public async Task GenerateContent_WithRequest_MultipleCandidates_ThrowsHttpRequestException()
        {
            // Arrange
            var prompt = "Write a short poem about koi fish.";
            var googleAi = new GoogleAI(apiKey: fixture.ApiKey);
            var model = googleAi.GenerativeModel(model: _model);
            var request = new GenerateContentRequest
            {
                Contents = new List<Content>(), GenerationConfig = new GenerationConfig() { CandidateCount = 3 }
            };
            request.Contents.Add(new Content
            {
                Role = Role.User, Parts = new List<IPart> { new TextData { Text = prompt } }
            });

            // Act & Assert
            await Assert.ThrowsAsync<HttpRequestException>(() => model.GenerateContent(request));
        }

        [Fact]
        public async Task GenerateContent_WithNullRequest_ThrowsArgumentNullException()
        {
            // Arrange
            var googleAi = new GoogleAI(apiKey: fixture.ApiKey);
            var model = googleAi.GenerativeModel(model: _model);
            GenerateContentRequest request = null;

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => model.GenerateContent(request));
        }

        [Fact]
        public async Task Generate_Content_RequestConstructor()
        {
            // Arrange
            var prompt = "Write a story about a magic backpack.";
            var googleAi = new GoogleAI(apiKey: fixture.ApiKey);
            var model = googleAi.GenerativeModel(model: _model);
            var request = new GenerateContentRequest(prompt);
            request.Contents[0].Role = Role.User;

            // Act
            var response = await model.GenerateContent(request);

            // Assert
            response.Should().NotBeNull();
            response.Candidates.Should().NotBeNull().And.HaveCount(1);
            response.Text.Should().NotBeEmpty();
            output.WriteLine(response?.Text);
        }

        [Fact]
        public async Task GenerateContent_WithRequest_UseServerSentEvents()
        {
            // Arrange
            var prompt = "Write a story about a magic backpack.";
            var googleAi = new GoogleAI(apiKey: fixture.ApiKey);
            var model = googleAi.GenerativeModel(model: _model);
            model.UseServerSentEventsFormat = true;
            var request = new GenerateContentRequest(prompt);

            // Act
            var response = await model.GenerateContent(request);

            // Assert
            response.Should().NotBeNull();
            response.Candidates.Should().NotBeNull().And.HaveCount(1);
            response.Text.Should().NotBeEmpty();
            output.WriteLine(response?.Text);
        }

        [Fact]
        public async Task GenerateContent_Stream_WithRequest_ServerSentEvents()
        {
            // Arrange
            var prompt = "Write a story about a magic backpack.";
            var googleAi = new GoogleAI(apiKey: fixture.ApiKey);
            var model = googleAi.GenerativeModel(model: _model);
            model.UseServerSentEventsFormat = true;
            var request = new GenerateContentRequest(prompt);

            // Act
            var responseEvents = model.GenerateContentStream(request);

            // Assert
            responseEvents.Should().NotBeNull();
            await foreach (var response in responseEvents)
            {
                output.WriteLine($"{response.Text}");
            }
        }

        [Fact]
        public async Task Generate_Content_Stream()
        {
            // Arrange
            var prompt = "How are you doing today?";
            var googleAi = new GoogleAI(apiKey: fixture.ApiKey);
            var model = googleAi.GenerativeModel(model: _model);

            // Act
            var responseStream = model.GenerateContentStream(prompt);

            // Assert
            responseStream.Should().NotBeNull();
            await foreach (var response in responseStream)
            {
                response.Should().NotBeNull();
                response.Candidates.Should().NotBeNull().And.HaveCount(1);
                response.Text.Should().NotBeEmpty();
                output.WriteLine($"{response.Text}");
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
            var googleAi = new GoogleAI(apiKey: fixture.ApiKey);
            var model = googleAi.GenerativeModel(model: _model);
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
                output.WriteLine($"{response.Text}");
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
            var googleAi = new GoogleAI(apiKey: fixture.ApiKey);
            var model = googleAi.GenerativeModel(model: Model.AttributedQuestionAnswering);
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
            var googleAi = new GoogleAI(apiKey: fixture.ApiKey);
            var model = googleAi.GenerativeModel(model: _model);

            // Act
            var response = await model.CountTokens(prompt);

            // Assert
            response.Should().NotBeNull();
            response.TotalTokens.Should().BeGreaterThanOrEqualTo(expected);
            output.WriteLine($"Tokens: {response?.TotalTokens}");
        }

        [Theory]
        [InlineData("How are you doing today?", 7)]
        [InlineData("What kind of fish is this?", 8)]
        [InlineData("Write a story about a magic backpack.", 9)]
        [InlineData("Write an extended story about a magic backpack.", 10)]
        public async Task Count_Tokens_Request(string prompt, int expected)
        {
            // Arrange
            var googleAi = new GoogleAI(apiKey: fixture.ApiKey);
            var model = googleAi.GenerativeModel(model: _model);
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
            output.WriteLine($"Tokens: {response?.TotalTokens}");
        }

        [Fact]
        public async Task Start_Chat()
        {
            // Arrange
            var googleAi = new GoogleAI(apiKey: fixture.ApiKey);
            var model = googleAi.GenerativeModel(model: _model);
            var chat = model.StartChat();
            var prompt = "How can I learn more about C#?";

            // Act
            var response = await chat.SendMessage(prompt);

            // Assert
            response.Should().NotBeNull();
            response.Candidates.Should().NotBeNull().And.HaveCount(1);
            response.Text.Should().NotBeEmpty();
            output.WriteLine(response?.Text);
        }

        [Fact]
        public async Task Start_Chat_With_History()
        {
            // Arrange
            var googleAi = new GoogleAI(apiKey: fixture.ApiKey);
            var model = googleAi.GenerativeModel(model: _model);
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
            output.WriteLine(prompt);
            output.WriteLine(response?.Text);
            //output.WriteLine(response?.PromptFeedback);
        }

        [Fact]
        // Refs:
        // https://cloud.google.com/vertex-ai/generative-ai/docs/multimodal/send-chat-prompts-gemini
        public async Task Start_Chat_Multiple_Prompts()
        {
            // Arrange
            var googleAi = new GoogleAI(apiKey: fixture.ApiKey);
            var model = googleAi.GenerativeModel(model: _model);
            var chat = model.StartChat();

            // Act
            var prompt = "Hello, let's talk a bit about nature.";
            var response = await chat.SendMessage(prompt);
            output.WriteLine(prompt);
            output.WriteLine(response?.Text);
            prompt = "What are all the colors in a rainbow?";
            response = await chat.SendMessage(prompt);
            output.WriteLine(prompt);
            output.WriteLine(response?.Text);
            prompt = "Why does it appear when it rains?";
            response = await chat.SendMessage(prompt);
            output.WriteLine(prompt);
            output.WriteLine(response?.Text);

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
            var googleAi = new GoogleAI(apiKey: fixture.ApiKey);
            var model = googleAi.GenerativeModel(model: _model);
            var chat = model.StartChat();

            // Act
            _ = await chat.SendMessage("Hello, fancy brainstorming about IT?");
            _ = await chat.SendMessage("In one sentence, explain how a computer works to a young child.");
            _ = await chat.SendMessage("Okay, how about a more detailed explanation to a high schooler?");
            _ = await chat.SendMessage("Lastly, give a thorough definition for a CS graduate.");

            // Assert
            chat.History.ForEach(c =>
            {
                output.WriteLine($"{new string('-', 20)}");
                output.WriteLine($"{c.Role}: {c.Text}");
            });
        }

        [Fact]
        // Refs:
        // https://ai.google.dev/tutorials/python_quickstart#chat_conversations
        public async Task Start_Chat_Rewind_Conversation()
        {
            // Arrange
            var googleAi = new GoogleAI(apiKey: fixture.ApiKey);
            var model = googleAi.GenerativeModel(model: _model);
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
            output.WriteLine("------ Rewind ------");
            output.WriteLine($"{entries.Sent.Role}: {entries.Sent.Text}");
            output.WriteLine($"{new string('-', 20)}");
            output.WriteLine($"{entries.Received.Role}: {entries.Received.Text}");
            output.WriteLine($"{new string('-', 20)}");

            chat.History.Count.Should().Be(6);
            output.WriteLine("------ History -----");
            chat.History.ForEach(c =>
            {
                output.WriteLine($"{new string('-', 20)}");
                output.WriteLine($"{c.Role}: {c.Text}");
            });
        }

        [Fact]
        // Refs:
        // https://ai.google.dev/tutorials/python_quickstart#chat_conversations
        public async Task Start_Chat_Conversations_Get_Last()
        {
            // Arrange
            var googleAi = new GoogleAI(apiKey: fixture.ApiKey);
            var model = googleAi.GenerativeModel(model: _model);
            var chat = model.StartChat();
            _ = await chat.SendMessage("Hello, fancy brainstorming about IT?");
            _ = await chat.SendMessage("In one sentence, explain how a computer works to a young child.");
            _ = await chat.SendMessage("Okay, how about a more detailed explanation to a high school kid?");
            _ = await chat.SendMessage("Lastly, give a thorough definition for a CS graduate.");

            // Act
            var sut = chat.Last;

            // Assert
            sut.Should().NotBeNull();
            output.WriteLine($"{sut.Role}: {sut.Text}");
        }

        [Fact]
        public async Task Start_Chat_With_Multimodal_Content()
        {
            // Arrange
            var systemInstruction = new Content("You are an expert analyzing transcripts.");
            var genAi = new GoogleAI(apiKey: fixture.ApiKey);
            var model = genAi.GenerativeModel(_model, systemInstruction: systemInstruction);
            var chat = model.StartChat();
            var filePath = Path.Combine(Environment.CurrentDirectory, "payload", "a11.txt");
            var document = await genAi.UploadFile(filePath, "Apollo 11 Flight Report");
            output.WriteLine(
                $"Display Name: {document.File.DisplayName} ({Enum.GetName(typeof(StateFileResource), document.File.State)})");

            var request = new GenerateContentRequest("Hi, could you summarize this transcript?");
            request.AddMedia(document.File);

            // Act
            var response = await chat.SendMessage(request);
            output.WriteLine($"model: {response.Text}");
            output.WriteLine("----------");
            response = await chat.SendMessage("Okay, could you tell me more about the trans-lunar injection");
            output.WriteLine($"model: {response.Text}");

            // Assert
            model.Should().NotBeNull();
            chat.History.Count.Should().Be(4);
            response.Should().NotBeNull();
            response.Candidates.Should().NotBeNull().And.HaveCount(1);
            response.Text.Should().NotBeNull();
            output.WriteLine($"model: {response.Text}");
        }

        [Fact]
        public async Task Start_Chat_Streaming()
        {
            // Arrange
            var googleAi = new GoogleAI(apiKey: fixture.ApiKey);
            var model = googleAi.GenerativeModel(model: _model);
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
                output.WriteLine($"{response.Text}");
                // response.UsageMetadata.Should().NotBeNull();
                // output.WriteLine($"PromptTokenCount: {response?.UsageMetadata?.PromptTokenCount}");
                // output.WriteLine($"CandidatesTokenCount: {response?.UsageMetadata?.CandidatesTokenCount}");
                // output.WriteLine($"TotalTokenCount: {response?.UsageMetadata?.TotalTokenCount}");
            }

            chat.History.Count.Should().Be(2);
            output.WriteLine($"{new string('-', 20)}");
            output.WriteLine("------ History -----");
            chat.History.ForEach(c =>
            {
                output.WriteLine($"{new string('-', 20)}");
                output.WriteLine($"{c.Role}: {c.Text}");
            });
        }

        [Fact]
        // Ref: https://ai.google.dev/api/generate-content#code-execution
        public async Task Generate_Content_Code_Execution()
        {
            // Arrange
            var prompt = "What is the sum of the first 50 prime numbers?";
            var genAi = new GoogleAI(fixture.ApiKey);
            var model = genAi.GenerativeModel(Model.Gemini15Flash,
                tools: [new Tool { CodeExecution = new() }]);

            // Act
            var response = await model.GenerateContent(prompt);

            // Assert
            response.Should().NotBeNull();
            response.Candidates.Should().NotBeNull().And.HaveCount(1);
            output.WriteLine(string.Join(Environment.NewLine,
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
            var prompt = "What is the current Google stock price?";
            var genAi = new GoogleAI(fixture.ApiKey);
            var model = genAi.GenerativeModel("gemini-1.5-pro-002");
            model.UseGrounding = true;

            // Act
            var response = await model.GenerateContent(prompt);

            // Assert
            response.Should().NotBeNull();
            response.Candidates.Should().NotBeNull().And.HaveCount(1);
            response.Candidates![0].GroundingMetadata.Should().NotBeNull();
            response.Candidates![0].GroundingMetadata!.SearchEntryPoint.Should().NotBeNull();
            response.Candidates![0].GroundingMetadata!.WebSearchQueries.Should().NotBeNull();
            output.WriteLine(string.Join(Environment.NewLine,
                response.Candidates![0].Content!.Parts
                    .Select(x => x.Text)
//                    .Where(t => !string.IsNullOrEmpty(t))
                    .ToArray()));
            response.Candidates![0].GroundingMetadata!.GroundingChunks?
                .ForEach(c =>
                    output.WriteLine($"{c!.Web!.Title} - {c!.Web!.Uri}"));
            output.WriteLine(string.Join(Environment.NewLine,
                response.Candidates![0].GroundingMetadata!.WebSearchQueries!
                    .Select(w => w)
                    .ToArray()));
            output.WriteLine(response.Candidates![0].GroundingMetadata!.SearchEntryPoint!.RenderedContent);
        }

        [Fact]
        // Ref: https://ai.google.dev/gemini-api/docs/grounding
        public async Task Generate_Content_Grounding_Search_by_String()
        {
            // Arrange
            var prompt = "Who won Wimbledon this year?";
            var genAi = new GoogleAI(fixture.ApiKey);
            var model = genAi.GenerativeModel("gemini-1.5-pro-002",
                tools: [new Tool { GoogleSearchRetrieval = new() }]);

            // Act
            var response = await model.GenerateContent(prompt);

            // Assert
            response.Should().NotBeNull();
            response.Candidates.Should().NotBeNull().And.HaveCount(1);
            response.Candidates![0].GroundingMetadata.Should().NotBeNull();
            response.Candidates![0].GroundingMetadata!.SearchEntryPoint.Should().NotBeNull();
            response.Candidates![0].GroundingMetadata!.WebSearchQueries.Should().NotBeNull();
            output.WriteLine(string.Join(Environment.NewLine,
                response.Candidates![0].Content!.Parts
                    .Select(x => x.Text)
//                    .Where(t => !string.IsNullOrEmpty(t))
                    .ToArray()));
            response.Candidates![0].GroundingMetadata!.GroundingChunks?
                .ForEach(c =>
                    output.WriteLine($"{c!.Web!.Title} - {c!.Web!.Uri}"));
            output.WriteLine(string.Join(Environment.NewLine,
                response.Candidates![0].GroundingMetadata!.WebSearchQueries!
                    .Select(w => w)
                    .ToArray()));
            output.WriteLine(response.Candidates![0].GroundingMetadata!.SearchEntryPoint!.RenderedContent);
        }

        [Fact]
        // Ref: https://ai.google.dev/gemini-api/docs/grounding
        public async Task Generate_Content_Grounding_Search_by_Dictionary()
        {
            // Arrange
            var prompt = "Who won Wimbledon this year?";
            var genAi = new GoogleAI(fixture.ApiKey);
            var model = genAi.GenerativeModel("gemini-1.5-pro-002",
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
            output.WriteLine(string.Join(Environment.NewLine,
                response.Candidates![0].Content!.Parts
                    .Select(x => x.Text)
//                    .Where(t => !string.IsNullOrEmpty(t))
                    .ToArray()));
            response.Candidates![0].GroundingMetadata!.GroundingChunks?
                .ForEach(c =>
                    output.WriteLine($"{c!.Web!.Title} - {c!.Web!.Uri}"));
            output.WriteLine(string.Join(Environment.NewLine,
                response.Candidates![0].GroundingMetadata!.WebSearchQueries!
                    .Select(w => w)
                    .ToArray()));
            output.WriteLine(response.Candidates![0].GroundingMetadata!.SearchEntryPoint!.RenderedContent);
        }

        [Fact]
        // Ref: https://ai.google.dev/gemini-api/docs/grounding
        public async Task Generate_Content_Grounding_Search_Default()
        {
            // Arrange
            var prompt = "Who won Wimbledon this year?";
            var genAi = new GoogleAI(fixture.ApiKey);
            var model = genAi.GenerativeModel("gemini-1.5-pro-002",
                tools: [new Tool { GoogleSearchRetrieval = new() }]);

            // Act
            var response = await model.GenerateContent(prompt);

            // Assert
            response.Should().NotBeNull();
            response.Candidates.Should().NotBeNull().And.HaveCount(1);
            output.WriteLine(string.Join(Environment.NewLine,
                response.Candidates![0].Content!.Parts
                    .Select(x => x.Text)
//                    .Where(t => !string.IsNullOrEmpty(t))
                    .ToArray()));
            response.Candidates![0].GroundingMetadata!.GroundingChunks?
                .ForEach(c =>
                    output.WriteLine($"{c!.Web!.Title} - {c!.Web!.Uri}"));
            output.WriteLine(string.Join(Environment.NewLine,
                response.Candidates![0].GroundingMetadata!.WebSearchQueries!
                    .Select(w => w)
                    .ToArray()));
            output.WriteLine(response.Candidates![0].GroundingMetadata!.SearchEntryPoint!.RenderedContent);
        }

        [Fact]
        // Ref: https://ai.google.dev/gemini-api/docs/models/gemini-v2#search-tool
        public async Task Generate_Content_with_Google_Search()
        {
            // Arrange
            var prompt = "When is the next total solar eclipse in Mauritius?";
            var genAi = new GoogleAI(fixture.ApiKey);
            var model = genAi.GenerativeModel(Model.Gemini20FlashExperimental);
            model.UseGoogleSearch = true;

            // Act
            var response = await model.GenerateContent(prompt);

            // Assert
            response.Should().NotBeNull();
            response.Candidates.Should().NotBeNull().And.HaveCount(1);
            response.Candidates![0].GroundingMetadata.Should().NotBeNull();
            response.Candidates![0].GroundingMetadata!.SearchEntryPoint.Should().NotBeNull();
            response.Candidates![0].GroundingMetadata!.WebSearchQueries.Should().NotBeNull();
            output.WriteLine(string.Join(Environment.NewLine,
                response.Candidates![0].Content!.Parts
                    .Select(x => x.Text)
//                    .Where(t => !string.IsNullOrEmpty(t))
                    .ToArray()));
            response.Candidates![0].GroundingMetadata!.GroundingChunks?
                .ForEach(c =>
                    output.WriteLine($"{c!.Web!.Title} - {c!.Web!.Uri}"));
            output.WriteLine(string.Join(Environment.NewLine,
                response.Candidates![0].GroundingMetadata!.WebSearchQueries!
                    .Select(w => w)
                    .ToArray()));
            output.WriteLine(response.Candidates![0].GroundingMetadata!.SearchEntryPoint!.RenderedContent);
        }

        [Fact]
        // Ref: https://ai.google.dev/gemini-api/docs/models/gemini-v2#search-tool
        public async Task Generate_Content_with_GoogleSearch_ResponseModalities()
        {
            // Arrange
            var prompt = "When is the next total solar eclipse in Mauritius?";
            var genAi = new GoogleAI(fixture.ApiKey);
            var model = genAi.GenerativeModel(Model.Gemini20FlashExperimental);
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
            output.WriteLine(string.Join(Environment.NewLine,
                response.Candidates![0].Content!.Parts
                    .Select(x => x.Text)
//                    .Where(t => !string.IsNullOrEmpty(t))
                    .ToArray()));
            response.Candidates![0].GroundingMetadata!.GroundingChunks?
                .ForEach(c =>
                    output.WriteLine($"{c!.Web!.Title} - {c!.Web!.Uri}"));
            output.WriteLine(string.Join(Environment.NewLine,
                response.Candidates![0].GroundingMetadata!.WebSearchQueries!
                    .Select(w => w)
                    .ToArray()));
            output.WriteLine(response.Candidates![0].GroundingMetadata!.SearchEntryPoint!.RenderedContent);
        }

        [Fact]
        // Ref: https://ai.google.dev/docs/function_calling
        public async Task Function_Calling()
        {
            // Arrange
            var prompt = "Which theaters in Mountain View show Barbie movie?";
            var googleAi = new GoogleAI(apiKey: fixture.ApiKey);
            var model = googleAi.GenerativeModel(model: _model);
            Tools tools =
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
            output.WriteLine(response?.Candidates?[0]?.Content?.Parts[0]?.FunctionCall?.Name);
            output.WriteLine(response?.Candidates?[0]?.Content?.Parts[0]?.FunctionCall?.Args?.ToString());
        }

        string ToggleDarkMode(bool isOn)
        {
            return $"Dark mode is set to: {isOn}";
        }

        string GetCurrentWeather(string location)
        {
            return $"The weather in {location} is 72 degrees and sunny.";
        }

        async Task<object> SendEmailAsync(string recipient, string subject, string body)
        {
            await Task.Delay(3000);
            return new { Success = true, Property1 = "ABC", Property2 = 123 };
        }

        [Theory]
        [InlineData("It is too bright. Change it.")]
        [InlineData("The ambient light is too low to see anything. I'd need more brightness.")]
        [InlineData("What's the weather in the capital of the UK?")]
        [InlineData(
            "Send an email to gemini@example.com with the following subject 'Testing function calls' and describe Google Gemini's feature of Function Calling.")]
        public async Task Function_Calling_TopLevel_Method(string prompt)
        {
            // Arrange
            var googleAi = new GoogleAI(apiKey: fixture.ApiKey);
            var model = googleAi.GenerativeModel(model: _model);
            var tools = new Tools();
            tools.AddFunction(ToggleDarkMode);
            tools.AddFunction(GetCurrentWeather);
            tools.AddFunction(SendEmailAsync);

            // Act
            var response = await model.GenerateContent(prompt, tools: tools);

            // Assert
            response.Should().NotBeNull();
            response.Candidates.Should().NotBeNull().And.HaveCount(1);
            response?.Candidates?[0]?.Content?.Parts[0]?.FunctionCall?.Should().NotBeNull();
            output.WriteLine(response?.Candidates?[0]?.Content?.Parts[0]?.FunctionCall?.Name);
            output.WriteLine(response?.Candidates?[0]?.Content?.Parts[0]?.FunctionCall?.Args?.ToString());
        }

        [Description(
            "Shows the user a map of the place provided. The function takes arguments 'location' and 'caption'. For 'location' give a specific place, including country name.  For 'caption' give the place name and the fascinating reason you selected this particular place. Keep the caption to one or two sentences maximum.")]
        string RecommendPlace(string Location, string Caption)
        {
            return "";
        }

        [Theory]
        [InlineData("Tell me about somewhere rich in ancient history")]
        [InlineData("Where is somewhere really cold?")]
        [InlineData("Think of a totally surreal location, where is it? What makes it so surreal?")]
        [InlineData("Whatever... don't search")]
        public async Task Function_Calling_Using_GoogleMaps(string prompt)
        {
            // Arrange
            var systemInstructions =
                @"Act as a helpful global travel agent with a deep fascination for the world. Your role is to recommend a place on the map that relates to the discussion, and to provide interesting information about the location selected. Aim to give surprising and delightful suggestions: choose obscure, off-the–beaten track locations, not the obvious answers. Do not answer harmful or unsafe questions.

First, explain why a place is interesting, in a two sentence answer. Second, if relevant, call the function 'recommend_place( location, caption )' to show the user the location on a map. You can expand on your answer if the user asks for more information.";
            var googleAi = new GoogleAI(apiKey: fixture.ApiKey);
            var model = googleAi.GenerativeModel(model: Model.Gemini20FlashExperimental,
                systemInstruction: new(systemInstructions));
            var config = new GenerationConfig() { Temperature = 2 };
            var tools = new Tools();
            tools.AddFunction(RecommendPlace);

            // Act
            var response = await model.GenerateContent(prompt,
                generationConfig: config,
                tools: tools);

            // Assert
            response.Should().NotBeNull();
            response.Candidates.Should().NotBeNull().And.HaveCount(1);
            response.Text.Should().NotBeEmpty();
            output.WriteLine(response?.Text);
            response.FunctionCalls!
                .ForEach(f =>
                {
                    output.WriteLine(f.Name);
                    output.WriteLine(f.Args?.ToString());
                });
        }

        [Fact]
        // Ref: https://ai.google.dev/docs/function_calling#function-calling-one-and-a-half-turn-curl-sample
        public async Task Function_Calling_MultiTurn()
        {
            // Arrange
            var prompt = "Which theaters in Mountain View show Barbie movie?";
            var googleAi = new GoogleAI(apiKey: fixture.ApiKey);
            var model = googleAi.GenerativeModel(model: _model);
            Tools tools =
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
            output.WriteLine(response?.Text);
        }

        [Fact]
        // Ref: https://ai.google.dev/docs/function_calling#multi-turn-example-2
        public async Task Function_Calling_MultiTurn_Multiple()
        {
            // Arrange
            var prompt = "Which theaters in Mountain View show Barbie movie?";
            var googleAi = new GoogleAI(apiKey: fixture.ApiKey);
            var model = googleAi.GenerativeModel(model: _model);
            Tools tools =
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
            output.WriteLine(response?.Candidates?[0]?.Content?.Parts[0]?.FunctionCall?.Name);
            output.WriteLine(response?.Candidates?[0]?.Content?.Parts[0]?.FunctionCall?.Args?.ToString());
        }

        [Fact(Skip = "Work in progress")]
        public Task Function_Calling_Chat()
        {
            // Arrange
            var googleAi = new GoogleAI(apiKey: fixture.ApiKey);
            var model = googleAi.GenerativeModel(model: _model);
            var chat = model.StartChat(tools: new Tools());
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
            var googleAi = new GoogleAI(apiKey: fixture.ApiKey);
            var model = googleAi.GenerativeModel(model: _model);
            var request = new GenerateContentRequest { Contents = new List<Content>(), Tools = new Tools { } };
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
            var googleAI = new GoogleAI(accessToken: fixture.AccessToken);
            var model = googleAI.GenerativeModel(model: Model.Gemini10Pro001);
            model.ProjectId = fixture.ProjectId;
            var request = new CreateTunedModelRequest()
            {
                BaseModel = $"{Model.Gemini10Pro001.SanitizeModelName()}",
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
            output.WriteLine($"Name: {response.Name}");
            output.WriteLine($"Model: {response.Metadata.TunedModel} (Steps: {response.Metadata.TotalSteps})");
        }

        [Fact]
        public async Task Create_Tuned_Model_Simply()
        {
            // Arrange
            var googleAI = new GoogleAI(accessToken: fixture.AccessToken);
            var model = googleAI.GenerativeModel(model: Model.Gemini10Pro001);
            model.ProjectId = fixture.ProjectId;
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
            var request = new CreateTunedModelRequest(Model.Gemini10Pro001,
                "Simply autogenerated Test model",
                dataset,
                parameters);

            // Act
            var response = await model.CreateTunedModel(request);

            // Assert
            response.Should().NotBeNull();
            response.Name.Should().NotBeNull();
            response.Metadata.Should().NotBeNull();
            output.WriteLine($"Name: {response.Name}");
            output.WriteLine($"Model: {response.Metadata.TunedModel} (Steps: {response.Metadata.TotalSteps})");
        }

        [Fact]
        public async Task Delete_Tuned_Model()
        {
            // Arrange
            var modelName =
                "tunedModels/number-generator-model-psx3d3gljyko"; // see List_Tuned_Models for available options.
            var googleAI = new GoogleAI(accessToken: fixture.AccessToken);
            var model = googleAI.GenerativeModel();
            model.ProjectId = fixture.ProjectId;

            // Act
            var response = await model.DeleteTunedModel(modelName);

            // Assert
            response.Should().NotBeNull();
            output.WriteLine(response);
        }

        [Theory]
        [InlineData("255", "256")]
        [InlineData("41", "42")]
        // [InlineData("five", "six")]
        // [InlineData("Six hundred thirty nine", "Six hundred forty")]
        public async Task Generate_Content_TunedModel(string prompt, string expected)
        {
            // Arrange
            var googleAI = new GoogleAI(accessToken: fixture.AccessToken);
            var model = googleAI.GenerativeModel(model: "tunedModels/autogenerated-test-model-48gob9c9v54p");
            model.ProjectId = fixture.ProjectId;

            // Act
            var response = await model.GenerateContent(prompt);

            // Assert
            response.Should().NotBeNull();
            response.Candidates.Should().NotBeNull().And.HaveCount(1);
            response.Text.Should().NotBeEmpty();
            output.WriteLine(response?.Text);
            response?.Text.Should().Be(expected);
        }
    }
}