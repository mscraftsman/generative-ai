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
    /// The configuration for routing the request to a specific model. This can be used to control
    /// which model is used for the generation, either automatically or by specifying a model name.
    /// This data type is not supported in Gemini API.
    /// </summary>
    public partial class RoutingConfig
    {
        /// <summary>
        /// In this mode, the model is selected automatically based on the content of the request.
        /// </summary>
        public AutoRoutingMode? AutoMode { get; set; }
        /// <summary>
        /// In this mode, the model is specified manually.
        /// </summary>
        public ManualRoutingMode? ManualMode { get; set; }
    }
}