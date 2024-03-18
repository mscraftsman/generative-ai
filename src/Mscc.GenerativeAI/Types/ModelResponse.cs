#if NET472_OR_GREATER || NETSTANDARD2_0
using System;
using System.Collections.Generic;
#endif
using System.Diagnostics;

namespace Mscc.GenerativeAI
{
    internal class ListModelsResponse
    {
        public List<ModelResponse>? Models { get; set; }
    }

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
        
        // Properties related to tunedModels.
        public string? BaseModel { get; set; }
        public string? State { get; set; }
        public DateTime? CreateTime { get; set; }
        public DateTime? UpdateTime { get; set; }
        public TuningTask? TuningTask { get; set; }

    }
}