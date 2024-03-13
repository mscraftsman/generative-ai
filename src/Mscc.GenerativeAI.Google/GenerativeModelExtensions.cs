#if NET472_OR_GREATER || NETSTANDARD2_0
using System;
#endif

namespace Mscc.GenerativeAI.Google
{
    public static class GenerativeModelExtensions
    {
        public static GenerativeModelGoogle WithProjectId(this GenerativeModelGoogle vertex, string projectId)
        {
            vertex.ProjectId = projectId ?? throw new ArgumentNullException(nameof(projectId));
            return vertex;
        }

        public static GenerativeModelGoogle WithRegion(this GenerativeModelGoogle vertex, string region)
        {
            vertex.Region = region ?? throw new ArgumentNullException(nameof(region));
            return vertex;
        }
    }
}