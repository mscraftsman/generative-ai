#if NET472_OR_GREATER || NETSTANDARD2_0
using System.Text.Json.Serialization;
#endif

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
        /// Unset value, which will default to one of the other enum values.
        /// </summary>
        Unspecified = 0,
        /// <summary>
        /// Specifies the given text is a query in a search/retrieval setting.
        /// </summary>
        RetrievalQuery = 1,
        /// <summary>
        /// Specifies the given text is a document from the corpus being searched.
        /// </summary>
        RetrievalDocument = 2,
        /// <summary>
        /// Specifies the given text will be used for STS.
        /// </summary>
        SemanticSimilarity = 3,
        /// <summary>
        /// Specifies that the given text will be classified.
        /// </summary>
        Classification = 4,
        /// <summary>
        /// Specifies that the embeddings will be used for clustering.
        /// </summary>
        Clustering = 5
    }
}