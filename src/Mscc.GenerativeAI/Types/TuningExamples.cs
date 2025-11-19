using System.Collections.Generic;
using System.Diagnostics;

namespace Mscc.GenerativeAI
{
    /// <summary>
    /// A set of tuning examples. Can be training or validation data.
    /// </summary>
    public class TuningExamples
    {
        /// <summary>
        /// Required. The examples. Example input can be for text or discuss, but all examples in a set must be of the same type.
        /// </summary>
        public List<TuningExample> Examples { get; set; }
        /// <summary>
        /// Content examples. For multiturn conversations.
        /// </summary>
        public List<TuningMultiturnExample>? MultiturnExamples { get; set; }
    }
}