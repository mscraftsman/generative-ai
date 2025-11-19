using System.Text.Json.Serialization;

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