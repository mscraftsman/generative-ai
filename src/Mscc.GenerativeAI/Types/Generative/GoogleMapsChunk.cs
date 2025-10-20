namespace Mscc.GenerativeAI
{
    /// <summary>
    /// A grounding chunk from Google Maps. A Maps chunk corresponds to a single place.
    /// </summary>
    public sealed class GoogleMapsChunk
    {
        /// <summary>
        /// A URI linking to the source.
        /// </summary>
        public string? Uri { get; set; }
        /// <summary>
        /// The title of the source.
        /// </summary>
        public string? Title { get; set; }
        /// <summary>
        /// The description of the source.
        /// </summary>
        public string? Text { get; set; }
        /// <summary>
        /// A unique identifier for the place.
        /// </summary>
        public string? PlaceId { get; set; }
        /// <summary>
        /// A unique identifier for review.
        /// </summary>
        public string? ReviewId { get; set; }
        /// <summary>
        /// Sources that provide answers about the features of a given place in Google Maps.
        /// </summary>
        public PlaceAnswerSources? PlaceAnswerSources { get; set; }
    }
}