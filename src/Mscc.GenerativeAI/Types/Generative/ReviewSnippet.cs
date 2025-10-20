namespace Mscc.GenerativeAI
{
    /// <summary>
    /// Encapsulates a snippet of a user review that answers a question about the features of a specific place in Google Maps.
    /// </summary>
    public class ReviewSnippet
    {
        /// <summary>
        /// Title of the review.
        /// </summary>
        public string? Title { get; set; }
        /// <summary>
        /// A link that corresponds to the user review on Google Maps.
        /// </summary>
        public string? Uri { get; set; }
        /// <summary>
        /// The ID of the review snippet.
        /// </summary>
        public string? Id { get; set; }
    }
}