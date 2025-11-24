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
using System.Collections.Generic;

// *** AUTO-GENERATED FILE - DO NOT EDIT MANUALLY *** //

namespace Mscc.GenerativeAI.Types
{
	/// <summary>
	/// Metadata returned to client when grounding is enabled.
	/// </summary>
	public partial class GroundingMetadata
	{
		/// <summary>
		/// Optional. Resource name of the Google Maps widget context token that can be used with the PlacesContextElement widget in order to render contextual data. Only populated in the case that grounding with Google Maps is enabled.
		/// </summary>
		public string? GoogleMapsWidgetContextToken { get; set; }
		/// <summary>
		/// List of supporting references retrieved from specified grounding source.
		/// </summary>
		public List<GroundingChunk>? GroundingChunks { get; set; }
		/// <summary>
		/// List of grounding support.
		/// </summary>
		public List<GroundingSupport>? GroundingSupports { get; set; }
		/// <summary>
		/// Metadata related to retrieval in the grounding flow.
		/// </summary>
		public RetrievalMetadata? RetrievalMetadata { get; set; }
		/// <summary>
		/// Optional. Google search entry for the following-up web searches.
		/// </summary>
		public SearchEntryPoint? SearchEntryPoint { get; set; }
		/// <summary>
		/// Web search queries for the following-up web search.
		/// </summary>
		public List<string>? WebSearchQueries { get; set; }
    }
}