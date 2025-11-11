using System;

namespace Mscc.GenerativeAI
{
    /// <summary>
    /// A citation to a source for a portion of a specific response.
    /// </summary>
    public class CitationSource
    {
        /// <summary>
        /// Output only. Start index into the content.
        /// </summary>
        public int? StartIndex { get; internal set; }
        /// <summary>
        /// Output only. End index into the content.
        /// </summary>
        public int? EndIndex { get; internal set; }
        /// <summary>
        /// Output only. Url reference of the attribution.
        /// </summary>
        public string? Uri { get; internal set; }
        /// <summary>
        /// Output only. Title of the attribution.
        /// </summary>
        public string? Title { get; internal set; }
        /// <summary>
        /// Output only. License of the attribution.
        /// </summary>
        public string? License { get; internal set; }
        /// <summary>
        /// Output only. Publication date of the attribution.
        /// </summary>
        public DateTimeOffset? PublicationDate { get; internal set; }
    }
}