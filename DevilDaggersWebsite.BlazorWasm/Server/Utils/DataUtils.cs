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
				return Directory.GetFiles(GetPath(subDirectory));
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

		public static string GetPath(string subDirectory)
			=> Path.Combine(_root, subDirectory);

		public static string GetRelevantDisplayPath(string path)
		{
			char sep = Path.DirectorySeparatorChar;
			return path[path.IndexOf($"{sep}{_root}{sep}")..];
		}
	}
}
