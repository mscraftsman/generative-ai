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
	/// The response from the model, including candidate completions.
	/// </summary>
	public partial class GenerateTextResponse
	{
		/// <summary>
		/// Candidate responses from the model.
		/// </summary>
		public List<TextCompletion>? Candidates { get; set; }
		/// <summary>
		/// A set of content filtering metadata for the prompt and response text. This indicates which <see cref="SafetyCategory"/>(s) blocked a candidate from this response, the lowest <see cref="HarmProbability"/> that triggered a block, and the HarmThreshold setting for that category. This indicates the smallest change to the <see cref="SafetySettings"/> that would be necessary to unblock at least 1 response. The blocking is configured by the <see cref="SafetySettings"/> in the request (or the default <see cref="SafetySettings"/> of the API).
		/// </summary>
		public List<ContentFilter>? Filters { get; set; }
		/// <summary>
		/// Returns any safety feedback related to content filtering.
		/// </summary>
		public List<SafetyFeedback>? SafetyFeedback { get; set; }
    }
}
