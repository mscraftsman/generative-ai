using System.Diagnostics;

namespace Mscc.GenerativeAI
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
    public sealed class TextPrompt : AbstractText { }
}
