/*
 * Copyright 2024-2025 Jochen Kirstätter
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

namespace Mscc.GenerativeAI.Types
{
	/// <summary>
	/// A response candidate generated from the model.
	/// </summary>
	public partial class Candidate
	{
		/// <summary>
		/// Output only. Average log probability score of the candidate.
		/// </summary>
		public double? AvgLogprobs { get; set; }
		/// <summary>
		/// Output only. Citation information for model-generated candidate. This field may be populated with recitation information for any text included in the <see cref="content"/>. These are passages that are "recited" from copyrighted material in the foundational LLM's training data.
		/// </summary>
		public CitationMetadata? CitationMetadata { get; set; }
		/// <summary>
		/// Output only. Generated content returned from the model.
		/// </summary>
		public Content? Content { get; set; }
		/// <summary>
		/// Optional. Output only. Details the reason why the model stopped generating tokens. This is populated only when <see cref="finish_reason"/> is set.
		/// </summary>
		public string? FinishMessage { get; set; }
		/// <summary>
		/// Optional. Output only. The reason why the model stopped generating tokens. If empty, the model has not stopped generating tokens.
		/// </summary>
		public FinishReason? FinishReason { get; set; }
		/// <summary>
		/// Output only. Attribution information for sources that contributed to a grounded answer. This field is populated for <see cref="GenerateAnswer"/> calls.
		/// </summary>
		public List<GroundingAttribution>? GroundingAttributions { get; set; }
		/// <summary>
		/// Output only. Grounding metadata for the candidate. This field is populated for <see cref="GenerateContent"/> calls.
		/// </summary>
		public GroundingMetadata? GroundingMetadata { get; set; }
		/// <summary>
		/// Output only. Index of the candidate in the list of response candidates.
		/// </summary>
		public int? Index { get; set; }
		/// <summary>
		/// Output only. Log-likelihood scores for the response tokens and top tokens
		/// </summary>
		public LogprobsResult? LogprobsResult { get; set; }
		/// <summary>
		/// List of ratings for the safety of a response candidate. There is at most one rating per category.
		/// </summary>
		public List<SafetyRating>? SafetyRatings { get; set; }
		/// <summary>
		/// Output only. Token count for this candidate.
		/// </summary>
		public int? TokenCount { get; set; }
		/// <summary>
		/// Output only. Metadata related to url context retrieval tool.
		/// </summary>
		public UrlContextMetadata? UrlContextMetadata { get; set; }
    }
}
