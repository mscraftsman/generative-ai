namespace Mscc.GenerativeAI.Types
{
    /// <summary>
    /// The configuration for automatic function calling.
    /// </summary>
    public class AutomaticFunctionCallingConfig
    {
        /// <summary>
        /// Whether to disable automatic function calling.
        /// If not set or set to False, will enable automatic function calling.
        /// If set to True, will disable automatic function calling.
        /// </summary>
        public bool? Disable { get; set; }
        /// <summary>
        /// If automatic function calling is enabled, whether to ignore call history to the response.
        /// If not set, SDK will set ignore_call_history to false, and will append the call history to
        /// GenerateContentResponse.automatic_function_calling_history.
        /// </summary>
        public bool? IgnoreCallHistory { get; set; }
        /// <summary>
        /// If automatic function calling is enabled, maximum number of remote calls for automatic function calling.
        /// This number should be a positive integer. If not set, SDK will set maximum number of remote calls to 10.
        /// </summary>
        public int? MaximumRemoteCalls { get; set; }
    }
}