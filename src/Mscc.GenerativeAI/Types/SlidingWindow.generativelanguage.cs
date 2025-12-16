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
	/// The SlidingWindow method operates by discarding content at the beginning of the context window. The resulting context will always begin at the start of a USER role turn. System instructions and any <c>BidiGenerateContentSetup.prefix_turns</c> will always remain at the beginning of the result.
	/// </summary>
	public partial class SlidingWindow
	{
		/// <summary>
		/// The target number of tokens to keep. The default value is trigger_tokens/2. Discarding parts of the context window causes a temporary latency increase so this value should be calibrated to avoid frequent compression operations.
		/// </summary>
		public long? TargetTokens { get; set; }
    }
}