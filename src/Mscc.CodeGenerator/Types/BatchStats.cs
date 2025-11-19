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
	/// Stats about the batch.
	/// </summary>
	public partial class BatchStats
	{
		/// <summary>
		/// Output only. The number of requests that failed to be processed.
		/// </summary>
		public long? FailedRequestCount { get; set; }
		/// <summary>
		/// Output only. The number of requests that are still pending processing.
		/// </summary>
		public long? PendingRequestCount { get; set; }
		/// <summary>
		/// Output only. The number of requests in the batch.
		/// </summary>
		public long? RequestCount { get; set; }
		/// <summary>
		/// Output only. The number of requests that were successfully processed.
		/// </summary>
		public long? SuccessfulRequestCount { get; set; }
    }
}
