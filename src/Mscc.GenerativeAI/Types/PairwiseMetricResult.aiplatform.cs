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
using System.Text.Json.Serialization;

// *** AUTO-GENERATED FILE - DO NOT EDIT MANUALLY *** //

namespace Mscc.GenerativeAI.Types
{
	/// <summary>
	/// Spec for pairwise metric result.
	/// </summary>
	public partial class PairwiseMetricResult
	{
		/// <summary>
		/// Output only. Spec for custom output.
		/// </summary>
		public CustomOutput? CustomOutput { get; set; }
		/// <summary>
		/// Output only. Explanation for pairwise metric score.
		/// </summary>
		public string? Explanation { get; set; }
		/// <summary>
		/// Output only. Pairwise metric choice.
		/// </summary>
		public PairwiseChoiceType? PairwiseChoice { get; set; }

		[JsonConverter(typeof(JsonStringEnumConverter<PairwiseChoiceType>))]
		public enum PairwiseChoiceType
		{
			/// <summary>
			/// Unspecified prediction choice.
			/// </summary>
			PairwiseChoiceUnspecified,
			/// <summary>
			/// Baseline prediction wins
			/// </summary>
			Baseline,
			/// <summary>
			/// Candidate prediction wins
			/// </summary>
			Candidate,
			/// <summary>
			/// Winner cannot be determined
			/// </summary>
			Tie,
		}
    }
}