namespace Mscc.GenerativeAI
{
    internal struct Constants
    {
        internal const uint ChunkSize = 8388608;
        internal const long MaxUploadFileSize = 104857600;
        internal const string MediaType = "application/json";
        internal const string RequestFailed = "Request failed";
        internal const string RequestFailedWithStatusCode = "Request failed with Status Code: ";
        internal static readonly int[] RetryStatusCodes = [408, 429, 500, 502, 503, 504];
    }
}