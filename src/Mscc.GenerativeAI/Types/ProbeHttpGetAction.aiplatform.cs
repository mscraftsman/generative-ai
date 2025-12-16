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
	/// HttpGetAction describes an action based on HTTP Get requests.
	/// </summary>
	public partial class ProbeHttpGetAction
	{
		/// <summary>
		/// Host name to connect to, defaults to the model serving container&apos;s IP. You probably want to set &quot;Host&quot; in httpHeaders instead.
		/// </summary>
		public string? Host { get; set; }
		/// <summary>
		/// Custom headers to set in the request. HTTP allows repeated headers.
		/// </summary>
		public List<ProbeHttpHeader>? HttpHeaders { get; set; }
		/// <summary>
		/// Path to access on the HTTP server.
		/// </summary>
		public string? Path { get; set; }
		/// <summary>
		/// Number of the port to access on the container. Number must be in the range 1 to 65535.
		/// </summary>
		public int? Port { get; set; }
		/// <summary>
		/// Scheme to use for connecting to the host. Defaults to HTTP. Acceptable values are &quot;HTTP&quot; or &quot;HTTPS&quot;.
		/// </summary>
		public string? Scheme { get; set; }
    }
}