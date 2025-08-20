namespace Mscc.GenerativeAI
{
    internal struct Constants
    {
        internal const uint ChunkSize = 8388608;
        internal const long MaxUploadFileSize = 2147483648;
        internal const string MediaType = "application/json";
        internal const string RequestFailed = "Request failed";
        internal const string RequestFailedWithStatusCode = "Request failed with Status Code: ";
        internal static readonly int[] RetryStatusCodes = [429, 500, 503, 504];
    }
}