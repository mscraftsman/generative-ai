using System.Text.Json.Serialization;

namespace Mscc.GenerativeAI
{
    /// <summary>
    /// Type of task for which the embedding will be used.
    /// Ref: https://ai.google.dev/api/rest/v1beta/TaskType
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter<ParameterType>))]
    public enum TaskType
    {
        /// <summary>
        /// Unset value, which will default to one of the other enum values. This enum value is not
        /// supported in Vertex AI.
        /// </summary>
        TaskTypeUnspecified = 0,
        /// <summary>
        /// Unset value, which will default to one of the other enum values. This enum value is not
        /// supported in Gemini API.
        /// </summary>
        Unspecified,
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