using System;
using System.IO;

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
	}
}
