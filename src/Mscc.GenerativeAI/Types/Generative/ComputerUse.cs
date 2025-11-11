using System.Collections.Generic;

namespace Mscc.GenerativeAI
{
    /// <summary>
    /// Computer Use tool type.
    /// </summary>
    public class ComputerUse
    {
        /// <summary>
        /// Required. The environment being operated.
        /// </summary>
        public ComputerUseEnvironment? Environment { get; set; }
        /// <summary>
        /// Optional. By default, predefined functions are included in the final model call.
        /// </summary>
        /// <remarks>
        /// Some of them can be explicitly excluded from being automatically included.
        /// This can serve two purposes:
        /// 1. Using a more restricted / different action space.
        /// 2. Improving the definitions / instructions of predefined functions. 
        /// </remarks>
        public List<string> ExcludedPredefinedFunctions { get; set; }
    }

    /// <summary>
    /// Tool to support the model interacting directly with the computer.
    /// </summary>
    public class ToolComputerUse : ComputerUse
    {
    }
}