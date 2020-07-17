using DevilDaggersCore.Leaderboards;
using DevilDaggersWebsite.Code.External;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace EntireLeaderboardThreadPoolTest
{
	public static class Program
	{
		private const int maxThreads = 64;

		public static Leaderboard Leaderboard { get; private set; }

		public static async Task Main()
		{
			Leaderboard = await Hasmodai.GetScores(1);

			Stopwatch stopwatch = new Stopwatch();
			stopwatch.Start();
			for (int i = 1; i < Leaderboard.Players / 100 + 1;)
			{
				SpawnAndWait(CreateActions(i), i);
				i += maxThreads;
			}
			stopwatch.Stop();

			Console.WriteLine($"Done! Execution took {stopwatch.Elapsed}. Entries fetched: {Leaderboard.Entries.Count} / {Leaderboard.Players}");
		}

		private static List<Action<int>> CreateActions(int page)
		{
			int start = page;
			int end = page + maxThreads;

			Console.WriteLine($"Creating {maxThreads} threads ({start} - {end - 1}).");

			List<Action<int>> actions = new List<Action<int>>();
			for (int i = start; i < end; i++)
				actions.Add(new Action<int>(async (i) => await GetEntries(i)));
			return actions;
		}

		private static async Task GetEntries(int page)
		{
			try
			{
				Console.WriteLine($"Page {page}");

				Leaderboard leaderboardPage = await Hasmodai.GetScores(page * 100 + 1);

				foreach (Entry entry in leaderboardPage.Entries)
					Leaderboard.Entries.Add(entry);

				Console.WriteLine($"Thread leaderboard page {page} (entries {page * 100 + 1} - {page * 100 + 101}) finished.");
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Thread leaderboard page {page} (entries {page * 100 + 1} - {page * 100 + 101}) failed with Exception: {ex.Message}");
			}
		}

		private static void SpawnAndWait(List<Action<int>> actions, int page)
		{
			ManualResetEvent[] handles = new ManualResetEvent[actions.Count];

			for (int i = page; i < page + actions.Count; i++)
			{
				handles[i] = new ManualResetEvent(false);
				Action<int> currentAction = actions[i];
				ManualResetEvent currentHandle = handles[i];

				void wrappedAction() { try { currentAction(i); } finally { currentHandle.Set(); } }

				ThreadPool.QueueUserWorkItem(x => wrappedAction());
			}

			WaitHandle.WaitAll(handles);
		}
	}
}