using System;

namespace DevilDaggersWebsite.Models.Leaderboard
{
	public class WorldRecordHolder
	{
		public int ID { get; set; }
		public string Username { get; set; }
		public TimeSpan TimeHeld { get; set; }

		public WorldRecordHolder(int id, string username, TimeSpan timeHeld)
		{
			ID = id;
			Username = username;
			TimeHeld = timeHeld;
		}
	}
}