using System.Text.Json.Serialization;

namespace Mscc.GenerativeAI.Types
{
	[JsonConverter(typeof(JsonStringEnumConverter<Role>))]
    public enum Role
    {
        /// <summary>
        /// The default value. This value is unused.
        /// </summary>
        RoleUnspecified,
        /// <summary>
        /// Owner can use, update, share and delete the resource.
        /// </summary>
        Owner,
        /// <summary>
        /// Writer can use, update and share the resource.
        /// </summary>
        Writer,
        /// <summary>
        /// Reader can use the resource.
        /// </summary>
        Reader,
    }
}