namespace Mscc.GenerativeAI
{
    /// <summary>
    /// A tool that the model can generate calls for.
    /// </summary>
    public class ChatTool
    {
        /// <summary>
        /// Required. The name of the tool.
        /// </summary>
        public ChatFunction Function { get; set; }
        /// <summary>
        /// Required. Required, must be \"function\".
        /// </summary>
        public string Type { get; set; } = "function";
    }
}