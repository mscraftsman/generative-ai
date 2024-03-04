using System.Diagnostics;

namespace Mscc.GenerativeAI
{
    [DebuggerDisplay("{Text}")]
    public class TextData : IPart
    {
        public string Text { get; set; } = default;
    }
}
