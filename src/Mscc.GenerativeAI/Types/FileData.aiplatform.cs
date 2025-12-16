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
	/// URI-based data. A FileData message contains a URI pointing to data of a specific media type. It is used to represent images, audio, and video stored in Google Cloud Storage.
	/// </summary>
	public partial class FileData
	{

		/// <summary>
		/// Optional. The display name of the file. Used to provide a label or filename to distinguish files. This field is only returned in <c>PromptMessage</c> for prompt management. It is used in the Gemini calls only when server side tools (<c>code_execution</c>, <c>google_search</c>, and <c>url_context</c>) are enabled.
		/// </summary>
		public string? DisplayName { get; set; }
    }
}