using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Mscc.GenerativeAI
{
    /// <summary>
    /// 
    /// </summary>
    public class ImageTextRequest
    {
        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<Instance> Instances { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public ImageTextParameters? Parameters { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ImageGenerationRequest"/> class.
        /// </summary>
        public ImageTextRequest() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ImageGenerationRequest"/> class.
        /// </summary>
        /// <param name="base64Image">The base64 encoded image to process.</param>
        /// <param name="question">The question to ask about the image.</param>
        /// <param name="sampleCount">The number of predictions.</param>
        /// <param name="language">Language of predicted text. Defaults to "en".</param>
        /// <param name="storageUri">Optional. Cloud Storage URI where to store the generated predictions.</param>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="base64Image"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when the <paramref name="sampleCount"/> is less than 1 or greater than 3.</exception>
        /// <exception cref="NotSupportedException">Thrown when the <paramref name="language"/> is not supported.</exception>
        public ImageTextRequest(string base64Image,
            string? question = null,
            int? sampleCount = null,
            string? language = null,
            string? storageUri = null) : this()
        {
            if (base64Image == null) throw new ArgumentNullException(nameof(base64Image));
            sampleCount ??= 1;
            if (sampleCount < 1 || sampleCount > 3) throw new ArgumentOutOfRangeException(nameof(sampleCount));
            language ??= "en";
            language.GuardSupportedLanguage();

            Instances = new[]
            {
                new Instance { Prompt = question, Image = new Image() { BytesBase64Encoded = base64Image } }
            };
            Parameters = new ImageTextParameters
            {
                SampleCount = sampleCount, 
                Language = language.ToLowerInvariant(),
                StorageUri = storageUri
            };
        }
    }
}