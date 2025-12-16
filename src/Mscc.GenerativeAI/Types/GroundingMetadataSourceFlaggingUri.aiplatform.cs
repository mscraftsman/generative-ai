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
	/// A URI that can be used to flag a place or review for inappropriate content. This is populated only when the grounding source is Google Maps.
	/// </summary>
	public partial class GroundingMetadataSourceFlaggingUri
	{
		/// <summary>
		/// The URI that can be used to flag the content.
		/// </summary>
		public string? FlagContentUri { get; set; }
		/// <summary>
		/// The ID of the place or review.
		/// </summary>
		public string? SourceId { get; set; }
    }
}