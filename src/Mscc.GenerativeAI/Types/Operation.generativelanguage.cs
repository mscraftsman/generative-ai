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
	/// This resource represents a long-running operation that is the result of a network API call.
	/// </summary>
	public partial class Operation
	{
		/// <summary>
		/// The error result of the operation in case of failure or cancellation.
		/// </summary>
		public Status? Error { get; set; }
		/// <summary>
		/// Service-specific metadata associated with the operation. It typically contains progress information and common metadata such as create time. Some services might not provide such metadata. Any method that returns a long-running operation should document the metadata type, if any.
		/// </summary>
		public object? Metadata { get; set; }
		/// <summary>
		/// The server-assigned name, which is only unique within the same service that originally returns it. If you use the default HTTP mapping, the <c>name</c> should be a resource name ending with <c>operations/{unique_id}</c>.
		/// </summary>
		public string? Name { get; set; }
		/// <summary>
		/// The normal, successful response of the operation. If the original method returns no data on success, such as <c>Delete</c>, the response is <c>google.protobuf.Empty</c>. If the original method is standard <c>Get</c>/<c>Create</c>/<c>Update</c>, the response should be the resource. For other methods, the response should have the type <c>XxxResponse</c>, where <c>Xxx</c> is the original method name. For example, if the original method name is <c>TakeSnapshot()</c>, the inferred response type is <c>TakeSnapshotResponse</c>.
		/// </summary>
		public object? Response { get; set; }
    }
}