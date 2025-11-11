using System.Text.Json.Serialization;

namespace Mscc.GenerativeAI
{
    /// <summary>
    /// A list of languages (lower case?!)
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter<ImagePromptLanguage>))]
    public enum ImagePromptLanguage
    {
        Auto,
        En,
        Hi,
        Ja,
        Ko
    }
}