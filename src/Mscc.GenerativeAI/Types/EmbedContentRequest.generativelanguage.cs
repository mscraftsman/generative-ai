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
	/// Request containing the <c>Content</c> for the model to embed.
	/// </summary>
	public partial class EmbedContentRequest
	{
		/// <summary>
		/// Required. The model&apos;s resource name. This serves as an ID for the Model to use. This name should match a model name returned by the <c>ListModels</c> method. Format: <c>models/{model}</c>
		/// </summary>
		public string? Model { get; set; }
		/// <summary>
		/// Optional. Optional reduced dimension for the output embedding. If set, excessive values in the output embedding are truncated from the end. Supported by newer models since 2024 only. You cannot set this value if using the earlier model (<c>models/embedding-001</c>).
		/// </summary>
		public int? OutputDimensionality { get; set; }
		/// <summary>
		/// Optional. Optional task type for which the embeddings will be used. Not supported on earlier models (<c>models/embedding-001</c>).
		/// </summary>
		public EmbedContentRequestTaskType? TaskType { get; set; }
		/// <summary>
		/// Optional. An optional title for the text. Only applicable when TaskType is <c>RETRIEVAL_DOCUMENT</c>. Note: Specifying a <c>title</c> for <c>RETRIEVAL_DOCUMENT</c> provides better quality embeddings for retrieval.
		/// </summary>
		public string? Title { get; set; }

		[JsonConverter(typeof(JsonStringEnumConverter<EmbedContentRequestTaskType>))]
		public enum EmbedContentRequestTaskType
		{
			/// <summary>
			/// Unset value, which will default to one of the other enum values.
			/// </summary>
			TaskTypeUnspecified,
			/// <summary>
			/// Specifies the given text is a query in a search/retrieval setting.
			/// </summary>
			RetrievalQuery,
			/// <summary>
			/// Specifies the given text is a document from the corpus being searched.
			/// </summary>
			RetrievalDocument,
			/// <summary>
			/// Specifies the given text will be used for STS.
			/// </summary>
			SemanticSimilarity,
			/// <summary>
			/// Specifies that the given text will be classified.
			/// </summary>
			Classification,
			/// <summary>
			/// Specifies that the embeddings will be used for clustering.
			/// </summary>
			Clustering,
			/// <summary>
			/// Specifies that the given text will be used for question answering.
			/// </summary>
			QuestionAnswering,
			/// <summary>
			/// Specifies that the given text will be used for fact verification.
			/// </summary>
			FactVerification,
			/// <summary>
			/// Specifies that the given text will be used for code retrieval.
			/// </summary>
			CodeRetrievalQuery,
		}
    }
}