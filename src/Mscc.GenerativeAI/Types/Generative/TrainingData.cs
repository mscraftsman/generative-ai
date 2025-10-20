#if NET472_OR_GREATER || NETSTANDARD2_0
using System.Collections.Generic;
#endif
using System.Diagnostics;

namespace Mscc.GenerativeAI
{
    /// <summary>
    /// Dataset for training or validation.
    /// </summary>
    // public class Dataset 
    public class TrainingData
    {
        /// <summary>
        /// Optional. Inline examples.
        /// </summary>
        public TuningExamples? Examples { get; set; }
    }
}