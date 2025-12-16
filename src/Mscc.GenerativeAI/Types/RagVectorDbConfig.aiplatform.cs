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
	/// Config for the Vector DB to use for RAG.
	/// </summary>
	public partial class RagVectorDbConfig
	{
		/// <summary>
		/// Authentication config for the chosen Vector DB.
		/// </summary>
		public ApiAuth? ApiAuth { get; set; }
		/// <summary>
		/// The config for the Pinecone.
		/// </summary>
		public RagVectorDbConfigPinecone? Pinecone { get; set; }
		/// <summary>
		/// Optional. Immutable. The embedding model config of the Vector DB.
		/// </summary>
		public RagEmbeddingModelConfig? RagEmbeddingModelConfig { get; set; }
		/// <summary>
		/// The config for the RAG-managed Vector DB.
		/// </summary>
		public RagVectorDbConfigRagManagedDb? RagManagedDb { get; set; }
		/// <summary>
		/// The config for the RAG-managed Vertex Vector Search 2.0.
		/// </summary>
		public RagVectorDbConfigRagManagedVertexVectorSearch? RagManagedVertexVectorSearch { get; set; }
		/// <summary>
		/// The config for the Vertex Feature Store.
		/// </summary>
		public RagVectorDbConfigVertexFeatureStore? VertexFeatureStore { get; set; }
		/// <summary>
		/// The config for the Vertex Vector Search.
		/// </summary>
		public RagVectorDbConfigVertexVectorSearch? VertexVectorSearch { get; set; }
		/// <summary>
		/// The config for the Weaviate.
		/// </summary>
		public RagVectorDbConfigWeaviate? Weaviate { get; set; }
    }
}