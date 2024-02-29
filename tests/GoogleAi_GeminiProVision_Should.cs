﻿using FluentAssertions;
using Mscc.GenerativeAI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit;
using Xunit.Abstractions;

namespace Test.Mscc.GenerativeAI
{
    [Collection(nameof(ConfigurationFixture))]
    public class GoogleAi_GeminiProVision_Should
    {
        private readonly ITestOutputHelper output;
        private readonly ConfigurationFixture fixture;
        private readonly string model = Model.GeminiProVision;

        public GoogleAi_GeminiProVision_Should(ITestOutputHelper output, ConfigurationFixture fixture)
        {
            this.output = output;
            this.fixture = fixture;
        }

        [Fact]
        public void Initialize_GeminiProVision()
        {
            // Arrange

            // Act
            var model = new GenerativeModel(apiKey: fixture.ApiKey, model: this.model);

            // Assert
            model.Should().NotBeNull();
            model.Name().Should().Be(Model.GeminiProVision);
        }

        [Fact]
        public async void Generate_Text_From_Image()
        {
            // Arrange
            var model = new GenerativeModel(apiKey: fixture.ApiKey, model: this.model);
            var request = new GenerateContentRequest { Contents = new List<Content>() };
            var base64image = "iVBORw0KGgoAAAANSUhEUgAAAAEAAAABCAYAAAAfFcSJAAAADUlEQVR42mP8z8BQDwAEhQGAhKmMIQAAAABJRU5ErkJggg==";
            var parts = new List<IPart>
            {
                new TextData { Text = "What is this picture about?" },
                new InlineData { MimeType = "image/jpeg", Data = base64image }
            };
            request.Contents.Add(new Content { Role = "user", Parts = parts });

            // Act
            var response = await model.GenerateContent(request);

            // Assert
            response.Should().NotBeNull();
            response.Candidates.Should().NotBeNull().And.HaveCount(1);
            response.Candidates.FirstOrDefault().Content.Should().NotBeNull();
            response.Candidates.FirstOrDefault().Content.Parts.Should().NotBeNull().And.HaveCountGreaterThanOrEqualTo(1);
            response.Text.Should().Contain("red");
            output.WriteLine(response?.Text);
        }

        [Theory]
        [InlineData("scones.jpg", "image/jpeg", "What is this picture?", "blueberries")]
        [InlineData("cat.jpg", "image/jpeg", "Describe this image", "snow")]
        [InlineData("cat.jpg", "image/jpeg", "Is it a cat?", "Yes")]
        public async void Generate_Text_From_ImageFile(string filename, string mimetype, string prompt, string expected)
        {
            // Arrange
            var model = new GenerativeModel(apiKey: fixture.ApiKey, model: this.model);
            var base64image = Convert.ToBase64String(File.ReadAllBytes(Path.Combine(Environment.CurrentDirectory, "payload", filename)));
            var parts = new List<IPart>
            {
                new TextData { Text = prompt },
                new InlineData { MimeType = mimetype, Data = base64image }
            };

            // Act
            var response = await model.GenerateContent(parts);

            // Assert
            response.Should().NotBeNull();
            response.Candidates.Should().NotBeNull().And.HaveCount(1);
            response.Candidates.FirstOrDefault().Content.Should().NotBeNull();
            response.Candidates.FirstOrDefault().Content.Parts.Should().NotBeNull().And.HaveCountGreaterThanOrEqualTo(1);
            response.Text.Should().Contain(expected);
            output.WriteLine(response?.Text);
        }
    }
}