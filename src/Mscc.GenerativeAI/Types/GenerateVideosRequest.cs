using System;
using System.Collections.Generic;

namespace Mscc.GenerativeAI.Types
{
    /// <summary>
    /// 
    /// </summary>
    public class GenerateVideosRequest
    {
        /// <summary>
        /// An array that contains the object with video details to get information about.
        /// </summary>
        public IEnumerable<Instance> Instances { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public GenerateVideosConfig? Parameters { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="GenerateVideosRequest"/> class.
        /// </summary>
        public GenerateVideosRequest() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="GenerateVideosRequest"/> class.
        /// </summary>
        /// <param name="prompt">The text prompt guides what videos the model generates.</param>
        /// <param name="numberOfVideos">The number of generated videos.</param>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="prompt"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when the <paramref name="numberOfVideos"/> is less than 1 or greater than 8.</exception>
        public GenerateVideosRequest(string prompt, int? numberOfVideos = 1) : this()
        {
            if (prompt == null) throw new ArgumentNullException(nameof(prompt));
            if (numberOfVideos < 1 || numberOfVideos > 8) throw new ArgumentOutOfRangeException(nameof(numberOfVideos));

            Instances = new[] { new Instance { Prompt = prompt } };
            Parameters = new GenerateVideosConfig { NumberOfVideos = numberOfVideos };
        }
    }
}