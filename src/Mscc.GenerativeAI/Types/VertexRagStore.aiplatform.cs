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
	/// Retrieve from Vertex RAG Store for grounding.
	/// </summary>
	public partial class VertexRagStore
	{
		/// <summary>
		/// Optional. Deprecated. Please use rag_resources instead.
		/// </summary>
		public List<string>? RagCorpora { get; set; }
		/// <summary>
		/// Optional. The representation of the rag source. It can be used to specify corpus only or ragfiles. Currently only support one corpus or multiple files from one corpus. In the future we may open up multiple corpora support.
		/// </summary>
		public List<VertexRagStoreRagResource>? RagResources { get; set; }
		/// <summary>
		/// Optional. The retrieval config for the Rag query.
		/// </summary>
		public RagRetrievalConfig? RagRetrievalConfig { get; set; }
		/// <summary>
		/// Optional. Number of top k results to return from the selected corpora.
		/// </summary>
		public int? SimilarityTopK { get; set; }
		/// <summary>
		/// Optional. Currently only supported for Gemini Multimodal Live API. In Gemini Multimodal Live API, if <c>store_context</c> bool is specified, Gemini will leverage it to automatically memorize the interactions between the client and Gemini, and retrieve context when needed to augment the response generation for users&apos; ongoing and future interactions.
		/// </summary>
		public bool? StoreContext { get; set; }
		/// <summary>
		/// Optional. Only return results with vector distance smaller than the threshold.
		/// </summary>
		public double? VectorDistanceThreshold { get; set; }
    }
}