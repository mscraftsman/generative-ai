#if NET472_OR_GREATER || NETSTANDARD2_0
using System.Text.Json.Serialization;
#endif

namespace Mscc.GenerativeAI
{
    /// <summary>
    /// 
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter<ControlReferenceType>))]
    public enum ControlReferenceType
    {
        /// <summary>
        /// Default control type.
        /// </summary>
        ControlTypeDefault,
        /// <summary>
        /// for canny edge
        /// </summary>
        ControlTypeCanny,
        /// <summary>
        /// for face mesh (person customization)
        /// </summary>
        ControlTypeFaceMesh,
        /// <summary>
        /// for scribble
        /// </summary>
        ControlTypeScribble
    }
}