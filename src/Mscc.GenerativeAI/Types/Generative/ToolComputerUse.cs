namespace Mscc.GenerativeAI
{
    /// <summary>
    /// Tool to support the model interacting directly with the computer.
    /// </summary>
    public class ToolComputerUse
    {
        /// <summary>
        /// Required. The environment being operated.
        /// </summary>
        public ComputerUseEnvironment? Environment { get; set; }
    }
}