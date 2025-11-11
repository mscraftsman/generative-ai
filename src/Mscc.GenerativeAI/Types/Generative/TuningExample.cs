using System.Collections.Generic;
using System.Diagnostics;

namespace Mscc.GenerativeAI
{
    /// <summary>
    /// A single example for tuning.
    /// </summary>
    [DebuggerDisplay("Input: {TextInput,nq} - Output: {Output,nq}")]
    public class TuningExample
    {
        /// <summary>
        /// Optional. Text model input.
        /// </summary>
        public string? TextInput { get; set; }
        /// <summary>
        /// Required. The expected model output.
        /// </summary>
        public string Output { get; set; }
    }
}