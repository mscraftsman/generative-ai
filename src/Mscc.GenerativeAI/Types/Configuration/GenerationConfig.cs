using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Mscc.GenerativeAI
{
    /// <summary>
    /// Configuration options for model generation and outputs. Not all parameters may be configurable for every model.
    /// Ref: https://ai.google.dev/api/rest/v1beta/GenerationConfig
    /// </summary>
    public class GenerationConfig : BaseConfig
    {
        /// <summary>
        /// Optional. Controls the randomness of predictions.
        /// Temperature controls the degree of randomness in token selection. Lower temperatures are good for prompts that expect a true or correct response, while higher temperatures can lead to more diverse or unexpected results. With a temperature of 0, the highest probability token is always selected. 
        /// </summary>
        public float? Temperature { get; set; } = default;
        /// <summary>
        /// Optional. Specifies the nucleus sampling threshold. The model considers only the smallest
        /// set of tokens whose cumulative probability is at least `top_p`. This helps generate more
        /// diverse and less repetitive responses. For example, a `top_p` of 0.9 means the model
        /// considers tokens until the cumulative probability of the tokens to select from reaches 0.9.
        /// It's recommended to adjust either temperature or `top_p`, but not both.
        /// </summary>
        public float? TopP { get; set; } = default;
        /// <summary>
        /// Optional. If specified, top-k sampling will be used.
        /// Top-k changes how the model selects tokens for output. A top-k of 1 means that the selected token is the most probable among all tokens in the model's vocabulary (also called greedy decoding), while a top-k of 3 means that the next token is selected from among the three most probable tokens (using temperature). 
        /// </summary>
        public float? TopK { get; set; } = default;
        /// <summary>
        /// Optional. Number of generated responses to return.
        /// If unset, this will default to 1. 
        /// </summary>
        /// <remarks>
        /// Please note that this doesn't work for previous generation models (Gemini 1.0 family)
        /// </remarks>
        public int? CandidateCount { get; set; }
        /// <summary>
        /// Optional. Number of generated responses to return.
        /// If unset, this will default to 1. Please note that this doesn't work for previous generation models (Gemini 1.0 family) 
        /// </summary>
        public int? MaxOutputTokens { get; set; } = default;
        /// <summary>
        /// Optional. Stop sequences.
        /// A stop sequence is a series of characters (including spaces) that stops response generation if the model encounters it. The sequence is not included as part of the response. You can add up to five stop sequences.
        /// </summary>
        public List<string>? StopSequences { get; set; }
        /// <summary>
        /// Optional. The IANA standard MIME type of the response. The model will generate output that
        /// conforms to this MIME type. Supported values include 'text/plain' (default) and
        /// 'application/json'. The model needs to be prompted to output the appropriate response type,
        /// otherwise the behavior is undefined. This is a preview feature.
        /// </summary>
        public string? ResponseMimeType { get; set; }
        /// <summary>
        /// Optional. Output response schema of the generated candidate text when response mime type can have schema.
        /// </summary>
        /// <remarks>
        /// Schema can be objects, primitives or arrays and is a subset of [OpenAPI schema](https://spec.openapis.org/oas/v3.0.3#schema).
        /// If set, a compatible response_mime_type must also be set. Compatible mimetypes: `application/json`: Schema for JSON response.
        /// </remarks>
        public Schema? ResponseSchema { get; set; }
        /// <summary>
        /// Optional. Presence penalty applied to the next token's logprobs if the token has already been seen in the response.
        /// </summary>
        /// <remarks>
        /// This penalty is binary on/off and not dependant on the number of times the token is used (after the first).
        /// Use frequencyPenalty for a penalty that increases with each use. A positive penalty will discourage
        /// the use of tokens that have already been used in the response, increasing the vocabulary. A negative
        /// penalty will encourage the use of tokens that have already been used in the response, decreasing
        /// the vocabulary.
        /// </remarks>
        public float? PresencePenalty { get; set; }
        /// <summary>
        /// Optional. Frequency penalty applied to the next token's logprobs, multiplied by the number
        /// of times each token has been seen in the response so far.
        /// </summary>
        /// <remarks>
        /// A positive penalty will discourage the use of tokens that have already been used, proportional to the number
        /// of times the token has been used: The more a token is used, the more difficult it is for the model to use
        /// that token again increasing the vocabulary of responses.
        /// Caution: A negative penalty will encourage the model to reuse tokens proportional to the number of times
        /// the token has been used. Small negative values will reduce the vocabulary of a response.
        /// Larger negative values will cause the model to start repeating a common token until it hits the
        /// maxOutputTokens limit: "...the the the the the...".
        /// </remarks>
        public float? FrequencyPenalty { get; set; }
        /// <summary>
        /// Optional. If true, export the logprobs results in response.
        /// </summary>
        public bool? ResponseLogprobs { get; set; }
        /// <summary>
        /// Optional. Only valid if responseLogprobs=True.
        /// This sets the number of top logprobs to return at each decoding step in the Candidate.logprobs_result.
        /// </summary>
        /// <remarks>The number must be in the range of [0, 20].</remarks>
        public int? Logprobs { get; set; }
        /// <summary>
        /// Optional. Enables enhanced civic answers. It may not be available for all models.
        /// </summary>
        public bool? EnableEnhancedCivicAnswers { get; set; }
        /// <summary>
        /// Optional. The requested modalities of the response.
        /// Represents the set of modalities that the model can return, and should be expected
        /// in the response. This is an exact match to the modalities of the response.
        /// A model may have multiple combinations of supported modalities. If the requested
        /// modalities do not match any of the supported combinations, an error will be returned.
        /// An empty list is equivalent to requesting only text.
        /// </summary>
        public List<ResponseModality>? ResponseModalities { get; set; }
        /// <summary>
        /// Optional. The speech generation config.
        /// </summary>
        public SpeechConfig? SpeechConfig { get; set; }
        /// <summary>
        /// Optional. The token resolution at which input media content is sampled. This is used to
        /// control the trade-off between the quality of the response and the number of tokens used to
        /// represent the media. A higher resolution allows the model to perceive more detail, which can
        /// lead to a more nuanced response, but it will also use more tokens. This does not affect the
        /// image dimensions sent to the model.
        /// </summary>
        public MediaResolution? MediaResolution { get; set; }
        /// <summary>
        /// Optional. A seed for the random number generator. By setting a seed, you can make the
        /// model's output mostly deterministic. For a given prompt and parameters (like temperature,
        /// top_p, etc.), the model will produce the same response every time. However, it's not a
        /// guaranteed absolute deterministic behavior. This is different from parameters like
        /// `temperature`, which control the *level* of randomness. `seed` ensures that the "random"
        /// choices the model makes are the same on every run, making it essential for testing and
        /// ensuring reproducible results.
        /// </summary>
        public int? Seed { get; set; }
        /// <summary>
        /// Optional. Config for thinking features. An error will be returned if this field is set for models that don't support thinking.
        /// </summary>
        public ThinkingConfig? ThinkingConfig { get; set; }
        /// <summary>
        /// Optional. Output schema of the generated response.
        /// This is an alternative to `response_schema` that accepts [JSON Schema](https://json-schema.org/).
        /// If set, `response_schema` must be omitted, but `response_mime_type` is required.
        /// While the full JSON Schema may be sent, not all features are supported.
        /// Specifically, only the following properties are supported:
        /// - `$id`
        /// - `$defs`
        /// - `$ref`
        /// - `$anchor`
        /// - `type`
        /// - `format`
        /// - `title`
        /// - `description`
        /// - `enum` (for strings and numbers)
        /// - `items`
        /// - `prefixItems`
        /// - `minItems`
        /// - `maxItems`
        /// - `minimum`
        /// - `maximum`
        /// - `anyOf`
        /// - `oneOf` (interpreted the same as `anyOf`)
        /// - `properties`
        /// - `additionalProperties`
        /// - `required`
        /// The non-standard `propertyOrdering` property may also be set.
        /// Cyclic references are unrolled to a limited degree and, as such, may only be used within non-required
        /// properties. (Nullable properties are not sufficient.) If `$ref` is set on a sub-schema, no other properties,
        /// except for than those starting as a `$`, may be set.
        /// </summary>
        public object? ResponseJsonSchema { get; set; }
        /// <summary>
        /// Optional. An internal detail. Use `responseJsonSchema` rather than this field.
        /// </summary>
        public object? ResponseJsonSchemaOrdered { get; set; }
        /// <summary>
        /// Optional. Config for image generation. An error will be returned if this field is set for models that don't support these config options.
        /// </summary>
        public ImageConfig? ImageConfig { get; set; }
        /// <summary>
        /// Optional. If enabled, the model will detect emotions and adapt its responses accordingly.
        /// For example, if the model detects that the user is frustrated, it may provide a more
        /// empathetic response. This field is not supported in Gemini API.
        /// </summary>
        public bool? EnableAffectiveDialog { get; set; }
    }
}
