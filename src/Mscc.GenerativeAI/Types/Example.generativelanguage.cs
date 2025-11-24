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

// *** AUTO-GENERATED FILE - DO NOT EDIT MANUALLY *** //

namespace Mscc.GenerativeAI.Types
{
	/// <summary>
	/// An input/output example used to instruct the Model. It demonstrates how the model should respond or format its response.
	/// </summary>
	public partial class Example
	{
		/// <summary>
		/// Required. An example of an input <c>Message</c> from the user.
		/// </summary>
		public Message? Input { get; set; }
		/// <summary>
		/// Required. An example of what the model should output given the input.
		/// </summary>
		public Message? Output { get; set; }
    }
}