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

using System;
using System.Linq;

namespace Mscc.GenerativeAI.Types
{
	/// <summary>
	/// The response from the model. This includes candidate messages and conversation history in the form of chronologically-ordered messages.
	/// </summary>
	public partial class GenerateMessageResponse
	{
		/// <summary>
		/// Responded text information of first candidate.
		/// </summary>
		public string? Text =>
			// if (SafetyFeedback?.FirstOrDefault()?.Output?.Rating?.Blocked!)
			//     return string.Empty;
			Candidates?.FirstOrDefault()?.Content;

		/// <summary>
		/// A convenience overload to easily access the responded text.
		/// </summary>
		/// <returns>The responded text information of first candidate.</returns>
		public override string ToString()
		{
			return Text ?? String.Empty;
		}
    }
}