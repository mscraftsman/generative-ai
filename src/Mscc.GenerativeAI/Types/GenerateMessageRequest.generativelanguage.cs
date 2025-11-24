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

// *** AUTO-GENERATED FILE - DO NOT EDIT MANUALLY *** //

namespace Mscc.GenerativeAI.Types
{
	/// <summary>
	/// Request to generate a message response from the model.
	/// </summary>
	public partial class GenerateMessageRequest
	{
		/// <summary>
		/// Optional. The number of generated response messages to return. This value must be between <c>[1, 8]</c>, inclusive. If unset, this will default to <c>1</c>.
		/// </summary>
		public int? CandidateCount { get; set; }
		/// <summary>
		/// Required. The structured textual input given to the model as a prompt. Given a prompt, the model will return what it predicts is the next message in the discussion.
		/// </summary>
		public MessagePrompt? Prompt { get; set; }
		/// <summary>
		/// Optional. Controls the randomness of the output. Values can range over <c>[0.0,1.0]</c>, inclusive. A value closer to <c>1.0</c> will produce responses that are more varied, while a value closer to <c>0.0</c> will typically result in less surprising responses from the model.
		/// </summary>
		public double? Temperature { get; set; }
		/// <summary>
		/// Optional. The maximum number of tokens to consider when sampling. The model uses combined Top-k and nucleus sampling. Top-k sampling considers the set of <c>top_k</c> most probable tokens.
		/// </summary>
		public int? TopK { get; set; }
		/// <summary>
		/// Optional. The maximum cumulative probability of tokens to consider when sampling. The model uses combined Top-k and nucleus sampling. Nucleus sampling considers the smallest set of tokens whose probability sum is at least <c>top_p</c>.
		/// </summary>
		public double? TopP { get; set; }
    }
}