namespace Mscc.GenerativeAI.Types
{
	/// <summary>
	/// Parameters for telling the service how to chunk the file. inspired by google3/cloud/ai/platform/extension/lib/retrieval/config/chunker_config.proto
	/// </summary>
	public partial class ChunkingConfig
	{
		/// <summary>
		/// White space chunking configuration.
		/// </summary>
		public WhiteSpaceConfig? WhiteSpaceConfig { get; set; }
    }
}
