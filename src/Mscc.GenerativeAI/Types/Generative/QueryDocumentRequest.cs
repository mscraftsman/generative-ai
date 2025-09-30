#if NET472_OR_GREATER || NETSTANDARD2_0
using System.Collections.Generic;
#endif

namespace Mscc.GenerativeAI
{
    /// <summary>
    /// Request for querying a <see cref="Document"/>.
    /// </summary>
    public class QueryDocumentRequest
    {
        /// <summary>
        /// Required. Query string to perform semantic search.
        /// </summary>
        public string Query { get; set; }
        /// <summary>
        /// Optional. Filter for `Chunk` metadata. Each `MetadataFilter` object should correspond to a unique key. Multiple `MetadataFilter` objects are joined by logical "AND"s. Note: `Document`-level filtering is not supported for this request because a `Document` name is already specified. Example query: (year >= 2020 OR year < 2010) AND (genre = drama OR genre = action) `MetadataFilter` object list: metadata_filters = [ {key = "chunk.custom_metadata.year" conditions = [{int_value = 2020, operation = GREATER_EQUAL}, {int_value = 2010, operation = LESS}}, {key = "chunk.custom_metadata.genre" conditions = [{string_value = "drama", operation = EQUAL}, {string_value = "action", operation = EQUAL}}] Example query for a numeric range of values: (year > 2015 AND year <= 2020) `MetadataFilter` object list: metadata_filters = [ {key = "chunk.custom_metadata.year" conditions = [{int_value = 2015, operation = GREATER}]}, {key = "chunk.custom_metadata.year" conditions = [{int_value = 2020, operation = LESS_EQUAL}]}] Note: "AND"s for the same key are only supported for numeric values. String values only support "OR"s for the same key.
        /// </summary>
        public List<MetadataFilter>? MetadataFilters { get; set; }
        /// <summary>
        /// Optional. The maximum number of `Chunk`s to return. The service may return fewer `Chunk`s. If unspecified, at most 10 `Chunk`s will be returned. The maximum specified result count is 100.
        /// </summary>
        public int? ResultsCount { get; set; }
    }
}