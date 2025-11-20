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
	/// Segment of the content.
	/// </summary>
	public partial class Segment
	{
		/// <summary>
		/// Output only. End index in the given Part, measured in bytes. Offset from the start of the Part, exclusive, starting at zero.
		/// </summary>
		public int? EndIndex { get; set; }
		/// <summary>
		/// Output only. The index of a Part object within its parent Content object.
		/// </summary>
		public int? PartIndex { get; set; }
		/// <summary>
		/// Output only. Start index in the given Part, measured in bytes. Offset from the start of the Part, inclusive, starting at zero.
		/// </summary>
		public int? StartIndex { get; set; }
		/// <summary>
		/// Output only. The text corresponding to the segment from the response.
		/// </summary>
		public string? Text { get; set; }
    }
}