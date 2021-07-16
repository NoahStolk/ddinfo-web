using System;
using System.IO;

namespace DevilDaggersWebsite.BlazorWasm.Server.Utils
{
	public static class DataUtils
	{
		private const string _root = "Data";

		public static string[] GetLeaderboardStatistics()
		{
			try
			{
				return Directory.GetFiles(Path.Combine(_root, "LeaderboardStatistics"));
			}
			catch
			{
				return Array.Empty<string>();
			}
		}
	}
}
