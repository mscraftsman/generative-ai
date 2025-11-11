using System.Text.Json.Serialization;

namespace Mscc.GenerativeAI
{
    /// <summary>
    /// Defines the programming language for executable code.
    /// </summary>
    /// <remarks>Python is the default language.</remarks>
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
        Python,
        /// <summary>
        /// Bash. Only available for Gemini 3.0 model or above.
        /// </summary>
        Bash
    }
}