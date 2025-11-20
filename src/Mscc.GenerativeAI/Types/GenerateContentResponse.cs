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
	/// Response from the model supporting multiple candidate responses. Safety ratings and content filtering are reported for both prompt in <see cref="GenerateContentResponse.prompt_feedback"/> and for each candidate in <see cref="finish_reason"/> and in <see cref="safety_ratings"/>. The API: - Returns either all requested candidates or none of them - Returns no candidates at all only if there was something wrong with the prompt (check <see cref="prompt_feedback"/>) - Reports feedback on each candidate in <see cref="finish_reason"/> and <see cref="safety_ratings"/>.
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
		/// Returns the prompt's feedback related to the content filters.
		/// </summary>
		public PromptFeedback? PromptFeedback { get; set; }
		/// <summary>
		/// Output only. response_id is used to identify each response.
		/// </summary>
		public string? ResponseId { get; set; }
		/// <summary>
		/// Output only. Metadata on the generation requests' token usage.
		/// </summary>
		public UsageMetadata? UsageMetadata { get; set; }
    }
}