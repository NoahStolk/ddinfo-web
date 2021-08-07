using System;
using System.IO;
using System.Linq;

namespace DevilDaggersWebsite.BlazorWasm.Server.Utils.Data
{
	public static class DataUtils
	{
		private const string _root = "Data";

		static DataUtils()
		{
			foreach (DataSubDirectory e in (DataSubDirectory[])Enum.GetValues(typeof(DataSubDirectory)))
				Directory.CreateDirectory(GetPath(e));
		}

		public static string[] GetLeaderboardHistoryPaths()
			=> TryGetFiles(DataSubDirectory.LeaderboardHistory);

		public static string[] GetLeaderboardStatisticsPaths()
			=> TryGetFiles(DataSubDirectory.LeaderboardStatistics);

		private static string[] TryGetFiles(DataSubDirectory subDirectory)
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

		public static string GetPath(DataSubDirectory subDirectory)
			=> Path.Combine(_root, subDirectory.ToString());

		public static string GetRelevantDisplayPath(string path)
		{
			char sep = Path.DirectorySeparatorChar;
			string rootIndicator = $"{sep}{_root}{sep}";
			if (!path.Contains(rootIndicator))
				return path;

			return path[path.IndexOf(rootIndicator)..];
		}
	}
}
