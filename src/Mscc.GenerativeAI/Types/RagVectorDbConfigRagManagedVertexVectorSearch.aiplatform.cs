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
	/// The config for the RAG-managed Vertex Vector Search 2.0.
	/// </summary>
	public partial class RagVectorDbConfigRagManagedVertexVectorSearch
	{
		/// <summary>
		/// Output only. The resource name of the Vector Search 2.0 Collection that RAG Created for the corpus. Only populated after the corpus is successfully created. Format: <c>projects/{project}/locations/{location}/collections/{collection_id}</c>
		/// </summary>
		public string? CollectionName { get; set; }
    }
}