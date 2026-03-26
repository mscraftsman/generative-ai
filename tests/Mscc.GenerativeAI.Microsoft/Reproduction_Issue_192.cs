using Shouldly;
using Microsoft.Extensions.AI;
using Mscc.GenerativeAI;
using Mscc.GenerativeAI.Microsoft;
using Mscc.GenerativeAI.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Xunit;
using mea = Microsoft.Extensions.AI;

namespace Test.Mscc.GenerativeAI.Microsoft
{
    public class Reproduction_Issue_192
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
        public void Role_Tool_Should_Map_To_Function()
        {
            // Arrange
            var chatMessages = new List<mea.ChatMessage>
            {
                new mea.ChatMessage(mea.ChatRole.Tool, [new mea.FunctionResultContent("call_123", "result_data")])
            };

            // Act
            var toRequestMethod = GetMethod("ToGeminiGenerateContentRequest");
            var request = toRequestMethod.Invoke(null, new object?[] { null, chatMessages, null }) as GenerateContentRequest;

            // Assert
            request.ShouldNotBeNull();
            request!.Contents.ShouldNotBeNull();
            request!.Contents.Count.ShouldBe(1);
            request.Contents[0].Role.ShouldBe(Role.Function);
        }

        [Fact]
        public void FunctionNames_Should_Persist_Across_Messages()
        {
            // Arrange
            var chatMessages = new List<mea.ChatMessage>
            {
                new mea.ChatMessage(mea.ChatRole.Assistant, [new mea.FunctionCallContent("call_123", "get_weather")]),
                new mea.ChatMessage(mea.ChatRole.Tool, [new mea.FunctionResultContent("call_123", "sunny")])
            };

            // Act
            var toRequestMethod = GetMethod("ToGeminiGenerateContentRequest");
            var request = toRequestMethod.Invoke(null, new object?[] { null, chatMessages, null }) as GenerateContentRequest;

            // Assert
            request.ShouldNotBeNull();
            request!.Contents.ShouldNotBeNull();
            request!.Contents.Count.ShouldBe(2);

            var toolMessage = request.Contents[1];
            toolMessage.PartTypes.ShouldNotBeNull();
            var part = toolMessage.PartTypes![0];
            part.FunctionResponse.ShouldNotBeNull();
            part.FunctionResponse!.Name.ShouldBe("get_weather");
        }

        [Fact]
        public void User_Messages_Should_Not_Have_ThoughtSignature()
        {
            // Arrange
            var chatMessages = new List<mea.ChatMessage>
            {
                new mea.ChatMessage(mea.ChatRole.User, "Hello")
            };

            // Act
            var toRequestMethod = GetMethod("ToGeminiGenerateContentRequest");
            var request = toRequestMethod.Invoke(null, new object?[] { null, chatMessages, null }) as GenerateContentRequest;

            // Assert
            request.ShouldNotBeNull();
            request!.Contents.ShouldNotBeNull();
            request!.Contents.Count.ShouldBe(1);

            var userMessage = request.Contents[0];
            userMessage.PartTypes.ShouldNotBeNull();
            var part = userMessage.PartTypes![0];
            part.ThoughtSignature.ShouldBeNull();
        }

        [Fact]
        public void Assistant_Messages_Should_Have_SkipThoughtValidation()
        {
            // Arrange
            var chatMessages = new List<mea.ChatMessage>
            {
                new mea.ChatMessage(mea.ChatRole.Assistant, "Hello")
            };

            // Act
            var toRequestMethod = GetMethod("ToGeminiGenerateContentRequest");
            var request = toRequestMethod.Invoke(null, new object?[] { null, chatMessages, null }) as GenerateContentRequest;

            // Assert
            request.ShouldNotBeNull();
            request!.Contents.ShouldNotBeNull();
            request!.Contents.Count.ShouldBe(1);

            var assistantMessage = request.Contents[0];
            assistantMessage.PartTypes.ShouldNotBeNull();
            var part = assistantMessage.PartTypes![0];
            part.ThoughtSignature.ShouldNotBeNull();
            var skipValue = Encoding.UTF8.GetBytes("skip_thought_signature_validator");
            part.ThoughtSignature.ShouldBeEquivalentTo(skipValue);
        }
    }
}
