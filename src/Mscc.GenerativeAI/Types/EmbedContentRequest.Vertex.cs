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
    /// Request containing the <see cref="Content"/> for the model to embed.
    /// </summary>
    public partial class EmbedContentRequest
    {
	    /// <summary>
	    /// Required. The content to embed. Only the <see cref="parts.text"/> fields will be counted.
	    /// </summary>
	    public ContentResponse? Content { get; set; }
		/// <summary>
        /// Optional. Whether to silently truncate the input content if it's longer than the maximum sequence length.
        /// </summary>
        public bool? AutoTruncate { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public EmbedContentRequest() { }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="prompt"></param>
        public EmbedContentRequest(string prompt)
        {
            Content = new() { Text = prompt, Role = Role.User};
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="prompts"></param>
        public EmbedContentRequest(List<string> prompts)
        {
            Content = new();
            foreach (var prompt in prompts)
            {
                Content.Parts.Add(new()
                {
                    Text = prompt
                });
            }
        }
    }
}