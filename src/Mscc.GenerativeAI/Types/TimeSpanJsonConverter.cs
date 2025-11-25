using System;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace Mscc.GenerativeAI
{
	public class TimeSpanJsonConverter : JsonConverter<TimeSpan>
	{
		public override TimeSpan Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
		{
			var value = reader.GetString();
			var seconds = int.Parse(Regex.Match(value, @"\d+").Value, CultureInfo.InvariantCulture);
			return TimeSpan.FromSeconds(seconds);
		}

		public override void Write(Utf8JsonWriter writer, TimeSpan value, JsonSerializerOptions options)
		{
			writer.WriteStringValue($"{value.TotalSeconds}s");
		}
	}
}