#if NET472_OR_GREATER || NETSTANDARD2_0
using System.Collections.Generic;
#endif

using System.Diagnostics;

namespace Mscc.GenerativeAI
{
    [DebuggerDisplay("{DisplayName} ({Name})")]
    public class ModelResponse
    {
        public string? Name { get; set; } = default;
        public string? Version { get; set; } = default;
        public string? DisplayName { get; set; } = default;
        public string? Description { get; set; } = default;
        public int? InputTokenLimit { get; set; } = default;
        public int? OutputTokenLimit { get; set; } = default;
        public List<string>? SupportedGenerationMethods { get; set; }
        public float? Temperature { get; set; } = default;
        public float? TopP { get; set; } = default;
        public int? TopK { get; set; } = default;
    }
}