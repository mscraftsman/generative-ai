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

namespace Mscc.GenerativeAI.Types
{
	/// <summary>
	/// Request to create a <see cref="Chunk"/>.
	/// </summary>
	public partial class CreateChunkRequest
	{
		/// <summary>
		/// Required. The <see cref="Chunk"/> to create.
		/// </summary>
		public Chunk? Chunk { get; set; }
		/// <summary>
		/// Required. The name of the <see cref="Document"/> where this <see cref="Chunk"/> will be created. Example: <see cref="corpora/my-corpus-123/documents/the-doc-abc"/>
		/// </summary>
		public string? Parent { get; set; }
    }
}