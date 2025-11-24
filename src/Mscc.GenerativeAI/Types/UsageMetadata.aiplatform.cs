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
using System.Text.Json.Serialization;

// *** AUTO-GENERATED FILE - DO NOT EDIT MANUALLY *** //

namespace Mscc.GenerativeAI.Types
{
	/// <summary>
	/// Usage metadata about the content generation request and response. This message provides a detailed breakdown of token usage and other relevant metrics.
	/// </summary>
	public partial class UsageMetadata
	{

		/// <summary>
		/// Output only. The traffic type for this request.
		/// </summary>
		public UsageMetadataTrafficType? TrafficType { get; set; }

		[JsonConverter(typeof(JsonStringEnumConverter<UsageMetadataTrafficType>))]
		public enum UsageMetadataTrafficType
		{
			/// <summary>
			/// Unspecified request traffic type.
			/// </summary>
			TrafficTypeUnspecified,
			/// <summary>
			/// Type for Pay-As-You-Go traffic.
			/// </summary>
			OnDemand,
			/// <summary>
			/// Type for Provisioned Throughput traffic.
			/// </summary>
			ProvisionedThroughput,
		}
    }
}