using System.Collections.Generic;

namespace Mscc.GenerativeAI
{
	public sealed class Help
	{
		public List<Link>? Links { get; set; }

		public sealed class Link
		{
			public string? Description { get; set; }
			public string? Url { get; set; }
		}
	}
}