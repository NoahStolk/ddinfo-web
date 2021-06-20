using DevilDaggersDiscordBot;
using DevilDaggersDiscordBot.Logging;
using DevilDaggersWebsite.Dto;
using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace DevilDaggersWebsite.Caches.LeaderboardHistory
{
	public sealed class LeaderboardHistoryCache : IDynamicCache
	{
		private const string _emote = "yellow_circle";

		private readonly ConcurrentDictionary<string, Leaderboard> _cache = new();

		private static readonly Lazy<LeaderboardHistoryCache> _lazy = new(() => new());

		private LeaderboardHistoryCache()
		{
		}

		public static LeaderboardHistoryCache Instance => _lazy.Value;

		public Leaderboard GetLeaderboardHistoryByFilePath(string filePath)
		{
			string name = Path.GetFileNameWithoutExtension(filePath);
			if (_cache.ContainsKey(name))
				return _cache[name];

			Leaderboard lb = JsonConvert.DeserializeObject<Leaderboard?>(File.ReadAllText(filePath, Encoding.UTF8)) ?? throw new($"Corrupt leaderboard history file: {name}");
			_cache.TryAdd(name, lb);
			return lb;
		}

		public async Task Clear(IWebHostEnvironment env)
		{
			int cacheCount = _cache.Count;
			_cache.Clear();
			await DiscordLogger.Instance.TryLog(Channel.CacheMonitoring, env.EnvironmentName, $":{_emote}: Successfully cleared dynamic `{nameof(LeaderboardHistoryCache)}`. (Removed `{cacheCount}` instances.)");
		}

		public string LogState(IWebHostEnvironment env)
			=> $":{_emote}: `{nameof(LeaderboardHistoryCache)}` has `{_cache.Count}` instances in memory.";
	}
}
