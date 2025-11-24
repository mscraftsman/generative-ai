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
using System.Collections.Generic;

// *** AUTO-GENERATED FILE - DO NOT EDIT MANUALLY *** //

namespace Mscc.GenerativeAI.Types
{
	/// <summary>
	/// A collection of DataItems and Annotations on them.
	/// </summary>
	public partial class Dataset
	{

		/// <summary>
		/// Output only. Timestamp when this Dataset was created.
		/// </summary>
		public DateTime? CreateTime { get; set; }
		/// <summary>
		/// Output only. The number of DataItems in this Dataset. Only apply for non-structured Dataset.
		/// </summary>
		public long? DataItemCount { get; set; }
		/// <summary>
		/// The description of the Dataset.
		/// </summary>
		public string? Description { get; set; }
		/// <summary>
		/// Required. The user-defined name of the Dataset. The name can be up to 128 characters long and can consist of any UTF-8 characters.
		/// </summary>
		public string? DisplayName { get; set; }
		/// <summary>
		/// Customer-managed encryption key spec for a Dataset. If set, this Dataset and all sub-resources of this Dataset will be secured by this key.
		/// </summary>
		public EncryptionSpec? EncryptionSpec { get; set; }
		/// <summary>
		/// Used to perform consistent read-modify-write updates. If not set, a blind &quot;overwrite&quot; update happens.
		/// </summary>
		public string? Etag { get; set; }
		/// <summary>
		/// The labels with user-defined metadata to organize your Datasets. Label keys and values can be no longer than 64 characters (Unicode codepoints), can only contain lowercase letters, numeric characters, underscores and dashes. International characters are allowed. No more than 64 user labels can be associated with one Dataset (System labels are excluded). See https://goo.gl/xmQnxf for more information and examples of labels. System reserved label keys are prefixed with &quot;aiplatform.googleapis.com/&quot; and are immutable. Following system labels exist for each Dataset: * &quot;aiplatform.googleapis.com/dataset_metadata_schema&quot;: output only, its value is the metadata_schema&apos;s title.
		/// </summary>
		public object? Labels { get; set; }
		/// <summary>
		/// Required. Additional information about the Dataset.
		/// </summary>
		public object? Metadata { get; set; }
		/// <summary>
		/// Output only. The resource name of the Artifact that was created in MetadataStore when creating the Dataset. The Artifact resource name pattern is <c>projects/{project}/locations/{location}/metadataStores/{metadata_store}/artifacts/{artifact}</c>.
		/// </summary>
		public string? MetadataArtifact { get; set; }
		/// <summary>
		/// Required. Points to a YAML file stored on Google Cloud Storage describing additional information about the Dataset. The schema is defined as an OpenAPI 3.0.2 Schema Object. The schema files that can be used here are found in gs://google-cloud-aiplatform/schema/dataset/metadata/.
		/// </summary>
		public string? MetadataSchemaUri { get; set; }
		/// <summary>
		/// Optional. Reference to the public base model last used by the dataset. Only set for prompt datasets.
		/// </summary>
		public string? ModelReference { get; set; }
		/// <summary>
		/// Output only. Identifier. The resource name of the Dataset. Format: <c>projects/{project}/locations/{location}/datasets/{dataset}</c>
		/// </summary>
		public string? Name { get; set; }
		/// <summary>
		/// Output only. Reserved for future use.
		/// </summary>
		public bool? SatisfiesPzi { get; set; }
		/// <summary>
		/// Output only. Reserved for future use.
		/// </summary>
		public bool? SatisfiesPzs { get; set; }
		/// <summary>
		/// All SavedQueries belong to the Dataset will be returned in List/Get Dataset response. The annotation_specs field will not be populated except for UI cases which will only use annotation_spec_count. In CreateDataset request, a SavedQuery is created together if this field is set, up to one SavedQuery can be set in CreateDatasetRequest. The SavedQuery should not contain any AnnotationSpec.
		/// </summary>
		public List<SavedQuery>? SavedQueries { get; set; }
		/// <summary>
		/// Output only. Timestamp when this Dataset was last updated.
		/// </summary>
		public DateTime? UpdateTime { get; set; }
    }
}