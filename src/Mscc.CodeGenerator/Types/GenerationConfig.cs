namespace Mscc.GenerativeAI.Types
{
	/// <summary>
	/// Configuration options for model generation and outputs. Not all parameters are configurable for every model.
	/// </summary>
	public partial class GenerationConfig
	{
		/// <summary>
		/// Optional. Output schema of the generated response. This is an alternative to <see cref="response_schema"/> that accepts [JSON Schema](https://json-schema.org/). If set, <see cref="response_schema"/> must be omitted, but <see cref="response_mime_type"/> is required. While the full JSON Schema may be sent, not all features are supported. Specifically, only the following properties are supported: - <see cref="$id"/> - <see cref="$defs"/> - <see cref="$ref"/> - <see cref="$anchor"/> - <see cref="type"/> - <see cref="format"/> - <see cref="title"/> - <see cref="description"/> - <see cref="enum"/> (for strings and numbers) - <see cref="items"/> - <see cref="prefixItems"/> - <see cref="minItems"/> - <see cref="maxItems"/> - <see cref="minimum"/> - <see cref="maximum"/> - <see cref="anyOf"/> - <see cref="oneOf"/> (interpreted the same as <see cref="anyOf"/>) - <see cref="properties"/> - <see cref="additionalProperties"/> - <see cref="required"/> The non-standard <see cref="propertyOrdering"/> property may also be set. Cyclic references are unrolled to a limited degree and, as such, may only be used within non-required properties. (Nullable properties are not sufficient.) If <see cref="$ref"/> is set on a sub-schema, no other properties, except for than those starting as a <see cref="$"/>, may be set.
		/// </summary>
		public object? Responsejsonschema { get; set; }
		/// <summary>
		/// Optional. Number of generated responses to return. If unset, this will default to 1. Please note that this doesn't work for previous generation models (Gemini 1.0 family)
		/// </summary>
		public int? CandidateCount { get; set; }
		/// <summary>
		/// Optional. Enables enhanced civic answers. It may not be available for all models.
		/// </summary>
		public bool? EnableEnhancedCivicAnswers { get; set; }
		/// <summary>
		/// Optional. Frequency penalty applied to the next token's logprobs, multiplied by the number of times each token has been seen in the respponse so far. A positive penalty will discourage the use of tokens that have already been used, proportional to the number of times the token has been used: The more a token is used, the more difficult it is for the model to use that token again increasing the vocabulary of responses. Caution: A _negative_ penalty will encourage the model to reuse tokens proportional to the number of times the token has been used. Small negative values will reduce the vocabulary of a response. Larger negative values will cause the model to start repeating a common token until it hits the max_output_tokens limit.
		/// </summary>
		public double? FrequencyPenalty { get; set; }
		/// <summary>
		/// Optional. Config for image generation. An error will be returned if this field is set for models that don't support these config options.
		/// </summary>
		public ImageConfig? ImageConfig { get; set; }
		/// <summary>
		/// Optional. Only valid if response_logprobs=True. This sets the number of top logprobs to return at each decoding step in the Candidate.logprobs_result. The number must be in the range of [0, 20].
		/// </summary>
		public int? Logprobs { get; set; }
		/// <summary>
		/// Optional. The maximum number of tokens to include in a response candidate. Note: The default value varies by model, see the <see cref="Model.output_token_limit"/> attribute of the <see cref="Model"/> returned from the <see cref="getModel"/> function.
		/// </summary>
		public int? MaxOutputTokens { get; set; }
		/// <summary>
		/// Optional. If specified, the media resolution specified will be used.
		/// </summary>
		public MediaResolution? MediaResolution { get; set; }
		/// <summary>
		/// Optional. Presence penalty applied to the next token's logprobs if the token has already been seen in the response. This penalty is binary on/off and not dependant on the number of times the token is used (after the first). Use frequency_penalty for a penalty that increases with each use. A positive penalty will discourage the use of tokens that have already been used in the response, increasing the vocabulary. A negative penalty will encourage the use of tokens that have already been used in the response, decreasing the vocabulary.
		/// </summary>
		public double? PresencePenalty { get; set; }
		/// <summary>
		/// Optional. An internal detail. Use <see cref="responseJsonSchema"/> rather than this field.
		/// </summary>
		public object? ResponseJsonSchema { get; set; }
		/// <summary>
		/// Optional. If true, export the logprobs results in response.
		/// </summary>
		public bool? ResponseLogprobs { get; set; }
		/// <summary>
		/// Optional. MIME type of the generated candidate text. Supported MIME types are: <see cref="text/plain"/>: (default) Text output. <see cref="application/json"/>: JSON response in the response candidates. <see cref="text/x.enum"/>: ENUM as a string response in the response candidates. Refer to the [docs](https://ai.google.dev/gemini-api/docs/prompting_with_media#plain_text_formats) for a list of all supported text MIME types.
		/// </summary>
		public string? ResponseMimeType { get; set; }
		/// <summary>
		/// Optional. The requested modalities of the response. Represents the set of modalities that the model can return, and should be expected in the response. This is an exact match to the modalities of the response. A model may have multiple combinations of supported modalities. If the requested modalities do not match any of the supported combinations, an error will be returned. An empty list is equivalent to requesting only text.
		/// </summary>
		public List<ResponseModalities>? ResponseModalities { get; set; }
		/// <summary>
		/// Optional. Output schema of the generated candidate text. Schemas must be a subset of the [OpenAPI schema](https://spec.openapis.org/oas/v3.0.3#schema) and can be objects, primitives or arrays. If set, a compatible <see cref="response_mime_type"/> must also be set. Compatible MIME types: <see cref="application/json"/>: Schema for JSON response. Refer to the [JSON text generation guide](https://ai.google.dev/gemini-api/docs/json-mode) for more details.
		/// </summary>
		public Schema? ResponseSchema { get; set; }
		/// <summary>
		/// Optional. Seed used in decoding. If not set, the request uses a randomly generated seed.
		/// </summary>
		public int? Seed { get; set; }
		/// <summary>
		/// Optional. The speech generation config.
		/// </summary>
		public SpeechConfig? SpeechConfig { get; set; }
		/// <summary>
		/// Optional. The set of character sequences (up to 5) that will stop output generation. If specified, the API will stop at the first appearance of a <see cref="stop_sequence"/>. The stop sequence will not be included as part of the response.
		/// </summary>
		public List<string>? StopSequences { get; set; }
		/// <summary>
		/// Optional. Controls the randomness of the output. Note: The default value varies by model, see the <see cref="Model.temperature"/> attribute of the <see cref="Model"/> returned from the <see cref="getModel"/> function. Values can range from [0.0, 2.0].
		/// </summary>
		public double? Temperature { get; set; }
		/// <summary>
		/// Optional. Config for thinking features. An error will be returned if this field is set for models that don't support thinking.
		/// </summary>
		public ThinkingConfig? ThinkingConfig { get; set; }
		/// <summary>
		/// Optional. The maximum number of tokens to consider when sampling. Gemini models use Top-p (nucleus) sampling or a combination of Top-k and nucleus sampling. Top-k sampling considers the set of <see cref="top_k"/> most probable tokens. Models running with nucleus sampling don't allow top_k setting. Note: The default value varies by <see cref="Model"/> and is specified by the<see cref="Model.top_p"/> attribute returned from the <see cref="getModel"/> function. An empty <see cref="top_k"/> attribute indicates that the model doesn't apply top-k sampling and doesn't allow setting <see cref="top_k"/> on requests.
		/// </summary>
		public int? TopK { get; set; }
		/// <summary>
		/// Optional. The maximum cumulative probability of tokens to consider when sampling. The model uses combined Top-k and Top-p (nucleus) sampling. Tokens are sorted based on their assigned probabilities so that only the most likely tokens are considered. Top-k sampling directly limits the maximum number of tokens to consider, while Nucleus sampling limits the number of tokens based on the cumulative probability. Note: The default value varies by <see cref="Model"/> and is specified by the<see cref="Model.top_p"/> attribute returned from the <see cref="getModel"/> function. An empty <see cref="top_k"/> attribute indicates that the model doesn't apply top-k sampling and doesn't allow setting <see cref="top_k"/> on requests.
		/// </summary>
		public double? TopP { get; set; }
    }
}
