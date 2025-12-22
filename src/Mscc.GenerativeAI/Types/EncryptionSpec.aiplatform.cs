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
	/// Represents a customer-managed encryption key specification that can be applied to a Vertex AI resource.
	/// </summary>
	public partial class EncryptionSpec
	{
		/// <summary>
		/// Required. Resource name of the Cloud KMS key used to protect the resource. The Cloud KMS key must be in the same region as the resource. It must have the format <c>projects/{project}/locations/{location}/keyRings/{key_ring}/cryptoKeys/{crypto_key}</c>.
		/// </summary>
		public string? KmsKeyName { get; set; }
    }
}