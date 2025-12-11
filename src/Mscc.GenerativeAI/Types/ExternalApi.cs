using System;

namespace Mscc.GenerativeAI.Types
{
	/// <summary>
	/// Retrieve from data source powered by external API for grounding.
	/// The external API is not owned by Google, but need to follow the pre-defined API spec.
	/// </summary>
	public partial class ExternalApi
	{
		/// <summary>
		/// The authentication config to access the API.
		/// </summary>
		[Obsolete("Deprecated. Please use auth_config instead.")]
		public ApiAuth? ApiAuth { get; set; }
	}
}