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
	/// The Model Registry Model and Online Prediction Endpoint associated with this TuningJob.
	/// </summary>
	public partial class TunedModelResponse
	{

		/// <summary>
		/// Output only. The checkpoints associated with this TunedModel. This field is only populated for tuning jobs that enable intermediate checkpoints.
		/// </summary>
		public List<TunedModelCheckpoint>? Checkpoints { get; set; }
		/// <summary>
		/// Output only. A resource name of an Endpoint. Format: <c>projects/{project}/locations/{location}/endpoints/{endpoint}</c>.
		/// </summary>
		public string? Endpoint { get; set; }
		/// <summary>
		/// Output only. The resource name of the TunedModel. Format: <c>projects/{project}/locations/{location}/models/{model}@{version_id}</c> When tuning from a base model, the version ID will be 1. For continuous tuning, if the provided tuned_model_display_name is set and different from parent model&apos;s display name, the tuned model will have a new parent model with version 1. Otherwise the version id will be incremented by 1 from the last version ID in the parent model. E.g., <c>projects/{project}/locations/{location}/models/{model}@{last_version_id + 1}</c>
		/// </summary>
		public string? Model { get; set; }
    }
}