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
	/// Identifier for a part within a <see cref="GroundingPassage"/>.
	/// </summary>
	public partial class GroundingPassageId
	{
		/// <summary>
		/// Output only. Index of the part within the <see cref="GenerateAnswerRequest"/>'s <see cref="GroundingPassage.content"/>.
		/// </summary>
		public int? PartIndex { get; set; }
		/// <summary>
		/// Output only. ID of the passage matching the <see cref="GenerateAnswerRequest"/>'s <see cref="GroundingPassage.id"/>.
		/// </summary>
		public string? PassageId { get; set; }
    }
}