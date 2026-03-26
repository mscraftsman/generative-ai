using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Mscc.GenerativeAI;
using Mscc.GenerativeAI.Types;
using Shouldly;
using Xunit;

namespace Test.Mscc.GenerativeAI
{
    public class SerializationTests
    {
        private class MockHttpMessageHandler : HttpMessageHandler
        {
            public string LastPayload { get; private set; }
            public HttpRequestMessage LastRequest { get; private set; }

            protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
            {
                LastRequest = request;
                if (request.Content != null)
                {
                    LastPayload = await request.Content.ReadAsStringAsync();
                }

                return new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent("{}")
                };
            }
        }

        [Fact]
        public async Task EmbedContent_List_Should_Use_BatchEmbedContentsRequest()
        {
            // Arrange
            var handler = new MockHttpMessageHandler();
            var model = new GenerativeModel(handler);
            var requests = new List<EmbedContentRequest>
            {
                new EmbedContentRequest("Hello"),
                new EmbedContentRequest("World")
            };

            // Act
            await model.EmbedContent(requests);

            // Assert
            handler.LastPayload.ShouldNotBeNull();
            var json = JsonDocument.Parse(handler.LastPayload);
            json.RootElement.TryGetProperty("requests", out _).ShouldBeTrue();
            json.RootElement.TryGetProperty("instances", out _).ShouldBeFalse();

            var requestsArray = json.RootElement.GetProperty("requests");
            requestsArray.GetArrayLength().ShouldBe(2);
            requestsArray[0].GetProperty("content").GetProperty("parts")[0].GetProperty("text").GetString().ShouldBe("Hello");
        }

        [Fact]
        public async Task GenerateImages_Should_Use_Instances()
        {
            // Arrange
            var handler = new MockHttpMessageHandler();
            var request = new GenerateImagesRequest("A futuristic city");

            // Act
            var genModel = new GenerativeModel(handler);
            await genModel.GenerateImages(request);

            // Assert
            handler.LastPayload.ShouldNotBeNull();
            var json = JsonDocument.Parse(handler.LastPayload);
            json.RootElement.TryGetProperty("instances", out _).ShouldBeTrue();
            json.RootElement.TryGetProperty("requests", out _).ShouldBeFalse();

            var instancesArray = json.RootElement.GetProperty("instances");
            instancesArray.GetArrayLength().ShouldBe(1);
            instancesArray[0].GetProperty("prompt").GetString().ShouldBe("A futuristic city");
        }

        [Fact]
        public async Task CountTokens_GoogleAI_Should_Use_Contents()
        {
            // Arrange
            var handler = new MockHttpMessageHandler();
            var model = new GenerativeModel(handler); // Default is Google AI
            var request = new GenerateContentRequest("Hello");

            // Act
            await model.CountTokens(request);

            // Assert
            handler.LastPayload.ShouldNotBeNull();
            var json = JsonDocument.Parse(handler.LastPayload);
            json.RootElement.TryGetProperty("generateContentRequest", out _).ShouldBeTrue();
            json.RootElement.GetProperty("generateContentRequest").TryGetProperty("contents", out _).ShouldBeTrue();
            json.RootElement.TryGetProperty("instances", out _).ShouldBeFalse();
        }

        [Fact]
        public async Task CountTokens_VertexAI_Should_Use_Instances_And_Preserve_Data()
        {
            // Arrange
            var handler = new MockHttpMessageHandler();
            var model = new GenerativeModel("dummy_project", "us-central1", "dummy_model", "dummy_token", endpoint: null, generationConfig: null, safetySettings: null, tools: null, systemInstruction: null, toolConfig: null, httpClientFactory: null, logger: null);
            typeof(BaseModel).GetField("_httpClient", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(model, new HttpClient(handler));
            var request = new GenerateContentRequest("Hello")
            {
                GenerationConfig = new GenerationConfig { Temperature = 0.5f },
                Tools = new Tools { new Tool { GoogleSearchRetrieval = new GoogleSearchRetrieval() } },
                SystemInstruction = new Content("Be a helpful assistant")
            };

            // Act
            await model.CountTokens(request);

            // Assert
            handler.LastPayload.ShouldNotBeNull();
            var json = JsonDocument.Parse(handler.LastPayload);
            json.RootElement.TryGetProperty("instances", out _).ShouldBeTrue();
            json.RootElement.TryGetProperty("generationConfig", out _).ShouldBeTrue();
            json.RootElement.GetProperty("generationConfig").GetProperty("temperature").GetSingle().ShouldBe(0.5f);
            json.RootElement.TryGetProperty("tools", out _).ShouldBeTrue();
            json.RootElement.TryGetProperty("systemInstruction", out _).ShouldBeTrue();
            json.RootElement.GetProperty("systemInstruction").GetProperty("parts")[0].GetProperty("text").GetString().ShouldBe("Be a helpful assistant");

            json.RootElement.TryGetProperty("generateContentRequest", out _).ShouldBeFalse();
            json.RootElement.TryGetProperty("contents", out _).ShouldBeFalse();
        }

        [Fact]
        public async Task EmbedContent_GoogleAI_Should_Use_Content()
        {
            // Arrange
            var handler = new MockHttpMessageHandler();
            var model = new GenerativeModel(); // Default is Google AI
            typeof(BaseModel).GetField("_httpClient", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(model, new HttpClient(handler));

            // Act
            await model.EmbedContent("Hello");

            // Assert
            handler.LastPayload.ShouldNotBeNull();
            var json = JsonDocument.Parse(handler.LastPayload);
            json.RootElement.TryGetProperty("content", out _).ShouldBeTrue();
            json.RootElement.TryGetProperty("instance", out _).ShouldBeFalse();
        }

        [Fact]
        public async Task EmbedContent_VertexAI_Should_Use_Instance()
        {
            // Arrange
            var handler = new MockHttpMessageHandler();
            var model = new GenerativeModel("dummy_project", "us-central1", "dummy_model", "dummy_token", endpoint: null, generationConfig: null, safetySettings: null, tools: null, systemInstruction: null, toolConfig: null, httpClientFactory: null, logger: null);
            typeof(BaseModel).GetField("_httpClient", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(model, new HttpClient(handler));

            // Act
            await model.EmbedContent("Hello");

            // Assert
            handler.LastPayload.ShouldNotBeNull();
            var json = JsonDocument.Parse(handler.LastPayload);
            json.RootElement.TryGetProperty("instance", out _).ShouldBeTrue();
            json.RootElement.TryGetProperty("content", out _).ShouldBeFalse();
        }

        [Fact]
        public async Task ComputeTokens_VertexAI_Should_Use_Instances()
        {
            // Arrange
            var handler = new MockHttpMessageHandler();
            var model = new GenerativeModel("dummy_project", "us-central1", "dummy_model", "dummy_token", endpoint: null, generationConfig: null, safetySettings: null, tools: null, systemInstruction: null, toolConfig: null, httpClientFactory: null, logger: null);
            typeof(BaseModel).GetField("_httpClient", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(model, new HttpClient(handler));
            var request = new ComputeTokensRequest
            {
                Contents = new List<Content> { new Content { Role = "user", Parts = new List<IPart> { new TextData { Text = "Hello" } } } }
            };

            // Act
            await model.ComputeTokens(request);

            // Assert
            handler.LastPayload.ShouldNotBeNull();
            var json = JsonDocument.Parse(handler.LastPayload);
            json.RootElement.TryGetProperty("instances", out _).ShouldBeTrue();
            json.RootElement.TryGetProperty("contents", out _).ShouldBeFalse();
        }

        [Fact]
        public async Task ComputeTokens_GoogleAI_Should_Use_Contents()
        {
            // Arrange
            var handler = new MockHttpMessageHandler();
            var model = new GenerativeModel(); // Default is Google AI
            typeof(BaseModel).GetField("_httpClient", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(model, new HttpClient(handler));
            var request = new ComputeTokensRequest
            {
                Instances = new List<object> { new Content { Role = "user", Parts = new List<IPart> { new TextData { Text = "Hello" } } } }
            };

            // Act
            // ComputeTokens throws if not useVertexAi, let's use a workaround to test the serialization part
            // or just verify the PrepareForSerialization directly.
            request.PrepareForSerialization(false);
            var options = new JsonSerializerOptions(JsonSerializerDefaults.Web)
            {
                DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            };
            var jsonStr = JsonSerializer.Serialize(request, options);

            // Assert
            var json = JsonDocument.Parse(jsonStr);
            json.RootElement.TryGetProperty("contents", out _).ShouldBeTrue();
            json.RootElement.TryGetProperty("instances", out _).ShouldBeFalse();
        }
    }
}
