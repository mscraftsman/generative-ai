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
	/// A transport that can stream HTTP requests and responses. Next ID: 6
	/// </summary>
	public partial class StreamableHttpTransport
	{
		/// <summary>
		/// Optional: Fields for authentication headers, timeouts, etc., if needed.
		/// </summary>
		public object? Headers { get; set; }
		/// <summary>
		/// Timeout for SSE read operations.
		/// </summary>
		public string? SseReadTimeout { get; set; }
		/// <summary>
		/// Whether to close the client session when the transport closes.
		/// </summary>
		public bool? TerminateOnClose { get; set; }
		/// <summary>
		/// HTTP timeout for regular operations.
		/// </summary>
		public string? Timeout { get; set; }
		/// <summary>
		/// The full URL for the MCPServer endpoint. Example: &quot;https://api.example.com/mcp&quot;
		/// </summary>
		public string? Url { get; set; }
    }
}