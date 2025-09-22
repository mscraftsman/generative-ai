#if NET472_OR_GREATER || NETSTANDARD2_0
using System;
using System.Collections.Generic;
#endif
using System.Diagnostics;

namespace Mscc.GenerativeAI
{
    /// <summary>
    /// Response from `ListCorpora` containing a paginated list of `Corpora`.
    /// </summary>
    /// <remarks>
    /// The results are sorted by ascending `corpus.create_time`.
    /// </remarks>
    internal class ListCorporaResponse
    {
        /// <summary>
        /// The returned corpora.
        /// </summary>
        public List<Corpus> Corpora { get; set; }
        /// <summary>
        /// A token, which can be sent as `page_token` to retrieve the next page.
        /// If this field is omitted, there are no more pages.
        /// </summary>
        public string? NextPageToken { get; set; }
    }
    
    /// <summary>
    /// A `Corpus` is a collection of `Document`s. A project can create up to 10 corpora.
    /// </summary>
    [DebuggerDisplay("{DisplayName} ({Name})")]
    public class Corpus
    {
        /// <summary>
        /// Immutable. Identifier. The `Corpus` resource name. The ID (name excluding the \"corpora/\" prefix) can contain up to 40 characters that are lowercase alphanumeric or dashes (-). The ID cannot start or end with a dash. If the name is empty on create, a unique name will be derived from `display_name` along with a 12 character random suffix. Example: `corpora/my-awesome-corpora-123a456b789c`
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Optional. The human-readable display name for the `Corpus`. The display name must be no more than 512 characters in length, including spaces. Example: \"Docs on Semantic Retriever\"
        /// </summary>
        public string? DisplayName { get; set; }
        /// <summary>
        /// Output only. The Timestamp of when the `Corpus` was created.
        /// </summary>
        public DateTime? CreateTime { get; set; }
        /// <summary>
        /// Output only. The Timestamp of when the `Corpus` was last updated.
        /// </summary>
        public DateTime? UpdateTime { get; set; }
    }
}