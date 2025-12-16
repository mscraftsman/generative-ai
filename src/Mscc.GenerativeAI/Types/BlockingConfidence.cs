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

	[JsonConverter(typeof(JsonStringEnumConverter<BlockingConfidence>))]
	public enum BlockingConfidence
	{
		/// <summary>
		/// Defaults to unspecified.
		/// </summary>
		PhishBlockThresholdUnspecified,
		/// <summary>
		/// Blocks Low and above confidence URL that is risky.
		/// </summary>
		BlockLowAndAbove,
		/// <summary>
		/// Blocks Medium and above confidence URL that is risky.
		/// </summary>
		BlockMediumAndAbove,
		/// <summary>
		/// Blocks High and above confidence URL that is risky.
		/// </summary>
		BlockHighAndAbove,
		/// <summary>
		/// Blocks Higher and above confidence URL that is risky.
		/// </summary>
		BlockHigherAndAbove,
		/// <summary>
		/// Blocks Very high and above confidence URL that is risky.
		/// </summary>
		BlockVeryHighAndAbove,
		/// <summary>
		/// Blocks Extremely high confidence URL that is risky.
		/// </summary>
		BlockOnlyExtremelyHigh,
	}
}