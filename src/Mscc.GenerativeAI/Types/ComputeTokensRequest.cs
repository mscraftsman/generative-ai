using System.Collections.Generic;

namespace Mscc.GenerativeAI.Types
{
    /// <summary>
    /// Request message for ComputeTokens RPC call.
    /// </summary>
    public partial class ComputeTokensRequest : IVertexAware
    {
        /// <summary>
        /// Optional parameters for the request.
        /// </summary>
        public ComputeTokensConfig? Config { get; set; }

        public void PrepareForSerialization(bool useVertexAi)
        {
            if (useVertexAi)
            {
                if (Contents != null && Instances == null)
                {
                    Instances = new List<object>(Contents);
                    Contents = null;
                }
            }
            else
            {
                if (Instances != null && Contents == null)
                {
                    Contents = new List<Content>();
                    foreach (var instance in Instances)
                    {
                        if (instance is Content content)
                        {
                            Contents.Add(content);
                        }
                    }
                    Instances = null;
                }
            }
        }
    }
}