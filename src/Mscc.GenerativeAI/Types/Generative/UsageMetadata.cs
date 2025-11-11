using System.Collections.Generic;

namespace Mscc.GenerativeAI
{
    /// <summary>
    /// Usage metadata about the content generation request and response. This message provides a
    /// detailed breakdown of token usage and other relevant metrics. This data type is not supported
    /// in Gemini API.
    /// </summary>
    public class UsageMetadata
    {
        /// <summary>
        /// The total number of tokens in the prompt. This includes any text, images, or other media
        /// provided in the request. When `cached_content` is set, this also includes the number of
        /// tokens in the cached content.
        /// </summary>
        public int PromptTokenCount { get; set; } = default;
        /// <summary>
        /// The total number of tokens in the generated candidates.
        /// </summary>
        public int CandidatesTokenCount { get; set; } = default;
        /// <summary>
        /// The total number of tokens for the entire request. This is the sum of `prompt_token_count`,
        /// `candidates_token_count`, `tool_use_prompt_token_count`, and `thoughts_token_count`.
        /// </summary>
        public int TotalTokenCount { get; set; } = default;
        /// <summary>
        /// Number of text characters.
        /// </summary>
        public int TextCount { get; set; } = default;
        /// <summary>
        /// Number of images.
        /// </summary>
        public int ImageCount { get; set; } = default;
        /// <summary>
        /// Duration of video in seconds.
        /// </summary>
        public int VideoDurationSeconds { get; set; } = default;
        /// <summary>
        /// Duration of audio in seconds.
        /// </summary>
        public int AudioDurationSeconds { get; set; } = default;
        /// <summary>
        /// The number of tokens in the cached content that was used for this request.
        /// </summary>
        public int CachedContentTokenCount { get; set; } = default;
        /// <summary>
        /// Output only. The number of tokens in the results from tool executions, which are provided
        /// back to the model as input, if applicable.
        /// </summary>
        public int ToolUsePromptTokenCount { get; set; } = default;
        /// <summary>
        /// Output only. The number of tokens that were part of the model's generated "thoughts" output,
        /// if applicable.
        /// </summary>
        public int ThoughtsTokenCount { get; set; } = default;
        /// <summary>
        /// Output only. A detailed breakdown of the token count for each modality in the prompt.
        /// </summary>
        public List<ModalityTokenCount>? PromptTokensDetails { get; set; }
        /// <summary>
        /// Output only. A detailed breakdown of the token count for each modality in the generated
        /// candidates.
        /// </summary>
        public List<ModalityTokenCount>? CandidatesTokensDetails { get; set; }
        /// <summary>
        /// Output only. A detailed breakdown of the token count for each modality in the cache.
        /// </summary>
        public List<ModalityTokenCount>? CacheTokensDetails { get; set; }
        /// <summary>
        /// Output only. A detailed breakdown by modality of the token counts from the results of tool
        /// executions, which are provided back to the model as input.
        /// </summary>
        public List<ModalityTokenCount>? ToolUsePromptTokensDetails { get; set; }
        /// <summary>
        /// Output only. The traffic type for this request.
        /// </summary>
        public TrafficType? TrafficType { get; set; }
    }
}