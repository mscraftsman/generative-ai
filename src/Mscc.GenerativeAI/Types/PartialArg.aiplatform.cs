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
	/// Partial argument value of the function call.
	/// </summary>
	public partial class PartialArg
	{
		/// <summary>
		/// Optional. Represents a boolean value.
		/// </summary>
		public bool? BoolValue { get; set; }
		/// <summary>
		/// Required. A JSON Path (RFC 9535) to the argument being streamed. https://datatracker.ietf.org/doc/html/rfc9535. e.g. &quot;$.foo.bar[0].data&quot;.
		/// </summary>
		public string? JsonPath { get; set; }
		/// <summary>
		/// Optional. Represents a null value.
		/// </summary>
		public NullValueType? NullValue { get; set; }
		/// <summary>
		/// Optional. Represents a double value.
		/// </summary>
		public double? NumberValue { get; set; }
		/// <summary>
		/// Optional. Represents a string value.
		/// </summary>
		public string? StringValue { get; set; }
		/// <summary>
		/// Optional. Whether this is not the last part of the same json_path. If true, another PartialArg message for the current json_path is expected to follow.
		/// </summary>
		public bool? WillContinue { get; set; }

		[JsonConverter(typeof(JsonStringEnumConverter<NullValueType>))]
		public enum NullValueType
		{
			/// <summary>
			/// Null value.
			/// </summary>
			NullValue,
		}
    }
}