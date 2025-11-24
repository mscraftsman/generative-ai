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
	/// Request message for ComputeTokens RPC call.
	/// </summary>
	public partial class ComputeTokensRequest
	{
		/// <summary>
		/// Optional. Input content.
		/// </summary>
		public List<Content>? Contents { get; set; }
		/// <summary>
		/// Optional. The instances that are the input to token computing API call. Schema is identical to the prediction schema of the text model, even for the non-text models, like chat models, or Codey models.
		/// </summary>
		public List<object>? Instances { get; set; }
		/// <summary>
		/// Optional. The name of the publisher model requested to serve the prediction. Format: projects/{project}/locations/{location}/publishers/*/models/*
		/// </summary>
		public string? Model { get; set; }
    }
}