#if NET472_OR_GREATER || NETSTANDARD2_0
using System;
using System.Collections.Generic;
using System.Linq;
#endif
using Microsoft.Extensions.AI;

namespace Mscc.GenerativeAI.Microsoft.MicrosoftAi
{
    /// <summary>
    /// Provides mapping functionality between Mscc.GenerativeAI and Microsoft.Extensions.AI models.
    /// </summary>
    public static class AbstractionMapper
    {
        /// <summary>
        /// Converts a Microsoft.Extensions.AI messages and options to a <see cref="GenerateContentRequest"/>.
        /// </summary>
        /// <param name="chatMessages">A list of chat messages.</param>
        /// <param name="options">Optional. Chat options to configure the request.</param>
        /// <returns></returns>
        public static GenerateContentRequest? ToGeminiGenerateContentRequest(IList<ChatMessage> chatMessages, ChatOptions? options)
        {
            var prompt = string.Join<ChatMessage>(" ", chatMessages.ToArray()) ?? "";
            
            GenerationConfig? generationConfig = null;
            if (options?.AdditionalProperties?.Any() ?? false)
            {
                generationConfig = new GenerationConfig();
                TryAddOption<float?>(options, "Temperature", v => generationConfig.Temperature = v);
                TryAddOption<float?>(options, "TopP", v => generationConfig.TopP = v);
                TryAddOption<int?>(options, "TopK", v => generationConfig.TopK = v);
                TryAddOption<int?>(options, "MaxOutputTokens", v => generationConfig.MaxOutputTokens = v);
                TryAddOption<string?>(options, "ResponseMimeType", v => generationConfig.ResponseMimeType = v);
                TryAddOption<string?>(options, "ResponseSchema", v => generationConfig.ResponseSchema = v);
                TryAddOption<float?>(options, "PresencePenalty", v => generationConfig.PresencePenalty = v);
                TryAddOption<float?>(options, "FrequencyPenalty", v => generationConfig.FrequencyPenalty = v);
                TryAddOption<bool?>(options, "ResponseLogprobs", v => generationConfig.ResponseLogprobs = v);
                TryAddOption<int?>(options, "Logprobs", v => generationConfig.Logprobs = v);
            }

            if (options?.Tools is { Count: > 0 })
            {
                
            }
            
            return new GenerateContentRequest(prompt, generationConfig: generationConfig);
        }

        /// <summary>
        /// Converts a <see cref="ChatOptions"/> to a <see cref="RequestOptions"/>.
        /// </summary>
        /// <param name="options">Chat options to configure the request.</param>
        /// <returns></returns>
        public static RequestOptions? ToGeminiGenerateContentRequestOptions(ChatOptions? options)
        {
            if (options is null) return null;

            if (options.AdditionalProperties?.Any() ?? false)
            {
                var retry = new Retry();
                TimeSpan? timeout = null;
                TryAddOption<int?>(options, "RetryInitial", v => retry.Initial = v ?? 0);
                TryAddOption<int?>(options, "RetryMultiplies", v => retry.Multiplies = v ?? 0);
                TryAddOption<int?>(options, "RetryMaximum", v => retry.Maximum = v ?? 0);
                TryAddOption<int?>(options, "RetryTimeout", v => retry.Timeout = v ?? 0);
                TryAddOption<TimeSpan?>(options, "Timeout", v => timeout = v);
                
                if (retry.Initial > 0 || timeout is not null)
                    return new RequestOptions(retry, timeout);

                return null;
            }

            return null;
        }

        /// <summary>
        /// Converts values and options to a <see cref="EmbedContentRequest"/>.
        /// </summary>
        /// <param name="values">The values to get embeddings for</param>
        /// <param name="options">The options for the embeddings</param>
        public static EmbedContentRequest ToGeminiEmbedContentRequest(IEnumerable<string> values, EmbeddingGenerationOptions? options)
        {
            return new EmbedContentRequest(string.Join(" ", values))
            {
                Model = options?.ModelId ?? null    // will be set GeminiApiClient.SelectedModel, if not set
            };
        }
        
        /// <summary>
        /// Converts a <see cref="GenerateContentResponse"/> to a <see cref="ChatCompletion"/>.
        /// </summary>
        /// <param name="response">The response with completion data.</param>
        /// <returns></returns>
        public static ChatCompletion? ToChatCompletion(GenerateContentResponse? response)
        {
            if (response is null) return null;

            var chatMessage = ToChatMessage(response);

            return new ChatCompletion(chatMessage)
            {
                FinishReason = ToFinishReason(response.Candidates?.FirstOrDefault()?.FinishReason),
                AdditionalProperties = null,
                Choices = [chatMessage],
                CompletionId = null,
                CreatedAt = null,
                ModelId = null,
                RawRepresentation = response,
                Usage = ParseContentResponseUsage(response)
            };
        }

        /// <summary>
        /// Converts a <see cref="GenerateContentResponse"/> to a <see cref="StreamingChatCompletionUpdate"/>.
        /// </summary>
        /// <param name="response">The response stream to convert.</param>
        public static StreamingChatCompletionUpdate ToStreamingChatCompletionUpdate(GenerateContentResponse? response)
        {
            return new StreamingChatCompletionUpdate
            {
                // no need to set "Contents" as we set the text
                CompletionId = null,
                ChoiceIndex = 0, // should be left at 0 as Mscc.GenerativeAI does not support this
                CreatedAt = null,
                AdditionalProperties = null,
                FinishReason = response?.Candidates?.FirstOrDefault()?.FinishReason == FinishReason.Other ? ChatFinishReason.Stop : null,
                RawRepresentation = response,
                Text = response?.Text,
                Role = ToAbstractionRole(response?.Candidates?.FirstOrDefault()?.Content?.Role)
            };
        }

        public static GeneratedEmbeddings<Embedding<float>> ToGeneratedEmbeddings(EmbedContentRequest request, EmbedContentResponse response)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));
            if (response == null) throw new ArgumentNullException(nameof(response));

            AdditionalPropertiesDictionary? responseProps = null;
            UsageDetails? usage = null;

            return new GeneratedEmbeddings<Embedding<float>>([
                new Embedding<float>(response.Embedding?.Values.ToArray() ?? [])
                {
                    CreatedAt = DateTimeOffset.Now,
                    ModelId = request.Model
                }])
            {
                AdditionalProperties = responseProps, 
                Usage = usage
            };
        }

        /// <summary>
        /// Maps a <see cref="GenerateContentResponse"/> to a <see cref="ChatMessage"/>.
        /// </summary>
        /// <param name="response">The response to map.</param>
        private static ChatMessage ToChatMessage(GenerateContentResponse response)
        {
            var contents = new List<AIContent>();
            if (response.Text?.Length > 0)
                contents.Insert(0, new TextContent(response.Text));

            return new ChatMessage(ToAbstractionRole(response.Candidates?.FirstOrDefault()?.Content?.Role), contents)
            {
                RawRepresentation = response
            };
        }

        /// <summary>
        /// Maps a <see cref="Mscc.GenerativeAI.Role"/> to a <see cref="ChatRole"/>.
        /// </summary>
        /// <param name="role">The role to map.</param>
        private static ChatRole ToAbstractionRole(string? role)
        {
            if (string.IsNullOrEmpty(role)) return new ChatRole("unknown");

            return role switch
            {
                Role.User => ChatRole.User,
                Role.Model => ChatRole.Assistant,
                Role.System => ChatRole.System,
                Role.Function => ChatRole.Tool,
                _ => new ChatRole(role)
            };
        }

        /// <summary>
        /// Maps a <see cref="FinishReason"/> to a <see cref="ChatFinishReason"/>.
        /// </summary>
        /// <param name="finishReason">The finish reason to map.</param>
        private static ChatFinishReason? ToFinishReason(FinishReason? finishReason)
        {
            return finishReason switch
            {
                null => null,
                FinishReason.MaxTokens => ChatFinishReason.Length,
                FinishReason.Stop => ChatFinishReason.Stop,
                FinishReason.Safety => ChatFinishReason.ContentFilter,
                FinishReason.ProhibitedContent => ChatFinishReason.ContentFilter,
                FinishReason.Recitation => ChatFinishReason.ContentFilter,
                FinishReason.Spii => ChatFinishReason.ContentFilter,
                FinishReason.Blocklist => ChatFinishReason.ContentFilter,
                FinishReason.MalformedFunctionCall => ChatFinishReason.ToolCalls,
                _ => new ChatFinishReason(Enum.GetName(typeof(FinishReason), finishReason)!)
            };
        }

        /// <summary>
        /// Parses usage details from a <see cref="GenerateContentResponse"/>
        /// </summary>
        /// <param name="response">The response to parse.</param>
        /// <returns>A <see cref="UsageDetails"/> instance containing the parsed usage details.</returns>
        private static UsageDetails? ParseContentResponseUsage(GenerateContentResponse response)
        {
            if (response.UsageMetadata is null) return null;

            return new()
            {
                InputTokenCount = response.UsageMetadata.PromptTokenCount,
                OutputTokenCount = response.UsageMetadata.CandidatesTokenCount,
                TotalTokenCount = response.UsageMetadata.TotalTokenCount
            };
        }

        /// <summary>
        /// Tries to find Mscc.GenerativeAI options in the additional properties and sets them.
        /// </summary>
        /// <param name="chatOptions">The chat options from the Microsoft abstraction</param>
        /// <param name="option">The Gemini setting to add</param>
        /// <param name="optionSetter">The <see cref="Action"/> to set the Gemini option, if available in the chat options</param>
        /// <typeparam name="T">The type of the option</typeparam>
        private static void TryAddOption<T>(ChatOptions chatOptions, string option, Action<T> optionSetter)
        {
            if (chatOptions.AdditionalProperties?.TryGetValue(option, out var value) ?? false)
                optionSetter((T)value);
        }
    }
}