using System;
using System.Collections.Generic;

namespace Mscc.GenerativeAI
{
    /// <summary>
    /// 
    /// </summary>
    public class ImagesGenerationsResponse
    {
        /// <summary>
        /// 
        /// </summary>
        public string Model { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<Image> Data { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime Created { get; set; }
    }
}