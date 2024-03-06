#if NET472_OR_GREATER || NETSTANDARD2_0
using System.Collections.Generic;
#endif

namespace Mscc.GenerativeAI
{
    internal class ListModelsResponse
    {
        public List<ModelResponse>? Models { get; set; }
    }
}