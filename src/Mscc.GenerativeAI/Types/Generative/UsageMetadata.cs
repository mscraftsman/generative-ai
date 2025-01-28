#if NET472_OR_GREATER || NETSTANDARD2_0
using System.Collections.Generic;
#endif

namespace Mscc.GenerativeAI
{
    public class UsageMetadata
    {
        /// <summary>
        /// Number of tokens in the request.
        /// </summary>
        public int PromptTokenCount { get; set; } = default;
        /// <summary>
        /// Number of tokens in the response(s).
        /// </summary>
        public int CandidatesTokenCount { get; set; } = default;
        /// <summary>
        /// Number of tokens in the response(s).
        /// </summary>
        public int TotalTokenCount { get; set; } = default;
        /// <summary>
        /// Number of tokens in the cached content.
        /// </summary>
        public int CachedContentTokenCount { get; set; } = default;
        /// <summary>
        /// Output only. List of modalities that were processed in the request input.
        /// </summary>
        public List<ModalityTokenCount>? PromptTokensDetails { get; set; }
        /// <summary>
        /// Output only. List of modalities that were returned in the response.
        /// </summary>
        public List<ModalityTokenCount>? CandidatesTokensDetails { get; set; }
        /// <summary>
        /// Output only. List of modalities of the cached content in the request input.
        /// </summary>
        public List<ModalityTokenCount>? CacheTokensDetails { get; set; }
    }
}