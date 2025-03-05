#if NET472_OR_GREATER || NETSTANDARD2_0
using System.Text.Json.Serialization;
#endif

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