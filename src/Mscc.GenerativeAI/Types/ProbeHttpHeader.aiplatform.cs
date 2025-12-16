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
	/// HttpHeader describes a custom header to be used in HTTP probes
	/// </summary>
	public partial class ProbeHttpHeader
	{
		/// <summary>
		/// The header field name. This will be canonicalized upon output, so case-variant names will be understood as the same header.
		/// </summary>
		public string? Name { get; set; }
		/// <summary>
		/// The header field value
		/// </summary>
		public string? Value { get; set; }
    }
}