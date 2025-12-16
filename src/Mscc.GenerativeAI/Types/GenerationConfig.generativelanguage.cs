/*
 * Copyleft 2024-2025 Jochen Kirstätter and contributors
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *      https://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */
using System.Collections.Generic;
using System.Text.Json.Serialization;

// *** AUTO-GENERATED FILE - DO NOT EDIT MANUALLY *** //

namespace Mscc.GenerativeAI.Types
{
	/// <summary>
	/// Configuration options for model generation and outputs. Not all parameters are configurable for every model.
	/// </summary>
	public partial class GenerationConfig
	{
		/// <summary>
		/// Optional. Output schema of the generated response. This is an alternative to <c>response_schema</c> that accepts [JSON Schema](https://json-schema.org/). If set, <c>response_schema</c> must be omitted, but <c>response_mime_type</c> is required. While the full JSON Schema may be sent, not all features are supported. Specifically, only the following properties are supported: - <c>$id</c> - <c>$defs</c> - <c>$ref</c> - <c>$anchor</c> - <c>type</c> - <c>format</c> - <c>title</c> - <c>description</c> - <c>enum</c> (for strings and numbers) - <c>items</c> - <c>prefixItems</c> - <c>minItems</c> - <c>maxItems</c> - <c>minimum</c> - <c>maximum</c> - <c>anyOf</c> - <c>oneOf</c> (interpreted the same as <c>anyOf</c>) - <c>properties</c> - <c>additionalProperties</c> - <c>required</c> The non-standard <c>propertyOrdering</c> property may also be set. Cyclic references are unrolled to a limited degree and, as such, may only be used within non-required properties. (Nullable properties are not sufficient.) If <c>$ref</c> is set on a sub-schema, no other properties, except for than those starting as a <c>$</c>, may be set.
		/// </summary>
		public object? _responseJsonSchema { get; set; }
		/// <summary>
		/// Optional. Number of generated responses to return. If unset, this will default to 1. Please note that this doesn&apos;t work for previous generation models (Gemini 1.0 family)
		/// </summary>
		public int? CandidateCount { get; set; }
		/// <summary>
		/// Optional. If enabled, the model will detect emotions and adapt its responses accordingly.
		/// </summary>
		public bool? EnableAffectiveDialog { get; set; }
		/// <summary>
		/// Optional. Enables enhanced civic answers. It may not be available for all models.
		/// </summary>
		public bool? EnableEnhancedCivicAnswers { get; set; }
		/// <summary>
		/// Optional. Frequency penalty applied to the next token&apos;s logprobs, multiplied by the number of times each token has been seen in the respponse so far. A positive penalty will discourage the use of tokens that have already been used, proportional to the number of times the token has been used: The more a token is used, the more difficult it is for the model to use that token again increasing the vocabulary of responses. Caution: A _negative_ penalty will encourage the model to reuse tokens proportional to the number of times the token has been used. Small negative values will reduce the vocabulary of a response. Larger negative values will cause the model to start repeating a common token until it hits the max_output_tokens limit.
		/// </summary>
		public double? FrequencyPenalty { get; set; }
		/// <summary>
		/// Optional. Config for image generation. An error will be returned if this field is set for models that don&apos;t support these config options.
		/// </summary>
		public ImageConfig? ImageConfig { get; set; }
		/// <summary>
		/// Optional. Only valid if response_logprobs=True. This sets the number of top logprobs to return at each decoding step in the Candidate.logprobs_result. The number must be in the range of [0, 20].
		/// </summary>
		public int? Logprobs { get; set; }
		/// <summary>
		/// Optional. The maximum number of tokens to include in a response candidate. Note: The default value varies by model, see the <c>Model.output_token_limit</c> attribute of the <c>Model</c> returned from the <c>getModel</c> function.
		/// </summary>
		public int? MaxOutputTokens { get; set; }
		/// <summary>
		/// Optional. If specified, the media resolution specified will be used.
		/// </summary>
		public MediaResolutionType? MediaResolution { get; set; }
		/// <summary>
		/// Optional. Presence penalty applied to the next token&apos;s logprobs if the token has already been seen in the response. This penalty is binary on/off and not dependant on the number of times the token is used (after the first). Use frequency_penalty for a penalty that increases with each use. A positive penalty will discourage the use of tokens that have already been used in the response, increasing the vocabulary. A negative penalty will encourage the use of tokens that have already been used in the response, decreasing the vocabulary.
		/// </summary>
		public double? PresencePenalty { get; set; }
		/// <summary>
		/// Optional. An internal detail. Use <c>responseJsonSchema</c> rather than this field.
		/// </summary>
		public object? ResponseJsonSchema { get; set; }
		/// <summary>
		/// Optional. If true, export the logprobs results in response.
		/// </summary>
		public bool? ResponseLogprobs { get; set; }
		/// <summary>
		/// Optional. MIME type of the generated candidate text. Supported MIME types are: <c>text/plain</c>: (default) Text output. <c>application/json</c>: JSON response in the response candidates. <c>text/x.enum</c>: ENUM as a string response in the response candidates. Refer to the [docs](https://ai.google.dev/gemini-api/docs/prompting_with_media#plain_text_formats) for a list of all supported text MIME types.
		/// </summary>
		public string? ResponseMimeType { get; set; }
		/// <summary>
		/// Optional. The requested modalities of the response. Represents the set of modalities that the model can return, and should be expected in the response. This is an exact match to the modalities of the response. A model may have multiple combinations of supported modalities. If the requested modalities do not match any of the supported combinations, an error will be returned. An empty list is equivalent to requesting only text.
		/// </summary>
		public List<ResponseModality>? ResponseModalities { get; set; }
		/// <summary>
		/// Optional. Output schema of the generated candidate text. Schemas must be a subset of the [OpenAPI schema](https://spec.openapis.org/oas/v3.0.3#schema) and can be objects, primitives or arrays. If set, a compatible <c>response_mime_type</c> must also be set. Compatible MIME types: <c>application/json</c>: Schema for JSON response. Refer to the [JSON text generation guide](https://ai.google.dev/gemini-api/docs/json-mode) for more details.
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
		/// Optional. The set of character sequences (up to 5) that will stop output generation. If specified, the API will stop at the first appearance of a <c>stop_sequence</c>. The stop sequence will not be included as part of the response.
		/// </summary>
		public List<string>? StopSequences { get; set; }
		/// <summary>
		/// Optional. Controls the randomness of the output. Note: The default value varies by model, see the <c>Model.temperature</c> attribute of the <c>Model</c> returned from the <c>getModel</c> function. Values can range from [0.0, 2.0].
		/// </summary>
		public double? Temperature { get; set; }
		/// <summary>
		/// Optional. Config for thinking features. An error will be returned if this field is set for models that don&apos;t support thinking.
		/// </summary>
		public ThinkingConfig? ThinkingConfig { get; set; }
		/// <summary>
		/// Optional. The maximum number of tokens to consider when sampling. Gemini models use Top-p (nucleus) sampling or a combination of Top-k and nucleus sampling. Top-k sampling considers the set of <c>top_k</c> most probable tokens. Models running with nucleus sampling don&apos;t allow top_k setting. Note: The default value varies by <c>Model</c> and is specified by the<c>Model.top_p</c> attribute returned from the <c>getModel</c> function. An empty <c>top_k</c> attribute indicates that the model doesn&apos;t apply top-k sampling and doesn&apos;t allow setting <c>top_k</c> on requests.
		/// </summary>
		public int? TopK { get; set; }
		/// <summary>
		/// Optional. The maximum cumulative probability of tokens to consider when sampling. The model uses combined Top-k and Top-p (nucleus) sampling. Tokens are sorted based on their assigned probabilities so that only the most likely tokens are considered. Top-k sampling directly limits the maximum number of tokens to consider, while Nucleus sampling limits the number of tokens based on the cumulative probability. Note: The default value varies by <c>Model</c> and is specified by the<c>Model.top_p</c> attribute returned from the <c>getModel</c> function. An empty <c>top_k</c> attribute indicates that the model doesn&apos;t apply top-k sampling and doesn&apos;t allow setting <c>top_k</c> on requests.
		/// </summary>
		public double? TopP { get; set; }

		[JsonConverter(typeof(JsonStringEnumConverter<MediaResolutionType>))]
		public enum MediaResolutionType
		{
			/// <summary>
			/// Media resolution has not been set.
			/// </summary>
			MediaResolutionUnspecified,
			/// <summary>
			/// Media resolution set to low (64 tokens).
			/// </summary>
			MediaResolutionLow,
			/// <summary>
			/// Media resolution set to medium (256 tokens).
			/// </summary>
			MediaResolutionMedium,
			/// <summary>
			/// Media resolution set to high (zoomed reframing with 256 tokens).
			/// </summary>
			MediaResolutionHigh,
		}
    }
}