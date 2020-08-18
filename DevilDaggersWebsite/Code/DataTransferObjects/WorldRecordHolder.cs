using System;
using System.Collections.Generic;

namespace DevilDaggersWebsite.Code.DataTransferObjects
{
	public class WorldRecordHolder
	{
		public WorldRecordHolder(int id, string username, TimeSpan totalTimeHeld, TimeSpan longestTimeHeldConsecutively, int worldRecordCount, DateTime lastHeld)
		{
			Id = id;
			Usernames.Add(username);
			TotalTimeHeld = totalTimeHeld;
			LongestTimeHeldConsecutively = longestTimeHeldConsecutively;
			WorldRecordCount = worldRecordCount;
			LastHeld = lastHeld;

			MostRecentUsername = username;
		}

		public int Id { get; set; }
		public List<string> Usernames { get; } = new List<string>();
		public TimeSpan TotalTimeHeld { get; set; }
		public TimeSpan LongestTimeHeldConsecutively { get; set; }
		public int WorldRecordCount { get; set; }
		public DateTime LastHeld { get; set; }

		public string MostRecentUsername { get; set; }
	}
}