using System.Text.Json.Serialization;

namespace Mscc.GenerativeAI
{
    /// <summary>
    /// Enum representing the subject type of a subject reference image.
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter<SubjectReferenceType>))]
    public enum SubjectReferenceType
    {
        /// <summary>
        /// Default subject type.
        /// </summary>
        SubjectTypeDefault,
        /// <summary>
        /// Person subject type.
        /// </summary>
        SubjectTypePerson,
        /// <summary>
        /// Animal subject type.
        /// </summary>
        SubjectTypeAnimal,
        /// <summary>
        /// Product subject type.
        /// </summary>
        SubjectTypeProduct
    }
}