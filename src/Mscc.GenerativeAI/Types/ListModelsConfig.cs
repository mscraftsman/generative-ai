namespace Mscc.GenerativeAI.Types
{
    /// <summary>
    /// Configuration for retrieving models.
    /// </summary>
    public class ListModelsConfig : BaseListConfig
    {
        /// <summary>
        /// If query_base is set to True in the config or not set (default), the API will return all available base models. If set to False, it will return all tuned models.
        /// </summary>
        public bool QueryBase { get; set; } = true;
    }
}