using FluentAssertions;
using Microsoft.Extensions.AI;
using Mscc.GenerativeAI;
using Mscc.GenerativeAI.Microsoft;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Xunit;
using mea = Microsoft.Extensions.AI;

namespace Test.Mscc.GenerativeAI.Microsoft
{
    public class AbstractionMapper_Should
    {
        private static readonly Type MapperType = typeof(GeminiChatClient).Assembly
            .GetType("Mscc.GenerativeAI.Microsoft.AbstractionMapper") 
            ?? throw new InvalidOperationException("AbstractionMapper type not found");

        private static MethodInfo GetMethod(string name)
        {
            return MapperType.GetMethod(name, BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Public) 
                ?? throw new InvalidOperationException($"Method '{name}' not found on AbstractionMapper");
        }

        [Fact]
        public void Preserve_ThoughtSignature_When_Converting_DataContent_RoundTrip()
        {
            // Arrange
            // Simulate a response from Gemini API with an image part that has ThoughtSignature
            var thoughtSignature = new byte[] { 0x01, 0x02, 0x03, 0x04 };
            var imageData = Convert.ToBase64String(Encoding.UTF8.GetBytes("fake_image_data"));
            
            // Create ContentResponse using the public constructor and then modify Parts
            var contentResponse = new ContentResponse("placeholder", "model");
            contentResponse.Parts.Clear();
            contentResponse.Parts.Add(new Part
            {
                InlineData = new InlineData
                {
                    Data = imageData,
                    MimeType = "image/png"
                },
                ThoughtSignature = thoughtSignature
            });

            var response = new GenerateContentResponse
            {
                Candidates = new List<Candidate>
                {
                    new Candidate
                    {
                        Content = contentResponse
                    }
                }
            };

            // Act - Convert GenerateContentResponse to ChatResponse
            var toChatResponseMethod = GetMethod("ToChatResponse");
            var chatResponse = toChatResponseMethod.Invoke(null, new object?[] { response }) as mea.ChatResponse;

            // Assert - Check that RawRepresentation is set on DataContent
            chatResponse.Should().NotBeNull();
            chatResponse!.Messages.Should().NotBeNull().And.HaveCount(1);
            var messageContent = chatResponse.Messages[0].Contents.FirstOrDefault() as mea.DataContent;
            messageContent.Should().NotBeNull();
            messageContent!.RawRepresentation.Should().NotBeNull();
            messageContent.RawRepresentation.Should().BeOfType<Part>();
            var partFromRaw = messageContent.RawRepresentation as Part;
            partFromRaw!.ThoughtSignature.Should().BeEquivalentTo(thoughtSignature);

            // Act - Convert back to GenerateContentRequest
            var chatMessages = chatResponse.Messages;
            var toRequestMethod = GetMethod("ToGeminiGenerateContentRequest");
            var request = toRequestMethod.Invoke(null, new object?[] { null, chatMessages, null }) as GenerateContentRequest;

            // Assert - Check that ThoughtSignature is preserved in the request
            request.Should().NotBeNull();
            request!.Contents.Should().NotBeNull().And.HaveCount(1);
            request.Contents[0].PartTypes.Should().NotBeNull().And.HaveCount(1);
            var requestPart = request.Contents[0].PartTypes![0];
            requestPart.ThoughtSignature.Should().BeEquivalentTo(thoughtSignature);
            requestPart.InlineData.Should().NotBeNull();
            requestPart.InlineData!.MimeType.Should().Be("image/png");
        }

        [Fact]
        public void Create_DataContent_Without_ThoughtSignature_When_Not_Present()
        {
            // Arrange
            var imageData = Convert.ToBase64String(Encoding.UTF8.GetBytes("fake_image_data"));
            
            // Create ContentResponse using the public constructor and then modify Parts
            var contentResponse = new ContentResponse("placeholder", "model");
            contentResponse.Parts.Clear();
            contentResponse.Parts.Add(new Part
            {
                InlineData = new InlineData
                {
                    Data = imageData,
                    MimeType = "image/png"
                }
                // No ThoughtSignature
            });

            var response = new GenerateContentResponse
            {
                Candidates = new List<Candidate>
                {
                    new Candidate
                    {
                        Content = contentResponse
                    }
                }
            };

            // Act
            var toChatResponseMethod = GetMethod("ToChatResponse");
            var chatResponse = toChatResponseMethod.Invoke(null, new object?[] { response }) as mea.ChatResponse;

            // Assert
            chatResponse.Should().NotBeNull();
            chatResponse!.Messages.Should().NotBeNull().And.HaveCount(1);
            var messageContent = chatResponse.Messages[0].Contents.FirstOrDefault() as mea.DataContent;
            messageContent.Should().NotBeNull();
            messageContent!.RawRepresentation.Should().NotBeNull();
            var partFromRaw = messageContent.RawRepresentation as Part;
            partFromRaw!.ThoughtSignature.Should().BeNull();
        }

        [Fact]
        public void Create_InlineData_When_DataContent_Has_No_RawRepresentation()
        {
            // Arrange - Create DataContent without RawRepresentation (user-provided)
            var imageBytes = Encoding.UTF8.GetBytes("fake_image_data");
            var dataContent = new mea.DataContent(imageBytes, "image/png");
            // RawRepresentation is null by default

            var chatMessages = new List<mea.ChatMessage>
            {
                new mea.ChatMessage(mea.ChatRole.User, new List<mea.AIContent> { dataContent })
            };

            // Act
            var toRequestMethod = GetMethod("ToGeminiGenerateContentRequest");
            var request = toRequestMethod.Invoke(null, new object?[] { null, chatMessages, null }) as GenerateContentRequest;

            // Assert - Should create InlineData without ThoughtSignature
            request.Should().NotBeNull();
            request!.Contents.Should().NotBeNull().And.HaveCount(1);
            request.Contents[0].PartTypes.Should().NotBeNull().And.HaveCount(1);
            var requestPart = request.Contents[0].PartTypes![0];
            requestPart.ThoughtSignature.Should().BeNull();
            requestPart.InlineData.Should().NotBeNull();
            requestPart.InlineData!.MimeType.Should().Be("image/png");
        }
    }
}
