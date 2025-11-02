#if NET472_OR_GREATER || NETSTANDARD2_0
using System;
using System.Text.Json.Serialization;
#endif

namespace Mscc.GenerativeAI
{
    /// <summary>
    /// Controls whether the model can generate people.
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter<PersonGeneration>))]
    public enum PersonGeneration
    {
        /// <summary>
        /// Generation images of people unspecified.
        /// </summary>
        PersonGenerationUnspecified = 0,
        [Obsolete("Use value AllowNone instead.")]
        DontAllow,
        /// <summary>
        /// Generate images of adults, but not children.
        /// </summary>
        AllowAdult,
        /// <summary>
        /// Generate images that include adults and children.
        /// </summary>
        AllowAll,
        /// <summary>
        /// Block generation of images of people.
        /// </summary>
        AllowNone
    }
}