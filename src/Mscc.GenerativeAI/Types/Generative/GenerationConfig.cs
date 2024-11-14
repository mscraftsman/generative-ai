#if NET472_OR_GREATER || NETSTANDARD2_0
using System.Collections.Generic;
#endif

namespace Mscc.GenerativeAI
{
    /// <summary>
    /// Configuration options for model generation and outputs. Not all parameters may be configurable for every model.
    /// Ref: https://ai.google.dev/api/rest/v1beta/GenerationConfig
    /// </summary>
    public class GenerationConfig
    {
        /// <summary>
        /// Optional. Controls the randomness of predictions.
        /// Temperature controls the degree of randomness in token selection. Lower temperatures are good for prompts that expect a true or correct response, while higher temperatures can lead to more diverse or unexpected results. With a temperature of 0, the highest probability token is always selected. 
        /// </summary>
        public float? Temperature { get; set; } = default;
        /// <summary>
        /// Optional. If specified, nucleus sampling will be used.
        /// Top-p changes how the model selects tokens for output. Tokens are selected from most probable to least until the sum of their probabilities equals the top-p value. For example, if tokens A, B and C have a probability of .3, .2 and .1 and the top-p value is .5, then the model will select either A or B as the next token (using temperature).
        /// </summary>
        public float? TopP { get; set; } = default;
        /// <summary>
        /// Optional. If specified, top-k sampling will be used.
        /// Top-k changes how the model selects tokens for output. A top-k of 1 means that the selected token is the most probable among all tokens in the model's vocabulary (also called greedy decoding), while a top-k of 3 means that the next token is selected from among the three most probable tokens (using temperature). 
        /// </summary>
        public int? TopK { get; set; } = default;
        /// <summary>
        /// Optional. Number of generated responses to return.
        /// This value must be between [1, 8], inclusive. If unset, this will default to 1.
        /// </summary>
        public int? CandidateCount { get; set; }
        /// <summary>
        /// Optional. The maximum number of output tokens to generate per message.
        /// Token limit determines the maximum amount of text output from one prompt. A token is approximately four characters. 
        /// </summary>
        public int? MaxOutputTokens { get; set; } = default;
        /// <summary>
        /// Optional. Stop sequences.
        /// A stop sequence is a series of characters (including spaces) that stops response generation if the model encounters it. The sequence is not included as part of the response. You can add up to five stop sequences.
        /// </summary>
        public List<string>? StopSequences { get; set; }
        /// <summary>
        /// Optional. Output response mimetype of the generated candidate text.
        /// Supported mimetype: `text/plain`: (default) Text output. `application/json`: JSON response in the candidates.
        /// </summary>
        public string? ResponseMimeType { get; set; }
        /// <summary>
        /// Optional. Output response schema of the generated candidate text when response mime type can have schema.
        /// </summary>
        /// <remarks>
        /// Schema can be objects, primitives or arrays and is a subset of [OpenAPI schema](https://spec.openapis.org/oas/v3.0.3#schema).
        /// If set, a compatible response_mime_type must also be set. Compatible mimetypes: `application/json`: Schema for JSON response.
        /// </remarks>
        public string? ResponseSchema { get; set; }
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
        /// Optional. Frequency penalty applied to the next token's logprobs, multiplied by the number of times each token has been seen in the respponse so far.
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
        public int? Logprobs { get; set; }
        /// <summary>
        /// Optional. Enables enhanced civic answers. It may not be available for all models.
        /// </summary>
        public bool? EnableEnhancedCivicAnswers { get; set; }
    }
}
