namespace Mscc.GenerativeAI.Types
{
	/// <summary>
	/// A grounding chunk from Google Maps. A Maps chunk corresponds to a single place.
	/// </summary>
	public partial class Maps
	{
		/// <summary>
		/// Sources that provide answers about the features of a given place in Google Maps.
		/// </summary>
		public PlaceAnswerSources? PlaceAnswerSources { get; set; }
		/// <summary>
		/// This ID of the place, in <see cref="places/{place_id}"/> format. A user can use this ID to look up that place.
		/// </summary>
		public string? PlaceId { get; set; }
		/// <summary>
		/// Text description of the place answer.
		/// </summary>
		public string? Text { get; set; }
		/// <summary>
		/// Title of the place.
		/// </summary>
		public string? Title { get; set; }
		/// <summary>
		/// URI reference of the place.
		/// </summary>
		public string? Uri { get; set; }
    }
}
