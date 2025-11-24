using System.Text.Json.Serialization;

namespace Mscc.GenerativeAI.Types
{
    /// <summary>
    /// An image of the product.
    /// </summary>
    public partial class ProductImage
    {
        [JsonPropertyName("productImage")]
        public Image? ProductImageField { get; set; }
    }
}