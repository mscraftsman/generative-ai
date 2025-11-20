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
    /// Defines a tool that model can call to access external knowledge.
    /// </summary>
    public partial class Tool
    {
        /// <summary>
        /// Optional. Retrieval tool type. System will always execute the provided retrieval tool(s)
        /// to get external knowledge to answer the prompt. Retrieval results are presented
        /// to the model for generation.
        /// </summary>
        public Retrieval? Retrieval { get; set; }
        /// <summary>
        /// Optional. Enterprise web search tool type.
        /// Specialized retrieval tool that is powered by Vertex AI Search and Sec4 compliance.
        /// </summary>
        public EnterpriseWebSearch? EnterpriseWebSearch { get; set; }
    }
}