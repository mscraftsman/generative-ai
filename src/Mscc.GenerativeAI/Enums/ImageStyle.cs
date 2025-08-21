#if NET472_OR_GREATER || NETSTANDARD2_0
using System.Text.Json.Serialization;
#endif

namespace Mscc.GenerativeAI
{
    /// <summary>
    /// The style of the generated images. 
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter<ImageStyle>))]
    public enum ImageStyle
    {
        /// <summary>
        /// Vivid causes the model to lean towards generating hyper-real and dramatic images.
        /// </summary>
        Vivid,
        /// <summary>
        /// Natural causes the model to produce more natural, less hyper-real looking images.
        /// </summary>
        Natural
    }
}