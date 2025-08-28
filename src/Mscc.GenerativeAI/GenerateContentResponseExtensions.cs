#if NET472_OR_GREATER || NETSTANDARD2_0
using System;
using System.Collections.Generic;
using System.Linq;
#endif

namespace Mscc.GenerativeAI
{
    internal static class GenerateContentResponseExtensions
    {
        /// <summary>
        /// Merges a sequence of <see cref="GenerateContentResponse"/> objects into a single one.
        /// The first candidate of each response is taken; their <see cref="Part"/> collections are
        /// concatenated in the order the responses are given.
        /// For duplicated metadata fields the value from the *last* response that provides a
        /// non-null value overwrites earlier ones (“last value wins”).
        /// </summary>
        public static GenerateContentResponse Merge(this IEnumerable<GenerateContentResponse> responses)
        {
            if (responses is null) throw new ArgumentNullException(nameof(responses));

            if (responses is not IReadOnlyCollection<GenerateContentResponse> responseList)
                responseList = responses.ToList();
            if (responseList.Count == 0)
                return new GenerateContentResponse();

            // Collect parts and remember the latest candidate seen (used for metadata copy).
            var allParts = new List<Part>();
            Candidate? lastCandidateWithMetadata = null;

            // Result object we will keep overwriting.
            var merged = new GenerateContentResponse();

            foreach (var r in responseList)
            {
                // Overwrite-on-conflict for top-level fields.
                if (r.ModelVersion   is not null) merged.ModelVersion   = r.ModelVersion;
                if (r.PromptFeedback is not null) merged.PromptFeedback = r.PromptFeedback;
                if (r.UsageMetadata  is not null) merged.UsageMetadata  = r.UsageMetadata;
                if (r.ResponseId     is not null) merged.ResponseId     = r.ResponseId;

                var cand = r.Candidates?.FirstOrDefault();
                if (cand != null)
                {
                    lastCandidateWithMetadata = cand;

                    if (cand.Content?.Parts is { Count: > 0 } parts)
                        allParts.AddRange(parts);
                }
            }

            // Build the merged candidate from the collected information.
            if (allParts.Count > 0)
            {
                var mergedCandidate = new Candidate
                {
                    Content = new ContentResponse
                    {
                        Parts = allParts,
                        Role  = lastCandidateWithMetadata!.Content!.Role,
                    },
                    // Copy metadata from the *latest* candidate that had any.
                    FinishReason          = lastCandidateWithMetadata.FinishReason,
                    FinishMessage         = lastCandidateWithMetadata.FinishMessage,
                    SafetyRatings         = lastCandidateWithMetadata.SafetyRatings,
                    CitationMetadata      = lastCandidateWithMetadata.CitationMetadata,
                    FunctionCall          = lastCandidateWithMetadata.FunctionCall,
                    GroundingMetadata     = lastCandidateWithMetadata.GroundingMetadata,
                    TokenCount            = lastCandidateWithMetadata.TokenCount,
                    GroundingAttributions = lastCandidateWithMetadata.GroundingAttributions,
                    AvgLogprobs           = lastCandidateWithMetadata.AvgLogprobs,
                    LogprobsResult        = lastCandidateWithMetadata.LogprobsResult,
                    UrlRetrievalMetadata  = lastCandidateWithMetadata.UrlRetrievalMetadata,
                    UrlContextMetadata    = lastCandidateWithMetadata.UrlContextMetadata,
                    Index = 0,
                };

                merged.Candidates = [mergedCandidate];
            }

            return merged;
        }
    }
}