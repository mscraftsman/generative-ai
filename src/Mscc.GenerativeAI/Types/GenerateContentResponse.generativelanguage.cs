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

// *** AUTO-GENERATED FILE - DO NOT EDIT MANUALLY *** //

namespace Mscc.GenerativeAI.Types
{
	/// <summary>
	/// Response from the model supporting multiple candidate responses. Safety ratings and content filtering are reported for both prompt in <c>GenerateContentResponse.prompt_feedback</c> and for each candidate in <c>finish_reason</c> and in <c>safety_ratings</c>. The API: - Returns either all requested candidates or none of them - Returns no candidates at all only if there was something wrong with the prompt (check <c>prompt_feedback</c>) - Reports feedback on each candidate in <c>finish_reason</c> and <c>safety_ratings</c>.
	/// </summary>
	public partial class GenerateContentResponse
	{
		/// <summary>
		/// Candidate responses from the model.
		/// </summary>
		public List<Candidate>? Candidates { get; set; }
		/// <summary>
		/// Output only. The model version used to generate the response.
		/// </summary>
		public string? ModelVersion { get; set; }
		/// <summary>
		/// Returns the prompt&apos;s feedback related to the content filters.
		/// </summary>
		public PromptFeedback? PromptFeedback { get; set; }
		/// <summary>
		/// Output only. response_id is used to identify each response.
		/// </summary>
		public string? ResponseId { get; set; }
		/// <summary>
		/// Output only. Metadata on the generation requests&apos; token usage.
		/// </summary>
		public UsageMetadata? UsageMetadata { get; set; }
    }
}