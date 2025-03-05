#if NET472_OR_GREATER || NETSTANDARD2_0
using System.Text.Json.Serialization;
#endif

namespace Mscc.GenerativeAI
{
    /// <summary>
    /// Enum representing the subject type of a subject reference image.
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter<SubjectReferenceType>))]
    public enum SubjectReferenceType
    {
        SubjectTypeDefault,
        SubjectTypePerson,
        SubjectTypeAnimal,
        SubjectTypeProduct
    }
}