/*
 * Copyright 2024-2025 Jochen Kirstätter
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

namespace Mscc.GenerativeAI.Types
{
	/// <summary>
	/// Metadata returned to client when grounding is enabled.
	/// </summary>
	public partial class GroundingMetadata
	{
		/// <summary>
		/// 
		/// </summary>
		public List<GroundingAttribution>? GroundingAttributions { get; set; }
		/// <summary>
		/// Optional. The queries that were executed by the retrieval tools. This field is populated
		/// only when the grounding source is a retrieval tool, such as Vertex AI Search. This field is
		/// not supported in Gemini API.
		/// </summary>
		public List<string>? RetrievalQueries { get; set; }
		/// <summary>
		/// Optional. Output only. A list of URIs that can be used to flag a place or review for
		/// inappropriate content. This field is populated only when the grounding source is Google
		/// Maps. This field is not supported in Gemini API.
		/// </summary>
		public List<GroundingMetadataSourceFlaggingUri>? SourceFlaggingUris { get; set; }
    }
}