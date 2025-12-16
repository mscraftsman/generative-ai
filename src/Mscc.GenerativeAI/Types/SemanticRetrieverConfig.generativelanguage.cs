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
	/// Configuration for retrieving grounding content from a <c>Corpus</c> or <c>Document</c> created using the Semantic Retriever API.
	/// </summary>
	public partial class SemanticRetrieverConfig
	{
		/// <summary>
		/// Optional. Maximum number of relevant <c>Chunk</c>s to retrieve.
		/// </summary>
		public int? MaxChunksCount { get; set; }
		/// <summary>
		/// Optional. Filters for selecting <c>Document</c>s and/or <c>Chunk</c>s from the resource.
		/// </summary>
		public List<MetadataFilter>? MetadataFilters { get; set; }
		/// <summary>
		/// Optional. Minimum relevance score for retrieved relevant <c>Chunk</c>s.
		/// </summary>
		public double? MinimumRelevanceScore { get; set; }
		/// <summary>
		/// Required. Query to use for matching <c>Chunk</c>s in the given resource by similarity.
		/// </summary>
		public Content? Query { get; set; }
		/// <summary>
		/// Required. Name of the resource for retrieval. Example: <c>corpora/123</c> or <c>corpora/123/documents/abc</c>.
		/// </summary>
		public string? Source { get; set; }
    }
}