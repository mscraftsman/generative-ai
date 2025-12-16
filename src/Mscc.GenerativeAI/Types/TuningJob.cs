using System.Collections.Generic;
using System.Diagnostics;

namespace Mscc.GenerativeAI.Types
{
    /// <summary>
    /// 
    /// </summary>
    public partial class ListTuningJobResponse
    {
        public List<TuningJob> TuningJobs { get; set; }
    }
    
    /// <summary>
    /// 
    /// </summary>
    [DebuggerDisplay("{Name} ({BaseModel}): {Endpoint}")]
    public partial class TuningJob
    {
        /// <summary>
        /// 
        /// </summary>
        public string? Endpoint => TunedModel?.Endpoint;
        /// <summary>
        /// 
        /// </summary>
        public bool HasEnded => EndTime.HasValue;
    }
}