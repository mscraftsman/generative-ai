namespace Mscc.GenerativeAI.Types
{
    /// <summary>
    /// A function that the model can generate calls for.
    /// </summary>
    public partial class ChatFunction
    {
        /// <summary>
        /// Required. The name of the function.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Optional. A description of the function.
        /// </summary>
        public string? Description { get; set; }
        /// <summary>
        /// Optional. Whether the schema validation is strict.
        /// If true, the model will fail if the schema is not valid.
        /// NOTE: This parameter is currently ignored.
        /// </summary>
        public bool Strict { get; set; } = false;
        /// <summary>
        /// Optional. The parameters of the function.
        /// </summary>
        public object? Parameters { get; set; }
    }
}