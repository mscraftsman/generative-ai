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

// *** AUTO-GENERATED FILE - DO NOT EDIT MANUALLY *** //

namespace Mscc.GenerativeAI.Types
{
	/// <summary>
	/// A <c>FileSearchStore</c> is a collection of <c>Document</c>s.
	/// </summary>
	public partial class FileSearchStore
	{
		/// <summary>
		/// Output only. The number of documents in the <c>FileSearchStore</c> that are active and ready for retrieval.
		/// </summary>
		public long? ActiveDocumentsCount { get; set; }
		/// <summary>
		/// Output only. The Timestamp of when the <c>FileSearchStore</c> was created.
		/// </summary>
		public DateTime? CreateTime { get; set; }
		/// <summary>
		/// Optional. The human-readable display name for the <c>FileSearchStore</c>. The display name must be no more than 512 characters in length, including spaces. Example: &quot;Docs on Semantic Retriever&quot;
		/// </summary>
		public string? DisplayName { get; set; }
		/// <summary>
		/// Output only. The number of documents in the <c>FileSearchStore</c> that have failed processing.
		/// </summary>
		public long? FailedDocumentsCount { get; set; }
		/// <summary>
		/// Output only. Immutable. Identifier. The <c>FileSearchStore</c> resource name. It is an ID (name excluding the &quot;fileSearchStores/&quot; prefix) that can contain up to 40 characters that are lowercase alphanumeric or dashes (-). It is output only. The unique name will be derived from <c>display_name</c> along with a 12 character random suffix. Example: <c>fileSearchStores/my-awesome-file-search-store-123a456b789c</c> If <c>display_name</c> is not provided, the name will be randomly generated.
		/// </summary>
		public string? Name { get; set; }
		/// <summary>
		/// Output only. The number of documents in the <c>FileSearchStore</c> that are being processed.
		/// </summary>
		public long? PendingDocumentsCount { get; set; }
		/// <summary>
		/// Output only. The size of raw bytes ingested into the <c>FileSearchStore</c>. This is the total size of all the documents in the <c>FileSearchStore</c>.
		/// </summary>
		public long? SizeBytes { get; set; }
		/// <summary>
		/// Output only. The Timestamp of when the <c>FileSearchStore</c> was last updated.
		/// </summary>
		public DateTime? UpdateTime { get; set; }
    }
}