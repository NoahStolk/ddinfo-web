using System;
using System.Collections.Generic;

namespace DevilDaggersWebsite.Dto
{
	public class ChangelogEntry
	{
		public Version VersionNumber { get; set; } = null!;

		public DateTime Date { get; set; }

		public IReadOnlyList<Change> Changes { get; set; } = new List<Change>();
	}
}
