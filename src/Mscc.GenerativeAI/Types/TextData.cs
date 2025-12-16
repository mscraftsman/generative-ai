using System.Diagnostics;

namespace Mscc.GenerativeAI.Types
{
    /// <summary>
    /// 
    /// </summary>
    [DebuggerDisplay("{Text}")]
    public abstract class AbstractText 
    {
        /// <summary>
        /// Required. The prompt text.
        /// </summary>
        public string? Text { get; set; }
    }
    
    /// <summary>
    /// 
    /// </summary>
    public sealed class TextData : AbstractText, IPart { }

    /// <summary>
    /// 
    /// </summary>
    public partial class TextPrompt : AbstractText { }
}
