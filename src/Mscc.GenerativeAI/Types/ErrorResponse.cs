using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Mscc.GenerativeAI
{
	public sealed class ErrorResponse
	{
		public ErrorResponseError? Error { get; set; }

		public sealed class ErrorResponseError
		{
			private const string TypeGoogleapisComGoogleRpc = "type.googleapis.com/google.rpc.";
			private readonly JsonSerializerOptions _options = new() { PropertyNameCaseInsensitive = true };

			public int Code { get; set; }
			public string? Message { get; set; }
			public string? Status { get; set; }	// enum Status { RESOURCE_EXHAUSTED }
			public List<ErrorResponseDetail>? Details { get; set; }

			public bool TryGetDetail<T>(out T? value) where T : class
			{
				value = null;
				try
				{
					var detail = Details!
						.FirstOrDefault(d =>
							d.Type != null &&
							d.Type.Equals($"{TypeGoogleapisComGoogleRpc}{typeof(T).Name}",
								StringComparison.OrdinalIgnoreCase));
					if (detail != null)
					{
						var json = JsonSerializer.Serialize(detail.Metadata);
						value = JsonSerializer.Deserialize<T>(json, _options);
					}

					return value != null;
				}
				catch (Exception _)
				{
					return false;
				}
			}
		}

		public sealed class ErrorResponseDetail
		{
			[JsonPropertyName("@type")] 
			public string? Type { get; set; }
			[JsonExtensionData] public Dictionary<string, object>? Metadata { get; set; }
		}
	}
}