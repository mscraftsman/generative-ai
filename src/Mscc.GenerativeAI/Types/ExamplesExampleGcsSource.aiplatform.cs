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
	/// The Cloud Storage input instances.
	/// </summary>
	public partial class ExamplesExampleGcsSource
	{
		/// <summary>
		/// The format in which instances are given, if not specified, assume it&apos;s JSONL format. Currently only JSONL format is supported.
		/// </summary>
		public DataFormatType? DataFormat { get; set; }
		/// <summary>
		/// The Cloud Storage location for the input instances.
		/// </summary>
		public GcsSource? GcsSource { get; set; }

		[JsonConverter(typeof(JsonStringEnumConverter<DataFormatType>))]
		public enum DataFormatType
		{
			/// <summary>
			/// Format unspecified, used when unset.
			/// </summary>
			DataFormatUnspecified,
			/// <summary>
			/// Examples are stored in JSONL files.
			/// </summary>
			Jsonl,
		}
    }
}