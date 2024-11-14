namespace Mscc.GenerativeAI
{
    /// <summary>
    /// Schema for the response.
    /// </summary>
    public class ResponseFormatSchema
    {
        /// <summary>
        /// Required. Name of the object type represented by the schema.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Optional. Description of the object represented by the schema.
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// Optional. Whether the schema validation is strict.
        /// If true, the model will fail if the schema is not valid.
        /// NOTE: This parameter is currently ignored.
        /// </summary>
        public bool Strict { get; set; }
        /// <summary>
        /// Optional. The JSON schema to follow.
        /// </summary>
        public object? Schema { get; set; } = false;
    }
}