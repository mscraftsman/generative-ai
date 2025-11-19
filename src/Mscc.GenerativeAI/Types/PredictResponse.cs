using System.Collections.Generic;

namespace Mscc.GenerativeAI
{
    /// <summary>
    /// Response message for [PredictionService.Predict].
    /// </summary>
    public class PredictResponse
    {
        /// <summary>
        /// The outputs of the prediction call.
        /// </summary>
        public List<object> Predictions { get; set; }
    }
}