#if NET472_OR_GREATER || NETSTANDARD2_0
using System.Text.Json.Serialization;
#endif

namespace Mscc.GenerativeAI
{
    /// <summary>
    /// An image of the product.
    /// </summary>
    public class ProductImage
    {
        [JsonPropertyName("productImage")]
        public Image? ProductImageField { get; set; }
    }
}