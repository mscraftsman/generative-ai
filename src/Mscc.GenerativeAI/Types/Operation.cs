/*
 * Copyright 2024-2025 Jochen Kirstätter
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

namespace Mscc.GenerativeAI.Types
{
	/// <summary>
	/// This resource represents a long-running operation that is the result of a network API call.
	/// </summary>
	public partial class Operation
	{
		/// <summary>
		/// If the value is <see cref="false"/>, it means the operation is still in progress. If <see cref="true"/>, the operation is completed, and either <see cref="error"/> or <see cref="response"/> is available.
		/// </summary>
		public bool Done { get; set; }
		/// <summary>
		/// The error result of the operation in case of failure or cancellation.
		/// </summary>
		public Status? Error { get; set; }
		/// <summary>
		/// Service-specific metadata associated with the operation. It typically contains progress information and common metadata such as create time. Some services might not provide such metadata. Any method that returns a long-running operation should document the metadata type, if any.
		/// </summary>
		public object? Metadata { get; set; }
		/// <summary>
		/// The server-assigned name, which is only unique within the same service that originally returns it. If you use the default HTTP mapping, the <see cref="name"/> should be a resource name ending with <see cref="operations/{unique_id}"/>.
		/// </summary>
		public string? Name { get; set; }
		/// <summary>
		/// The normal, successful response of the operation. If the original method returns no data on success, such as <see cref="Delete"/>, the response is <see cref="google.protobuf.Empty"/>. If the original method is standard <see cref="Get"/>/<see cref="Create"/>/<see cref="Update"/>, the response should be the resource. For other methods, the response should have the type <see cref="XxxResponse"/>, where <see cref="Xxx"/> is the original method name. For example, if the original method name is <see cref="TakeSnapshot()"/>, the inferred response type is <see cref="TakeSnapshotResponse"/>.
		/// </summary>
		public object? Response { get; set; }
    }
}