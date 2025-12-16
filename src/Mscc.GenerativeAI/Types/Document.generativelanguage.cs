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
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

// *** AUTO-GENERATED FILE - DO NOT EDIT MANUALLY *** //

namespace Mscc.GenerativeAI.Types
{
	/// <summary>
	/// A <c>Document</c> is a collection of <c>Chunk</c>s.
	/// </summary>
	public partial class Document
	{
		/// <summary>
		/// Output only. The Timestamp of when the <c>Document</c> was created.
		/// </summary>
		public DateTime? CreateTime { get; set; }
		/// <summary>
		/// Optional. User provided custom metadata stored as key-value pairs used for querying. A <c>Document</c> can have a maximum of 20 <c>CustomMetadata</c>.
		/// </summary>
		public List<CustomMetadata>? CustomMetadata { get; set; }
		/// <summary>
		/// Optional. The human-readable display name for the <c>Document</c>. The display name must be no more than 512 characters in length, including spaces. Example: &quot;Semantic Retriever Documentation&quot;
		/// </summary>
		public string? DisplayName { get; set; }
		/// <summary>
		/// Output only. The mime type of the Document.
		/// </summary>
		public string? MimeType { get; set; }
		/// <summary>
		/// Immutable. Identifier. The <c>Document</c> resource name. The ID (name excluding the &quot;fileSearchStores/*/documents/&quot; prefix) can contain up to 40 characters that are lowercase alphanumeric or dashes (-). The ID cannot start or end with a dash. If the name is empty on create, a unique name will be derived from <c>display_name</c> along with a 12 character random suffix. Example: <c>fileSearchStores/{file_search_store_id}/documents/my-awesome-doc-123a456b789c</c>
		/// </summary>
		public string? Name { get; set; }
		/// <summary>
		/// Output only. The size of raw bytes ingested into the Document.
		/// </summary>
		public long? SizeBytes { get; set; }
		/// <summary>
		/// Output only. Current state of the <c>Document</c>.
		/// </summary>
		public StateType? State { get; set; }
		/// <summary>
		/// Output only. The Timestamp of when the <c>Document</c> was last updated.
		/// </summary>
		public DateTime? UpdateTime { get; set; }

		[JsonConverter(typeof(JsonStringEnumConverter<StateType>))]
		public enum StateType
		{
			/// <summary>
			/// The default value. This value is used if the state is omitted.
			/// </summary>
			StateUnspecified,
			/// <summary>
			/// Some `Chunks` of the `Document` are being processed (embedding and vector storage).
			/// </summary>
			StatePending,
			/// <summary>
			/// All `Chunks` of the `Document` is processed and available for querying.
			/// </summary>
			StateActive,
			/// <summary>
			/// Some `Chunks` of the `Document` failed processing.
			/// </summary>
			StateFailed,
		}
    }
}