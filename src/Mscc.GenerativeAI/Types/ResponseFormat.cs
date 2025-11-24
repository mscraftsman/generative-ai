namespace Mscc.GenerativeAI.Types
{
    /// <summary>
    /// Defines the format of the response.
    /// </summary>
    public partial class ResponseFormat
    {
        /// <summary>
        /// Required. Type of the response.
        /// Can be either:
        /// - \"text\": Format the response as text.
        /// - \"json_object\": Format the response as a JSON object.
        /// - \"json_schema\": Format the response as a JSON object following the given schema.
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// Optional. The JSON schema to follow. Only used if type is \"json_schema\".
        /// </summary>
        public ResponseFormatSchema? JsonSchema { get; set; }
    }
}