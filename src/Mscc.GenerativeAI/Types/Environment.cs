using System.Text.Json.Serialization;

namespace Mscc.GenerativeAI.Types
{
	[JsonConverter(typeof(JsonStringEnumConverter<Environment>))]
    public enum Environment
    {
        /// <summary>
        /// Defaults to browser.
        /// </summary>
        EnvironmentUnspecified,
        /// <summary>
        /// Operates in a web browser.
        /// </summary>
        EnvironmentBrowser,
    }
}