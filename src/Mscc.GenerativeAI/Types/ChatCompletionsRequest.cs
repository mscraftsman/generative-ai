using System.Collections.Generic;

namespace Mscc.GenerativeAI
{
    /// <summary>
    /// Request for chat completions.
    /// </summary>
    public class ChatCompletionsRequest
    {
        /// <summary>
        /// Required. The name of the `Model` to use for generating the completion.
        /// The model name will prefixed by \"models/\" if no slash appears in it.
        /// </summary>
        public string Model { get; set; }
        /// <summary>
        /// Required. The chat history to use for generating the completion.
        /// Supports single and multi-turn queries.
        /// Note: This is a polymorphic field, it is deserialized to a InternalChatMessage.
        /// </summary>
        public List<object> Messages { get; set; }
        /// <summary>
        /// Optional. The maximum number of tokens to include in a response candidate.
        /// Must be a positive integer.
        /// </summary>
        public int? MaxCompletionTokens { get; set; }
        /// <summary>
        /// Optional. The maximum number of tokens to include in a response candidate.
        /// Must be a positive integer. This field is deprecated by the SDK.
        /// </summary>
        public int? MaxTokens { get; set; }
        /// <summary>
        /// Optional. Amount of candidate completions to generate.
        /// Must be a positive integer. Defaults to 1 if not set.
        /// </summary>
        public int? N { get; set; }
        /// <summary>
        /// Optional. Defines the format of the response. If not set, the response will be formatted as text.
        /// </summary>
        public ResponseFormat? ResponseFormat { get; set; }
        /// <summary>
        /// Optional. The set of character sequences that will stop output generation.
        /// Note: This is a polymorphic field. It is meant to contain a string or repeated strings.
        /// </summary>
        public string?[] Stop { get; set; }
        /// <summary>
        /// Optional. Whether to stream the response or return a single response.
        /// If true, the \"object\" field in the response will be \"chat.completion.chunk\".
        /// Otherwise it will be \"chat.completion\".
        /// </summary>
        public bool? Stream { get; set; }
        /// <summary>
        /// Optional. Options for streaming requests.
        /// </summary>
        public StreamOptions? StreamOptions { get; set; }
        /// <summary>
        /// Optional. Controls the randomness of the output.
        /// </summary>
        public float? Temperature { get; set; }
        /// <summary>
        /// Optional. The maximum cumulative probability of tokens to consider when sampling.
        /// </summary>
        public float? TopP { get; set; }
        /// <summary>
        /// Optional. Controls whether the model should use a tool or not, and which tool to use.
        /// Can be either:
        /// - The string \"none\", to disable tools.
        /// - The string \"auto\", to let the model decide.
        /// - The string \"required\", to force the model to use a tool.
        /// - A function name descriptor object, specifying the tool to use. The last option follows the following schema: { \"type\": \"function\", \"function\": {\"name\" : \"the_function_name\"} }
        /// </summary>
        public string? ToolChoice { get; set; }
        /// <summary>
        /// Optional. The set of tools the model can generate calls for.
        /// Each tool declares its signature.
        /// </summary>
        public List<ChatTool>? Tools { get; set; }
        /// <summary>
        /// Optional. Options for audio generation.
        /// </summary>
        public AudioOptions? Audio { get; set; }
        /// <summary>
        /// Optional. Modalities for the request.
        /// </summary>
        public string? Modalities { get; set; }
        /// <summary>
        /// Optional. Whether to call tools in parallel.
        /// </summary>
        /// <remarks>
        /// Included here for compatibility with the SDK, but only false is supported.
        /// </remarks>
        public bool? ParallelToolCalls { get; set; }
        /// <summary>
        /// Optional. Penalizes new tokens based on previous appearances. Valid ranges are [-2, 2]. Default is 0.
        /// </summary>
        public float? PresencePenalty { get; set; }
        /// <summary>
        /// Optional. The user name used for tracking the request. Not used, only for compatibility with the SDK.
        /// </summary>
        public string? User { get; set; }
    }
}