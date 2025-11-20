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

namespace Mscc.GenerativeAI.Types
{
	/// <summary>
	/// A file generated on behalf of a user.
	/// </summary>
	public partial class GeneratedFile
	{
		/// <summary>
		/// Error details if the GeneratedFile ends up in the STATE_FAILED state.
		/// </summary>
		public Status? Error { get; set; }
		/// <summary>
		/// MIME type of the generatedFile.
		/// </summary>
		public string? MimeType { get; set; }
		/// <summary>
		/// Identifier. The name of the generated file. Example: <see cref="generatedFiles/abc-123"/>
		/// </summary>
		public string? Name { get; set; }
    }
}