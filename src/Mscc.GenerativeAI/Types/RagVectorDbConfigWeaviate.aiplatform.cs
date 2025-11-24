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
	/// The config for the Weaviate.
	/// </summary>
	public partial class RagVectorDbConfigWeaviate
	{
		/// <summary>
		/// The corresponding collection this corpus maps to. This value cannot be changed after it&apos;s set.
		/// </summary>
		public string? CollectionName { get; set; }
		/// <summary>
		/// Weaviate DB instance HTTP endpoint. e.g. 34.56.78.90:8080 Vertex RAG only supports HTTP connection to Weaviate. This value cannot be changed after it&apos;s set.
		/// </summary>
		public string? HttpEndpoint { get; set; }
    }
}