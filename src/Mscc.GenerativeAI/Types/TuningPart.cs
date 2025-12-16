using System.Diagnostics;

namespace Mscc.GenerativeAI.Types
{
	/// <summary>
	/// A datatype containing data that is part of a multi-part `TuningContent` message.
	/// This is a subset of the Part used for model inference, with limited type support.
	/// A `Part` consists of data which has an associated datatype.
	/// A `Part` can only contain one of the accepted types in `Part.data`.
	/// </summary>
	[DebuggerDisplay("{Text}")]
	public partial class TuningPart : Part
	{
	}
}