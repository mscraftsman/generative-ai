using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Shouldly;
using Mscc.GenerativeAI;
using Xunit;
using Xunit.Abstractions;

namespace Test.Mscc.GenerativeAI
{
    [Collection(nameof(ConfigurationFixture))]
    public class GoogleAiCachingShould
    {
        private readonly ITestOutputHelper output;
        private readonly ConfigurationFixture fixture;
        private readonly string _model = Model.Gemini20Flash;
        private readonly GoogleAI _genAi;
        private readonly CachedContentModel _cachedContent;

        public GoogleAiCachingShould(ITestOutputHelper output, ConfigurationFixture fixture)
        {
            this.output = output;
            this.fixture = fixture;
            _genAi = new GoogleAI(apiKey: fixture.ApiKey);
            _cachedContent = _genAi.CachedContent();
        }
        [Fact]
        public async Task Create_ShouldThrowArgumentNullException_WhenRequestIsNull()
        {
            // Act
            Func<Task> action = async () => await _cachedContent.Create(request: null);

            // Assert
            var exception = await action.ShouldThrowAsync<ArgumentNullException>();
            exception.ParamName.ShouldBe("request");
        }
        
        [Fact]
        public async Task Initialize_Caching_with_Exception()
        {
            // Act
            Func<Task> action = async () => await _cachedContent.Create(_model);
            
            // Assert
            await action.ShouldThrowAsync<HttpRequestException>();
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
                output.WriteLine("Waiting for video to be processed");
                Thread.Sleep(200);
                file = await _genAi.GenerativeModel().GetFile(file.Name);
            }
            // Waiting for video to be processed
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
            // Arrange
            
            // Act
            var result = await _cachedContent.List();

            // Assert
            result.ShouldNotBeNull();
            foreach (CachedContent item in result)
            {
                output.WriteLine($"{item.Name} ({item.Expiration})");
            }
        }

        [Theory]
        [InlineData("cachedContents/test-content")]
        public async Task Get_CachedContent(string cachedContentName)
        {
            // Arrange

            // Act
            var result = await _cachedContent.Get(cachedContentName);

            // Assert
            result.ShouldNotBeNull();
            result.Name.ShouldBe(cachedContentName);
        }

        [Fact]
        public async Task Delete_CachedContent()
        {
            // Arrange
            var cachedContents = await _cachedContent.List();
            var name = cachedContents.FirstOrDefault()?.Name;
            output.WriteLine(name);
            
            // Act
            var response = await _cachedContent.Delete(name);
            
            // Assert
            response.ShouldNotBeNull();
            output.WriteLine(response);
        }
        
        [Fact]
        public async Task Delete_ShouldThrowArgumentException_WhenCachedContentNameIsNull()
        {
            // Arrange

            // Act
            Func<Task> action = async () => await _cachedContent.Delete(null);

            // Assert
            var exception = await action.ShouldThrowAsync<ArgumentException>();
            exception.ParamName.ShouldBe("cachedContentName");
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
            cache.ShouldNotBeNull();
            model.ShouldNotBeNull();
            response.ShouldNotBeNull();
            response.Candidates.ShouldNotBeNull();
            response.Candidates.Count.ShouldBe(1);
            response.Text.ShouldNotBeNull();
            output.WriteLine($"{response.Text}");
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
            cache.ShouldNotBeNull();
            apollo_model.ShouldNotBeNull();
            response.ShouldNotBeNull();
            response.Candidates.ShouldNotBeNull();
            response.Candidates.Count.ShouldBe(1);
            response.Text.ShouldNotBeNull();
            output.WriteLine($"{response.Text}");
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
            output.WriteLine($"model: {response.Text}");
            response = await chat.SendMessage("Okay, could you tell me more about the trans-lunar injection");
            output.WriteLine($"model: {response.Text}");
            
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
            cache.ShouldNotBeNull();
            model.ShouldNotBeNull();
            response.ShouldNotBeNull();
            response.Candidates.ShouldNotBeNull();
            response.Candidates.Count.ShouldBe(1);
            response.Text.ShouldNotBeNull();
            output.WriteLine($"model: {response.Text}");
        }        
    }
}