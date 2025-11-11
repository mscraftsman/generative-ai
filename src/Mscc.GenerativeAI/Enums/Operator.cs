using System.Text.Json.Serialization;

namespace Mscc.GenerativeAI
{
    /// <summary>
    /// Defines the valid operators that can be applied to a key-value pair.
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter<Operator>))]
    public enum Operator
    {
        /// <summary>
        /// The default value. This value is unused.
        /// </summary>
        OperatorUnspecified = 0,
        /// <summary>
        /// Supported by numeric.
        /// </summary>
        Less = 1,
        /// <summary>
        /// Supported by numeric.
        /// </summary>
        LessEqual = 2,
        /// <summary>
        /// Supported by numeric and string.
        /// </summary>
        Equal = 3,
        /// <summary>
        /// Supported by numeric.
        /// </summary>
        GreaterEqual = 4,
        /// <summary>
        /// Supported by numeric.
        /// </summary>
        Greater = 5,
        /// <summary>
        /// Supported by numeric and string.
        /// </summary>
        NotEqual = 6,
        /// <summary>
        /// Supported by string only when <see cref="CustomMetadata" /> value type for the given key has a stringListValue.
        /// </summary>
        Includes = 7,
        /// <summary>
        /// Supported by string only when <see cref="CustomMetadata" /> value type for the given key has a stringListValue.
        /// </summary>
        Excludes = 8
    }
}