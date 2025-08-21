#if NET472_OR_GREATER || NETSTANDARD2_0
using System.Text.Json.Serialization;
#endif

namespace Mscc.GenerativeAI
{
    /// <summary>
    /// A list of reasons why content may have been blocked.
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter<EditMode>))]
    public enum EditMode
    {
        /// <summary>
        /// Default editing mode.
        /// </summary>
        EditModeDefault = 0,
        /// <summary>
        /// Background swap editing mode.
        /// </summary>
        EditModeBgswap,
        /// <summary>
        /// Controlled editing mode.
        /// </summary>
        EditModeControlledEditing,
        /// <summary>
        /// Inpainting insertion editing mode.
        /// </summary>
        EditModeInpaintInsertion,
        /// <summary>
        /// Inpainting removal editing mode.
        /// </summary>
        EditModeInpaintRemoval,
        /// <summary>
        /// Outpainting editing mode.
        /// </summary>
        EditModeOutpaint,
        /// <summary>
        /// Product image editing mode.
        /// </summary>
        EditModeProductImage,
        /// <summary>
        /// Style editing mode.
        /// </summary>
        EditModeStyle
    }
}