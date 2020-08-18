using System.Collections.Generic;

namespace DevilDaggersWebsite.Code.DataTransferObjects
{
	public class Change
	{
		public Change(string description)
		{
			Description = description;
		}

		public string Description { get; set; }

		public IReadOnlyList<Change>? SubChanges { get; set; }
	}
}