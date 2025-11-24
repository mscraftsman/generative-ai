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
	/// The response message for Operations.ListOperations.
	/// </summary>
	public partial class ListOperationsResponse
	{
		/// <summary>
		/// The standard List next-page token.
		/// </summary>
		public string? NextPageToken { get; set; }
		/// <summary>
		/// A list of operations that matches the specified filter in the request.
		/// </summary>
		public List<Operation>? Operations { get; set; }
		/// <summary>
		/// Unordered list. Unreachable resources. Populated when the request sets <c>ListOperationsRequest.return_partial_success</c> and reads across collections. For example, when attempting to list all resources across all supported locations.
		/// </summary>
		public List<string>? Unreachable { get; set; }
    }
}