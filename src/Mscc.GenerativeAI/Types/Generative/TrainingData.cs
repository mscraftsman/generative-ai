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

    /// <summary>
    /// A tuning example with multiturn input.
    /// </summary>
    public class TuningMultiturnExample
    {
        /// <summary>
        /// Each Content represents a turn in the conversation.
        /// </summary>
        public List<TuningContent> Contents { get; set; }
        /// <summary>
        /// Optional. Developer set system instructions. Currently, text only.
        /// </summary>
        public TuningContent? SystemInstruction { get; set; }
    }
}