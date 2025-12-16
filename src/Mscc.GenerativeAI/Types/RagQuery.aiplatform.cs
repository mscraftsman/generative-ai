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
	/// A query to retrieve relevant contexts.
	/// </summary>
	public partial class RagQuery
	{
		/// <summary>
		/// Optional. The retrieval config for the query.
		/// </summary>
		public RagRetrievalConfig? RagRetrievalConfig { get; set; }
		/// <summary>
		/// Optional. Configurations for hybrid search results ranking.
		/// </summary>
		public RagQueryRanking? Ranking { get; set; }
		/// <summary>
		/// Optional. The number of contexts to retrieve.
		/// </summary>
		public int? SimilarityTopK { get; set; }
		/// <summary>
		/// Optional. The query in text format to get relevant contexts.
		/// </summary>
		public string? Text { get; set; }
    }
}