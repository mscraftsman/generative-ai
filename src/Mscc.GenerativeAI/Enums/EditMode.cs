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
        EditModeDefault = 0,
        EditModeBgswap,
        EditModeControlledEditing,
        EditModeInpaintInsertion,
        EditModeInpaintRemoval,
        EditModeOutpaint,
        EditModeProductImage,
        EditModeStyle
    }
}