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
	/// Content filter results for a prompt sent in the request. Note: This is sent only in the first stream chunk and only if no candidates were generated due to content violations.
	/// </summary>
	public partial class GenerateContentResponsePromptFeedback
	{
		/// <summary>
		/// Output only. The reason why the prompt was blocked.
		/// </summary>
		public BlockReason? BlockReason { get; set; }
		/// <summary>
		/// Output only. A readable message that explains the reason why the prompt was blocked.
		/// </summary>
		public string? BlockReasonMessage { get; set; }
		/// <summary>
		/// Output only. A list of safety ratings for the prompt. There is one rating per category.
		/// </summary>
		public List<SafetyRating>? SafetyRatings { get; set; }
    }
}