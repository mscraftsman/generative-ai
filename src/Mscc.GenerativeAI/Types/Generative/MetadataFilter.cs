#if NET472_OR_GREATER || NETSTANDARD2_0
using System.Collections.Generic;
#endif

namespace Mscc.GenerativeAI
{
    /// <summary>
    /// User provided filter to limit retrieval based on Chunk or Document level metadata values. Example (genre = drama OR genre = action): key = "document.custom_metadata.genre" conditions = [{stringValue = "drama", operation = EQUAL}, {stringValue = "action", operation = EQUAL}]
    /// </summary>
    public class MetadataFilter
    {
        /// <summary>
        /// Required. The key of the metadata to filter on.
        /// </summary>
        public string Key { get; set; }
        /// <summary>
        /// Required. The Conditions for the given key that will trigger this filter. Multiple Conditions are joined by logical ORs.
        /// </summary>
        public List<Condition> Conditions { get; set; }
    }

    /// <summary>
    /// Filter condition applicable to a single key.
    /// </summary>
    public class Condition
    {
        /// <summary>
        /// Required. Operator applied to the given key-value pair to trigger the condition.
        /// </summary>
        public Operator Operation { get; set; }
        /// <summary>
        /// The string value to filter the metadata on.
        /// </summary>
        public string? StringValue { get; set; }
        /// <summary>
        /// The numeric value to filter the metadata on.
        /// </summary>
        public float? NumericValue { get; set; }
    }
}