using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Mscc.GenerativeAI
{
    public class ErrorResponse
    {
        [JsonPropertyName("error")]
        public Error? Error { get; set; }
    }

    public class Error
    {
        [JsonPropertyName("code")]
        public int Code { get; set; }
        [JsonPropertyName("message")]
        public string? Message { get; set; }
        [JsonPropertyName("status")]
        public string? Status { get; set; }
        [JsonPropertyName("details")]
        public List<Detail>? Details { get; set; }
    }

    public class Detail
    {
        [JsonPropertyName("@type")]
        public string? Type { get; set; }
        [JsonExtensionData]
        public Dictionary<string, object>? Metadata { get; set; }
    }
}