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
	/// The definition of the Rag resource.
	/// </summary>
	public partial class VertexRagStoreRagResource
	{
		/// <summary>
		/// Optional. RagCorpora resource name. Format: <c>projects/{project}/locations/{location}/ragCorpora/{rag_corpus}</c>
		/// </summary>
		public string? RagCorpus { get; set; }
		/// <summary>
		/// Optional. rag_file_id. The files should be in the same rag_corpus set in rag_corpus field.
		/// </summary>
		public List<string>? RagFileIds { get; set; }
    }
}