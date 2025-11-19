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
	/// Represents a time interval, encoded as a Timestamp start (inclusive) and a Timestamp end (exclusive). The start must be less than or equal to the end. When the start equals the end, the interval is empty (matches no time). When both start and end are unspecified, the interval matches any time.
	/// </summary>
	public partial class Interval
	{
		/// <summary>
		/// Optional. Exclusive end of the interval. If specified, a Timestamp matching this interval will have to be before the end.
		/// </summary>
		public DateTime? EndTime { get; set; }
		/// <summary>
		/// Optional. Inclusive start of the interval. If specified, a Timestamp matching this interval will have to be the same or after the start.
		/// </summary>
		public DateTime? StartTime { get; set; }
    }
}
