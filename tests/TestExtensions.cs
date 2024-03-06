using Google.Protobuf;
using System.Net.Http;
using System.Threading.Tasks;

namespace Test.Mscc.GenerativeAI
{
    internal static class TestExtensions
    {
        internal static async Task<string> ReadImageFileBase64Async(string url)
        {
            using (var client = new HttpClient())
            {
                using (var response = await client.GetAsync(url))
                {
                    byte[] imageBytes = await response.Content.ReadAsByteArrayAsync();
                    return ByteString.CopyFrom(imageBytes).ToBase64();
                }
            }
        }

        internal static async Task<ByteString> ReadImageFileAsync(string url)
        {
            using (var client = new HttpClient())
            {
                using (var response = await client.GetAsync(url))
                {
                    byte[] imageBytes = await response.Content.ReadAsByteArrayAsync();
                    return ByteString.CopyFrom(imageBytes);
                }
            }
        }
    }
}
