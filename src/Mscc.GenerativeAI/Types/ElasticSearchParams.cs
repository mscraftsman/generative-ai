namespace Mscc.GenerativeAI
{
	/// <summary>
	/// The search parameters to use for the ELASTIC_SEARCH spec.
	/// </summary>
	public class ElasticSearchParams
	{
		/// <summary>
		/// The ElasticSearch index to use.
		/// </summary>
		public string? Index { get; set; }
		/// <summary>
		/// Optional. Number of hits (chunks) to request.
		/// When specified, it is passed to Elasticsearch as the `num_hits` param.
		/// </summary>
		public int? NumHits { get; set; }
		/// <summary>
		/// The ElasticSearch search template to use.
		/// </summary>
		public string? SearchTemplate { get; set; }
	}
}