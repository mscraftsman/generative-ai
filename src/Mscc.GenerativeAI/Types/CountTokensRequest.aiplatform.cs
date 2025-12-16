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
	/// Request message for PredictionService.CountTokens.
	/// </summary>
	public partial class CountTokensRequest
	{

		/// <summary>
		/// Optional. Generation config that the model will use to generate the response.
		/// </summary>
		public GenerationConfig? GenerationConfig { get; set; }
		/// <summary>
		/// Optional. The instances that are the input to token counting call. Schema is identical to the prediction schema of the underlying model.
		/// </summary>
		public List<object>? Instances { get; set; }
		/// <summary>
		/// Optional. The name of the publisher model requested to serve the prediction. Format: <c>projects/{project}/locations/{location}/publishers/*/models/*</c>
		/// </summary>
		public string? Model { get; set; }
		/// <summary>
		/// Optional. The user provided system instructions for the model. Note: only text should be used in parts and content in each part will be in a separate paragraph.
		/// </summary>
		public Content? SystemInstruction { get; set; }
		/// <summary>
		/// Optional. A list of <c>Tools</c> the model may use to generate the next response. A <c>Tool</c> is a piece of code that enables the system to interact with external systems to perform an action, or set of actions, outside of knowledge and scope of the model.
		/// </summary>
		public Tools? Tools { get; set; }
    }
}