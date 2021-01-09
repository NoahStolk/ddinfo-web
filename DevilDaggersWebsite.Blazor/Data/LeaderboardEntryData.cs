using DevilDaggersCore.Game;
using DevilDaggersCore.Utils;
using DevilDaggersWebsite.Dto;
using DevilDaggersWebsite.Entities;
using System.Globalization;

namespace DevilDaggersWebsite.Blazor.Data
{
	public class LeaderboardEntryData
	{
		public LeaderboardEntryData(Entry entry, Player? player, GameVersion gameVersion)
		{
			RowClass = player?.IsBanned == true ? "table-danger" : string.Empty;

			Dagger dagger = GameInfo.GetDaggerFromTime(entry.Time);
			DaggerCssClass = $"text-{dagger.Name.ToLower(CultureInfo.InvariantCulture)}";

			Death? death = GameInfo.GetDeathByType(entry.DeathType, gameVersion);
			DeathName = death?.Name ?? "Unknown";
			DeathHexColor = $"#{death?.ColorCode ?? "444"}";

			FlagCode = player?.CountryCode ?? string.Empty;

			Rank = entry.Rank;
			Username = entry.Username;
			Time = entry.Time;
			Kills = entry.Kills;
			Gems = entry.Gems;
			DaggersHit = entry.DaggersHit;
			DaggersFired = entry.DaggersFired;
			DaggerStatistics = FormatUtils.FormatDaggersInt32(entry.DaggersHit, entry.DaggersFired, false);
			Accuracy = entry.Accuracy;

			TotalTime = entry.TimeTotal;
			TotalKills = entry.KillsTotal;
			TotalGems = entry.GemsTotal;
			TotalDeaths = entry.DeathsTotal;
			TotalDaggersHit = entry.DaggersHitTotal;
			TotalDaggersFired = entry.DaggersFiredTotal;
			TotalDaggerStatistics = FormatUtils.FormatDaggersUInt64(entry.DaggersHitTotal, entry.DaggersFiredTotal, false);
			TotalAccuracy = entry.AccuracyTotal;

			AverageTime = entry.DeathsTotal == 0 ? 0 : (entry.TimeTotal / entry.DeathsTotal);
			AverageKills = entry.DeathsTotal == 0 ? 0 : (entry.KillsTotal / (double)entry.DeathsTotal);
			AverageGems = entry.DeathsTotal == 0 ? 0 : (entry.GemsTotal / (double)entry.DeathsTotal);
			AverageDaggersHit = entry.DeathsTotal == 0 ? 0 : (entry.DaggersHitTotal / (double)entry.DeathsTotal);
			AverageDaggersFired = entry.DeathsTotal == 0 ? 0 : (entry.DaggersFiredTotal / (double)entry.DeathsTotal);
		}

		public string RowClass { get; }

		public string DaggerCssClass { get; }

		public string DeathName { get; }
		public string DeathHexColor { get; }

		public string FlagCode { get; }

		public int Rank { get; }
		public string Username { get; }
		public int Time { get; }
		public int Kills { get; }
		public int Gems { get; }
		public int DaggersHit { get; }
		public int DaggersFired { get; }
		public string DaggerStatistics { get; }
		public double Accuracy { get; }

		public ulong TotalTime { get; }
		public ulong TotalKills { get; }
		public ulong TotalGems { get; }
		public ulong TotalDeaths { get; }
		public ulong TotalDaggersHit { get; }
		public ulong TotalDaggersFired { get; }
		public string TotalDaggerStatistics { get; }
		public double TotalAccuracy { get; }

		public double AverageTime { get; }
		public double AverageKills { get; }
		public double AverageGems { get; }
		public double AverageDaggersHit { get; }
		public double AverageDaggersFired { get; }
	}
}