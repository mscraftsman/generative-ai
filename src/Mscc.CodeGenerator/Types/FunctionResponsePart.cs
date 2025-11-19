namespace Mscc.GenerativeAI.Types
{
	/// <summary>
	/// A datatype containing media that is part of a <see cref="FunctionResponse"/> message. A <see cref="FunctionResponsePart"/> consists of data which has an associated datatype. A <see cref="FunctionResponsePart"/> can only contain one of the accepted types in <see cref="FunctionResponsePart.data"/>. A <see cref="FunctionResponsePart"/> must have a fixed IANA MIME type identifying the type and subtype of the media if the <see cref="inline_data"/> field is filled with raw bytes.
	/// </summary>
	public partial class FunctionResponsePart
	{
		/// <summary>
		/// Inline media bytes.
		/// </summary>
		public FunctionResponseBlob? InlineData { get; set; }
    }
}
