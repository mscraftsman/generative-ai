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
	/// Grounding chunk.
	/// </summary>
	public partial class GroundingChunk
	{
		/// <summary>
		/// Optional. Grounding chunk from Google Maps.
		/// </summary>
		public Maps? Maps { get; set; }
		/// <summary>
		/// Optional. Grounding chunk from context retrieved by the file search tool.
		/// </summary>
		public RetrievedContext? RetrievedContext { get; set; }
		/// <summary>
		/// Grounding chunk from the web.
		/// </summary>
		public Web? Web { get; set; }
    }
}