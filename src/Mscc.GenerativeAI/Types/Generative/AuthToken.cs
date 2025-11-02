#if NET472_OR_GREATER || NETSTANDARD2_0
using System;
#endif

namespace Mscc.GenerativeAI
{
    /// <summary>
    /// A request to create an ephemeral authentication token.
    /// </summary>
    public class AuthToken
    {
        /// <summary>
        /// Optional. Input only. Immutable. Configuration specific to `BidiGenerateContent`.
        /// </summary>
        public BidiGenerateContentSetup? BidiGenerateContentSetup { get; set; }
        /// <summary>
        /// Optional. Input only. Immutable. An optional time after which, when using the resulting token, messages in BidiGenerateContent sessions will be rejected. (Gemini may preemptively close the session after this time.) If not set then this defaults to 30 minutes in the future. If set, this value must be less than 20 hours in the future.
        /// </summary>
        public DateTime? ExpireTime { get; set; }
        /// <summary>
        /// Optional. Input only. Immutable. If field_mask is empty, and `bidi_generate_content_setup` is not present, then the effective `BidiGenerateContentSetup` message is taken from the Live API connection. If field_mask is empty, and `bidi_generate_content_setup` _is_ present, then the effective `BidiGenerateContentSetup` message is taken entirely from `bidi_generate_content_setup` in this request. The setup message from the Live API connection is ignored. If field_mask is not empty, then the corresponding fields from `bidi_generate_content_setup` will overwrite the fields from the setup message in the Live API connection.
        /// </summary>
        public string? FieldMask { get; set; }
        /// <summary>
        /// Output only. Identifier. The token itself.
        /// </summary>
        public string? Name { get; set; }
        /// <summary>
        /// Optional. Input only. Immutable. The time after which new Live API sessions using the token resulting from this request will be rejected. If not set this defaults to 60 seconds in the future. If set, this value must be less than 20 hours in the future.
        /// </summary>
        public DateTime? NewSessionExpireTime { get; set; }
        /// <summary>
        /// Optional. Input only. Immutable. The number of times the token can be used. If this value is zero then no limit is applied. Resuming a Live API session does not count as a use. If unspecified, the default is 1.
        /// </summary>
        public int? Uses { get; set; }
    }
}