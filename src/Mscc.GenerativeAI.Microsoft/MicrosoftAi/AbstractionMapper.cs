using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Diagnostics;
using System.Text;
using System.Text.Json;
using mea = Microsoft.Extensions.AI;

namespace Mscc.GenerativeAI.Microsoft
{
    /// <summary>
    /// Provides mapping functionality between Mscc.GenerativeAI and Microsoft.Extensions.AI models.
    /// </summary>
    internal static class AbstractionMapper
    {
        /// <summary>
        /// Converts a Microsoft.Extensions.AI messages and options to a <see cref="GenerateContentRequest"/>.
        /// </summary>
        /// <param name="client">The chat client.</param>
        /// <param name="messages">A list of chat messages.</param>
        /// <param name="options">Optional. Chat options to configure the request.</param>
        /// <returns></returns>
        public static GenerateContentRequest? ToGeminiGenerateContentRequest(mea.IChatClient client,
            IEnumerable<mea.ChatMessage> messages, mea.ChatOptions? options)
        {
            GenerateContentRequest request =
                options?.RawRepresentationFactory?.Invoke(client) as GenerateContentRequest ?? new();

            void AddSystemInstruction(string? text)
            {
                if (!string.IsNullOrWhiteSpace(text))
                {
                    ((request.SystemInstruction ??= new()).Parts ??= []).Add(new TextData() { Text = text });
                }
            }

            request.Contents ??= [];
            foreach (var message in messages)
            {
                if (message.Role == mea.ChatRole.System)
                {
                    AddSystemInstruction(message.Text);
                    continue;
                }

                Content c = new(message.Text);
                c.Parts ??= [];
                c.Parts.Clear();

                c.Role = message.Role == mea.ChatRole.Assistant ? "model" : "user";

                Dictionary<string, string> functionNames = new();

                foreach (var content in message.Contents)
                {                   
                    switch (content)
                    {
                        case mea.TextReasoningContent trc:
                            var thoughtPart = new Part
                            {
                                Text = trc.Text,
                                Thought = true,
                                ThoughtSignature = !string.IsNullOrEmpty(trc.ProtectedData) 
                                    ? Convert.FromBase64String(trc.ProtectedData) 
                                    : null
                            };
                            c.Parts.Add(thoughtPart);
                            break;

                        case mea.TextContent tc:
                            c.Parts.Add(new TextData() { Text = tc.Text });
                            break;

                        case mea.DataContent dc:
                            byte[]? thoughtSignature = null;
                            if (dc.AdditionalProperties?.TryGetValue("ThoughtSignature", out var sigObj) == true)
                            {
                                // Handle both string (in-memory) and JsonElement (after JSON deserialization)
                                string? sigBase64 = sigObj switch
                                {
                                    string s => s,
                                    System.Text.Json.JsonElement je when je.ValueKind == System.Text.Json.JsonValueKind.String => je.GetString(),
                                    _ => null
                                };
                                
                                if (!string.IsNullOrEmpty(sigBase64))
                                {
                                    thoughtSignature = Convert.FromBase64String(sigBase64);
                                }
                            }
                            
                            if (thoughtSignature != null)
                            {
                                c.Parts.Add(new Part
                                {
                                    InlineData = new InlineData
                                    {
                                        Data = dc.Base64Data.ToString(),
                                        MimeType = dc.MediaType
                                    },
                                    ThoughtSignature = thoughtSignature
                                });
                            }
                            else
                            {
                                c.Parts.Add(new InlineData() { Data = dc.Base64Data.ToString(), MimeType = dc.MediaType });
                            }
                            break;

                        case mea.UriContent uc:
                            c.Parts.Add(new FileData() { FileUri = uc.Uri.AbsoluteUri, MimeType = uc.MediaType });
                            break;

                        case mea.FunctionCallContent fcc:
                            functionNames[fcc.CallId] = fcc.Name;
                            c.Parts.Add(new FunctionCall() { Id = fcc.CallId, Name = fcc.Name, Args = fcc.Arguments });
                            break;

                        case mea.FunctionResultContent frc:
                            var functionName = frc.CallId;
                            if (functionNames.TryGetValue(frc.CallId, out var name))
                            {
                                functionName = name;
                            }

                            // If we receive anything other than a JsonElement that is an object, wrap it in an object { "result": jsonElement }
                            if (frc.Result is not JsonElement { ValueKind: JsonValueKind.Object })
                            {
                                frc.Result = WrapInObject(frc.Result);
                            }

                            c.Parts.Add(new FunctionResponse()
                            {
                                Id = frc.CallId, Name = functionName, Response = frc.Result
                            });
                            break;
                    }
                }

                if (c.Parts.Count > 0)
                {
                    request.Contents.Add(c);
                }
            }

            if (options is not null)
            {
                AddSystemInstruction(options.Instructions);

                request.GenerationConfig ??= new();
                
                if (options.FrequencyPenalty is not null)
                {
                    request.GenerationConfig.FrequencyPenalty = options.FrequencyPenalty;
                }

                if (options.PresencePenalty is not null)
                {
                    request.GenerationConfig.PresencePenalty = options.PresencePenalty;
                }

                if (options.TopP is not null)
                {
                    request.GenerationConfig.TopP = options.TopP;
                }

                if (options.TopK is not null)
                {
                    request.GenerationConfig.TopK = options.TopK;
                }

                if (options.StopSequences is not null)
                {
                    request.GenerationConfig.StopSequences = options.StopSequences?.ToList();
                }

                if (options.MaxOutputTokens is not null)
                {
                    request.GenerationConfig.MaxOutputTokens = options.MaxOutputTokens;
                }

                if (options.Seed is not null)
                {
                    request.GenerationConfig.Seed = (int?)options.Seed;
                }

                if (options.Temperature is not null)
                {
                    request.GenerationConfig.Temperature = options.Temperature;
                }

                if (options.ResponseFormat is mea.ChatResponseFormatJson jsonFormat)
                {
                    request.GenerationConfig.ResponseMimeType = "application/json";
                    if (jsonFormat.Schema is not null)
                        request.GenerationConfig.ResponseSchema = jsonFormat.Schema is {} schema ? Schema.FromJsonElement(schema) : null;
                }

                if (options.AdditionalProperties?.Any() is true)
                {
                    if (options.AdditionalProperties.TryGetValue("ThinkingConfig", out var thinkingConfig) &&
                        thinkingConfig is IReadOnlyDictionary<string, object> thinkingConfigDict)
                    {
                        request.GenerationConfig.ThinkingConfig ??= new ThinkingConfig();
                        if (thinkingConfigDict.TryGetValue("IncludeThoughts", out var includeThoughts))
                        {
                            if (includeThoughts is bool b)
                            {
                                request.GenerationConfig.ThinkingConfig.IncludeThoughts = b;
                            }
                            else if (bool.TryParse(includeThoughts.ToString(), out var b2))
                            {
                                request.GenerationConfig.ThinkingConfig.IncludeThoughts = b2;
                            }
                        }

                        if (thinkingConfigDict.TryGetValue("ThinkingBudget", out var thinkingBudget))
                        {
                            if (thinkingBudget is int i)
                            {
                                request.GenerationConfig.ThinkingConfig.ThinkingBudget = i;
                            }
                            else if (int.TryParse(thinkingBudget.ToString(), out var i2))
                            {
                                request.GenerationConfig.ThinkingConfig.ThinkingBudget = i2;
                            }
                        }
                    }
                }

                if (options.Tools is { } aiTools)
                {
                    List<FunctionDeclaration> functionDeclarations = [];
                    foreach (var tool in aiTools)
                    {
                        switch (tool)
                        {
                            case mea.AIFunctionDeclaration aif:
                                functionDeclarations.Add(new FunctionDeclaration
                                {
                                    Name = aif.Name,
                                    Description = aif.Description,
                                    Parameters = ToGeminiSchema(aif.JsonSchema),
                                    Response = aif.ReturnJsonSchema is { } rjs
                                        ? ToGeminiSchema(rjs)
                                        : null,
                                });
                                break;

                            case mea.HostedWebSearchTool wst:
                                (request.Tools ??= []).Add(new Tool() { GoogleSearch = new() });
                                break;

                            case mea.HostedCodeInterpreterTool cit:
                                (request.Tools ??= []).Add(new Tool() { CodeExecution = new() });
                                break;
                        }
                    }

                    if (functionDeclarations.Count > 0)
                    {
                        (request.Tools ??= []).Add(new Tool() { FunctionDeclarations = functionDeclarations });
                    }

                    switch (options.ToolMode)
                    {
                        case mea.NoneChatToolMode:
                            request.ToolConfig ??= new();
                            request.ToolConfig.FunctionCallingConfig ??= new();
                            request.ToolConfig.FunctionCallingConfig.Mode = FunctionCallingConfigMode.None;
                            break;

                        case mea.AutoChatToolMode:
                        case null:
                            request.ToolConfig ??= new();
                            request.ToolConfig.FunctionCallingConfig ??= new();
                            request.ToolConfig.FunctionCallingConfig.Mode = FunctionCallingConfigMode.Auto;
                            break;

                        case mea.RequiredChatToolMode rctm:
                            request.ToolConfig ??= new();
                            request.ToolConfig.FunctionCallingConfig ??= new();
                            request.ToolConfig.FunctionCallingConfig.Mode = FunctionCallingConfigMode.Any;
                            if (rctm.RequiredFunctionName is { } name)
                            {
                                (request.ToolConfig.FunctionCallingConfig.AllowedFunctionNames ??= []).Add(name);
                            }

                            break;
                    }
                }
            }

            return request;
        }

        /// <summary>
        /// Wraps a value into a new <see cref="JsonElement"/> and the specified <paramref name="key"/>.
        /// </summary>
        /// <param name="value">The value to wrap.</param>
        /// <param name="key">Property to use as wrapper.</param>
        /// <returns></returns>
        private static JsonElement WrapInObject(object? value, string key = "result")
        {
            using MemoryStream stream = new();
            using Utf8JsonWriter writer = new(stream);

            writer.WriteStartObject();
            writer.WritePropertyName(key);
            if (value is JsonElement jsonElement)
            {
                jsonElement.WriteTo(writer);
            }
            else
            {
                JsonSerializer.Serialize(writer, value);
            }

            writer.WriteEndObject();
            writer.Flush();

            using var jsonDoc = JsonDocument.Parse(stream.ToArray());
            return jsonDoc.RootElement.Clone();
        }

        /// <summary>
        /// Converts a <see cref="mea.ChatOptions"/> to a <see cref="RequestOptions"/>.
        /// </summary>
        /// <param name="options">Chat options to configure the request.</param>
        /// <returns></returns>
        public static RequestOptions? ToGeminiGenerateContentRequestOptions(mea.ChatOptions? options)
        {
            if (options is null) return null;

            if (options.AdditionalProperties?.Any() ?? false)
            {
                var retry = new Retry();
                TimeSpan? timeout = null;
                TryAddOption<int?>(options, "RetryInitial", v => retry.Initial = v ?? 0);
                TryAddOption<int?>(options, "RetryMultiplies", v => retry.Multiplies = v ?? 0);
                TryAddOption<int?>(options, "RetryMaximum", v => retry.Maximum = v ?? 0);
                TryAddOption<int?>(options, "RetryTimeout", v =>
                    retry.Timeout = v.HasValue ? TimeSpan.FromSeconds((double)v.Value) : null);
                TryAddOption<TimeSpan?>(options, "Timeout", v => timeout = v);

                if (retry.Initial > 0 && timeout is not null)
                {
                    return new RequestOptions(retry, timeout);
                }
                else if (timeout is not null)
                {
                    return new RequestOptions(timeout: timeout);
                }
            }

            return null;
        }

        /// <summary>
        /// Converts values and options to a <see cref="EmbedContentRequest"/>.
        /// </summary>
        /// <param name="values">The values to get embeddings for</param>
        /// <param name="options">The options for the embeddings</param>
        public static EmbedContentRequest ToGeminiEmbedContentRequest(IEnumerable<string> values,
            mea.EmbeddingGenerationOptions? options)
        {
            return new EmbedContentRequest(string.Join(" ", values))
            {
                Model = options?.ModelId ?? null // will be set GeminiApiClient.SelectedModel, if not set
            };
        }

        /// <summary>
        /// Converts a <see cref="JsonElement"/> to a <see cref="Schema"/>.
        /// </summary>
        /// <param name="jsonElement">The schema definition as a <see cref="JsonElement"/>.</param>
        /// <returns></returns>
        private static Schema ToGeminiSchema(JsonElement jsonElement)
        {
            return Schema.FromJsonElement(jsonElement);
        }

        /// <summary>
        /// Converts a <see cref="GenerateContentResponse"/> to a <see cref="mea.ChatResponse"/>.
        /// </summary>
        /// <param name="response">The response with completion data.</param>
        /// <returns></returns>
        public static mea.ChatResponse? ToChatResponse(GenerateContentResponse? response)
        {
            if (response is null) return null;

            var chatMessage = ToChatMessage(response);

            return new mea.ChatResponse(chatMessage)
            {
                AdditionalProperties = chatMessage.AdditionalProperties,
                CreatedAt = chatMessage.CreatedAt,
                FinishReason = ToFinishReason(response.Candidates?.FirstOrDefault()?.FinishReason),
                ModelId = response.ModelVersion,
                RawRepresentation = response,
                ResponseId = response.ResponseId,
                Usage = ParseContentResponseUsage(response)
            };
        }

        /// <summary>
        /// Converts a <see cref="GenerateContentResponse"/> to a <see cref="mea.ChatResponseUpdate"/>.
        /// </summary>
        /// <param name="response">The response stream to convert.</param>
        public static mea.ChatResponseUpdate? ToChatResponseUpdate(GenerateContentResponse? response)
        {
            if (response is null) return null;

            var chatMessage = ToChatMessage(response);

            return new mea.ChatResponseUpdate(ToAbstractionRole(response.Candidates?.FirstOrDefault()?.Content?.Role),
                chatMessage.Contents)
            {
                AuthorName = chatMessage.AuthorName,
                AdditionalProperties = chatMessage.AdditionalProperties,
                CreatedAt = chatMessage.CreatedAt,
                FinishReason = ToFinishReason(response.Candidates?.FirstOrDefault()?.FinishReason),
                MessageId = response.ResponseId,
                ModelId = response.ModelVersion,
                RawRepresentation = response,
                ResponseId = response.ResponseId,
                Role = chatMessage.Role,
            };
        }

        /// <summary>
        /// Converts a <see cref="EmbedContentRequest"/> to a <see cref="EmbedContentResponse"/>.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="response"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">Thrown when the specified request or response is null.</exception>
        public static mea.GeneratedEmbeddings<mea.Embedding<float>> ToGeneratedEmbeddings(EmbedContentRequest request,
            EmbedContentResponse response)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));
            if (response == null) throw new ArgumentNullException(nameof(response));

            mea.AdditionalPropertiesDictionary? responseProps = null;
            mea.UsageDetails? usage = null;

            return new mea.GeneratedEmbeddings<mea.Embedding<float>>([
                new mea.Embedding<float>(response.Embedding?.Values.ToArray() ?? [])
                {
                    CreatedAt = DateTimeOffset.Now,
                    ModelId = request.Model
                }
            ]) { AdditionalProperties = responseProps, Usage = usage };
        }

        /// <summary>
        /// Maps a <see cref="GenerateContentResponse"/> to a <see cref="ChatMessage"/>.
        /// </summary>
        /// <param name="response">The response to map.</param>
        private static mea.ChatMessage ToChatMessage(GenerateContentResponse response)
        {
            var contents = new List<mea.AIContent>();
            Candidate? candidate = response.Candidates?.FirstOrDefault();
            if (candidate?.Content is not null)
            {
                foreach (var part in candidate.Content.Parts)
                {                   
					if (part.Thought is true)
                        contents.Add(new mea.TextReasoningContent(part.Text)
                        {
                            ProtectedData = part.ThoughtSignature is not null ? Convert.ToBase64String(part.ThoughtSignature) : null
                        });
                    else if (!string.IsNullOrEmpty(part.Text))
                        contents.Add(new mea.TextContent(part.Text));
                    else if (!string.IsNullOrEmpty(part.InlineData?.Data))
                    {
                        var dataContent = new mea.DataContent(
                            Encoding.UTF8.GetBytes(part.InlineData.Data),
                            part.InlineData.MimeType);
                        // Store the original Part to preserve ThoughtSignature for round-trip
                        dataContent.RawRepresentation = part;
                        
                        // ALSO store ThoughtSignature in AdditionalProperties for serialization persistence
                        if (part.ThoughtSignature != null)
                        {
                            dataContent.AdditionalProperties ??= new mea.AdditionalPropertiesDictionary();
                            dataContent.AdditionalProperties["ThoughtSignature"] = Convert.ToBase64String(part.ThoughtSignature);
                        }
                        
                        contents.Add(dataContent);
                    }
                    else if (!string.IsNullOrEmpty(part.FileData?.FileUri))
                    {
                        var dataContent = new mea.DataContent(part.FileData.FileUri,
                            part.FileData.MimeType);
                        // Store the original Part to preserve ThoughtSignature for round-trip
                        dataContent.RawRepresentation = part;
                        
                        // ALSO store ThoughtSignature in AdditionalProperties for serialization persistence
                        if (part.ThoughtSignature != null)
                        {
                            dataContent.AdditionalProperties ??= new mea.AdditionalPropertiesDictionary();
                            dataContent.AdditionalProperties["ThoughtSignature"] = Convert.ToBase64String(part.ThoughtSignature);
                        }
                        
                        contents.Add(dataContent);
                    }
                    else if (part.FunctionCall is not null)
                        contents.Add(ToFunctionCallContent(part.FunctionCall));
                    else if (part.FunctionResponse is not null)
                        contents.Add(ToFunctionResultContent(part.FunctionResponse));
                    else if (part.CodeExecutionResult is not null)
                        contents.Add(new mea.TextContent(part.CodeExecutionResult.Output));
                    else if (part.ExecutableCode is not null)
                        contents.Add(new mea.TextContent(part.ExecutableCode.Code));
                    else if (part.VideoMetadata is not null)
                        Debug.WriteLine("Video meta data returned.");
                    else
                        Debug.WriteLine($"Part is not a string, inline data, or function call: {part.GetType()}");
                }
            }
            
            return new mea.ChatMessage(ToAbstractionRole(response.Candidates?.FirstOrDefault()?.Content?.Role),
                contents) 
            {
                CreatedAt = response.CreateTime,
                MessageId = response.ResponseId,
                RawRepresentation = response 
            };
        }

        /// <summary>
        /// Maps a <see cref="FunctionCall"/> to a <see cref="mea.FunctionCallContent"/>.
        /// </summary>
        /// <param name="functionCall"></param>
        /// <returns></returns>
        private static mea.FunctionCallContent ToFunctionCallContent(FunctionCall functionCall)
        {
            IDictionary<string, object?>? arguments = functionCall.Args switch
            {
                IReadOnlyDictionary<string, object?> dictionary => dictionary.ToDictionary(x => x.Key, x => x.Value),
                JsonElement jsonElement => jsonElement.Deserialize<IDictionary<string, object?>>(),
                _ => null,
            };

            return new mea.FunctionCallContent(
                functionCall.Id ?? Guid.NewGuid().ToString(),
                functionCall.Name,
                arguments);
        }

        /// <summary>
        /// Maps a <see cref="FunctionResponse"/> to a <see cref="mea.FunctionResultContent"/>.
        /// </summary>
        /// <param name="functionResponse"></param>
        /// <returns></returns>
        private static mea.FunctionResultContent ToFunctionResultContent(FunctionResponse functionResponse)
        {
            return new mea.FunctionResultContent(functionResponse.Id!, functionResponse.Response);
        }

        /// <summary>
        /// Maps a <see cref="Mscc.GenerativeAI.Role"/> to a <see cref="mea.ChatRole"/>.
        /// </summary>
        /// <param name="role">The role to map.</param>
        private static mea.ChatRole ToAbstractionRole(string? role)
        {
            if (string.IsNullOrEmpty(role)) return new mea.ChatRole("unknown");

            return role switch
            {
                Role.User => mea.ChatRole.User,
                Role.Model => mea.ChatRole.Assistant,
                Role.System => mea.ChatRole.System,
                Role.Function => mea.ChatRole.Tool,
                _ => new mea.ChatRole(role)
            };
        }

        /// <summary>
        /// Maps a <see cref="FinishReason"/> to a <see cref="mea.ChatFinishReason"/>.
        /// </summary>
        /// <param name="finishReason">The finish reason to map.</param>
        private static mea.ChatFinishReason? ToFinishReason(FinishReason? finishReason)
        {
            return finishReason switch
            {
                null => null,
                FinishReason.MaxTokens => mea.ChatFinishReason.Length,
                FinishReason.Stop => mea.ChatFinishReason.Stop,
                FinishReason.Other => mea.ChatFinishReason.Stop,
                FinishReason.Safety => mea.ChatFinishReason.ContentFilter,
                FinishReason.ProhibitedContent => mea.ChatFinishReason.ContentFilter,
                FinishReason.Recitation => mea.ChatFinishReason.ContentFilter,
                FinishReason.Spii => mea.ChatFinishReason.ContentFilter,
                FinishReason.Blocklist => mea.ChatFinishReason.ContentFilter,
                FinishReason.MalformedFunctionCall => mea.ChatFinishReason.ToolCalls,
                _ => new mea.ChatFinishReason(Enum.GetName(typeof(FinishReason), finishReason)!)
            };
        }

        /// <summary>
        /// Parses usage details from a <see cref="GenerateContentResponse"/>
        /// </summary>
        /// <param name="response">The response to parse.</param>
        /// <returns>A <see cref="mea.UsageDetails"/> instance containing the parsed usage details.</returns>
        private static mea.UsageDetails? ParseContentResponseUsage(GenerateContentResponse response)
        {
            if (response.UsageMetadata is null) return null;

            mea.UsageDetails details = new()
            {
                InputTokenCount = response.UsageMetadata.PromptTokenCount,
                OutputTokenCount = response.UsageMetadata.CandidatesTokenCount,
                TotalTokenCount = response.UsageMetadata.TotalTokenCount
            };

            if (response.UsageMetadata.AudioDurationSeconds != 0)
            {
                (details.AdditionalCounts ??= [])[nameof(response.UsageMetadata.AudioDurationSeconds)] = response.UsageMetadata.AudioDurationSeconds;
            }

            if (response.UsageMetadata.CachedContentTokenCount != 0)
            {
                (details.AdditionalCounts ??= [])[nameof(response.UsageMetadata.CachedContentTokenCount)] = response.UsageMetadata.CachedContentTokenCount;
            }

            if (response.UsageMetadata.ThoughtsTokenCount != 0)
            {
                (details.AdditionalCounts ??= [])[nameof(response.UsageMetadata.ThoughtsTokenCount)] = response.UsageMetadata.ThoughtsTokenCount;
            }

            if (response.UsageMetadata.ToolUsePromptTokenCount != 0)
            {
                (details.AdditionalCounts ??= [])[nameof(response.UsageMetadata.ToolUsePromptTokenCount)] = response.UsageMetadata.ToolUsePromptTokenCount;
            }

            if (response.UsageMetadata.VideoDurationSeconds != 0)
            {
                (details.AdditionalCounts ??= [])[nameof(response.UsageMetadata.VideoDurationSeconds)] = response.UsageMetadata.VideoDurationSeconds;
            }

            return details;
        }

        /// <summary>
        /// Tries to find Mscc.GenerativeAI options in the additional properties and sets them.
        /// </summary>
        /// <param name="chatOptions">The chat options from the Microsoft abstraction</param>
        /// <param name="option">The Gemini setting to add</param>
        /// <param name="optionSetter">The <see cref="Action"/> to set the Gemini option, if available in the chat options</param>
        /// <typeparam name="T">The type of the option</typeparam>
        private static void TryAddOption<T>(mea.ChatOptions chatOptions, string option, Action<T> optionSetter)
        {
            if (chatOptions.AdditionalProperties?.TryGetValue(option, out var value) ?? false)
                optionSetter((T)value!);
        }
    }
}