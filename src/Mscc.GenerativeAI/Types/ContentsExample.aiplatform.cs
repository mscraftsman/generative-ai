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
	/// A single example of a conversation with the model.
	/// </summary>
	public partial class ContentsExample
	{
		/// <summary>
		/// Required. The content of the conversation with the model that resulted in the expected output.
		/// </summary>
		public List<Content>? Contents { get; set; }
		/// <summary>
		/// Required. The expected output for the given <c>contents</c>. To represent multi-step reasoning, this is a repeated field that contains the iterative steps of the expected output.
		/// </summary>
		public List<ContentsExampleExpectedContent>? ExpectedContents { get; set; }
    }
}