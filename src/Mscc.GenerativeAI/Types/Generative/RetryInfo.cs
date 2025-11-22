using System;
using System.Text.Json.Serialization;

namespace Mscc.GenerativeAI
{
    public class RetryInfo
    {
        [JsonPropertyName("retryDelay")]
        [JsonConverter(typeof(TimeSpanConverter))]
        public TimeSpan RetryDelay { get; set; }
    }
}