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
	/// Counts the number of tokens in the <c>prompt</c> sent to a model. Models may tokenize text differently, so each model may return a different <c>token_count</c>.
	/// </summary>
	public partial class CountMessageTokensRequest
	{
		/// <summary>
		/// Required. The prompt, whose token count is to be returned.
		/// </summary>
		public MessagePrompt? Prompt { get; set; }
    }
}