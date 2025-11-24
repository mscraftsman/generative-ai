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
	/// A grounding chunk from Google Maps. A Maps chunk corresponds to a single place.
	/// </summary>
	public partial class Maps
	{
		/// <summary>
		/// Sources that provide answers about the features of a given place in Google Maps.
		/// </summary>
		public PlaceAnswerSources? PlaceAnswerSources { get; set; }
		/// <summary>
		/// This ID of the place, in <c>places/{place_id}</c> format. A user can use this ID to look up that place.
		/// </summary>
		public string? PlaceId { get; set; }
		/// <summary>
		/// Text description of the place answer.
		/// </summary>
		public string? Text { get; set; }
		/// <summary>
		/// Title of the place.
		/// </summary>
		public string? Title { get; set; }
		/// <summary>
		/// URI reference of the place.
		/// </summary>
		public string? Uri { get; set; }
    }
}