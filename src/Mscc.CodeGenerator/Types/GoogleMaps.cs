namespace Mscc.GenerativeAI.Types
{
	/// <summary>
	/// The GoogleMaps Tool that provides geospatial context for the user's query.
	/// </summary>
	public partial class GoogleMaps
	{
		/// <summary>
		/// Optional. Whether to return a widget context token in the GroundingMetadata of the response. Developers can use the widget context token to render a Google Maps widget with geospatial context related to the places that the model references in the response.
		/// </summary>
		public bool? EnableWidget { get; set; }
    }
}
