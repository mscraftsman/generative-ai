using System.Collections.Generic;

namespace Mscc.GenerativeAI
{
	public sealed class QuotaFailure
	{
		public List<Violation>? Violations { get; set; }

		public sealed class Violation
		{
			public string QuotaMetric { get; set; }
			public string QuotaId { get; set; }
			public QuotaDimensions QuotaDimensions { get; set; }
		}

		public sealed class QuotaDimensions
		{
			public string Model { get; set; }
			public string Location { get; set; }
		}
	}
}