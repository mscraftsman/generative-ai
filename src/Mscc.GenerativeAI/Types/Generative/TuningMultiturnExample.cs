#if NET472_OR_GREATER || NETSTANDARD2_0
using System.Collections.Generic;
#endif
using System.Diagnostics;

namespace Mscc.GenerativeAI
{
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