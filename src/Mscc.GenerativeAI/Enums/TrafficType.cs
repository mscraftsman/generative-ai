#if NET472_OR_GREATER || NETSTANDARD2_0
using System.Text.Json.Serialization;
#endif

namespace Mscc.GenerativeAI
{
    /// <summary>
    /// The traffic type for this request.
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter<TrafficType>))]
    public enum TrafficType
    {
        /// <summary>
        /// Unspecified request traffic type.
        /// </summary>
        TrafficTypeUnspecified,
        /// <summary>
        /// The request was processed using Pay-As-You-Go quota.
        /// </summary>
        OnDemand,
        /// <summary>
        /// Type for Provisioned Throughput traffic.
        /// </summary>
        ProvisionedThroughput
    }
}