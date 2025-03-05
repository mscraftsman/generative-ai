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
        ControlTypeDefault = 0,
        ControlTypeCanny,
        ControlTypeFaceMesh,
        ControlTypeScribble
    }
}