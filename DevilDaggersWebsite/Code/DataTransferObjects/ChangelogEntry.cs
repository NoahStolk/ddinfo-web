using System;
using System.Collections.Generic;

namespace DevilDaggersWebsite.Code.DataTransferObjects
{
	public class ChangelogEntry
	{
		public Version VersionNumber { get; set; }

		public DateTime Date { get; set; }

		public IReadOnlyList<Change> Changes { get; set; }
	}
}