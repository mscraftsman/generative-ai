namespace Mscc.GenerativeAI.Types
{
	/// <summary>
	/// Response from <see cref="ListPermissions"/> containing a paginated list of permissions.
	/// </summary>
	public partial class ListPermissionsResponse
	{
		/// <summary>
		/// A token, which can be sent as <see cref="page_token"/> to retrieve the next page. If this field is omitted, there are no more pages.
		/// </summary>
		public string? NextPageToken { get; set; }
		/// <summary>
		/// Returned permissions.
		/// </summary>
		public List<Permission>? Permissions { get; set; }
    }
}
