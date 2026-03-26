namespace Mscc.GenerativeAI.Types
{
	public partial class BatchEmbedContentsRequest : IVertexAware
	{
		public void PrepareForSerialization(bool useVertexAi)
		{
			if (Requests != null)
			{
				foreach (var request in Requests)
				{
					request.PrepareForSerialization(useVertexAi);
				}
			}
		}
	}
}
