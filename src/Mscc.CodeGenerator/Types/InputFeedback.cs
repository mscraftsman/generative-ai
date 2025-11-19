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
using System.Collections.Generic;

namespace Mscc.GenerativeAI.Types
{
	/// <summary>
	/// Feedback related to the input data used to answer the question, as opposed to the model-generated response to the question.
	/// </summary>
	public partial class InputFeedback
	{
		/// <summary>
		/// Optional. If set, the input was blocked and no candidates are returned. Rephrase the input.
		/// </summary>
		public BlockReason? BlockReason { get; set; }
		/// <summary>
		/// Ratings for safety of the input. There is at most one rating per category.
		/// </summary>
		public List<SafetyRating>? SafetyRatings { get; set; }
    }
}
