using System;
using System.Collections.Generic;

namespace Mscc.GenerativeAI.Types
{
    /// <summary>
    /// 
    /// </summary>
    public partial class ImagesGenerationsResponse
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