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
	/// Request for <see cref="ImportFile"/> to import a File API file with a <see cref="FileSearchStore"/>. LINT.IfChange(ImportFileRequest)
	/// </summary>
	public partial class ImportFileRequest
	{
		/// <summary>
		/// Optional. Config for telling the service how to chunk the file. If not provided, the service will use default parameters.
		/// </summary>
		public ChunkingConfig? ChunkingConfig { get; set; }
		/// <summary>
		/// Custom metadata to be associated with the file.
		/// </summary>
		public List<CustomMetadata>? CustomMetadata { get; set; }
		/// <summary>
		/// Required. The name of the <see cref="File"/> to import. Example: <see cref="files/abc-123"/>
		/// </summary>
		public string? FileName { get; set; }
    }
}
