using System.Text.Json.Serialization;

namespace Mscc.GenerativeAI.Types
{
	[JsonConverter(typeof(JsonStringEnumConverter<GranteeType>))]
    public enum GranteeType
    {
        /// <summary>
        /// The default value. This value is unused.
        /// </summary>
        GranteeTypeUnspecified,
        /// <summary>
        /// Represents a user. When set, you must provide email_address for the user.
        /// </summary>
        User,
        /// <summary>
        /// Represents a group. When set, you must provide email_address for the group.
        /// </summary>
        Group,
        /// <summary>
        /// Represents access to everyone. No extra information is required.
        /// </summary>
        Everyone,
    }
}