using System.Text.Json.Serialization;

namespace Mscc.GenerativeAI.Types
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