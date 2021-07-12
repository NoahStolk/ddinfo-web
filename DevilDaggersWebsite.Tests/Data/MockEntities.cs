using DevilDaggersWebsite.BlazorWasm.Server.Entities;
using DevilDaggersWebsite.Enumerators;
using DevilDaggersWebsite.Tests.Extensions;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Linq;
using System.Reflection;

namespace DevilDaggersWebsite.Tests.Data
{
	public class MockEntities
	{
		public MockEntities()
		{
			SpawnsetFile.Player = TestPlayer1;
			CustomLeaderboard.SpawnsetFile = SpawnsetFile;
			CustomEntry.CustomLeaderboard = CustomLeaderboard;
			CustomEntry.Player = TestPlayer1;

			PropertyInfo[] properties = typeof(MockEntities).GetProperties();

			MockDbSetPlayers = new Mock<DbSet<Player>>().SetUpDbSet(GetEntities<Player>());
			MockDbSetSpawnsetFiles = new Mock<DbSet<SpawnsetFile>>().SetUpDbSet(GetEntities<SpawnsetFile>());
			MockDbSetCustomLeaderboards = new Mock<DbSet<CustomLeaderboard>>().SetUpDbSet(GetEntities<CustomLeaderboard>());
			MockDbSetCustomEntries = new Mock<DbSet<CustomEntry>>().SetUpDbSet(GetEntities<CustomEntry>());
			MockDbSetCustomEntryData = new Mock<DbSet<CustomEntryData>>().SetUpDbSet(GetEntities<CustomEntryData>());

			T[] GetEntities<T>() => properties
				.Where(pi => pi.PropertyType == typeof(T))
				.Select(pi => (T)pi.GetValue(this)!)
				.ToArray();
		}

		public Mock<DbSet<Player>> MockDbSetPlayers { get; }
		public Mock<DbSet<SpawnsetFile>> MockDbSetSpawnsetFiles { get; }
		public Mock<DbSet<CustomLeaderboard>> MockDbSetCustomLeaderboards { get; }
		public Mock<DbSet<CustomEntry>> MockDbSetCustomEntries { get; }
		public Mock<DbSet<CustomEntryData>> MockDbSetCustomEntryData { get; }

		public Player TestPlayer1 { get; } = new()
		{
			Id = 1,
			PlayerName = "TestPlayer1",
		};

		public Player TestPlayer2 { get; } = new()
		{
			Id = 2,
			PlayerName = "TestPlayer2",
		};

		public SpawnsetFile SpawnsetFile { get; } = new()
		{
			Id = 1,
			LastUpdated = DateTime.UtcNow,
			Name = "V3",
			PlayerId = 1,
			HtmlDescription = string.Empty,
			MaxDisplayWaves = 5,
		};

		public CustomLeaderboard CustomLeaderboard { get; } = new()
		{
			Id = 1,
			TimeBronze = 600000,
			TimeSilver = 1200000,
			TimeGolden = 2500000,
			TimeDevil = 5000000,
			TimeLeviathan = 10000000,
			Category = CustomLeaderboardCategory.Default,
			DateCreated = DateTime.UtcNow,
			DateLastPlayed = DateTime.UtcNow,
			SpawnsetFileId = 1,
			TotalRunsSubmitted = 666,
		};

		public CustomEntry CustomEntry { get; } = new()
		{
			Id = 1,
			ClientVersion = TestConstants.DdclVersion,
			CustomLeaderboardId = 1,
			DaggersFired = 15,
			DaggersHit = 6,
			DeathType = 1,
			EnemiesAlive = 6,
			GemsCollected = 3,
			HomingDaggers = 0,
			EnemiesKilled = 2,
			PlayerId = 1,
			Time = 166666,
			LevelUpTime2 = 0,
			LevelUpTime3 = 0,
			LevelUpTime4 = 0,
			SubmitDate = DateTime.UtcNow,
		};
	}
}
