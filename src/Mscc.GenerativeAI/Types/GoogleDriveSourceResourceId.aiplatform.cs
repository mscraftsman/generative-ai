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
using System.Text.Json.Serialization;

// *** AUTO-GENERATED FILE - DO NOT EDIT MANUALLY *** //

namespace Mscc.GenerativeAI.Types
{
	/// <summary>
	/// The type and ID of the Google Drive resource.
	/// </summary>
	public partial class GoogleDriveSourceResourceId
	{
		/// <summary>
		/// Required. The ID of the Google Drive resource.
		/// </summary>
		public string? ResourceId { get; set; }
		/// <summary>
		/// Required. The type of the Google Drive resource.
		/// </summary>
		public GoogleDriveSourceResourceIdResourceType? ResourceType { get; set; }

		[JsonConverter(typeof(JsonStringEnumConverter<GoogleDriveSourceResourceIdResourceType>))]
		public enum GoogleDriveSourceResourceIdResourceType
		{
			/// <summary>
			/// Unspecified resource type.
			/// </summary>
			ResourceTypeUnspecified,
			/// <summary>
			/// File resource type.
			/// </summary>
			ResourceTypeFile,
			/// <summary>
			/// Folder resource type.
			/// </summary>
			ResourceTypeFolder,
		}
    }
}