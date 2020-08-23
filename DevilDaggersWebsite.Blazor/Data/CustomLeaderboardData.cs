using DevilDaggersWebsite.Core.Entities;
using System;

namespace DevilDaggersWebsite.Blazor.Data
{
	public class CustomLeaderboardData
	{
		public CustomLeaderboardData(CustomLeaderboard cl, int totalPlayers, CustomEntry worldRecord)
		{
			SpawnsetFileId = cl.SpawnsetFileId;
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
			TotalPlayers = totalPlayers;
			WorldRecordHolderUsername = worldRecord.Player.Username;
			WorldRecordTime = worldRecord.Time;
			WorldRecordDagger = cl.GetDagger(worldRecord.Time);
		}

		public int SpawnsetFileId { get; }
		public string SpawnsetName { get; }
		public string SpawnsetAuthorName { get; }
		public CustomLeaderboardCategory Category { get; }
		public int Bronze { get; }
		public int Silver { get; }
		public int Golden { get; }
		public int Devil { get; }
		public int Homing { get; }
		public int TotalPlayers { get; }

		public DateTime? DateLastPlayed { get; }
		public DateTime? DateCreated { get; }

		public string WorldRecordHolderUsername { get; }
		public int WorldRecordTime { get; }
		public string WorldRecordDagger { get; }
	}

	public enum CustomLeaderboardCategory
	{
		Default,
		Speedrun,
	}
}