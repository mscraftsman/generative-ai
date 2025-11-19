namespace Mscc.GenerativeAI.Types
{
	/// <summary>
	/// The base unit of structured text. A <see cref="Message"/> includes an <see cref="author"/> and the <see cref="content"/> of the <see cref="Message"/>. The <see cref="author"/> is used to tag messages when they are fed to the model as text.
	/// </summary>
	public partial class Message
	{
		/// <summary>
		/// Optional. The author of this Message. This serves as a key for tagging the content of this Message when it is fed to the model as text. The author can be any alphanumeric string.
		/// </summary>
		public string? Author { get; set; }
		/// <summary>
		/// Output only. Citation information for model-generated <see cref="content"/> in this <see cref="Message"/>. If this <see cref="Message"/> was generated as output from the model, this field may be populated with attribution information for any text included in the <see cref="content"/>. This field is used only on output.
		/// </summary>
		public CitationMetadata? CitationMetadata { get; set; }
		/// <summary>
		/// Required. The text content of the structured <see cref="Message"/>.
		/// </summary>
		public string? Content { get; set; }
    }
}
