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
using System.Collections.Generic;

// *** AUTO-GENERATED FILE - DO NOT EDIT MANUALLY *** //

namespace Mscc.GenerativeAI.Types
{
	/// <summary>
	/// A tuning example with multiturn input.
	/// </summary>
	public partial class TuningMultiturnExample
	{
		/// <summary>
		/// Each Content represents a turn in the conversation.
		/// </summary>
		public List<TuningContent>? Contents { get; set; }
		/// <summary>
		/// Optional. Developer set system instructions. Currently, text only.
		/// </summary>
		public TuningContent? SystemInstruction { get; set; }
    }
}