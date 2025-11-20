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
using System.Collections.Generic;

namespace Mscc.GenerativeAI.Types
{
	/// <summary>
	/// Batch request to get a text embedding from the model.
	/// </summary>
	public partial class BatchEmbedTextRequest
	{
		/// <summary>
		/// Optional. Embed requests for the batch. Only one of <see cref="texts"/> or <see cref="requests"/> can be set.
		/// </summary>
		public List<EmbedTextRequest>? Requests { get; set; }
		/// <summary>
		/// Optional. The free-form input texts that the model will turn into an embedding. The current limit is 100 texts, over which an error will be thrown.
		/// </summary>
		public List<string>? Texts { get; set; }
    }
}