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
	/// Response message for PredictionService.Predict.
	/// </summary>
	public partial class PredictResponse
	{

		/// <summary>
		/// ID of the Endpoint&apos;s DeployedModel that served this prediction.
		/// </summary>
		public string? DeployedModelId { get; set; }
		/// <summary>
		/// Output only. Request-level metadata returned by the model. The metadata type will be dependent upon the model implementation.
		/// </summary>
		public object? Metadata { get; set; }
		/// <summary>
		/// Output only. The resource name of the Model which is deployed as the DeployedModel that this prediction hits.
		/// </summary>
		public string? Model { get; set; }
		/// <summary>
		/// Output only. The display name of the Model which is deployed as the DeployedModel that this prediction hits.
		/// </summary>
		public string? ModelDisplayName { get; set; }
		/// <summary>
		/// Output only. The version ID of the Model which is deployed as the DeployedModel that this prediction hits.
		/// </summary>
		public string? ModelVersionId { get; set; }
    }
}