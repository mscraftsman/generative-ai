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
	/// The base unit of structured text. A <c>Message</c> includes an <c>author</c> and the <c>content</c> of the <c>Message</c>. The <c>author</c> is used to tag messages when they are fed to the model as text.
	/// </summary>
	public partial class Message
	{
		/// <summary>
		/// Optional. The author of this Message. This serves as a key for tagging the content of this Message when it is fed to the model as text. The author can be any alphanumeric string.
		/// </summary>
		public string? Author { get; set; }
		/// <summary>
		/// Output only. Citation information for model-generated <c>content</c> in this <c>Message</c>. If this <c>Message</c> was generated as output from the model, this field may be populated with attribution information for any text included in the <c>content</c>. This field is used only on output.
		/// </summary>
		public CitationMetadata? CitationMetadata { get; set; }
		/// <summary>
		/// Required. The text content of the structured <c>Message</c>.
		/// </summary>
		public string? Content { get; set; }
    }
}