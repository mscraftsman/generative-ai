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
    /// The configuration for automated routing. When automated routing is specified, the routing will
    /// be determined by the pretrained routing model and customer provided model routing preference.
    /// This data type is not supported in Gemini API.
    /// </summary>
    public class AutoRoutingMode
    {
	    /// <summary>
	    /// The model routing preference.
	    /// </summary>
	    public ModelRoutingPreference? ModelRoutingPreference { get; set; }
    }
}