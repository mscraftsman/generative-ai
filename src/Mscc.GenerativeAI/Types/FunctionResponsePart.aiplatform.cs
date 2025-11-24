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
	/// A datatype containing media that is part of a <c>FunctionResponse</c> message. A <c>FunctionResponsePart</c> consists of data which has an associated datatype. A <c>FunctionResponsePart</c> can only contain one of the accepted types in <c>FunctionResponsePart.data</c>. A <c>FunctionResponsePart</c> must have a fixed IANA MIME type identifying the type and subtype of the media if the <c>inline_data</c> field is filled with raw bytes.
	/// </summary>
	public partial class FunctionResponsePart
	{
		/// <summary>
		/// URI based data.
		/// </summary>
		public FunctionResponseFileData? FileData { get; set; }
    }
}