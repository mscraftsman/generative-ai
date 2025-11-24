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
    /// Media resolution for the input media.
    /// </summary>
    public partial class PartMediaResolution : IPart
    {
        /// <summary>
        /// The tokenization quality used for given media.
        /// </summary>
        public MediaResolution? Level { get; set; }
        
        /// <summary>
        /// Specifies the required sequence length for media tokenization.
        /// </summary>
        public int? NumTokens { get; set; }
    }
}