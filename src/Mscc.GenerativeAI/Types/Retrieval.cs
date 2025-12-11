using System;

namespace Mscc.GenerativeAI.Types
{
    /// <summary>
    /// Defines a retrieval tool that model can call to access external knowledge.
    /// </summary>
    public partial class Retrieval : ITool
    {
        /// <summary>
        /// Optional. Disable using the result from this tool in detecting grounding attribution.
        /// </summary>
        /// <remarks>This does not affect how the result is given to the model for generation.</remarks>
        [Obsolete("This item is deprecated!")]
        public bool? DisableAttribution { get; set; }
    }
}