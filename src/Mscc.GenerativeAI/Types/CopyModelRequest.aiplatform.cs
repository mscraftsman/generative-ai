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
	/// Request message for ModelService.CopyModel.
	/// </summary>
	public partial class CopyModelRequest
	{
		/// <summary>
		/// Customer-managed encryption key options. If this is set, then the Model copy will be encrypted with the provided encryption key.
		/// </summary>
		public EncryptionSpec? EncryptionSpec { get; set; }
		/// <summary>
		/// Optional. Copy source_model into a new Model with this ID. The ID will become the final component of the model resource name. This value may be up to 63 characters, and valid characters are <c>[a-z0-9_-]</c>. The first character cannot be a number or hyphen.
		/// </summary>
		public string? ModelId { get; set; }
		/// <summary>
		/// Optional. Specify this field to copy source_model into this existing Model as a new version. Format: <c>projects/{project}/locations/{location}/models/{model}</c>
		/// </summary>
		public string? ParentModel { get; set; }
		/// <summary>
		/// Required. The resource name of the Model to copy. That Model must be in the same Project. Format: <c>projects/{project}/locations/{location}/models/{model}</c>
		/// </summary>
		public string? SourceModel { get; set; }
    }
}