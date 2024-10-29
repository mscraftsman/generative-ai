#if NET472_OR_GREATER || NETSTANDARD2_0
using System.Text.Json.Serialization;
#endif

namespace Mscc.GenerativeAI
{
    /// <summary>
    /// 
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter<Language>))]
    public enum Language
    {
        /// <summary>
        /// Unspecified language. This value should not be used.
        /// </summary>
        LanguageUnspecified,
        /// <summary>
        /// Python >= 3.10, with numpy and simpy available.
        /// </summary>
        Python
    }
}