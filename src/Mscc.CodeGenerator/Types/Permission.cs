namespace Mscc.GenerativeAI.Types
{
	/// <summary>
	/// Permission resource grants user, group or the rest of the world access to the PaLM API resource (e.g. a tuned model, corpus). A role is a collection of permitted operations that allows users to perform specific actions on PaLM API resources. To make them available to users, groups, or service accounts, you assign roles. When you assign a role, you grant permissions that the role contains. There are three concentric roles. Each role is a superset of the previous role's permitted operations: - reader can use the resource (e.g. tuned model, corpus) for inference - writer has reader's permissions and additionally can edit and share - owner has writer's permissions and additionally can delete
	/// </summary>
	public partial class Permission
	{
		/// <summary>
		/// Optional. Immutable. The email address of the user of group which this permission refers. Field is not set when permission's grantee type is EVERYONE.
		/// </summary>
		public string? EmailAddress { get; set; }
		/// <summary>
		/// Optional. Immutable. The type of the grantee.
		/// </summary>
		public GranteeType? GranteeType { get; set; }
		/// <summary>
		/// Output only. Identifier. The permission name. A unique name will be generated on create. Examples: tunedModels/{tuned_model}/permissions/{permission} corpora/{corpus}/permissions/{permission} Output only.
		/// </summary>
		public string? Name { get; set; }
		/// <summary>
		/// Required. The role granted by this permission.
		/// </summary>
		public Role? Role { get; set; }
    }
}
