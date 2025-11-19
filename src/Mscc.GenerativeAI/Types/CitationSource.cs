using System;

namespace Mscc.GenerativeAI
{
    /// <summary>
    /// A citation to a source for a portion of a specific response.
    /// </summary>
    public class CitationSource
    {
        /// <summary>
        /// Output only. The start index of the citation in the content.
        /// </summary>
        public int? StartIndex { get; internal set; }
        /// <summary>
        /// Output only. The end index of the citation in the content.
        /// </summary>
        public int? EndIndex { get; internal set; }
        /// <summary>
        /// Output only. The URI of the source of the citation.
        /// </summary>
        public string? Uri { get; internal set; }
        /// <summary>
        /// Output only. The title of the source of the citation.
        /// </summary>
        public string? Title { get; internal set; }
        /// <summary>
        /// Output only. The license of the source of the citation.
        /// </summary>
        public string? License { get; internal set; }
        /// <summary>
        /// Output only. The publication date of the source of the citation.
        /// </summary>
        public DateTimeOffset? PublicationDate { get; internal set; }
    }
}