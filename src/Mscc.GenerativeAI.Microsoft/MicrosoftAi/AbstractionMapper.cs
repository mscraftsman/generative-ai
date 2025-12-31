using Mscc.GenerativeAI.Types;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
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
		internal const int EmbeddingDimensions = 768;

		/// <summary>A thought signature that can be used to skip thought validation when sending foreign function calls.</summary>
		/// <remarks>
		/// See https://ai.google.dev/gemini-api/docs/thought-signatures#faqs.
		/// This is more common in agentic scenarios, where a chat history is built up across multiple providers/models.
		/// </remarks>
		private static readonly byte[] s_skipThoughtValidation = Encoding.UTF8.GetBytes("skip_thought_signature_validator");
		
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
			byte[]? thoughtSignature = null;
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

				Dictionary<string, string>? functionNames = null;

				foreach (var content in message.Contents)
				{
					if (content is mea.FunctionCallContent fc)
					{
						(functionNames ??= new())[fc.CallId] = fc.Name;
						functionNames[""] = fc.Name; // track last function name in case calls don't have IDs
					}
					
					Part? part = null;
					switch (content)
					{
						case mea.AIContent aic when aic.RawRepresentation is Part rawPart:
							part = rawPart;
							break;
						
						case mea.TextReasoningContent trc:
							part = new Part
							{
								Thought = true,
								Text = !string.IsNullOrWhiteSpace(trc.Text) ? trc.Text : null, 
							};
							break;

						case mea.TextContent tc:
							part = new Part { Text = tc.Text };
							break;

						case mea.DataContent dc:
							part = new Part
							{
								InlineData = new InlineData
								{
									Data = dc.Base64Data.ToString(), 
									MimeType = dc.MediaType,
									DisplayName = dc.Name
								}
							};
							break;

						case mea.UriContent uc:
							part = new Part
							{
								FileData = new FileData
								{
									FileUri = uc.Uri.AbsoluteUri, 
									MimeType = uc.MediaType
								}
							};
							break;

						case mea.FunctionCallContent fcc:
							(functionNames ??= new())[fcc.CallId] = fcc.Name;
							functionNames[""] = fcc.Name; // track last function name in case calls don't have IDs
							part = new Part
							{
								FunctionCall = new FunctionCall()
								{
									Id = fcc.CallId, 
									Name = fcc.Name, 
									Args = fcc.Arguments is null ? null : 
										fcc.Arguments as Dictionary<string, object> ?? new(fcc.Arguments!)
								},
							};
							break;

						case mea.FunctionResultContent frc:
							var functionName = frc.CallId;
							if (functionNames?.TryGetValue(frc.CallId, out string? name) is true ||
							    functionNames?.TryGetValue("", out name) is true)
							{
								functionName = name;
							}

							// If we receive anything other than a JsonElement that is an object, wrap it in an object { "result": jsonElement }
							if (frc.Result is not JsonElement { ValueKind: JsonValueKind.Object })
							{
								frc.Result = WrapInObject(frc.Result);
							}

							part = new Part
							{
								FunctionResponse = new FunctionResponse
								{
									Id = frc.CallId, 
									Name = functionName, 
									Response = frc.Result
								}
							};
							break;

						default:
							Debug.WriteLine($"This AIContent type is not supported yet: '{nameof(content)}'");
							break;
					}

					if (part is not null)
					{
						thoughtSignature = ToGeminiThoughtSignature(content);
						part.ThoughtSignature = thoughtSignature ?? s_skipThoughtValidation;
						// part.Thought = thoughtSignature is not null ? true : null;
						c.Parts.Add(part);
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
						request.GenerationConfig.ResponseSchema =
							jsonFormat.Schema is { } schema ? Schema.FromJsonElement(schema) : null;
				}

				if (options.AdditionalProperties?.Any() is true)
				{
					if (options.AdditionalProperties.TryGetValue(nameof(ToolConfig), out var objToolConfig) is true &&
					    objToolConfig is ToolConfig toolConfig)
					{
						request.ToolConfig ??= new();
						request.ToolConfig.RetrievalConfig = toolConfig.RetrievalConfig;
					}
					
					if (options.AdditionalProperties.TryGetValue(nameof(ThinkingConfig), out var thinkingConfig) &&
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

						if (thinkingConfigDict.TryGetValue("ThinkingLevel", out var thinkingLevel))
						{
							if (Enum.TryParse(thinkingLevel.ToString(), out ThinkingLevel level))
							{
								request.GenerationConfig.ThinkingConfig.ThinkingLevel = level;
							}
						}
					}
				}

				if (options.Tools is { } aiTools)
				{
					List<FunctionDeclaration>? functionDeclarations = null;
					foreach (var tool in aiTools)
					{
						switch (tool)
						{
							case GeminiChatClient.GeminiAITool<GoogleMaps> geminiAiTool:
								(request.Tools ??= []).Add(new Tool() { GoogleMaps = geminiAiTool.Tool });
								break;
							
							case GeminiChatClient.GeminiAITool<EnterpriseWebSearch> geminiAiTool:
								(request.Tools ??= []).Add(new Tool() { EnterpriseWebSearch = geminiAiTool.Tool });
								break;
							
							case GeminiChatClient.GeminiAITool<GoogleSearch> geminiAiTool:
								(request.Tools ??= []).Add(new Tool() { GoogleSearch = geminiAiTool.Tool });
								break;
							
							case GeminiChatClient.GeminiAITool<GoogleSearchRetrieval> geminiAiTool:
								(request.Tools ??= []).Add(new Tool() { GoogleSearchRetrieval = geminiAiTool.Tool });
								break;
							
							case GeminiChatClient.GeminiAITool<Retrieval> geminiAiTool:
								(request.Tools ??= []).Add(new Tool() { Retrieval = geminiAiTool.Tool });
								break;
							
							case GeminiChatClient.GeminiAITool<ComputerUse> geminiAiTool:
								(request.Tools ??= []).Add(new Tool() { ComputerUse = geminiAiTool.Tool });
								break;
							
							case GeminiChatClient.GeminiAITool<UrlContext> geminiAiTool:
								(request.Tools ??= []).Add(new Tool() { UrlContext = geminiAiTool.Tool });
								break;
							
							// case ToolAITool raw:
							// 	(request.Tools ??= []).Add(raw.Tool);
							// 	break;
							
							case mea.AIFunctionDeclaration aif:
								functionDeclarations ??= new();
								functionDeclarations.Add(new FunctionDeclaration
								{
									Name = aif.Name,
									Description = aif.Description,
									ParametersJsonSchema = aif.JsonSchema,
									// Parameters = ToGeminiSchema(aif.JsonSchema),
									// Response = aif.ReturnJsonSchema is { } rjs
									// 	? ToGeminiSchema(rjs)
									// 	: null,
								});
								break;

							case mea.HostedWebSearchTool wst:
								// ToDo: Differentiate between Google Search and Grounding in Vertex AI
								// Ref: https://github.com/dotnet/extensions/issues/7115
								List<string>? excludeDomains = null;
								if (wst.AdditionalProperties.TryGetValue("ExcludeDomains",
									    out object? objExcludeDomains))
								{
									excludeDomains = objExcludeDomains as List<string>;
								}

								BlockingConfidence? blockingConfidence = null;
								if (wst.AdditionalProperties.TryGetValue(nameof(BlockingConfidence),
									    out object? objBlockingConfidence))
								{
									if (Enum.TryParse((string)objBlockingConfidence,
										    out BlockingConfidence parsedBlockingConfidence))
									{
										blockingConfidence = parsedBlockingConfidence;
									}
								}

								var googleSearchTool = new Tool() { GoogleSearch = new() };
								googleSearchTool.GoogleSearch.ExcludeDomains = excludeDomains;
								googleSearchTool.GoogleSearch.BlockingConfidence = blockingConfidence;
								(request.Tools ??= []).Add(googleSearchTool);
								break;

							case mea.HostedCodeInterpreterTool cit:
								// ToDo: Consider HostedFileContent being used as inlined bytes (InlineData).
								// Ref: https://docs.cloud.google.com/vertex-ai/generative-ai/docs/multimodal/code-execution#googlegenaisdk_tools_code_exec_with_txt-drest
								var files = cit.Inputs?
									.OfType<mea.HostedFileContent>()
									.Select(f => f.FileId)
									.ToList() ?? [];
								
								(request.Tools ??= []).Add(new Tool() { CodeExecution = new() });
								break;
							
							case mea.HostedFileSearchTool fst:
								List<string> stores = fst.Inputs?
									.OfType<mea.HostedVectorStoreContent>()
									.Select(c => c.VectorStoreId)
									.ToList() ?? [];
								
								if (stores.Count == 0)
								{
									if (fst.AdditionalProperties.TryGetValue(nameof(FileSearchStore), out var storesObject))
									{
										stores = storesObject switch
										{
											List<string> ls => ls,
											string s => [s],
											_ => null
										};
									}
								}

								if (stores?.Count > 0)
								{
									(request.Tools ??= []).AddFileSearch(stores);
								}
								break;
							
							case mea.HostedImageGenerationTool igt:
								Debug.WriteLine($"This AITool type is not supported yet: '{nameof(tool)}'");
								break;
							
							case mea.HostedMcpServerTool mst:
								Debug.WriteLine($"This AITool type is not supported yet: '{nameof(tool)}'");
								break;
							
							default:
								Debug.WriteLine($"This AITool type is not supported yet: '{nameof(tool)}'");
								break;
						}
					}

					if (functionDeclarations is { Count: > 0 })
					{
						(request.Tools ??= []).Add(new Tool() { FunctionDeclarations = functionDeclarations });

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

				if (options.ResponseFormat is mea.ChatResponseFormatJson responseFormat)
				{
					request.GenerationConfig.ResponseMimeType = "application/json";
					if (responseFormat.Schema is { } schema)
					{
						request.GenerationConfig.ResponseJsonSchema = schema;
					}
				}
			}

			return request;
		}

		/// <summary>
		/// Retrieves the ThoughtSignature, if present.
		/// </summary>
		/// <param name="content">The <see cref="mea.AIContent"/> containing the ThoughtSignature.</param>
		/// <returns></returns>
		private static byte[]? ToGeminiThoughtSignature(mea.AIContent content)
		{
			byte[]? thoughtSignature = null;
			if (content is mea.TextReasoningContent trc)
			{
				return trc.ProtectedData is { } ? Convert.FromBase64String(trc.ProtectedData) : null;
			}

			if (content.AdditionalProperties?.TryGetValue("ThoughtSignature", out var sigObj) == true)
			{
				// Handle both string (in-memory) and JsonElement (after JSON deserialization)
				string? sigBase64 = sigObj switch
				{
					string s => s,
					JsonElement je when je.ValueKind == JsonValueKind.String => je.GetString(),
					_ => null
				};

				if (!string.IsNullOrEmpty(sigBase64))
				{
					thoughtSignature = Convert.FromBase64String(sigBase64);
				}
			}

			return thoughtSignature;
		}

		/// <summary>
		/// Wraps a value into a new <see cref="JsonElement"/> and the specified <paramref name="key"/>.
		/// </summary>
		/// <param name="value">The value to wrap.</param>
		/// <param name="key">Property to use as wrapper.</param>
		/// <returns></returns>
		private static JsonElement? WrapInObject(object? value, string key = "result")
		{
			if (value is null) return null;
			
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
				TryAddOption<int?>(options, "RetryInitial", v => retry.Initial = v ?? retry.Initial);
				TryAddOption<int?>(options, "RetryMultiplies", v => retry.Multiplies = v ?? retry.Multiplies);
				TryAddOption<int?>(options, "RetryMaximum", v => retry.Maximum = v ?? retry.Maximum);
				TryAddOption<int?>(options, "RetryTimeout", v =>
					retry.Timeout = v.HasValue ? TimeSpan.FromSeconds((double)v.Value) : retry.Timeout);
				TryAddOption<int[]?>(options, "RetryStatusCodes", v => retry.StatusCodes = v ?? retry.StatusCodes);
				TryAddOption<TimeSpan?>(options, "Timeout", v => timeout = v);

				if (retry.Initial > 0 && timeout is not null)
				{
					return new RequestOptions(retry, timeout);
				}
				else if (timeout is not null)
				{
					return new RequestOptions(timeout: timeout);
				}
				else
				{
					return new RequestOptions(retry: retry);
				}
			}

			return null;
		}

		/// <summary>
		/// Converts values and options to a <see cref="EmbedContentRequest"/>.
		/// </summary>
		/// <remarks>
		/// Currently, all models return 768-dimensional embeddings.
		/// See https://ai.google.dev/gemini-api/docs/models/gemini?#text-embedding
		/// </remarks>
		/// <param name="values">The values to get embeddings for</param>
		/// <param name="options">The options for the embeddings</param>
		public static EmbedContentRequest ToGeminiEmbedContentRequest(IEnumerable<string> values,
			mea.EmbeddingGenerationOptions? options)
		{
			var request = new EmbedContentRequest(values.ToList())
			{
				Model = options?.ModelId ?? null, // will be set GeminiApiClient.SelectedModel, if not set
				OutputDimensionality = options?.Dimensions ?? EmbeddingDimensions
			};
            if (request.Content != null)
            {
                request.Content.Role = Role.User;
            }
            return request;
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
		/// <param name="createdAt">The datetime when the response was created.</param>
		/// <returns></returns>
		public static mea.ChatResponse? ToChatResponse(GenerateContentResponse? response, DateTimeOffset createdAt)
		{
			if (response is null) return null;

			var chatMessage = ToChatMessage(response);

			return new mea.ChatResponse(chatMessage)
			{
				AdditionalProperties = chatMessage.AdditionalProperties,
				CreatedAt = chatMessage.CreatedAt ?? createdAt,
				FinishReason = ToChatFinishReason(response.Candidates?.FirstOrDefault()?.FinishReason),
				ModelId = response.ModelVersion,
				RawRepresentation = response,
				ResponseId = response.ResponseId,
				Usage = ToUsageDetails(response.UsageMetadata)
			};
		}

		/// <summary>
		/// Converts a <see cref="GenerateContentResponse"/> to a <see cref="mea.ChatResponseUpdate"/>.
		/// </summary>
		/// <param name="response">The response stream to convert.</param>
		/// <param name="createdAt">The datetime when the response was created.</param>
		/// <returns></returns>
		public static mea.ChatResponseUpdate? ToChatResponseUpdate(GenerateContentResponse? response, DateTimeOffset createdAt)
		{
			if (response is null) return null;

			var chatMessage = ToChatMessage(response);

			return new mea.ChatResponseUpdate(ToAbstractionRole(response.Candidates?.FirstOrDefault()?.Content?.Role),
				chatMessage.Contents)
			{
				AuthorName = chatMessage.AuthorName,
				AdditionalProperties = chatMessage.AdditionalProperties,
				CreatedAt = chatMessage.CreatedAt ?? createdAt,
				FinishReason = ToChatFinishReason(response.Candidates?.FirstOrDefault()?.FinishReason),
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
			mea.UsageDetails? usage = ToUsageDetails(response.UsageMetadata);
			var embeddings = new List<mea.Embedding<float>>();
			if (response.Embedding != null)
			{
				embeddings.Add(new mea.Embedding<float>(response.Embedding.Values?.ToArray() ?? [])
				{
					CreatedAt = DateTimeOffset.Now,
					ModelId = request.Model
				});
			}
			else if (response.Embeddings != null)
			{
				foreach (var embedding in response.Embeddings)
				{
					embeddings.Add(new mea.Embedding<float>(embedding.Values?.ToArray() ?? [])
					{
						CreatedAt = DateTimeOffset.Now,
						ModelId = request.Model
					});
				}
			}

			return new mea.GeneratedEmbeddings<mea.Embedding<float>>(embeddings) { AdditionalProperties = responseProps, Usage = usage };
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
					mea.AIContent content;
					
					if (!string.IsNullOrEmpty(part.Text))
						content = part.Thought is true
							? new mea.TextReasoningContent(part.Text)
							: new mea.TextContent(part.Text);
					else if (!string.IsNullOrEmpty(part.InlineData?.Data))
					{
						content = new mea.DataContent(
							Convert.FromBase64String(part.InlineData.Data),
							part.InlineData.MimeType ?? "application/octet-stream")
						{
							Name = part.InlineData.DisplayName
						};
					}
					else if (!string.IsNullOrEmpty(part.FileData?.FileUri))
					{
						content = new mea.UriContent(new Uri(part.FileData.FileUri),
							part.FileData.MimeType ?? "application/octet-stream");
					}
					else if (part.FunctionCall?.Name is not null)
					{
						content = ToFunctionCallContent(part.FunctionCall);
					}
					else if (part.FunctionResponse is not null)
					{
						content = ToFunctionResultContent(part.FunctionResponse);
					}
					else if (part.ExecutableCode?.Code is not null)
					{
						content = ToCodeInterpreterToolCallContent(part.ExecutableCode);
					}
					else if (part.CodeExecutionResult?.Output is not null)
					{
						content = ToCodeInterpreterToolResultContent(part.CodeExecutionResult);
					}
					else
					{
						Debug.WriteLine($"Part '{part.GetType()}' has not been mapped to AIContent. Using RawRepresentation only.");
						content = new mea.AIContent();
					}

					content.RawRepresentation = part;
					contents.Add(content);
					
					if (part.ThoughtSignature is { } thoughtSignature)
					{
						contents.Add(new mea.TextReasoningContent(null)
						{
							ProtectedData = Convert.ToBase64String(thoughtSignature),
						});
					}
				}
			}

			if (candidate.CitationMetadata is { Citations: { Count: > 0 } citations }
			    && contents.OfType<mea.TextContent>().FirstOrDefault() is mea.TextContent textContent)
			{
				foreach (var citation in citations)
				{
					textContent.Annotations = new List<mea.AIAnnotation>()
					{
						new mea.CitationAnnotation()
						{
							Title = citation.Title,
							Url = Uri.TryCreate(citation.Uri, UriKind.Absolute, out Uri? uri) ? uri : null,
							AnnotatedRegions = new List<mea.AnnotatedRegion>()
							{
								new mea.TextSpanAnnotatedRegion()
								{
									StartIndex = citation.StartIndex, 
									EndIndex = citation.EndIndex,
								}
							}
						}
					};
				}
			}

			if (response.PromptFeedback is { } promptFeedback)
			{
				contents.Add(new mea.ErrorContent(promptFeedback.BlockReasonMessage));
			}
			
			return new mea.ChatMessage(ToAbstractionRole(response.Candidates?.FirstOrDefault()?.Content?.Role),
				contents)
			{
				CreatedAt = response.CreateTime, MessageId = response.ResponseId, RawRepresentation = response
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
			return new mea.FunctionResultContent(functionResponse.Id ?? "", functionResponse.Response);
			// var content = new mea.FunctionResultContent(
			// 	functionResponse.Id ?? "",
			// 	functionResponse.Response?.TryGetValue("output", out var output) is true ? output :
			// 	functionResponse.Response?.TryGetValue("error", out var error) is true ? error :
			// 	null);
			// return content;
		}

		/// <summary>
		/// Maps a <see cref="ExecutableCode"/> to a <see cref="mea.CodeInterpreterToolCallContent"/>.
		/// </summary>
		/// <param name="executableCode"></param>
		/// <returns></returns>
		private static mea.CodeInterpreterToolCallContent ToCodeInterpreterToolCallContent(
			ExecutableCode executableCode)
		{
			var content = new mea.CodeInterpreterToolCallContent()
			{
				Inputs = new List<mea.AIContent>()
				{
					new mea.DataContent(Encoding.UTF8.GetBytes(executableCode.Code),
						executableCode.Language switch
						{
							ExecutableCode.LanguageType.Python => "text/x-python",
							_ => "text/x-source-code",
						})
				}
			};
			return content;
		}

		/// <summary>
		/// Maps a <see cref="CodeExecutionResult"/> to a <see cref="mea.CodeInterpreterToolResultContent"/>.
		/// </summary>
		/// <param name="codeExecutionResult"></param>
		/// <returns></returns>
		private static mea.CodeInterpreterToolResultContent ToCodeInterpreterToolResultContent(
			CodeExecutionResult codeExecutionResult)
		{
			var content = new mea.CodeInterpreterToolResultContent()
			{
				Outputs = new List<mea.AIContent>()
				{
					codeExecutionResult.Outcome is Outcome.OutcomeOk
						? new mea.TextContent(codeExecutionResult.Output)
						: new mea.ErrorContent(codeExecutionResult.Output)
						{
							ErrorCode = codeExecutionResult.Outcome.ToString()
						}
				}
			};
			return content;
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
		private static mea.ChatFinishReason? ToChatFinishReason(FinishReason? finishReason)
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
		/// <param name="usageMetadata">The usage metadata of the response to parse.</param>
		/// <returns>A <see cref="mea.UsageDetails"/> instance containing the parsed usage details.</returns>
		private static mea.UsageDetails? ToUsageDetails(UsageMetadata? usageMetadata)
		{
			if (usageMetadata is null) return null;

			mea.UsageDetails details = new()
			{
				InputTokenCount = usageMetadata.PromptTokenCount,
				OutputTokenCount = (usageMetadata.CandidatesTokenCount ?? 0) +
				                   (usageMetadata.ThoughtsTokenCount ?? 0),
				TotalTokenCount = usageMetadata.TotalTokenCount,
				CachedInputTokenCount = usageMetadata.CachedContentTokenCount,
				ReasoningTokenCount = usageMetadata.ThoughtsTokenCount
			};

			if (usageMetadata.ToolUsePromptTokenCount is { } tc)
			{
				(details.AdditionalCounts ??= [])[nameof(usageMetadata.ToolUsePromptTokenCount)] = tc;
			}

			if (usageMetadata.AudioDurationSeconds is { } ads)
			{
				(details.AdditionalCounts ??= [])[nameof(usageMetadata.AudioDurationSeconds)] = ads;
			}

			if (usageMetadata.VideoDurationSeconds is { } vds)
			{
				(details.AdditionalCounts ??= [])[nameof(usageMetadata.VideoDurationSeconds)] = vds;
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