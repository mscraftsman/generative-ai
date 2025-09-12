#if NET472_OR_GREATER || NETSTANDARD2_0
using System.Text.Json.Serialization;
#endif

namespace Mscc.GenerativeAI
{
    /// <summary>
    /// 
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter<MaskReferenceMode>))]
    public enum MaskReferenceMode
    {
        /// <summary>
        /// 
        /// </summary>
        MaskModeDefault,
        /// <summary>
        /// automatically generate a mask using background segmentation.
        /// </summary>
        MaskModeBackground,
        /// <summary>
        /// automatically generate a mask using foreground segmentation.
        /// </summary>
        MaskModeForeground,
        /// <summary>
        /// automatically generate a mask using semantic segmentation, and the given mask class.
        /// </summary>
        MaskModeSemantic,
        /// <summary>
        /// the reference image is a mask image.
        /// </summary>
        MaskModeUserProvided
    }
}