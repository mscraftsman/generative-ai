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

using System;

namespace Mscc.GenerativeAI.Types
{
	/// <summary>
	/// A resource representing a batch of <see cref="EmbedContent"/> requests.
	/// </summary>
	public partial class EmbedContentBatch
	{
		/// <summary>
		/// Output only. Stats about the batch.
		/// </summary>
		public EmbedContentBatchStats? BatchStats { get; set; }
		/// <summary>
		/// Output only. The time at which the batch was created.
		/// </summary>
		public DateTime? CreateTime { get; set; }
		/// <summary>
		/// Required. The user-defined name of this batch.
		/// </summary>
		public string? DisplayName { get; set; }
		/// <summary>
		/// Output only. The time at which the batch processing completed.
		/// </summary>
		public DateTime? EndTime { get; set; }
		/// <summary>
		/// Required. Input configuration of the instances on which batch processing are performed.
		/// </summary>
		public InputEmbedContentConfig? InputConfig { get; set; }
		/// <summary>
		/// Required. The name of the <see cref="Model"/> to use for generating the completion. Format: <see cref="models/{model}"/>.
		/// </summary>
		public string? Model { get; set; }
		/// <summary>
		/// Output only. Identifier. Resource name of the batch. Format: <see cref="batches/{batch_id}"/>.
		/// </summary>
		public string? Name { get; set; }
		/// <summary>
		/// Output only. The output of the batch request.
		/// </summary>
		public EmbedContentBatchOutput? Output { get; set; }
		/// <summary>
		/// Optional. The priority of the batch. Batches with a higher priority value will be processed before batches with a lower priority value. Negative values are allowed. Default is 0.
		/// </summary>
		public long? Priority { get; set; }
		/// <summary>
		/// Output only. The time at which the batch was last updated.
		/// </summary>
		public DateTime? UpdateTime { get; set; }
    }
}