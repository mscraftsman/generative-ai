using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Mscc.GenerativeAI
{
    public class ListTunedModelResponse
    {
        public List<ModelResponse> TunedModels { get; set; }
    }

    [DebuggerDisplay("{DisplayName} ({Name})")]
    public class TunedModelResponse
    {
        public string Name { get; set; }
        public string BaseModel { get; set; }
        public string DisplayName { get; set; }
        public string State { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }
        public TuningTask TuningTask { get; set; }
        public float Temperature { get; set; }
        public float TopP { get; set; }
        public int TopK { get; set; }
    }
}