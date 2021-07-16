using System;
using System.Collections.Generic;

namespace DevilDaggersWebsite.BlazorWasm.Shared.Dto.WorldRecords
{
	public class GetWorldRecordHolderPublic
	{
		public GetWorldRecordHolderPublic(int id, string username, TimeSpan totalTimeHeld, TimeSpan longestTimeHeldConsecutively, int worldRecordCount, DateTime firstHeld, DateTime lastHeld)
		{
			Id = id;
			Usernames = new() { username };
			TotalTimeHeld = totalTimeHeld;
			LongestTimeHeldConsecutively = longestTimeHeldConsecutively;
			WorldRecordCount = worldRecordCount;
			FirstHeld = firstHeld;
			LastHeld = lastHeld;

			MostRecentUsername = username;
		}

		public int Id { get; }
		public List<string> Usernames { get; }
		public TimeSpan TotalTimeHeld { get; set; }
		public TimeSpan LongestTimeHeldConsecutively { get; set; }
		public int WorldRecordCount { get; set; }
		public DateTime FirstHeld { get; set; }
		public DateTime LastHeld { get; set; }

		public string MostRecentUsername { get; set; }
	}
}
