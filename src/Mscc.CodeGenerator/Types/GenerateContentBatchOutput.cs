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
	/// The output of a batch request. This is returned in the <see cref="BatchGenerateContentResponse"/> or the <see cref="GenerateContentBatch.output"/> field.
	/// </summary>
	public partial class GenerateContentBatchOutput
	{
		/// <summary>
		/// Output only. The responses to the requests in the batch. Returned when the batch was built using inlined requests. The responses will be in the same order as the input requests.
		/// </summary>
		public InlinedResponses? InlinedResponses { get; set; }
		/// <summary>
		/// Output only. The file ID of the file containing the responses. The file will be a JSONL file with a single response per line. The responses will be <see cref="GenerateContentResponse"/> messages formatted as JSON. The responses will be written in the same order as the input requests.
		/// </summary>
		public string? ResponsesFile { get; set; }
    }
}
