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
	/// All of the structured input text passed to the model as a prompt. A <see cref="MessagePrompt"/> contains a structured set of fields that provide context for the conversation, examples of user input/model output message pairs that prime the model to respond in different ways, and the conversation history or list of messages representing the alternating turns of the conversation between the user and the model.
	/// </summary>
	public partial class MessagePrompt
	{
		/// <summary>
		/// Optional. Text that should be provided to the model first to ground the response. If not empty, this <see cref="context"/> will be given to the model first before the <see cref="examples"/> and <see cref="messages"/>. When using a <see cref="context"/> be sure to provide it with every request to maintain continuity. This field can be a description of your prompt to the model to help provide context and guide the responses. Examples: "Translate the phrase from English to French." or "Given a statement, classify the sentiment as happy, sad or neutral." Anything included in this field will take precedence over message history if the total input size exceeds the model's <see cref="input_token_limit"/> and the input request is truncated.
		/// </summary>
		public string? Context { get; set; }
		/// <summary>
		/// Optional. Examples of what the model should generate. This includes both user input and the response that the model should emulate. These <see cref="examples"/> are treated identically to conversation messages except that they take precedence over the history in <see cref="messages"/>: If the total input size exceeds the model's <see cref="input_token_limit"/> the input will be truncated. Items will be dropped from <see cref="messages"/> before <see cref="examples"/>.
		/// </summary>
		public List<Example>? Examples { get; set; }
		/// <summary>
		/// Required. A snapshot of the recent conversation history sorted chronologically. Turns alternate between two authors. If the total input size exceeds the model's <see cref="input_token_limit"/> the input will be truncated: The oldest items will be dropped from <see cref="messages"/>.
		/// </summary>
		public List<Message>? Messages { get; set; }
    }
}