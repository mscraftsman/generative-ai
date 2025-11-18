namespace Mscc.GenerativeAI
{
    /// <summary>
    /// Partial argument value of the function call.
    /// </summary>
    /// <remarks>
    /// This field is not supported in Gemini API.
    /// </remarks>
    public class PartialArg
    {
        /// <summary>
        /// Optional. Represents a null value.
        /// </summary>
        public string? NullValue { get; set; }
        /// <summary>
        /// Optional. Represents a double value.
        /// </summary>
        public double? NumberValue { get; set; }
        /// <summary>
        /// Optional. Represents a string value.
        /// </summary>
        public string? StringValue { get; set; }
        /// <summary>
        /// Optional. Represents a boolean value.
        /// </summary>
        public bool? BoolValue { get; set; }
        /// <summary>
        /// A JSON Path (RFC 9535) to the argument being streamed.
        /// https://datatracker.ietf.org/doc/html/rfc9535. e.g. "$.foo.bar[0].data".
        /// </summary>
        public string? JsonPath {get; set;}
        /// <summary>
        /// Optional. Whether this is not the last part of the same json_path. If true, another
        /// <see cref="PartialArg"/> message for the current json_path is expected to follow.
        /// </summary>
        public bool? WillContinue { get; set; }
    }
}