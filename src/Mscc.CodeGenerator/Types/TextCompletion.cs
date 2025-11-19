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
	/// Output text returned from a model.
	/// </summary>
	public partial class TextCompletion
	{
		/// <summary>
		/// Output only. Citation information for model-generated <see cref="output"/> in this <see cref="TextCompletion"/>. This field may be populated with attribution information for any text included in the <see cref="output"/>.
		/// </summary>
		public CitationMetadata? CitationMetadata { get; set; }
		/// <summary>
		/// Output only. The generated text returned from the model.
		/// </summary>
		public string? Output { get; set; }
		/// <summary>
		/// Ratings for the safety of a response. There is at most one rating per category.
		/// </summary>
		public List<SafetyRating>? SafetyRatings { get; set; }
    }
}
