using System.Text.Json.Serialization;

namespace Mscc.GenerativeAI
{
    /// <summary>
    /// Adapter size for tuning job.
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter<AdapterSize>))]
    public enum AdapterSize
    {
        /// <summary>
        /// Unspecified adapter size.
        /// </summary>
        AdapterSizeUnspecified,
        /// <summary>
        /// Adapter size 1.
        /// </summary>
        AdapterSizeOne,
        /// <summary>
        /// Adapter size 4.
        /// </summary>
        AdapterSizeFour,
        /// <summary>
        /// Adapter size 8.
        /// </summary>
        AdapterSizeEight,
        /// <summary>
        /// Adapter size 16.
        /// </summary>
        AdapterSizeSixteen,
        /// <summary>
        /// Adapter size 32.
        /// </summary>
        AdapterSizeThirtyTwo
    }
}