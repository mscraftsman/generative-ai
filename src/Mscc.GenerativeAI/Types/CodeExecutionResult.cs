namespace Mscc.GenerativeAI
{
    /// <summary>
    /// Result of executing the `ExecutableCode`. Only generated when using the `CodeExecution`, and always follows a `part` containing the `ExecutableCode`.
    /// </summary>
    public class CodeExecutionResult : IPart
    {
        /// <summary>
        /// Required. Outcome of the code execution.
        /// </summary>
        public Outcome Outcome { get; set; }
        /// <summary>
        /// Optional. Contains stdout when code execution is successful, stderr or other description otherwise.
        /// </summary>
        public string? Output { get; set; }
    }
}