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
	/// The config for the default RAG-managed Vector DB.
	/// </summary>
	public partial class RagVectorDbConfigRagManagedDb
	{
		/// <summary>
		/// Performs an ANN search on RagCorpus. Use this if you have a lot of files (&gt; 10K) in your RagCorpus and want to reduce the search latency.
		/// </summary>
		public RagVectorDbConfigRagManagedDbANN? Ann { get; set; }
		/// <summary>
		/// Performs a KNN search on RagCorpus. Default choice if not specified.
		/// </summary>
		public RagVectorDbConfigRagManagedDbKNN? Knn { get; set; }
    }
}