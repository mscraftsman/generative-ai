/*
 * Copyleft 2024-2025 Jochen Kirstätter and contributors
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
using System;

// *** AUTO-GENERATED FILE - DO NOT EDIT MANUALLY *** //

namespace Mscc.GenerativeAI.Types
{
	/// <summary>
	/// SlackChannel contains the Slack channel ID and the time range to import.
	/// </summary>
	public partial class SlackSourceSlackChannelsSlackChannel
	{
		/// <summary>
		/// Required. The Slack channel ID.
		/// </summary>
		public string? ChannelId { get; set; }
		/// <summary>
		/// Optional. The ending timestamp for messages to import.
		/// </summary>
		public DateTime? EndTime { get; set; }
		/// <summary>
		/// Optional. The starting timestamp for messages to import.
		/// </summary>
		public DateTime? StartTime { get; set; }
    }
}