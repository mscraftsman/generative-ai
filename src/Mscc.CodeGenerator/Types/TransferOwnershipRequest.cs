namespace Mscc.GenerativeAI.Types
{
	/// <summary>
	/// Request to transfer the ownership of the tuned model.
	/// </summary>
	public partial class TransferOwnershipRequest
	{
		/// <summary>
		/// Required. The email address of the user to whom the tuned model is being transferred to.
		/// </summary>
		public string? EmailAddress { get; set; }
    }
}
