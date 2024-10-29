#if NET472_OR_GREATER || NETSTANDARD2_0
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
#endif
using FluentAssertions;
using Mscc.GenerativeAI;
using Xunit;
using Xunit.Abstractions;

namespace Test.Mscc.GenerativeAI
{
    [Collection(nameof(ConfigurationFixture))]
    public class GoogleAi_Caching_Should
    {
        private readonly ITestOutputHelper _output;
        private readonly ConfigurationFixture _fixture;
        private readonly string _model = Model.Gemini15Flash001;
        private readonly IGenerativeAI _genAi;
        private readonly CachedContentModel _cachedContent;

        public GoogleAi_Caching_Should(ITestOutputHelper output, ConfigurationFixture fixture)
        {
            // AppContext.SetSwitch("System.Net.SocketsHttpHandler.Http3Support", false);
            
            _output = output;
            _fixture = fixture;
            _genAi = new GoogleAI(_fixture.ApiKey);
            _cachedContent = ((GoogleAI)_genAi).CachedContent();
        }

        [Fact]
        public async Task Create_ShouldThrowArgumentNullException_WhenRequestIsNull()
        {
            // Act
            Func<Task> action = async () => await _cachedContent.Create(request: null);

            // Assert
            await action.Should().ThrowAsync<ArgumentNullException>().WithParameterName("request");
        }
        
        [Fact]
        public async Task Initialize_Caching_with_Exception()
        {
            // Act
            Func<Task> sut = async () => await _cachedContent.Create(_model);
            
            // Assert
            await sut.Should().ThrowAsync<HttpRequestException>();
        }

        public async Task Initialize_Caching()
        {
            
        }

        [Theory]
        [InlineData("Sherlock_Jr_FullMovie.mp4", "sherlock jr movie")]
        public async Task Initialize_Model_from_Caching(string filename, string displayName)
        {
            // Arrange
            var filePath = Path.Combine(Environment.CurrentDirectory, "payload", filename);
            var response = await _genAi.GenerativeModel().UploadFile(filePath);
            var file = response.File;
            while (file.State == StateFileResource.Processing)
            {
                _output.WriteLine("Waiting for video to be processed");
                Thread.Sleep(200);
                file = await _genAi.GenerativeModel().GetFile(file.Name);
            }
            var contents = new List<Content>();
            var content = new Content();
            content.Parts.Add(new FileData { FileUri = file.Uri, MimeType = file.MimeType });
            var cache = await _cachedContent.Create(_model,
                displayName: displayName,
                systemInstruction: new Content("You are an expert video analyzer, and your job is to answer the user's query based on the video file you have access to."),
                contents: contents,
                ttl:TimeSpan.FromMinutes(5));

            // Act
            var model = _genAi.GenerativeModel(cache);
            
            // Assert
        }

        [Fact]
        public async Task List_CachedContents()
        {
            // Act
            var result = await _cachedContent.List();
        }

        [Theory]
        [InlineData("cachedContents/test-content")]
        public async Task Get_CachedContent(string cachedContentName)
        {
            // Arrange

            // Act
            var result = await _cachedContent.Get(cachedContentName);

            // Assert
            result.Should().NotBeNull();
            result.Name.Should().Be(cachedContentName);
        }

        [Fact]
        public async Task Delete_CachedContent()
        {
            // Arrange
            var cachedContents = await _cachedContent.List();
            var name = cachedContents.FirstOrDefault()?.Name;
            
            // Act
            var response = await _cachedContent.Delete(name);
            
            // Assert
            response.Should().NotBeNull();
            _output.WriteLine(response);
        }
        
        [Fact]
        public async Task Delete_ShouldThrowArgumentException_WhenCachedContentNameIsNull()
        {
            // Act
            Func<Task> action = async () => await _cachedContent.Delete(null);

            // Assert
            await action.Should().ThrowAsync<ArgumentException>().WithParameterName("cachedContentName");
        }

        [Fact]
        public async Task Create_Basic()
        {
            // Arrange
            var filePath = Path.Combine(Environment.CurrentDirectory, "payload", "a11.txt");
            var document = await ((GoogleAI)_genAi).UploadFile(filePath);
            var contents = new List<Content>
            {
                new(
                    new FileData { FileUri = document.Uri, MimeType = document.MimeType })
            };
            var cache = await _cachedContent.Create(
                _model,
                systemInstruction: new Content("You are an expert analyzing transcripts."),
                contents: contents);

            // Act
            var model = _genAi.GenerativeModel(cache);
            var response = await model.GenerateContent("Please summarize this transcript");
            
            // Assert
            cache.Should().NotBeNull();
            model.Should().NotBeNull();
            response.Should().NotBeNull();
            response.Candidates.Should().NotBeNull().And.HaveCount(1);
            response.Text.Should().NotBeNull();
            _output.WriteLine($"{response?.Text}");
        }

        [Fact]
        public async Task Create_From_Name()
        {
            // Arrange
            var filePath = Path.Combine(Environment.CurrentDirectory, "payload", "a11.txt");
            var document = await ((GoogleAI)_genAi).UploadFile(filePath);
            var contents = new List<Content>
            {
                new (
                    new FileData { FileUri = document.Uri, MimeType = document.MimeType })
            };
            var cache = await _cachedContent.Create(
                _model,
                systemInstruction: new Content("You are an expert analyzing transcripts."),
                contents: contents);
            var name = cache.Name;

            // Act
            cache = await _cachedContent.Get(name);
            var apollo_model = _genAi.GenerativeModel(cache);
            var response = await apollo_model.GenerateContent("Find a lighthearted moment from this transcript");
            
            // Assert
            cache.Should().NotBeNull();
            apollo_model.Should().NotBeNull();
            response.Should().NotBeNull();
            response.Candidates.Should().NotBeNull().And.HaveCount(1);
            response.Text.Should().NotBeNull();
            _output.WriteLine($"{response?.Text}");
        }        

        [Fact]
        public async Task Create_From_Chat()
        {
            // Arrange
            var systemInstruction = new Content("You are an expert analyzing transcripts.");
            var model = _genAi.GenerativeModel(_model, systemInstruction: systemInstruction);
            var chat = model.StartChat();
            var filePath = Path.Combine(Environment.CurrentDirectory, "payload", "a11.txt");
            var document = await ((GoogleAI)_genAi).UploadFile(filePath);
            List<Part> parts =
            [
                new() { Text = "Hi, could you summarize this transcript?" },
                new( new FileData { FileUri = document.Uri, MimeType = document.MimeType })
            ];

            var response = await chat.SendMessage(parts);
            _output.WriteLine($"model: {response?.Text}");
            response = await chat.SendMessage("Okay, could you tell me more about the trans-lunar injection");
            _output.WriteLine($"model: {response?.Text}");
            
            var cache = await _cachedContent.Create(
                _model,
                systemInstruction: systemInstruction,
                history: chat.History);

            // Act
            model = _genAi.GenerativeModel(cache);
            chat = model.StartChat();
            response = await chat.SendMessage(
                "I didn't understand that last part, could you explain it in simpler language?");
            
            // Assert
            cache.Should().NotBeNull();
            model.Should().NotBeNull();
            response.Should().NotBeNull();
            response.Candidates.Should().NotBeNull().And.HaveCount(1);
            response.Text.Should().NotBeNull();
            _output.WriteLine($"model: {response?.Text}");
        }        
    }
}