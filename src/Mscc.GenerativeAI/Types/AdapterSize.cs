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

	[JsonConverter(typeof(JsonStringEnumConverter<AdapterSize>))]
	public enum AdapterSize
	{
		/// <summary>
		/// Adapter size is unspecified.
		/// </summary>
		AdapterSizeUnspecified,
		/// <summary>
		/// Adapter size 1.
		/// </summary>
		AdapterSizeOne,
		/// <summary>
		/// Adapter size 2.
		/// </summary>
		AdapterSizeTwo,
		/// <summary>
		/// Adapter size 4.
		/// </summary>
		AdapterSizeFour,
		/// <summary>
		/// Adapter size 8.
		/// </summary>
		AdapterSizeEight,
		/// <summary>
		/// Adapter size 16.
		/// </summary>
		AdapterSizeSixteen,
		/// <summary>
		/// Adapter size 32.
		/// </summary>
		AdapterSizeThirtyTwo,
	}
}