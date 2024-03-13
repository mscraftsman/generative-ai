#if NET472_OR_GREATER || NETSTANDARD2_0
using System.Text.Json.Serialization;
#endif

namespace Mscc.GenerativeAI
{
    [JsonConverter(typeof(JsonStringEnumConverter<ParameterType>))]
    public enum TaskType
    {
        /// <summary>
        /// 
        /// </summary>
        RetrievalQuery = 0,
        /// <summary>
        /// 
        /// </summary>
        RetrievalDocument = 1,
        /// <summary>
        /// 
        /// </summary>
        SemanticSimilarity = 2,
        /// <summary>
        /// 
        /// </summary>
        Classification = 3,
        /// <summary>
        /// 
        /// </summary>
        Clustering = 4
    }
}