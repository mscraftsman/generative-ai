using System;

namespace Mscc.GenerativeAI
{
	/// <summary>
	/// The generic reusable api auth config. 
	/// </summary>
	[Obsolete("Deprecated. Please use AuthConfig (google/cloud/aiplatform/master/auth.proto) instead.")]
	public class ApiAuth
	{
		/// <summary>
		/// The API secret.
		/// </summary>
		public ApiKeyConfig? ApiKeyConfig { get; set; }
	}
}