#if NET472_OR_GREATER || NETSTANDARD2_0
using System.Collections.Generic;
#endif
using System.Diagnostics;

namespace Mscc.GenerativeAI
{
    public class TrainingData
    {
        public TrainingDataExamples Examples { get; set; }
    }

    public class TrainingDataExamples
    {
        public List<TrainingDataExample> Examples { get; set; }
    }

    [DebuggerDisplay("Input: {TextInput,nq} - Output: {Output,nq}")]
    public class TrainingDataExample
    {
        public string TextInput { get; set; }
        public string Output { get; set; }
    }
}