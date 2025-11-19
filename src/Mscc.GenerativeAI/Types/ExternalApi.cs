using System;

namespace Mscc.GenerativeAI
{
	/// <summary>
	/// Retrieve from data source powered by external API for grounding.
	/// The external API is not owned by Google, but need to follow the pre-defined API spec.
	/// </summary>
	public class ExternalApi
	{
		/// <summary>
		/// The authentication config to access the API.
		/// </summary>
		[Obsolete("Deprecated. Please use auth_config instead.")]
		public ApiAuth? ApiAuth { get; set; }
		/// <summary>
		/// The authentication config to access the API.
		/// </summary>
		public AuthConfig? AuthConfig { get; set; }
		/// <summary>
		/// The API spec that the external API implements.
		/// </summary>
		public ApiSpec? ApiSpec { get; set; }
		/// <summary>
		/// Parameters for the elastic search API.
		/// </summary>
		public ElasticSearchParams? ElasticSearchParams { get; set; }
		/// <summary>
		/// Parameters for the simple search API.
		/// </summary>
		public SimpleSearchParams? SimpleSearchParams { get; set; }
		/// <summary>
		/// The endpoint of the external API.
		/// The system will call the API at this endpoint to retrieve the data for grounding. Example: https://acme.com:443/search
		/// </summary>
		public string? Endpoint { get; set; }
	}
}