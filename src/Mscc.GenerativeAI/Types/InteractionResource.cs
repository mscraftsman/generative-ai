using Mscc.GenerativeAI.Types;
using System;
using System.Collections.Generic;
using System.Linq;
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
		[JsonIgnore]
		public List<InteractionContent>? InputListContent { get; set; }
		[JsonIgnore]
		public List<InteractionTurn>? InputListTurn { get; set; }
		[JsonIgnore]
		public InteractionContent? InputContent { get; set; }
		[JsonIgnore]
		public string? InputString { get; set; }
		public string? SystemInstruction { get; set; }
		public Tools? Tools { get; set; }
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
        public string? Status { get; set; }	// enum State?
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
				        .Where(p => p.Type is "text")
				        .Select(x => x.Text)
				        .ToArray()!);
	        }
        }
    }
}

public partial class InteractionContent
{
    public string? Type { get; set; }
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
    public string? CallId { get; set; }
    public string? ServerName { get; set; }
    
    public object? Annotations { get; set; }
    public object? Summary { get; set; }
}

public partial class InteractionTurn
{
    public string? Role { get; set; }
    public List<InteractionContent>? Content { get; set; }
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

public partial class ModalityTokens
{
	public Modality? Modality { get; set; }
	public int? Tokens { get; set; }
}