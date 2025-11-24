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
using System.Text.Json.Serialization;

// *** AUTO-GENERATED FILE - DO NOT EDIT MANUALLY *** //

namespace Mscc.GenerativeAI.Types
{
	/// <summary>
	/// A trained machine learning Model.
	/// </summary>
	public partial class ModelResponse
	{

		/// <summary>
		/// Immutable. The path to the directory containing the Model artifact and any of its supporting files. Not required for AutoML Models.
		/// </summary>
		public string? ArtifactUri { get; set; }
		/// <summary>
		/// Optional. User input field to specify the base model source. Currently it only supports specifing the Model Garden models and Genie models.
		/// </summary>
		public ModelBaseModelSource? BaseModelSource { get; set; }
		/// <summary>
		/// Optional. Output only. The checkpoints of the model.
		/// </summary>
		public List<Checkpoint>? Checkpoints { get; set; }
		/// <summary>
		/// Input only. The specification of the container that is to be used when deploying this Model. The specification is ingested upon ModelService.UploadModel, and all binaries it contains are copied and stored internally by Vertex AI. Not required for AutoML Models.
		/// </summary>
		public ModelContainerSpec? ContainerSpec { get; set; }
		/// <summary>
		/// Output only. Timestamp when this Model was uploaded into Vertex AI.
		/// </summary>
		public DateTime? CreateTime { get; set; }
		/// <summary>
		/// The default checkpoint id of a model version.
		/// </summary>
		public string? DefaultCheckpointId { get; set; }
		/// <summary>
		/// Output only. The pointers to DeployedModels created from this Model. Note that Model could have been deployed to Endpoints in different Locations.
		/// </summary>
		public List<DeployedModelRef>? DeployedModels { get; set; }
		/// <summary>
		/// Customer-managed encryption key spec for a Model. If set, this Model and all sub-resources of this Model will be secured by this key.
		/// </summary>
		public EncryptionSpec? EncryptionSpec { get; set; }
		/// <summary>
		/// The default explanation specification for this Model. The Model can be used for requesting explanation after being deployed if it is populated. The Model can be used for batch explanation if it is populated. All fields of the explanation_spec can be overridden by explanation_spec of DeployModelRequest.deployed_model, or explanation_spec of BatchPredictionJob. If the default explanation specification is not set for this Model, this Model can still be used for requesting explanation by setting explanation_spec of DeployModelRequest.deployed_model and for batch explanation by setting explanation_spec of BatchPredictionJob.
		/// </summary>
		public ExplanationSpec? ExplanationSpec { get; set; }
		/// <summary>
		/// Immutable. An additional information about the Model; the schema of the metadata can be found in metadata_schema. Unset if the Model does not have any additional information.
		/// </summary>
		public object? Metadata { get; set; }
		/// <summary>
		/// Output only. The resource name of the Artifact that was created in MetadataStore when creating the Model. The Artifact resource name pattern is <c>projects/{project}/locations/{location}/metadataStores/{metadata_store}/artifacts/{artifact}</c>.
		/// </summary>
		public string? MetadataArtifact { get; set; }
		/// <summary>
		/// Immutable. Points to a YAML file stored on Google Cloud Storage describing additional information about the Model, that is specific to it. Unset if the Model does not have any additional information. The schema is defined as an OpenAPI 3.0.2 [Schema Object](https://github.com/OAI/OpenAPI-Specification/blob/main/versions/3.0.2.md#schemaObject). AutoML Models always have this field populated by Vertex AI, if no additional metadata is needed, this field is set to an empty string. Note: The URI given on output will be immutable and probably different, including the URI scheme, than the one given on input. The output URI will point to a location where the user only has a read access.
		/// </summary>
		public string? MetadataSchemaUri { get; set; }
		/// <summary>
		/// Output only. Source of a model. It can either be automl training pipeline, custom training pipeline, BigQuery ML, or saved and tuned from Genie or Model Garden.
		/// </summary>
		public ModelSourceInfo? ModelSourceInfo { get; set; }
		/// <summary>
		/// Output only. If this Model is a copy of another Model, this contains info about the original.
		/// </summary>
		public ModelOriginalModelInfo? OriginalModelInfo { get; set; }
		/// <summary>
		/// The schemata that describe formats of the Model&apos;s predictions and explanations as given and returned via PredictionService.Predict and PredictionService.Explain.
		/// </summary>
		public PredictSchemata? PredictSchemata { get; set; }
		/// <summary>
		/// Output only. Reserved for future use.
		/// </summary>
		public bool? SatisfiesPzi { get; set; }
		/// <summary>
		/// Output only. Reserved for future use.
		/// </summary>
		public bool? SatisfiesPzs { get; set; }
		/// <summary>
		/// Output only. When this Model is deployed, its prediction resources are described by the <c>prediction_resources</c> field of the Endpoint.deployed_models object. Because not all Models support all resource configuration types, the configuration types this Model supports are listed here. If no configuration types are listed, the Model cannot be deployed to an Endpoint and does not support online predictions (PredictionService.Predict or PredictionService.Explain). Such a Model can serve predictions by using a BatchPredictionJob, if it has at least one entry each in supported_input_storage_formats and supported_output_storage_formats.
		/// </summary>
		public List<SupportedDeploymentResourcesTypesType>? SupportedDeploymentResourcesTypes { get; set; }
		/// <summary>
		/// Output only. The formats in which this Model may be exported. If empty, this Model is not available for export.
		/// </summary>
		public List<ModelExportFormat>? SupportedExportFormats { get; set; }
		/// <summary>
		/// Output only. The formats this Model supports in BatchPredictionJob.input_config. If PredictSchemata.instance_schema_uri exists, the instances should be given as per that schema. The possible formats are: * <c>jsonl</c> The JSON Lines format, where each instance is a single line. Uses GcsSource. * <c>csv</c> The CSV format, where each instance is a single comma-separated line. The first line in the file is the header, containing comma-separated field names. Uses GcsSource. * <c>tf-record</c> The TFRecord format, where each instance is a single record in tfrecord syntax. Uses GcsSource. * <c>tf-record-gzip</c> Similar to <c>tf-record</c>, but the file is gzipped. Uses GcsSource. * <c>bigquery</c> Each instance is a single row in BigQuery. Uses BigQuerySource. * <c>file-list</c> Each line of the file is the location of an instance to process, uses <c>gcs_source</c> field of the InputConfig object. If this Model doesn&apos;t support any of these formats it means it cannot be used with a BatchPredictionJob. However, if it has supported_deployment_resources_types, it could serve online predictions by using PredictionService.Predict or PredictionService.Explain.
		/// </summary>
		public List<string>? SupportedInputStorageFormats { get; set; }
		/// <summary>
		/// Output only. The formats this Model supports in BatchPredictionJob.output_config. If both PredictSchemata.instance_schema_uri and PredictSchemata.prediction_schema_uri exist, the predictions are returned together with their instances. In other words, the prediction has the original instance data first, followed by the actual prediction content (as per the schema). The possible formats are: * <c>jsonl</c> The JSON Lines format, where each prediction is a single line. Uses GcsDestination. * <c>csv</c> The CSV format, where each prediction is a single comma-separated line. The first line in the file is the header, containing comma-separated field names. Uses GcsDestination. * <c>bigquery</c> Each prediction is a single row in a BigQuery table, uses BigQueryDestination . If this Model doesn&apos;t support any of these formats it means it cannot be used with a BatchPredictionJob. However, if it has supported_deployment_resources_types, it could serve online predictions by using PredictionService.Predict or PredictionService.Explain.
		/// </summary>
		public List<string>? SupportedOutputStorageFormats { get; set; }
		/// <summary>
		/// Output only. The resource name of the TrainingPipeline that uploaded this Model, if any.
		/// </summary>
		public string? TrainingPipeline { get; set; }
		/// <summary>
		/// Output only. Timestamp when this Model was most recently updated.
		/// </summary>
		public DateTime? UpdateTime { get; set; }
		/// <summary>
		/// User provided version aliases so that a model version can be referenced via alias (i.e. <c>projects/{project}/locations/{location}/models/{model_id}@{version_alias}</c> instead of auto-generated version id (i.e. <c>projects/{project}/locations/{location}/models/{model_id}@{version_id})</c>. The format is a-z{0,126}[a-z0-9] to distinguish from version_id. A default version alias will be created for the first version of the model, and there must be exactly one default version alias for a model.
		/// </summary>
		public List<string>? VersionAliases { get; set; }
		/// <summary>
		/// Output only. Timestamp when this version was created.
		/// </summary>
		public DateTime? VersionCreateTime { get; set; }
		/// <summary>
		/// The description of this version.
		/// </summary>
		public string? VersionDescription { get; set; }
		/// <summary>
		/// Output only. Timestamp when this version was most recently updated.
		/// </summary>
		public DateTime? VersionUpdateTime { get; set; }

		[JsonConverter(typeof(JsonStringEnumConverter<SupportedDeploymentResourcesTypesType>))]
		public enum SupportedDeploymentResourcesTypesType
		{
			/// <summary>
			/// Should not be used.
			/// </summary>
			DeploymentResourcesTypeUnspecified,
			/// <summary>
			/// Resources that are dedicated to the DeployedModel, and that need a higher degree of manual configuration.
			/// </summary>
			DedicatedResources,
			/// <summary>
			/// Resources that to large degree are decided by Vertex AI, and require only a modest additional configuration.
			/// </summary>
			AutomaticResources,
			/// <summary>
			/// Resources that can be shared by multiple DeployedModels. A pre-configured DeploymentResourcePool is required.
			/// </summary>
			SharedResources,
		}
    }
}