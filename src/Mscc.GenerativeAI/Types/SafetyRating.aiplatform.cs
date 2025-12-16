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
	/// A safety rating for a piece of content. The safety rating contains the harm category and the harm probability level.
	/// </summary>
	public partial class SafetyRating
	{

		/// <summary>
		/// Output only. The overwritten threshold for the safety category of Gemini 2.0 image out. If minors are detected in the output image, the threshold of each safety category will be overwritten if user sets a lower threshold.
		/// </summary>
		public OverwrittenThresholdType? OverwrittenThreshold { get; set; }
		/// <summary>
		/// Output only. The probability score of harm for this category.
		/// </summary>
		public double? ProbabilityScore { get; set; }
		/// <summary>
		/// Output only. The severity of harm for this category.
		/// </summary>
		public HarmSeverity? Severity { get; set; }
		/// <summary>
		/// Output only. The severity score of harm for this category.
		/// </summary>
		public double? SeverityScore { get; set; }

		[JsonConverter(typeof(JsonStringEnumConverter<OverwrittenThresholdType>))]
		public enum OverwrittenThresholdType
		{
			/// <summary>
			/// The harm block threshold is unspecified.
			/// </summary>
			HarmBlockThresholdUnspecified,
			/// <summary>
			/// Block content with a low harm probability or higher.
			/// </summary>
			BlockLowAndAbove,
			/// <summary>
			/// Block content with a medium harm probability or higher.
			/// </summary>
			BlockMediumAndAbove,
			/// <summary>
			/// Block content with a high harm probability.
			/// </summary>
			BlockOnlyHigh,
			/// <summary>
			/// Do not block any content, regardless of its harm probability.
			/// </summary>
			BlockNone,
			/// <summary>
			/// Turn off the safety filter entirely.
			/// </summary>
			Off,
		}
    }
}