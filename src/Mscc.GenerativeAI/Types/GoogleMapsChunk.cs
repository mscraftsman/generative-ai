namespace Mscc.GenerativeAI.Types
{
    /// <summary>
    /// A `Maps` chunk is a piece of evidence that comes from Google Maps. It contains information
    /// about a place, such as its name, address, and reviews. This is used to provide the user with
    /// rich, location-based information. This data type is not supported in Gemini API.
    /// </summary>
    public sealed class GoogleMapsChunk
    {
        /// <summary>
        /// The URI of the place.
        /// </summary>
        public string? Uri { get; set; }
        /// <summary>
        /// The title of the place.
        /// </summary>
        public string? Title { get; set; }
        /// <summary>
        /// The text of the place answer.
        /// </summary>
        public string? Text { get; set; }
        /// <summary>
        /// This Place's resource name, in `places/{place_id}` format. This can be used to look up the
        /// place in the Google Maps API.
        /// </summary>
        public string? PlaceId { get; set; }
        /// <summary>
        /// A unique identifier for review.
        /// </summary>
        public string? ReviewId { get; set; }
        /// <summary>
        /// The sources that were used to generate the place answer. This includes review snippets and
        /// photos that were used to generate the answer, as well as URIs to flag content.
        /// </summary>
        public PlaceAnswerSources? PlaceAnswerSources { get; set; }
    }
}