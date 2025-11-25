using System;
using System.Text.Json.Serialization;

namespace Mscc.GenerativeAI
{
	public sealed class RetryInfo
	{
		[JsonConverter(typeof(TimeSpanJsonConverter))]
		public TimeSpan RetryDelay { get; set; }
	}
}