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
using System.Collections.Generic;

namespace Mscc.GenerativeAI.Types
{
	/// <summary>
	/// A <see cref="Chunk"/> is a subpart of a <see cref="Document"/> that is treated as an independent unit for the purposes of vector representation and storage.
	/// </summary>
	public partial class Chunk
	{
		/// <summary>
		/// Output only. The Timestamp of when the <see cref="Chunk"/> was created.
		/// </summary>
		public DateTime? CreateTime { get; set; }
		/// <summary>
		/// Optional. User provided custom metadata stored as key-value pairs. The maximum number of <see cref="CustomMetadata"/> per chunk is 20.
		/// </summary>
		public List<CustomMetadata>? CustomMetadata { get; set; }
		/// <summary>
		/// Required. The content for the <see cref="Chunk"/>, such as the text string. The maximum number of tokens per chunk is 2043.
		/// </summary>
		public ChunkData? Data { get; set; }
		/// <summary>
		/// Immutable. Identifier. The <see cref="Chunk"/> resource name. The ID (name excluding the "corpora/*/documents/*/chunks/" prefix) can contain up to 40 characters that are lowercase alphanumeric or dashes (-). The ID cannot start or end with a dash. If the name is empty on create, a random 12-character unique ID will be generated. Example: <see cref="corpora/{corpus_id}/documents/{document_id}/chunks/123a456b789c"/>
		/// </summary>
		public string? Name { get; set; }
		/// <summary>
		/// Output only. Current state of the <see cref="Chunk"/>.
		/// </summary>
		public StateChunk? State { get; set; }
		/// <summary>
		/// Output only. The Timestamp of when the <see cref="Chunk"/> was last updated.
		/// </summary>
		public DateTime? UpdateTime { get; set; }
    }
}