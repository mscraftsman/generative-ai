using System.Text.Json.Serialization;

namespace Mscc.GenerativeAI.Types
{
	[JsonConverter(typeof(JsonStringEnumConverter<Type>))]
    public enum Type
    {
        /// <summary>
        /// Not specified, should not be used.
        /// </summary>
        TypeUnspecified,
        /// <summary>
        /// String type.
        /// </summary>
        String,
        /// <summary>
        /// Number type.
        /// </summary>
        Number,
        /// <summary>
        /// Integer type.
        /// </summary>
        Integer,
        /// <summary>
        /// Boolean type.
        /// </summary>
        Boolean,
        /// <summary>
        /// Array type.
        /// </summary>
        Array,
        /// <summary>
        /// Object type.
        /// </summary>
        Object,
        /// <summary>
        /// Null type.
        /// </summary>
        Null,
    }
}