#if NET472_OR_GREATER || NETSTANDARD2_0
using System.Collections.Generic;
#endif

namespace Mscc.GenerativeAI
{
    /// <summary>
    /// Config for the count_tokens method.
    /// </summary>
    public class CountTokensConfig : BaseConfig
    {
        /// <summary>
        /// Configuration that the model uses to generate the response. Not supported by the Gemini Developer API.
        /// </summary>
        public GenerationConfig? GenerationConfig { get; set; }
        /// <summary>
        /// Instructions for the model to steer it toward better performance.
        /// </summary>
        public Content? SystemInstruction { get; set; }
        /// <summary>
        /// Code that enables the system to interact with external systems to perform an action outside of the knowledge and scope of the model.
        /// </summary>
        public List<Tool>? Tools { get; set; }
    }
}