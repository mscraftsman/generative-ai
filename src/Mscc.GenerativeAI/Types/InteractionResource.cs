using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Mscc.GenerativeAI.Types
{
	public partial class InteractionRequest
	{
		public string? Model { get; set; }
		public string? Agent { get; set; }
		public object? Input => InputListContent ??
		                        InputListTurn ??
		                        (object?)InputContent ??
		                        InputString;
		[JsonIgnore] public List<InteractionContent>? InputListContent { get; set; }
		[JsonIgnore] public List<InteractionTurn>? InputListTurn { get; set; }
		[JsonIgnore] public InteractionContent? InputContent { get; set; }
		[JsonIgnore] public string? InputString { get; set; }
		public string? SystemInstruction { get; set; }
		public InteractionTools? Tools { get; set; }
		public object? ResponseFormat { get; set; }
		public string? ResponseMimeType { get; set; }
		public bool? Stream { get; set; }
		public bool? Store { get; set; }
		public bool? Background { get; set; }
		public GenerationConfig? GenerationConfig { get; set; }
		public object? AgentConfig { get; set; }
		public string? PreviousInteractionId { get; set; }
		public List<ResponseModality>? ResponseModalities { get; set; }
	}

	public partial class InteractionResource : BaseLogger
	{
		public string? Model { get; set; }
		public string? Agent { get; set; }
		public string? Id { get; set; }
		public string? Object { get; set; }
		public List<InteractionContent>? Outputs { get; set; }
		public string? Role { get; set; }
		public string? Status { get; set; } // enum State?
		// public bool? Store { get; set; }
		public DateTime? Created { get; set; }
		public DateTime? Updated { get; set; }
		public Usage? Usage { get; set; }
		public string? PreviousInteractionId { get; set; }

		/// <summary>
		/// A convenience property to get the responded text information of first candidate.
		/// </summary>
		[JsonIgnore]
		public string? Text
		{
			get
			{
				if (Outputs is null) return string.Empty;
				if (Outputs?.Count == 0) return string.Empty;

				return string.Join(Environment.NewLine,
					Outputs?
						.Where(p => p.Type is InteractionContentType.Text)
						.Select(x => x.Text)
						.ToArray()!);
			}
		}
	}

	[DebuggerDisplay("Type: {Type,nq}")]
	public partial class InteractionContent
	{
		public InteractionContentType? Type { get; set; }
		public string? Text { get; set; }
		public string? Signature { get; set; }
		public string? Data { get; set; }
		public string? Uri { get; set; }
		public string? MimeType { get; set; }
		public MediaResolution? Resolution { get; set; }
		public string? Name { get; set; }
		public string? Id { get; set; }
		public object? Arguments { get; set; }
		public bool? IsError { get; set; }
		public object? Result { get; set; }
		[JsonPropertyName("call_id")] public string? CallId { get; set; }
		public string? ServerName { get; set; }
		public object? Annotations { get; set; }
		public object? Summary { get; set; }
	}

	public enum InteractionContentType
	{
		Text,
		Image,
		Audio,
		Document,
		Video,
		Thought,
		ThoughtSignature,
		FunctionCall,
		FunctionResult,
		CodeExecutionCall,
		CodeExecutionResult,
		UrlContextCall,
		UrlContextResult,
		GoogleSearchCall,
		GoogleSearchResult,
		McpServerToolCall,
		McpServerToolResult,
		FileSearchResult
	}

	public partial class InteractionTurn
	{
		public string? Role { get; set; }

		public object? Content
		{
			get
			{
				return ContentList ??
				       (object?)ContentContent ??
				       ContentString;
			}
			set
			{
				switch (value)
				{
					case List<InteractionContent> lic:
						ContentList = lic;
						break;
					case InteractionContent ic:
						ContentContent = ic;
						break;
					default:
						ContentString = Convert.ToString(value);
						break;
				}
			}
		}

		[JsonIgnore] public List<InteractionContent>? ContentList { get; set; }
		[JsonIgnore] public InteractionContent? ContentContent { get; set; }
		[JsonIgnore] public string? ContentString { get; set; }
	}

	public partial class Usage
	{
		public int? TotalInputTokens { get; set; }
		public List<ModalityTokens>? InputTokensByModality { get; set; }
		public int? TotalCachedTokens { get; set; }
		public List<ModalityTokens>? CachedTokensByModality { get; set; }
		public int? TotalOutputTokens { get; set; }
		public List<ModalityTokens>? OutputTokensByModality { get; set; }
		public int? TotalToolUseTokens { get; set; }
		public List<ModalityTokens>? ToolUseTokensByModality { get; set; }
		public int? TotalReasoningTokens { get; set; }
		public int? TotalTokens { get; set; }
	}

	[DebuggerDisplay("Modality: {Modality,nq} - Tokens: {Tokens,nq}")]
	public partial class ModalityTokens
	{
		public ResponseModality? Modality { get; set; }
		public int? Tokens { get; set; }
	}

	[DebuggerDisplay("Type: {EventType,nq}")]
	public partial class InteractionSseEvent
	{
		public string? EventType { get; set; }
		public string? EventId { get; set; }
		public InteractionResource? Interaction { get; set; }
		public string? InteractionId { get; set; }
		public string? Status { get; set; } // enum State?
		public int? Index { get; set; }
		public InteractionContent? Content { get; set; }
		public InteractionContent? Delta { get; set; }
		public InteractionError? Error { get; set; }
	}

	public partial class InteractionError
	{
		public string? Code { get; set; }
		public string? Message { get; set; }
	}

	[DebuggerDisplay("Type: {Type,nq}")]
	public partial class InteractionTool
	{
		public InteractionToolType? Type { get; set; }
		public string? Name { get; set; }
		public string? Description { get; set; }
		public object? Parameters { get; set; }
		public ComputerUseEnvironment? Environment { get; set; }
		public List<string>? ExcludedPredefinedFunctions { get; set; }
		public string? Url { get; set; }

		public object? Headers { get; set; }

		// public AllowedTools? AllowedTools { get; set; }
		public List<string>? FileSearchStoreNames { get; set; }
		public int? TopK { get; set; }
		public string? MetadataFilter { get; set; }
	}

	public partial class InteractionTools : List<InteractionTool>
	{
		private readonly Dictionary<string, Delegate> _tools = new();

		public void Add(Delegate function, string? name = null, string? description = null)
		{
			if (function == null) throw new ArgumentNullException(nameof(function));

			var methodInfo = function.Method;
			name ??= methodInfo.Name;
			name = SanitizeFunctionName(name);

			var tool = new InteractionTool
			{
				Type = InteractionToolType.Function,
				Name = name,
				Description = description ?? $"Function {name}",
				Parameters = Schema.BuildParametersSchemaFromDelegate(function)
			};

			_tools[name] = function;
			base.Add(tool);
		}

		public object? Invoke(string name, object arguments)
		{
			if (!_tools.TryGetValue(name, out var function))
			{
				throw new ArgumentException($"Function '{name}' not found.");
			}

			var methodInfo = function.Method;
			var parameters = methodInfo.GetParameters();
			var args = new object?[parameters.Length];

			if (arguments is JsonElement jsonElement && jsonElement.ValueKind == JsonValueKind.Object)
			{
				for (int i = 0; i < parameters.Length; i++)
				{
					var param = parameters[i];
					if (jsonElement.TryGetProperty(param.Name!, out var prop))
					{
						args[i] = JsonSerializer.Deserialize(prop.GetRawText(), param.ParameterType);
					}
				}
			}
			else if (arguments is Dictionary<string, object> dict)
			{
				for (int i = 0; i < parameters.Length; i++)
				{
					var param = parameters[i];
					if (dict.TryGetValue(param.Name!, out var value))
					{
						if (value is JsonElement je)
						{
							args[i] = JsonSerializer.Deserialize(je.GetRawText(), param.ParameterType);
						}
						else
						{
							args[i] = Convert.ChangeType(value, param.ParameterType);
						}
					}
				}
			}

			return function.DynamicInvoke(args);
		}

		private static string SanitizeFunctionName(string? name)
		{
			if (string.IsNullOrWhiteSpace(name))
			{
				return "_function";
			}

			// Allowed: letters, digits, underscore, dash. Must start with letter or underscore. Max 63.
			StringBuilder sb = new(name.Length);
			foreach (char ch in name)
			{
				if (char.IsLetterOrDigit(ch) || ch == '_' || ch == '-')
				{
					sb.Append(ch);
				}
				else if (sb.Length == 0 || sb[sb.Length - 1] != '_')
				{
					sb.Append('_');
				}
			}

			string sanitized = sb.ToString();

			if (sanitized.Length == 0)
				sanitized = "_function";

			if (!char.IsLetter(sanitized[0]) && sanitized[0] != '_')
			{
				sanitized = "_" + sanitized;
			}

			if (sanitized.Length > 63)
			{
				sanitized = sanitized.Substring(0, 63);
			}

			return sanitized;
		}
	}

	public enum InteractionToolType
	{
		Function,
		GoogleSearch,
		CodeExecution,
		UrlContext,
		ComputerUse,
		McpServer,
		FileSearch
	}
}