#if NET472_OR_GREATER || NETSTANDARD2_0
using System.Text.Json.Serialization;
#endif

namespace Mscc.GenerativeAI
{
    /// <summary>
    /// 
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter<ImageReferenceType>))]
    public enum ImageReferenceType
    {
        /// <summary>
        /// 
        /// </summary>
        ReferenceTypeRaw,
        /// <summary>
        /// 
        /// </summary>
        ReferenceTypeMask,
        /// <summary>
        /// 
        /// </summary>
        ReferenceTypeControl,
        /// <summary>
        /// 
        /// </summary>
        ReferenceTypeSubject,
        /// <summary>
        /// 
        /// </summary>
        ReferenceTypeStyle
    }
}