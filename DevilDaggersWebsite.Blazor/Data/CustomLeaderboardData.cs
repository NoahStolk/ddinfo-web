using DevilDaggersWebsite.Core.Entities;
using System;

namespace DevilDaggersWebsite.Blazor.Data
{
	public class CustomLeaderboardData
	{
		public CustomLeaderboardData(CustomLeaderboard cl)
		{
			SpawnsetName = cl.SpawnsetFile.Name;
			SpawnsetAuthorName = cl.SpawnsetFile.Player.Username;
			Category = Enum.Parse<CustomLeaderboardCategory>(cl.Category.Name);
			Bronze = cl.Bronze;
			Silver = cl.Silver;
			Golden = cl.Golden;
			Devil = cl.Devil;
			Homing = cl.Homing;
			DateLastPlayed = cl.DateLastPlayed;
			DateCreated = cl.DateCreated;
		}

		public string SpawnsetName { get; set; }
		public string SpawnsetAuthorName { get; set; }
		public CustomLeaderboardCategory Category { get; set; }
		public int Bronze { get; set; }
		public int Silver { get; set; }
		public int Golden { get; set; }
		public int Devil { get; set; }
		public int Homing { get; set; }

		public DateTime? DateLastPlayed { get; set; }
		public DateTime? DateCreated { get; set; }
	}

	public enum CustomLeaderboardCategory
	{
		Default,
		Speedrun,
	}
}