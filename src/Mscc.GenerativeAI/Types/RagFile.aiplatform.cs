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
using System.Text.Json.Serialization;

// *** AUTO-GENERATED FILE - DO NOT EDIT MANUALLY *** //

namespace Mscc.GenerativeAI.Types
{
	/// <summary>
	/// A RagFile contains user data for chunking, embedding and indexing.
	/// </summary>
	public partial class RagFile
	{
		/// <summary>
		/// Output only. Timestamp when this RagFile was created.
		/// </summary>
		public DateTime? CreateTime { get; set; }
		/// <summary>
		/// Optional. The description of the RagFile.
		/// </summary>
		public string? Description { get; set; }
		/// <summary>
		/// Output only. The RagFile is encapsulated and uploaded in the UploadRagFile request.
		/// </summary>
		public DirectUploadSource? DirectUploadSource { get; set; }
		/// <summary>
		/// Required. The display name of the RagFile. The name can be up to 128 characters long and can consist of any UTF-8 characters.
		/// </summary>
		public string? DisplayName { get; set; }
		/// <summary>
		/// Output only. State of the RagFile.
		/// </summary>
		public FileStatus? FileStatus { get; set; }
		/// <summary>
		/// Output only. Google Cloud Storage location of the RagFile. It does not support wildcards in the Cloud Storage uri for now.
		/// </summary>
		public GcsSource? GcsSource { get; set; }
		/// <summary>
		/// Output only. Google Drive location. Supports importing individual files as well as Google Drive folders.
		/// </summary>
		public GoogleDriveSource? GoogleDriveSource { get; set; }
		/// <summary>
		/// The RagFile is imported from a Jira query.
		/// </summary>
		public JiraSource? JiraSource { get; set; }
		/// <summary>
		/// Output only. The resource name of the RagFile.
		/// </summary>
		public string? Name { get; set; }
		/// <summary>
		/// Output only. The type of the RagFile.
		/// </summary>
		public RagFileRagFileType? RagFileType { get; set; }
		/// <summary>
		/// The RagFile is imported from a SharePoint source.
		/// </summary>
		public SharePointSources? SharePointSources { get; set; }
		/// <summary>
		/// Output only. The size of the RagFile in bytes.
		/// </summary>
		public long? SizeBytes { get; set; }
		/// <summary>
		/// The RagFile is imported from a Slack channel.
		/// </summary>
		public SlackSource? SlackSource { get; set; }
		/// <summary>
		/// Output only. Timestamp when this RagFile was last updated.
		/// </summary>
		public DateTime? UpdateTime { get; set; }
		/// <summary>
		/// Output only. The metadata for metadata search. The user_metadata Needs to be in JSON format.
		/// </summary>
		public string? UserMetadata { get; set; }

		[JsonConverter(typeof(JsonStringEnumConverter<RagFileRagFileType>))]
		public enum RagFileRagFileType
		{
			/// <summary>
			/// RagFile type is unspecified.
			/// </summary>
			RagFileTypeUnspecified,
			/// <summary>
			/// RagFile type is TXT.
			/// </summary>
			RagFileTypeTxt,
			/// <summary>
			/// RagFile type is PDF.
			/// </summary>
			RagFileTypePdf,
		}
    }
}