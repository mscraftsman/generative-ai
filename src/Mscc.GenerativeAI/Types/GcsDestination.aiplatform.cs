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
	/// The Google Cloud Storage location where the output is to be written to.
	/// </summary>
	public partial class GcsDestination
	{
		/// <summary>
		/// Required. Google Cloud Storage URI to output directory. If the uri doesn&apos;t end with &apos;/&apos;, a &apos;/&apos; will be automatically appended. The directory is created if it doesn&apos;t exist.
		/// </summary>
		public string? OutputUriPrefix { get; set; }
    }
}