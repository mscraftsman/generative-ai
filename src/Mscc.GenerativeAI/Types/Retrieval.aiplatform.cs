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
	/// Defines a retrieval tool that model can call to access external knowledge.
	/// </summary>
	public partial class Retrieval
	{
		/// <summary>
		/// Optional. Deprecated. This option is no longer supported.
		/// </summary>
		public bool? DisableAttribution { get; set; }
		/// <summary>
		/// Use data source powered by external API for grounding.
		/// </summary>
		public ExternalApi? ExternalApi { get; set; }
		/// <summary>
		/// Set to use data source powered by Vertex AI Search.
		/// </summary>
		public VertexAISearch? VertexAiSearch { get; set; }
		/// <summary>
		/// Set to use data source powered by Vertex RAG store. User data is uploaded via the VertexRagDataService.
		/// </summary>
		public VertexRagStore? VertexRagStore { get; set; }
    }
}