using System.Collections.Generic;

namespace Mscc.GenerativeAI
{
    /// <summary>
    /// Request for querying a `Corpus`.
    /// </summary>
    public class CorpusQueryRequest
    {
        /// <summary>
        /// Required. Query string to perform semantic search.
        /// </summary>
        public string Query { get; set; }
        /// <summary>
        /// Optional. Filter for `Chunk` and `Document` metadata.
        /// Each `MetadataFilter` object should correspond to a unique key. Multiple `MetadataFilter` objects are joined by logical \"AND\"s. Example query at document level: (year &gt;= 2020 OR year &lt; 2010) AND (genre = drama OR genre = action) `MetadataFilter` object list: metadata_filters = [ {key = \"document.custom_metadata.year\" conditions = [{int_value = 2020, operation = GREATER_EQUAL}, {int_value = 2010, operation = LESS}]}, {key = \"document.custom_metadata.year\" conditions = [{int_value = 2020, operation = GREATER_EQUAL}, {int_value = 2010, operation = LESS}]}, {key = \"document.custom_metadata.genre\" conditions = [{string_value = \"drama\", operation = EQUAL}, {string_value = \"action\", operation = EQUAL}]}] Example query at chunk level for a numeric range of values: (year &gt; 2015 AND year &lt;= 2020) `MetadataFilter` object list: metadata_filters = [ {key = \"chunk.custom_metadata.year\" conditions = [{int_value = 2015, operation = GREATER}]}, {key = \"chunk.custom_metadata.year\" conditions = [{int_value = 2020, operation = LESS_EQUAL}]}] Note: \"AND\"s for the same key are only supported for numeric values. String values only support \"OR\"s for the same key.
        /// </summary>
        public List<MetadataFilter>? MetadataFilters { get; set; }
        /// <summary>
        /// Optional. The maximum number of `Chunk`s to return.
        /// The service may return fewer `Chunk`s. If unspecified, at most 10 `Chunk`s will be returned. The maximum specified result count is 100.
        /// </summary>
        public int? ResultsCount { get; set; }
    }
}