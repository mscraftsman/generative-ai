using System.Collections.Generic;
using System.Diagnostics;

namespace Mscc.GenerativeAI
{
    /// <summary>
    /// User provided metadata stored as key-value pairs.
    /// </summary>
    [DebuggerDisplay("{Key})")]
    public class CustomMetadata
    {
        /// <summary>
        /// Required. The key of the metadata to store.
        /// </summary>
        public string Key { get; set; }
        /// <summary>
        /// The numeric value of the metadata to store.
        /// </summary>
        public float NumericValue { get; set; }
        /// <summary>
        /// The string value of the metadata to store.
        /// </summary>
        public string? StringValue { get; set; }
        /// <summary>
        /// The StringList value of the metadata to store.
        /// </summary>
        // public StringList StringListValue { get; set; }
        public List<string>? StringListValue { get; set; }
    }
}