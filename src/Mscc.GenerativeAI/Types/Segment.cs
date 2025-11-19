namespace Mscc.GenerativeAI
{
    /// <summary>
    /// A segment of the content.
    /// </summary>
    public class Segment
    {
        /// <summary>
        /// Output only. The text of the segment.
        /// </summary>
        public string? Text { get; set; }
        /// <summary>
        /// Output only. The start index of the segment in the `Part`, measured in bytes. This marks the
        /// beginning of the segment and is inclusive, meaning the byte at this index is the first byte
        /// of the segment.
        /// </summary>
        public int? StartIndex { get; set; }
        /// <summary>
        /// Output only. The index of the `Part` object that this segment belongs to. This is useful for
        /// associating the segment with a specific part of the content.
        /// </summary>
        public int? PartIndex { get; set; }
        /// <summary>
        /// Output only. The end index of the segment in the `Part`, measured in bytes. This marks the
        /// end of the segment and is exclusive, meaning the segment includes content up to, but not
        /// including, the byte at this index.
        /// </summary>
        public int? EndIndex { get; set; }
    }
}