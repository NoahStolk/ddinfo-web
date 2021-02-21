﻿using DevilDaggersWebsite.BlazorServer.Extensions;
using DevilDaggersWebsite.Entities;
using System;

namespace DevilDaggersWebsite.BlazorServer.Data
{
	public class CustomLeaderboardData
	{
		public CustomLeaderboardData(CustomLeaderboard cl, int totalPlayers, CustomEntry? worldRecord)
		{
			SpawnsetFileId = cl.SpawnsetFileId;
			SpawnsetName = cl.SpawnsetFile.Name;
			SpawnsetAuthorName = cl.SpawnsetFile.Player.PlayerName;
			Category = cl.Category.ToString();
			Bronze = cl.TimeBronze;
			Silver = cl.TimeSilver;
			Golden = cl.TimeGolden;
			Devil = cl.TimeDevil;
			Homing = cl.TimeLeviathan;
			DateLastPlayedUtc = cl.DateLastPlayed?.ToUniversalTime();
			DateCreated = cl.DateCreated;
			TotalPlayers = totalPlayers;
			TotalRunSubmitted = cl.TotalRunsSubmitted;
			WorldRecordHolderUsername = worldRecord?.Player.PlayerName ?? "N/A";
			WorldRecordTime = worldRecord?.Time ?? 0;
			WorldRecordDagger = cl.GetDagger(worldRecord?.Time ?? 0);
		}

		public int SpawnsetFileId { get; }
		public string SpawnsetName { get; }
		public string SpawnsetAuthorName { get; }
		public string Category { get; }
		public int Bronze { get; }
		public int Silver { get; }
		public int Golden { get; }
		public int Devil { get; }
		public int Homing { get; }
		public int TotalPlayers { get; }
		public int TotalRunSubmitted { get; }

		public DateTime? DateLastPlayedUtc { get; }
		public DateTime? DateCreated { get; }

		public string WorldRecordHolderUsername { get; }
		public int WorldRecordTime { get; }
		public string WorldRecordDagger { get; }
	}
}
