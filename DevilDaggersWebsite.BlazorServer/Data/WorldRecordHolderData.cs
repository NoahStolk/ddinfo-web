using DevilDaggersCore.Game;
using DevilDaggersWebsite.BlazorServer.Extensions;
using DevilDaggersWebsite.Dto;
using System;

namespace DevilDaggersWebsite.BlazorServer.Data
{
	public class WorldRecordHolderData
	{
		public WorldRecordHolderData(string mostRecentUsername, string totalTimeHeldString, string longestTimeHeldConsecutivelyString, int worldRecordCount, string firstHeldString, string lastHeldString)
		{
			MostRecentUsername = mostRecentUsername;
			TotalTimeHeldString = totalTimeHeldString;
			LongestTimeHeldConsecutivelyString = longestTimeHeldConsecutivelyString;
			WorldRecordCount = worldRecordCount;
			FirstHeldString = firstHeldString;
			LastHeldString = lastHeldString;
		}

		public string MostRecentUsername { get; }
		public string TotalTimeHeldString { get; }
		public string LongestTimeHeldConsecutivelyString { get; }
		public int WorldRecordCount { get; }
		public string FirstHeldString { get; }
		public string LastHeldString { get; }

		public static WorldRecordHolderData FromWorldRecordHolder(WorldRecordHolder wrh)
		{
			return new(wrh.MostRecentUsername, GetTotalTimeHeldString(wrh), GetLongestTimeHeldConsecutivelyString(wrh), wrh.WorldRecordCount, GetHistoryDateString(wrh.FirstHeld), GetHistoryDateString(wrh.LastHeld));

			string GetHistoryDateString(DateTime dateTime)
			{
				int daysAgo = (int)Math.Round((DateTime.UtcNow - dateTime).TotalDays);
				return $"{dateTime:MMM dd} '{dateTime:yy} ({daysAgo} day{daysAgo.Pluralize()} ago)";
			}

			string GetTotalTimeHeldString(WorldRecordHolder wrh)
			{
				TimeSpan total = DateTime.UtcNow - (GameInfo.GetReleaseDate(GameVersion.V1) ?? throw new("Could not retrieve release version from V1."));
				int days = (int)Math.Round(wrh.TotalTimeHeld.TotalDays);
				return $"{days} day{days.Pluralize()} ({wrh.TotalTimeHeld / total:00.00%})";
			}

			string GetLongestTimeHeldConsecutivelyString(WorldRecordHolder wrh)
			{
				int daysConsecutively = (int)Math.Round(wrh.LongestTimeHeldConsecutively.TotalDays);
				return $"{daysConsecutively} day{daysConsecutively.Pluralize()}";
			}

			//string GetLastHeldString(WorldRecordHolder wrh)
			//{
			//	bool isCurrentWr = wrh.LastHeld == _worldRecordHolders.Max(wrh => wrh.LastHeld);
			//	return isCurrentWr ? "Current holder" : GetHistoryDateString(wrh.LastHeld);
			//}
		}
	}
}
