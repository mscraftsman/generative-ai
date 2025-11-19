using System.Collections.Generic;

namespace Mscc.GenerativeAI
{
    /// <summary>
    /// A collection of source attributions for a piece of content.
    /// Ref: https://ai.google.dev/api/rest/v1beta/CitationMetadata
    /// </summary>
    public class CitationMetadata
    {
        /// <summary>
        /// Output only. List of citations.
        /// </summary>
        public List<CitationSource>? Citations { get; internal set; }   // citationSources
    }
}