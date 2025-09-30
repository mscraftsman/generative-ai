#if NET472_OR_GREATER || NETSTANDARD2_0
using System.Collections.Generic;
#endif

namespace Mscc.GenerativeAI
{
    /// <summary>
    /// The response message for Operations.ListOperations.
    /// </summary>
    public class ListOperationsResponse
    {
        /// <summary>
        /// A list of operations that matches the specified filter in the request.
        /// </summary>
        public List<Operation> Operations { get; set; }
        /// <summary>
        /// The standard List next-page token.
        /// </summary>
        public string NextPageToken { get; set; }
        /// <summary>
        /// Unordered list. Unreachable resources. Populated when the request sets `ListOperationsRequest.return_partial_success` and reads across collections e.g. when attempting to list all resources across all supported locations.
        /// </summary>
        public List<string>? Unreachable { get; set; }
    }
    
    /// <summary>
    /// This resource represents a long-running operation that is the result of a network API call.
    /// </summary>
    public class Operation
    {
        /// <summary>
        /// The server-assigned name, which is only unique within the same service that originally returns it.
        /// If you use the default HTTP mapping, the `name` should be a resource name ending with `operations/{unique_id}`.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// If the value is `false`, it means the operation is still in progress.
        /// If `true`, the operation is completed, and either `error` or `response` is available.
        /// </summary>
        public bool Done { get; set; }
        /// <summary>
        /// The error result of the operation in case of failure or cancellation.
        /// </summary>
        public Status? Error { get; set; }
        /// <summary>
        /// Service-specific metadata associated with the operation.
        /// </summary>
        /// <remarks>
        /// It typically contains progress information and common metadata such as create time.
        /// Some services might not provide such metadata. Any method that returns a long-running operation
        /// should document the metadata type, if any.
        /// </remarks>
        public object Metadata { get; set; }
        /// <summary>
        /// The normal, successful response of the operation.
        /// </summary>
        /// <remarks>
        /// If the original method returns no data on success, such as `Delete`, the response is `google.protobuf.Empty`.
        /// If the original method is standard `Get`/`Create`/`Update`, the response should be the resource.
        /// For other methods, the response should have the type `XxxResponse`, where `Xxx` is the original method name.
        /// For example, if the original method name is `TakeSnapshot()`, the inferred response type is `TakeSnapshotResponse`.
        /// </remarks>
        public object Response { get; set; }
    }
}