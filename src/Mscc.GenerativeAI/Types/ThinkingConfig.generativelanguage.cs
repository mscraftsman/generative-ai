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
	/// Config for thinking features.
	/// </summary>
	public partial class ThinkingConfig
	{
		/// <summary>
		/// Indicates whether to include thoughts in the response. If true, thoughts are returned only when available.
		/// </summary>
		public bool? IncludeThoughts { get; set; }
		/// <summary>
		/// The number of thoughts tokens that the model should generate.
		/// </summary>
		public int? ThinkingBudget { get; set; }
		/// <summary>
		/// Optional. Controls the maximum depth of the model&apos;s internal reasoning process before it produces a response. If not specified, the default is HIGH. Recommended for Gemini 3 or later models. Use with earlier models results in an error.
		/// </summary>
		public ThinkingLevel? ThinkingLevel { get; set; }
    }
}