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
using System.Collections.Generic;
using System.Text.Json.Serialization;

// *** AUTO-GENERATED FILE - DO NOT EDIT MANUALLY *** //

namespace Mscc.GenerativeAI.Types
{
	/// <summary>
	/// Represents export format supported by the Model. All formats export to Google Cloud Storage.
	/// </summary>
	public partial class ModelExportFormat
	{
		/// <summary>
		/// Output only. The content of this Model that may be exported.
		/// </summary>
		public List<ExportableContentsType>? ExportableContents { get; set; }
		/// <summary>
		/// Output only. The ID of the export format. The possible format IDs are: * <c>tflite</c> Used for Android mobile devices. * <c>edgetpu-tflite</c> Used for [Edge TPU](https://cloud.google.com/edge-tpu/) devices. * <c>tf-saved-model</c> A tensorflow model in SavedModel format. * <c>tf-js</c> A [TensorFlow.js](https://www.tensorflow.org/js) model that can be used in the browser and in Node.js using JavaScript. * <c>core-ml</c> Used for iOS mobile devices. * <c>custom-trained</c> A Model that was uploaded or trained by custom code. * <c>genie</c> A tuned Model Garden model.
		/// </summary>
		public string? Id { get; set; }

		[JsonConverter(typeof(JsonStringEnumConverter<ExportableContentsType>))]
		public enum ExportableContentsType
		{
			/// <summary>
			/// Should not be used.
			/// </summary>
			ExportableContentUnspecified,
			/// <summary>
			/// Model artifact and any of its supported files. Will be exported to the location specified by the `artifactDestination` field of the ExportModelRequest.output_config object.
			/// </summary>
			Artifact,
			/// <summary>
			/// The container image that is to be used when deploying this Model. Will be exported to the location specified by the `imageDestination` field of the ExportModelRequest.output_config object.
			/// </summary>
			Image,
		}
    }
}