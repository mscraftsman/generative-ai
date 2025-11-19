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
	/// Counts the number of tokens in the <see cref="prompt"/> sent to a model. Models may tokenize text differently, so each model may return a different <see cref="token_count"/>.
	/// </summary>
	public partial class CountTextTokensRequest
	{
		/// <summary>
		/// Required. The free-form input text given to the model as a prompt.
		/// </summary>
		public TextPrompt? Prompt { get; set; }
    }
}
