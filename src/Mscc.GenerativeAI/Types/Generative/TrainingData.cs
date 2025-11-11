using System.Collections.Generic;
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