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
	/// Safety rating for a piece of content. The safety rating contains the category of harm and the harm probability level in that category for a piece of content. Content is classified for safety across a number of harm categories and the probability of the harm classification is included here.
	/// </summary>
	public partial class SafetyRating
	{
		/// <summary>
		/// Was this content blocked because of this rating?
		/// </summary>
		public bool? Blocked { get; set; }
		/// <summary>
		/// Required. The category for this rating.
		/// </summary>
		public HarmCategory? Category { get; set; }
		/// <summary>
		/// Required. The probability of harm for this content.
		/// </summary>
		public HarmProbability? Probability { get; set; }
    }
}