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
	/// Enables context window compression — a mechanism for managing the model&apos;s context window so that it does not exceed a given length.
	/// </summary>
	public partial class ContextWindowCompressionConfig
	{
		/// <summary>
		/// A sliding-window mechanism.
		/// </summary>
		public SlidingWindow? SlidingWindow { get; set; }
		/// <summary>
		/// The number of tokens (before running a turn) required to trigger a context window compression. This can be used to balance quality against latency as shorter context windows may result in faster model responses. However, any compression operation will cause a temporary latency increase, so they should not be triggered frequently. If not set, the default is 80% of the model&apos;s context window limit. This leaves 20% for the next user request/model response.
		/// </summary>
		public long? TriggerTokens { get; set; }
    }
}