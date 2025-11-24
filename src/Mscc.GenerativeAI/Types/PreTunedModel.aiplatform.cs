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
	/// A pre-tuned model for continuous tuning.
	/// </summary>
	public partial class PreTunedModel
	{
		/// <summary>
		/// Output only. The name of the base model this PreTunedModel was tuned from.
		/// </summary>
		public string? BaseModel { get; set; }
		/// <summary>
		/// Optional. The source checkpoint id. If not specified, the default checkpoint will be used.
		/// </summary>
		public string? CheckpointId { get; set; }
		/// <summary>
		/// The resource name of the Model. E.g., a model resource name with a specified version id or alias: <c>projects/{project}/locations/{location}/models/{model}@{version_id}</c> <c>projects/{project}/locations/{location}/models/{model}@{alias}</c> Or, omit the version id to use the default version: <c>projects/{project}/locations/{location}/models/{model}</c>
		/// </summary>
		public string? TunedModelName { get; set; }
    }
}