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
	/// Retrieve from Vertex AI Search datastore or engine for grounding. datastore and engine are mutually exclusive. See https://cloud.google.com/products/agent-builder
	/// </summary>
	public partial class VertexAISearch
	{
		/// <summary>
		/// Specifications that define the specific DataStores to be searched, along with configurations for those data stores. This is only considered for Engines with multiple data stores. It should only be set if engine is used.
		/// </summary>
		public List<VertexAISearchDataStoreSpec>? DataStoreSpecs { get; set; }
		/// <summary>
		/// Optional. Fully-qualified Vertex AI Search data store resource ID. Format: <c>projects/{project}/locations/{location}/collections/{collection}/dataStores/{dataStore}</c>
		/// </summary>
		public string? Datastore { get; set; }
		/// <summary>
		/// Optional. Fully-qualified Vertex AI Search engine resource ID. Format: <c>projects/{project}/locations/{location}/collections/{collection}/engines/{engine}</c>
		/// </summary>
		public string? Engine { get; set; }
		/// <summary>
		/// Optional. Filter strings to be passed to the search API.
		/// </summary>
		public string? Filter { get; set; }
		/// <summary>
		/// Optional. Number of search results to return per query. The default value is 10. The maximumm allowed value is 10.
		/// </summary>
		public int? MaxResults { get; set; }
    }
}