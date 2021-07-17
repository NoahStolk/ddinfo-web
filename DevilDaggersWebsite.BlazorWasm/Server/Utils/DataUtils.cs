using System;
using System.IO;
using System.Linq;

namespace DevilDaggersWebsite.BlazorWasm.Server.Utils
{
	public static class DataUtils
	{
		private const string _root = "Data";

		public static string[] GetLeaderboardHistoryPaths()
			=> TryGetFiles("LeaderboardHistory");

		public static string[] GetLeaderboardStatisticsPaths()
			=> TryGetFiles("LeaderboardStatistics");

		private static string[] TryGetFiles(string subDirectory)
		{
			try
			{
				return Directory.GetFiles(Path.Combine(_root, subDirectory));
			}
			catch
			{
				return Array.Empty<string>();
			}
		}

		public static string GetLeaderboardHistoryPathFromDate(DateTime dateTime)
		{
			string[] paths = GetLeaderboardHistoryPaths();
			foreach (string path in paths.OrderByDescending(p => p))
			{
				if (HistoryUtils.HistoryJsonFileNameToDateTime(Path.GetFileName(path)) < dateTime)
					return path;
			}

			return paths[^1];
		}
	}
}
