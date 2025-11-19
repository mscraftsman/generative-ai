using System.Text.Json.Serialization;

namespace Mscc.GenerativeAI
{
    /// <summary>
    /// The mask mode of a reference image
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter<MaskReferenceMode>))]
    public enum MaskReferenceMode
    {
        /// <summary>
        /// Default mask mode.
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