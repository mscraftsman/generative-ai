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
	/// Safety setting, affecting the safety-blocking behavior. Passing a safety setting for a category changes the allowed probability that content is blocked.
	/// </summary>
	public partial class SafetySetting
	{
		/// <summary>
		/// Required. The category for this setting.
		/// </summary>
		public HarmCategory? Category { get; set; }
		/// <summary>
		/// Required. Controls the probability threshold at which harm is blocked.
		/// </summary>
		public HarmBlockThreshold? Threshold { get; set; }
    }
}