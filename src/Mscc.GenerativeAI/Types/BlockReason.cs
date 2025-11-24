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

	[JsonConverter(typeof(JsonStringEnumConverter<BlockReason>))]
	public enum BlockReason
	{
		/// <summary>
		/// The blocked reason is unspecified.
		/// </summary>
		BlockedReasonUnspecified,
		/// <summary>
		/// The prompt was blocked for safety reasons.
		/// </summary>
		Safety,
		/// <summary>
		/// The prompt was blocked for other reasons. For example, it may be due to the prompt&apos;s language, or because it contains other harmful content.
		/// </summary>
		Other,
		/// <summary>
		/// The prompt was blocked because it contains a term from the terminology blocklist.
		/// </summary>
		Blocklist,
		/// <summary>
		/// The prompt was blocked because it contains prohibited content.
		/// </summary>
		ProhibitedContent,
		/// <summary>
		/// The prompt was blocked by Model Armor.
		/// </summary>
		ModelArmor,
		/// <summary>
		/// The prompt was blocked because it contains content that is unsafe for image generation.
		/// </summary>
		ImageSafety,
		/// <summary>
		/// The prompt was blocked as a jailbreak attempt.
		/// </summary>
		Jailbreak,
	}
}