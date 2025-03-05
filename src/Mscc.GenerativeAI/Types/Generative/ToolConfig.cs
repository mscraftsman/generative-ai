namespace Mscc.GenerativeAI
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class ToolConfig
    {
        /// <summary>
        /// Optional. Function calling config.
        /// </summary>
        public FunctionCallingConfig FunctionCallingConfig { get; set; }
        public FunctionCallingConfig? FunctionCallingConfig { get; set; }
    }
}