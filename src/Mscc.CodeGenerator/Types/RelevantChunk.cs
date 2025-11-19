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
	/// The information for a chunk relevant to a query.
	/// </summary>
	public partial class RelevantChunk
	{
		/// <summary>
		/// <see cref="Chunk"/> associated with the query.
		/// </summary>
		public Chunk? Chunk { get; set; }
		/// <summary>
		/// <see cref="Chunk"/> relevance to the query.
		/// </summary>
		public double? ChunkRelevanceScore { get; set; }
		/// <summary>
		/// <see cref="Document"/> associated with the chunk.
		/// </summary>
		public Document? Document { get; set; }
    }
}
