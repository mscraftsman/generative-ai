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
using System;

namespace Mscc.GenerativeAI.Types
{
	/// <summary>
	/// A <see cref="FileSearchStore"/> is a collection of <see cref="Document"/>s.
	/// </summary>
	public partial class FileSearchStore
	{
		/// <summary>
		/// Output only. The number of documents in the <see cref="FileSearchStore"/> that are active and ready for retrieval.
		/// </summary>
		public long? ActiveDocumentsCount { get; set; }
		/// <summary>
		/// Output only. The Timestamp of when the <see cref="FileSearchStore"/> was created.
		/// </summary>
		public DateTime? CreateTime { get; set; }
		/// <summary>
		/// Optional. The human-readable display name for the <see cref="FileSearchStore"/>. The display name must be no more than 512 characters in length, including spaces. Example: "Docs on Semantic Retriever"
		/// </summary>
		public string? DisplayName { get; set; }
		/// <summary>
		/// Output only. The number of documents in the <see cref="FileSearchStore"/> that have failed processing.
		/// </summary>
		public long? FailedDocumentsCount { get; set; }
		/// <summary>
		/// Output only. Immutable. Identifier. The <see cref="FileSearchStore"/> resource name. It is an ID (name excluding the "fileSearchStores/" prefix) that can contain up to 40 characters that are lowercase alphanumeric or dashes (-). It is output only. The unique name will be derived from <see cref="display_name"/> along with a 12 character random suffix. Example: <see cref="fileSearchStores/my-awesome-file-search-store-123a456b789c"/> If <see cref="display_name"/> is not provided, the name will be randomly generated.
		/// </summary>
		public string? Name { get; set; }
		/// <summary>
		/// Output only. The number of documents in the <see cref="FileSearchStore"/> that are being processed.
		/// </summary>
		public long? PendingDocumentsCount { get; set; }
		/// <summary>
		/// Output only. The size of raw bytes ingested into the <see cref="FileSearchStore"/>. This is the total size of all the documents in the <see cref="FileSearchStore"/>.
		/// </summary>
		public long? SizeBytes { get; set; }
		/// <summary>
		/// Output only. The Timestamp of when the <see cref="FileSearchStore"/> was last updated.
		/// </summary>
		public DateTime? UpdateTime { get; set; }
    }
}