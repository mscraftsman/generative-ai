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
	/// GoogleSearch tool type. Tool to support Google Search in Model. Powered by Google.
	/// </summary>
	public partial class GoogleSearch : ITool
	{
		/// <summary>
		/// Optional. Filter search results to a specific time range. If customers set a start time, they must set an end time (and vice versa).
		/// </summary>
		public Interval? TimeRangeFilter { get; set; }
    }
}