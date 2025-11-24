using System.Collections.Generic;
using System.Diagnostics;

namespace Mscc.GenerativeAI.Types
{
    /// <summary>
    /// A tuning example with multiturn input.
    /// </summary>
    public partial class TuningMultiturnExample
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