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
	/// Config representing a model hosted on Vertex Prediction Endpoint.
	/// </summary>
	public partial class RagEmbeddingModelConfigVertexPredictionEndpoint
	{
		/// <summary>
		/// Required. The endpoint resource name. Format: <c>projects/{project}/locations/{location}/publishers/{publisher}/models/{model}</c> or <c>projects/{project}/locations/{location}/endpoints/{endpoint}</c>
		/// </summary>
		public string? Endpoint { get; set; }
		/// <summary>
		/// Output only. The resource name of the model that is deployed on the endpoint. Present only when the endpoint is not a publisher model. Pattern: <c>projects/{project}/locations/{location}/models/{model}</c>
		/// </summary>
		public string? Model { get; set; }
		/// <summary>
		/// Output only. Version ID of the model that is deployed on the endpoint. Present only when the endpoint is not a publisher model.
		/// </summary>
		public string? ModelVersionId { get; set; }
    }
}