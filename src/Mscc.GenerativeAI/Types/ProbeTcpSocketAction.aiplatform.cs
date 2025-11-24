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
	/// TcpSocketAction probes the health of a container by opening a TCP socket connection.
	/// </summary>
	public partial class ProbeTcpSocketAction
	{
		/// <summary>
		/// Optional: Host name to connect to, defaults to the model serving container&apos;s IP.
		/// </summary>
		public string? Host { get; set; }
		/// <summary>
		/// Number of the port to access on the container. Number must be in the range 1 to 65535.
		/// </summary>
		public int? Port { get; set; }
    }
}