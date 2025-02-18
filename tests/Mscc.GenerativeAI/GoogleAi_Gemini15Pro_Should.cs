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
using Mscc.GenerativeAI;
using System.Dynamic;
using System.Runtime.Serialization;
using Xunit;
using Xunit.Abstractions;

namespace Test.Mscc.GenerativeAI
{
    [Collection(nameof(ConfigurationFixture))]
    public class GoogleAiGemini15ProShould(ITestOutputHelper output, ConfigurationFixture fixture)
    {
        private readonly string _model = Model.Gemini20FlashExperimental;

        [Fact]
        public void Initialize_Gemini15Pro()
        {
            // Arrange

            // Act
            var googleAi = new GoogleAI(apiKey: fixture.ApiKey);
            var model = googleAi.GenerativeModel(model: _model);

            // Assert
            model.Should().NotBeNull();
            model.Name.Should().Be(Model.Gemini15ProLatest.SanitizeModelName());
        }

        [Fact]
        public async Task GenerateContent_WithInvalidAPIKey_ChangingBeforeRequest()
        {
            // Arrange
            var prompt = "Tell me 4 things about Taipei. Be short.";
            var googleAi = new GoogleAI(apiKey: "WRONG_API_KEY");
            var model = googleAi.GenerativeModel(model: _model);
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
            var model = googleAi.GenerativeModel(model: _model);
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
        public async Task GenerateContent_Using_JsonMode()
        {
            // Arrange
            var prompt = "List a few popular cookie recipes.";
            var googleAi = new GoogleAI(apiKey: fixture.ApiKey);
            var model = googleAi.GenerativeModel(model: _model);
            model.UseJsonMode = true;

            // Act
            var response = await model.GenerateContent(prompt);

            // Assert
            response.Should().NotBeNull();
            response.Candidates.Should().NotBeNull().And.HaveCount(1);
            response.Text.Should().NotBeEmpty();
            output.WriteLine(response?.Text);
        }

        [Fact]
        public async Task GenerateContent_Using_JsonMode_SchemaPrompt()
        {
            // Arrange
            var prompt =
                "List a few popular cookie recipes using this JSON schema: {'type': 'object', 'properties': { 'recipe_name': {'type': 'string'}}}";
            var googleAi = new GoogleAI(apiKey: fixture.ApiKey);
            var model = googleAi.GenerativeModel(model: _model);
            model.UseJsonMode = true;

            // Act
            var response = await model.GenerateContent(prompt);

            // Assert
            response.Should().NotBeNull();
            response.Candidates.Should().NotBeNull().And.HaveCount(1);
            response.Text.Should().NotBeEmpty();
            output.WriteLine(response?.Text);
        }

        class Recipe
        {
            public string Name { get; set; }
#if NET9_0
            public required string RecipeName { get; set; }
#endif
            public List<Ingredient> Ingredients { get; set; }
        }

        class Ingredient
        {
            public string Name { get; set; }
            public int Quantity { get; set; }
            public string Unit { get; set; }
        }

        [Fact]
        public async Task GenerateContent_Using_ResponseSchema_with_List()
        {
            // Arrange
            var prompt = "List a few popular cookie recipes.";
            var googleAi = new GoogleAI(apiKey: fixture.ApiKey);
            var model = googleAi.GenerativeModel(model: _model);
            var generationConfig = new GenerationConfig()
            {
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
            output.WriteLine(response?.Text);
        }

        class FlightSchedule
        {
            public string Time { get; set; }
            public string Destination { get; set; }
        }

        [Fact]
        public async Task GenerateContent_Using_ResponseSchema_with_List_From_InlineData()
        {
            // Arrange
            var prompt = "Parse the time and city from the airport board shown in this image into a list.";
            var googleAi = new GoogleAI(apiKey: fixture.ApiKey);
            var model = googleAi.GenerativeModel(model: _model);
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
            output.WriteLine(response?.Text);
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
        public async Task GenerateContent_Using_ResponseSchema_with_Enumeration(string prompt)
        {
            // Arrange
            var googleAi = new GoogleAI(apiKey: fixture.ApiKey);
            var model = googleAi.GenerativeModel(model: _model);

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
            output.WriteLine($"Response: {response.Text}");

            // Parse the enum (more robust error handling)
            if (Enum.TryParse(response.Text, out Instrument instrument))
            {
                output.WriteLine($"Parsed Instrument: {instrument}");
            }
            else
            {
                output.WriteLine($"Could not parse '{response.Text}' as a valid Instrument enum.");
            }
        }

        [Fact]
        public async Task GenerateContent_Using_ResponseSchema_with_Anonymous()
        {
            // Arrange
            var prompt = "List a few popular cookie recipes.";
            var googleAi = new GoogleAI(apiKey: fixture.ApiKey);
            var model = googleAi.GenerativeModel(model: _model);
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
            output.WriteLine(response?.Text);
        }

        [Fact(Skip = "ReadOnly declaration not accepted.")]
        public async Task GenerateContent_Using_ResponseSchema_with_Dynamic()
        {
            // Arrange
            var prompt = "List a few popular cookie recipes.";
            var googleAi = new GoogleAI(apiKey: fixture.ApiKey);
            var model = googleAi.GenerativeModel(model: _model);
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
            output.WriteLine(response?.Text);
        }
        
        [Fact]
        public async Task Generate_Text_From_Image()
        {
            // Arrange
            var googleAi = new GoogleAI(apiKey: fixture.ApiKey);
            var model = googleAi.GenerativeModel(model: _model);
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
            output.WriteLine(response?.Text);
        }

        [Fact]
        public async Task Describe_Image_From_InlineData()
        {
            // Arrange
            var prompt = "Parse the time and city from the airport board shown in this image into a list, in Markdown";
            var googleAi = new GoogleAI(apiKey: fixture.ApiKey);
            var model = googleAi.GenerativeModel(model: _model);
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
            output.WriteLine(response?.Text);
        }

        [Theory]
        [InlineData("scones.jpg", "image/jpeg", "What is this picture?", "blueberries")]
        [InlineData("cat.jpg", "image/jpeg", "Describe this image", "snow")]
        [InlineData("cat.jpg", "image/jpeg", "Is it a cat?", "Yes")]
        //[InlineData("animals.mp4", "video/mp4", "What's in the video?", "Zootopia")]
        public async Task Generate_Text_From_ImageFile(string filename, string mimetype, string prompt, string expected)
        {
            // Arrange
            var googleAi = new GoogleAI(apiKey: fixture.ApiKey);
            var model = googleAi.GenerativeModel(model: _model);
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
            output.WriteLine(response?.Text);
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
            var googleAi = new GoogleAI(apiKey: fixture.ApiKey);
            var model = googleAi.GenerativeModel(model: _model);
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
            output.WriteLine(response?.Text);
        }

        [Theory]
        [InlineData(
            "https://raw.githubusercontent.com/mscraftsman/generative-ai/refs/heads/main/tests/Mscc.GenerativeAI/payload/timetable.png",
            "Parse the time and city from the airport board shown in this image into a list, in Markdown")]
        [InlineData("http://groups.di.unipi.it/~occhiuto/sintassi.pdf", "Generate 5 exercise for a student")]
        public async Task Describe_AddMedia_From_Url(string uri, string prompt)
        {
            // Arrange
            var googleAi = new GoogleAI(apiKey: fixture.ApiKey, apiVersion: ApiVersion.V1Alpha);
            var model = googleAi.GenerativeModel(model: _model);
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
            output.WriteLine(response?.Text);
        }

        [Fact(Skip = "Bad Request due to FileData part")]
        public async Task Describe_AddMedia_From_UrlRemote()
        {
            // Arrange
            var prompt = "Parse the time and city from the airport board shown in this image into a list, in Markdown";
            var googleAi = new GoogleAI(apiKey: fixture.ApiKey);
            var model = googleAi.GenerativeModel(model: _model);
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
            output.WriteLine(response?.Text);
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
            IGenerativeAI genAi = new GoogleAI(fixture.ApiKey);
            var model = genAi.GenerativeModel(_model);

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
            output.WriteLine($"Uploaded file '{response?.File.DisplayName}' as: {response?.File.Uri}");
        }

        [Fact]
        public async Task Upload_File_TooLarge_ThrowsMaxUploadFileSizeException()
        {
            // Arrange
            IGenerativeAI genAi = new GoogleAI(fixture.ApiKey);
            var model = genAi.GenerativeModel(_model);
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
            IGenerativeAI genAi = new GoogleAI(fixture.ApiKey);
            var model = genAi.GenerativeModel(_model);
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
            output.WriteLine($"Uploaded file '{response?.File.DisplayName}' as: {response?.File.Uri}");

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
            IGenerativeAI genAi = new GoogleAI(fixture.ApiKey);
            var model = genAi.GenerativeModel(_model);

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
                output.WriteLine($"Uploaded file '{response?.File.DisplayName}' as: {response?.File.Uri}");
            }
        }

        [Fact]
        public async Task List_Files()
        {
            // Arrange
            IGenerativeAI genAi = new GoogleAI(fixture.ApiKey);
            var model = genAi.GenerativeModel(_model);

            // Act
            var sut = await ((GoogleAI)genAi).ListFiles();

            // Assert
            sut.Should().NotBeNull();
            sut.Files.Should().NotBeNull().And.HaveCountGreaterThanOrEqualTo(1);
            sut.Files.ForEach(x =>
            {
                output.WriteLine($"Display Name: {x.DisplayName} ({Enum.GetName(typeof(StateFileResource), x.State)})");
                output.WriteLine(
                    $"File: {x.Name} (MimeType: {x.MimeType}, Size: {x.SizeBytes} bytes, Created: {x.CreateTime} UTC, Updated: {x.UpdateTime} UTC)");
                output.WriteLine($"Uri: {x.Uri}");
            });
        }

        [Fact]
        public async Task Get_File()
        {
            // Arrange
            IGenerativeAI genAi = new GoogleAI(fixture.ApiKey);
            var model = genAi.GenerativeModel(_model);
            var files = await ((GoogleAI)genAi).ListFiles();
            var fileName = files.Files.FirstOrDefault().Name;

            // Act
            var sut = await model.GetFile(fileName);

            // Assert
            sut.Should().NotBeNull();
            output.WriteLine($"Retrieved file '{sut.DisplayName}'");
            output.WriteLine(
                $"File: {sut.Name} (MimeType: {sut.MimeType}, Size: {sut.SizeBytes} bytes, Created: {sut.CreateTime} UTC, Updated: {sut.UpdateTime} UTC)");
            output.WriteLine(($"Uri: {sut.Uri}"));
        }

        [Fact]
        public async Task Delete_File()
        {
            // Arrange
            IGenerativeAI genAi = new GoogleAI(fixture.ApiKey);
            var model = genAi.GenerativeModel(_model);
            var files = await ((GoogleAI)genAi).ListFiles();
            var fileName = files.Files.FirstOrDefault().Name;
            output.WriteLine($"File: {fileName}");

            // Act
            var response = await model.DeleteFile(fileName);

            // Assert
            response.Should().NotBeNull();
            output.WriteLine(response);
        }

        [Fact]
        public async Task Describe_Single_Media_From_FileAPI()
        {
            // Arrange
            var prompt = "Describe the image with a creative description";
            IGenerativeAI genAi = new GoogleAI(fixture.ApiKey);
            var model = genAi.GenerativeModel(_model);
            var request = new GenerateContentRequest(prompt);
            var files = await ((GoogleAI)genAi).ListFiles();
            var file = files.Files.Where(x => x.MimeType.StartsWith("image/")).FirstOrDefault();
            output.WriteLine($"File: {file.Name}\tName: '{file.DisplayName}'");
            request.AddMedia(file);

            // Act
            var response = await model.GenerateContent(request);

            // Assert
            response.Should().NotBeNull();
            response.Candidates.Should().NotBeNull().And.HaveCount(1);
            response.Candidates.FirstOrDefault().Content.Should().NotBeNull();
            response.Candidates.FirstOrDefault().Content.Parts.Should().NotBeNull().And
                .HaveCountGreaterThanOrEqualTo(1);
            output.WriteLine(response?.Text);
        }

        [Fact]
        public async Task Describe_Images_From_FileAPI()
        {
            // Arrange
            var prompt = "Make a short story from the media resources. The media resources are:";
            IGenerativeAI genAi = new GoogleAI(fixture.ApiKey);
            var model = genAi.GenerativeModel(_model);
            var request = new GenerateContentRequest(prompt);
            var files = await ((GoogleAI)genAi).ListFiles();
            foreach (var file in files.Files.Where(x => x.MimeType.StartsWith("image/")))
            {
                output.WriteLine($"File: {file.Name}\tName: '{file.DisplayName}'");
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
            output.WriteLine(response?.Text);
        }

        [Fact]
        public async Task Describe_Audio_From_FileAPI()
        {
            // Arrange
            var prompt = "Listen carefully to the following audio file. Provide a brief summary.";
            IGenerativeAI genAi = new GoogleAI(fixture.ApiKey);
            var model = genAi.GenerativeModel(_model);
            var request = new GenerateContentRequest(prompt);
            var files = await ((GoogleAI)genAi).ListFiles();
            foreach (var file in files.Files.Where(x => x.MimeType.StartsWith("audio/")))
            {
                output.WriteLine($"File: {file.Name}\tName: '{file.DisplayName}'");
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
            output.WriteLine(response?.Text);
        }

        [Theory]
        [InlineData(Model.Gemini20Flash)]
        [InlineData(Model.Gemini20FlashLite)]
        [InlineData(Model.Gemini20FlashThinking)]
        [InlineData(Model.Gemini20Pro)]
        public async Task Describe_Audio_with_Timestamps(string modelName)
        {
            // Arrange
            var prompt =
                @"Transcribe this audio into english texts. Break the text into small logical segments. Include punctuation where appropriate. Timestamps should have milli-second level accuracy.

Output the segments in the SRT format:

subtitle_id
start_time → end_time
content";
            IGenerativeAI genAi = new GoogleAI(fixture.ApiKey);
            var model = genAi.GenerativeModel(modelName);
            var request = new GenerateContentRequest(prompt);
            var filePath = Path.Combine(Environment.CurrentDirectory, "payload", "out.mp3");
            var file = await ((GoogleAI)genAi).UploadFile(filePath);
            output.WriteLine($"File: {file.File.Name}\tName: '{file.File.DisplayName}'");
            request.AddMedia(file.File);

            // Act
            var response = await model.GenerateContent(request);

            // Assert
            response.Should().NotBeNull();
            response.Candidates.Should().NotBeNull().And.HaveCount(1);
            response.Candidates.FirstOrDefault().Content.Should().NotBeNull();
            response.Candidates.FirstOrDefault().Content.Parts.Should().NotBeNull().And
                .HaveCountGreaterThanOrEqualTo(1);
            output.WriteLine(response?.Text);
        }

        [Fact]
        public async Task Summarize_Audio_From_FileAPI()
        {
            // Arrange
            var prompt = @"Please provide a summary for the audio.
Provide chapter titles with timestamps, be concise and short, no need to provide chapter summaries.
Do not make up any information that is not part of the audio and do not be verbose.
";
            IGenerativeAI genAi = new GoogleAI(fixture.ApiKey);
            var model = genAi.GenerativeModel(_model);
            var request = new GenerateContentRequest(prompt);
            var files = await ((GoogleAI)genAi).ListFiles();
            var file = files.Files.Where(x => x.MimeType.StartsWith("audio/")).FirstOrDefault();
            output.WriteLine($"File: {file.Name}\tName: '{file.DisplayName}'");
            request.AddMedia(file);

            // Act
            var response = await model.GenerateContent(request);

            // Assert
            response.Should().NotBeNull();
            response.Candidates.Should().NotBeNull().And.HaveCount(1);
            response.Candidates.FirstOrDefault().Content.Should().NotBeNull();
            response.Candidates.FirstOrDefault().Content.Parts.Should().NotBeNull().And
                .HaveCountGreaterThanOrEqualTo(1);
            output.WriteLine(response?.Text);
        }

        [Fact]
        public async Task Analyze_Document_PDF_From_FileAPI()
        {
            // Arrange
            var prompt =
                @"Your are a very professional document summarization specialist. Please summarize the given document.";
            IGenerativeAI genAi = new GoogleAI(fixture.ApiKey);
            var model = genAi.GenerativeModel(_model);
            var request = new GenerateContentRequest(prompt);
            var files = await ((GoogleAI)genAi).ListFiles();
            var file = files.Files.Where(x => x.MimeType.StartsWith("application/pdf")).FirstOrDefault();
            output.WriteLine($"File: {file.Name}\tName: '{file.DisplayName}'");
            request.AddMedia(file);

            // Act
            var response = await model.GenerateContent(request);

            // Assert
            response.Should().NotBeNull();
            response.Candidates.Should().NotBeNull().And.HaveCount(1);
            response.Candidates.FirstOrDefault().Content.Should().NotBeNull();
            response.Candidates.FirstOrDefault().Content.Parts.Should().NotBeNull().And
                .HaveCountGreaterThanOrEqualTo(1);
            output.WriteLine(response?.Text);
        }

        [Theory]
        [InlineData("application/rtf")]
        [InlineData("text/rtf")]
        public async Task Analyze_Document_RTF_From_FileAPI(string mimetype)
        {
            // Arrange
            var prompt =
                @"Your are a very professional document summarization specialist. Please summarize the given document.";
            IGenerativeAI genAi = new GoogleAI(fixture.ApiKey);
            var model = genAi.GenerativeModel(_model);
            var request = new GenerateContentRequest(prompt);
            var files = await ((GoogleAI)genAi).ListFiles();
            var file = files.Files.Where(x => x.MimeType.StartsWith(mimetype)).FirstOrDefault();
            output.WriteLine($"File: {file.Name}\tName: '{file.DisplayName}'");
            request.AddMedia(file);

            // Act
            var response = await model.GenerateContent(request);

            // Assert
            response.Should().NotBeNull();
            response.Candidates.Should().NotBeNull().And.HaveCount(1);
            response.Candidates.FirstOrDefault().Content.Should().NotBeNull();
            response.Candidates.FirstOrDefault().Content.Parts.Should().NotBeNull().And
                .HaveCountGreaterThanOrEqualTo(1);
            output.WriteLine(response?.Text);
        }

        [Fact]
        public async Task TranscribeStream_Audio_From_FileAPI()
        {
            // Arrange
            var prompt = @"Can you transcribe this interview, in the format of timecode, speaker, caption.
Use speaker A, speaker B, etc. to identify the speakers.
";
            IGenerativeAI genAi = new GoogleAI(fixture.ApiKey);
            var model = genAi.GenerativeModel(_model);
            var request = new GenerateContentRequest(prompt);
            var files = await ((GoogleAI)genAi).ListFiles();
            var file = files.Files.Where(x => x.MimeType.StartsWith("audio/")).FirstOrDefault();
            output.WriteLine($"File: {file.Name}\tName: '{file.DisplayName}'");
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
                output.WriteLine(response?.Text);
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
            IGenerativeAI genAi = new GoogleAI(fixture.ApiKey);
            var model = genAi.GenerativeModel(_model);
            model.UseServerSentEventsFormat = true;
            var request = new GenerateContentRequest(prompt);
            var files = await ((GoogleAI)genAi).ListFiles();
            var file = files.Files.Where(x => x.MimeType.StartsWith("audio/")).FirstOrDefault();
            output.WriteLine($"File: {file.Name}\tName: '{file.DisplayName}'");
            request.AddMedia(file);

            // Act
            var responseStream = model.GenerateContentStream(request);

            // Assert
            responseStream.Should().NotBeNull();
            await foreach (var response in responseStream)
            {
                response.Should().NotBeNull();
                response.Candidates.Should().NotBeNull().And.HaveCount(1);
                output.WriteLine(response?.Text);
            }
        }

        [Fact]
        public async Task Describe_Videos_From_FileAPI()
        {
            // Arrange
            var prompt = "Describe this video clip.";
            IGenerativeAI genAi = new GoogleAI(fixture.ApiKey);
            var model = genAi.GenerativeModel(_model);
            var request = new GenerateContentRequest(prompt);
            var files = await ((GoogleAI)genAi).ListFiles();
            foreach (var file in files.Files.Where(x => x.MimeType.StartsWith("video/")))
            {
                output.WriteLine($"File: {file.Name}\tName: '{file.DisplayName}'");
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
            output.WriteLine(response?.Text);
        }

        [Fact]
        public async Task Make_Story_using_Videos_From_FileAPI()
        {
            // Arrange
            var prompt = "Make a short story from the media resources. The media resources are:";
            IGenerativeAI genAi = new GoogleAI(fixture.ApiKey);
            var model = genAi.GenerativeModel(_model);
            var request = new GenerateContentRequest(prompt);
            var files = await ((GoogleAI)genAi).ListFiles();
            foreach (var file in files.Files.Where(x => x.MimeType.StartsWith("video/")))
            {
                output.WriteLine($"File: {file.Name}\tName: '{file.DisplayName}'");
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
            output.WriteLine(response?.Text);
        }

        [Fact(Skip = "Bad Request due to FileData part")]
        public async Task Describe_Image_From_StorageBucket()
        {
            // Arrange
            var prompt = "Describe the image with a creative description";
            var googleAi = new GoogleAI(apiKey: fixture.ApiKey);
            var model = googleAi.GenerativeModel(model: _model);
            var generationConfig = new GenerationConfig
            {
                Temperature = 0.4f, TopP = 1, TopK = 32, MaxOutputTokens = 2048
            };
            // var request = new GenerateContentRequest(prompt, generationConfig);
            var request = new GenerateContentRequest(prompt);
            request.Contents[0].Parts.Add(new FileData
            {
                FileUri = "gs://generativeai-downloads/images/scones.jpg", MimeType = "image/jpeg"
            });

            // Act
            var response = await model.GenerateContent(request);

            // Assert
            response.Should().NotBeNull();
            response.Candidates.Should().NotBeNull().And.HaveCount(1);
            response.Candidates.FirstOrDefault().Content.Should().NotBeNull();
            response.Candidates.FirstOrDefault().Content.Parts.Should().NotBeNull().And
                .HaveCountGreaterThanOrEqualTo(1);
            output.WriteLine(response?.Text);
        }

        [Fact(Skip = "Bad Request due to FileData part")]
        public async Task Describe_Image_From_URL()
        {
            // Arrange
            var prompt = "Parse the time and city from the airport board shown in this image into a list, in Markdown";
            var googleAi = new GoogleAI(apiKey: fixture.ApiKey);
            var model = googleAi.GenerativeModel(model: _model);
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
            output.WriteLine(response?.Text);
        }

        [Fact]
        public async Task Generate_Content_SystemInstruction()
        {
            // Arrange
            var systemInstruction = new Content("You are a friendly pirate. Speak like one.");
            var prompt = "Good morning! How are you?";
            IGenerativeAI genAi = new GoogleAI(fixture.ApiKey);
            var model = genAi.GenerativeModel(_model, systemInstruction: systemInstruction);
            var request = new GenerateContentRequest(prompt);

            // Act
            var response = await model.GenerateContent(request);

            // Assert
            response.Should().NotBeNull();
            response.Candidates.Should().NotBeNull().And.HaveCount(1);
            response.Text.Should().NotBeNull();
            output.WriteLine($"{response?.Text}");
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
            IGenerativeAI genAi = new GoogleAI(fixture.ApiKey);
            var model = genAi.GenerativeModel(_model, systemInstruction: systemInstruction);
            var request = new GenerateContentRequest(prompt, generationConfig, safetySettings);

            // Act
            var response = await model.GenerateContent(request);

            // Assert
            response.Should().NotBeNull();
            response.Candidates.Should().NotBeNull().And.HaveCount(1);
            response.Text.Should().NotBeNull();
            output.WriteLine($"{prompt} {response?.Text}");
        }

        [Fact(Skip = "The 'gs' scheme is not supported.")]
        public async Task Multimodal_Video_Input()
        {
            // Arrange
            var googleAi = new GoogleAI(apiKey: fixture.ApiKey);
            var model = googleAi.GenerativeModel(model: _model);
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
            output.WriteLine(response?.Text);
        }

        [Theory]
        [InlineData(78330)]
        public async Task Count_Tokens_Audio(int expected)
        {
            // Arrange
            IGenerativeAI genAi = new GoogleAI(fixture.ApiKey);
            var model = genAi.GenerativeModel(_model);
            var request = new GenerateContentRequest { Contents = new List<Content>() };
            var files = await ((GoogleAI)genAi).ListFiles();
            foreach (var file in files.Files.Where(x => x.MimeType.StartsWith("audio/")))
            {
                output.WriteLine($"File: {file.Name}");
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
            output.WriteLine($"Tokens: {response?.TotalTokens}");
        }

        [Fact]
        public async Task Count_Tokens_Audio_FileApi()
        {
            // Arrange
            IGenerativeAI genAi = new GoogleAI(fixture.ApiKey);
            var model = genAi.GenerativeModel(_model);
            var request = new GenerateContentRequest { Contents = new List<Content>() };
            var files = await ((GoogleAI)genAi).ListFiles();
            var file = files.Files.Where(x => x.MimeType.StartsWith("audio/")).FirstOrDefault();

            // Act
            var response = await model.CountTokens(file);

            // Assert
            response.Should().NotBeNull();
            output.WriteLine($"Tokens: {response?.TotalTokens}");
        }
    }
}