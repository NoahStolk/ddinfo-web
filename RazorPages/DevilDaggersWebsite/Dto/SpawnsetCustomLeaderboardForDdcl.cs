using System.Collections.Generic;

namespace DevilDaggersWebsite.Dto
{
	public class SpawnsetCustomLeaderboardForDdcl
	{
		public int CustomLeaderboardId { get; init; }

		public List<SpawnsetCustomEntryForDdcl> CustomEntries { get; init; } = new();
	}
}
