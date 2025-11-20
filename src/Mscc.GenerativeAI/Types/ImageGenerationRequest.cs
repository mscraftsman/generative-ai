using System;
using System.Collections.Generic;

namespace Mscc.GenerativeAI.Types
{
    /// <summary>
    /// 
    /// </summary>
    public class ImageGenerationRequest
    {
        /// <summary>
        /// An array that contains the object with image details to get information about.
        /// </summary>
        public List<Instance> Instances { get; set; } = [];

        /// <summary>
        /// 
        /// </summary>
        public ImageGenerationParameters Parameters { get; set; } = new();

        /// <summary>
        /// Initializes a new instance of the <see cref="ImageGenerationRequest"/> class.
        /// </summary>
        public ImageGenerationRequest() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ImageGenerationRequest"/> class.
        /// </summary>
        /// <param name="prompt">The text prompt guides what images the model generates.</param>
        /// <param name="sampleCount">The number of generated images.</param>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="prompt"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when the <paramref name="sampleCount"/> is less than 1 or greater than 8.</exception>
        public ImageGenerationRequest(string prompt, int? sampleCount = 4) : this()
        {
            if (prompt == null) throw new ArgumentNullException(nameof(prompt));
            if (sampleCount < 1 || sampleCount > 8) throw new ArgumentOutOfRangeException(nameof(sampleCount));

            Instances = [new Instance { Prompt = prompt }];
            Parameters = new ImageGenerationParameters { SampleCount = sampleCount };
        }
    }
}