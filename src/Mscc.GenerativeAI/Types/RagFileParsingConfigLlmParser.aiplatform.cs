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
	/// Specifies the LLM parsing for RagFiles.
	/// </summary>
	public partial class RagFileParsingConfigLlmParser
	{
		/// <summary>
		/// The prompt to use for parsing. If not specified, a default prompt will be used.
		/// </summary>
		public string? CustomParsingPrompt { get; set; }
		/// <summary>
		/// The maximum number of requests the job is allowed to make to the LLM model per minute in this project. Consult https://cloud.google.com/vertex-ai/generative-ai/docs/quotas and your document size to set an appropriate value here. If this value is not specified, max_parsing_requests_per_min will be used by indexing pipeline job as the global limit.
		/// </summary>
		public int? GlobalMaxParsingRequestsPerMin { get; set; }
		/// <summary>
		/// The maximum number of requests the job is allowed to make to the LLM model per minute. Consult https://cloud.google.com/vertex-ai/generative-ai/docs/quotas and your document size to set an appropriate value here. If unspecified, a default value of 5000 QPM would be used.
		/// </summary>
		public int? MaxParsingRequestsPerMin { get; set; }
		/// <summary>
		/// The name of a LLM model used for parsing. Format: * <c>projects/{project_id}/locations/{location}/publishers/{publisher}/models/{model}</c>
		/// </summary>
		public string? ModelName { get; set; }
    }
}