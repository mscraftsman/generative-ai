using System.Text.Json.Serialization;

namespace Mscc.GenerativeAI.Types
{
	[JsonConverter(typeof(JsonStringEnumConverter<Language>))]
    public enum Language
    {
        /// <summary>
        /// Unspecified language. This value should not be used.
        /// </summary>
        LanguageUnspecified,
        /// <summary>
        /// Python >= 3.10, with numpy and simpy available. Python is the default language.
        /// </summary>
        Python,
    }
}