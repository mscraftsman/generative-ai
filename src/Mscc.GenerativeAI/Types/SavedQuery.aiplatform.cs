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
	/// A SavedQuery is a view of the dataset. It references a subset of annotations by problem type and filters.
	/// </summary>
	public partial class SavedQuery
	{
		/// <summary>
		/// Output only. Filters on the Annotations in the dataset.
		/// </summary>
		public string? AnnotationFilter { get; set; }
		/// <summary>
		/// Output only. Number of AnnotationSpecs in the context of the SavedQuery.
		/// </summary>
		public int? AnnotationSpecCount { get; set; }
		/// <summary>
		/// Output only. Timestamp when this SavedQuery was created.
		/// </summary>
		public DateTime? CreateTime { get; set; }
		/// <summary>
		/// Required. The user-defined name of the SavedQuery. The name can be up to 128 characters long and can consist of any UTF-8 characters.
		/// </summary>
		public string? DisplayName { get; set; }
		/// <summary>
		/// Used to perform a consistent read-modify-write update. If not set, a blind &quot;overwrite&quot; update happens.
		/// </summary>
		public string? Etag { get; set; }
		/// <summary>
		/// Some additional information about the SavedQuery.
		/// </summary>
		public object? Metadata { get; set; }
		/// <summary>
		/// Output only. Resource name of the SavedQuery.
		/// </summary>
		public string? Name { get; set; }
		/// <summary>
		/// Required. Problem type of the SavedQuery. Allowed values: * IMAGE_CLASSIFICATION_SINGLE_LABEL * IMAGE_CLASSIFICATION_MULTI_LABEL * IMAGE_BOUNDING_POLY * IMAGE_BOUNDING_BOX * TEXT_CLASSIFICATION_SINGLE_LABEL * TEXT_CLASSIFICATION_MULTI_LABEL * TEXT_EXTRACTION * TEXT_SENTIMENT * VIDEO_CLASSIFICATION * VIDEO_OBJECT_TRACKING
		/// </summary>
		public string? ProblemType { get; set; }
		/// <summary>
		/// Output only. If the Annotations belonging to the SavedQuery can be used for AutoML training.
		/// </summary>
		public bool? SupportAutomlTraining { get; set; }
		/// <summary>
		/// Output only. Timestamp when SavedQuery was last updated.
		/// </summary>
		public DateTime? UpdateTime { get; set; }
    }
}