#if NET472_OR_GREATER || NETSTANDARD2_0
using System.Text.Json.Serialization;
#endif
using System.Collections.Generic;
using System.Linq;

namespace Mscc.GenerativeAI
{
    /// <summary>
    /// 
    /// </summary>
    public class Embedding
    {
        /// <summary>
        /// 
        /// </summary>
        public List<float> Values { get; set; }
    }
}
