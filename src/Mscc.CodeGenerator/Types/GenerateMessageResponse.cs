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
	/// The response from the model. This includes candidate messages and conversation history in the form of chronologically-ordered messages.
	/// </summary>
	public partial class GenerateMessageResponse
	{
		/// <summary>
		/// Candidate response messages from the model.
		/// </summary>
		public List<Message>? Candidates { get; set; }
		/// <summary>
		/// A set of content filtering metadata for the prompt and response text. This indicates which <see cref="SafetyCategory"/>(s) blocked a candidate from this response, the lowest <see cref="HarmProbability"/> that triggered a block, and the HarmThreshold setting for that category.
		/// </summary>
		public List<ContentFilter>? Filters { get; set; }
		/// <summary>
		/// The conversation history used by the model.
		/// </summary>
		public List<Message>? Messages { get; set; }
    }
}
