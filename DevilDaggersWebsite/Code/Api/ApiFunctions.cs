using DevilDaggersCore.CustomLeaderboards;
using DevilDaggersCore.Game;
using DevilDaggersCore.Leaderboards;
using DevilDaggersCore.Leaderboards.History;
using DevilDaggersCore.Spawnsets.Web;
using DevilDaggersCore.Utils;
using DevilDaggersCore.Website;
using DevilDaggersWebsite.Code.Database;
using DevilDaggersWebsite.Code.External;
using DevilDaggersWebsite.Code.Tasks;
using DevilDaggersWebsite.Code.Utils;
using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DevilDaggersWebsite.Code.Api
{
	public static class ApiFunctions
	{
		public static List<CustomLeaderboardBase> GetCustomLeaderboards(ApplicationDbContext context)
			=> context.CustomLeaderboards.Select(cl => new CustomLeaderboardBase(cl.SpawnsetFileName, cl.Bronze, cl.Silver, cl.Golden, cl.Devil, cl.Homing, cl.DateLastPlayed, cl.DateCreated)).ToList();

		public static List<Death> GetDeaths(string gameVersion)
		{
			List<Death> deaths = new List<Death>();
			if (!GameInfo.GameVersions.TryGetValue(gameVersion, out GameVersion version))
				version = GameInfo.GameVersions[GameInfo.DefaultGameVersion];

			return GameInfo.GetEntities<Death>(version));
		}

		public static IEnumerable<Enemy> GetEnemies(string enemyName, string gameVersion)
		{
			if (!GameInfo.GameVersions.TryGetValue(gameVersion, out GameVersion version))
				version = GameInfo.GameVersions[GameInfo.DefaultGameVersion];

			if (!string.IsNullOrEmpty(enemyName))
			{
				yield return GameInfo.GetEntities<Enemy>(version).FirstOrDefault(e => e.Name == enemyName);
				yield break;
			}

			foreach (Enemy enemy in GameInfo.GetEntities<Enemy>(version))
				yield return enemy;
		}

		public static Dictionary<string, GameVersion> GetGameVersions() => GameInfo.GameVersions;

		public static async Task<Leaderboard> GetLeaderboard(int rank) => await Hasmodai.GetScores(Math.Max(1, rank));

		public static bool TryGetSpawnsetPath(IWebHostEnvironment env, string fileName, out string path)
		{
			if (!string.IsNullOrEmpty(fileName) && File.Exists(Path.Combine(env.WebRootPath, "spawnsets", fileName)))
			{
				path = Path.Combine("spawnsets", fileName);
				return true;
			}

			path = string.Empty;
			return false;
		}

		public static IEnumerable<SpawnsetFile> GetSpawnsets(IWebHostEnvironment env, string searchAuthor, string searchName)
		{
			IEnumerable<SpawnsetFile> spawnsetFiles = Directory.GetFiles(Path.Combine(env.WebRootPath, "spawnsets")).Select(p => SpawnsetUtils.CreateSpawnsetFileFromSettingsFile(env, p));

			if (!string.IsNullOrEmpty(searchAuthor))
			{
				searchAuthor = searchAuthor.ToLower();
				spawnsetFiles = spawnsetFiles.Where(sf => sf.Author.ToLower().Contains(searchAuthor));
			}
			if (!string.IsNullOrEmpty(searchName))
			{
				searchName = searchName.ToLower();
				spawnsetFiles = spawnsetFiles.Where(sf => sf.Name.ToLower().Contains(searchName));
			}

			return spawnsetFiles;
		}

		public static bool TryGetToolPath(IWebHostEnvironment env, string toolName, out string fileName, out string path)
		{
			Tool tool = ToolList.Tools.FirstOrDefault(t => t.Name == toolName);
			if (tool != null)
			{
				fileName = $"{toolName}{tool.VersionNumber}.zip";
				path = Path.Combine("tools", toolName, fileName);

				if (!File.Exists(Path.Combine(env.WebRootPath, path)))
					throw new Exception($"Tool file '{path}' does not exist.");

				return true;
			}

			fileName = "";
			path = "";
			return false;
		}

		public static List<Tool> GetTools() => ToolList.Tools;

		public static async Task<Entry> GetUserById(int userId)
			=> await Hasmodai.GetUserById(userId);

		public static async Task<Entry> GetUserByRank(int rank)
		{
			List<Entry> entries = (await Hasmodai.GetScores(rank)).Entries;
			if (entries.Count == 0)
				return new Entry();
			return entries[0];
		}

		public static async Task<List<Entry>> GetUserByUsername(string username)
			=> (await Hasmodai.GetUserSearch(username)).Entries;

		public static SortedDictionary<DateTime, Entry> GetUserProgressionById(IWebHostEnvironment env, int userId)
		{
			SortedDictionary<DateTime, Entry> data = new SortedDictionary<DateTime, Entry>();

			if (userId != 0)
			{
				foreach (string leaderboardHistoryPath in Directory.GetFiles(Path.Combine(env.WebRootPath, "leaderboard-history"), "*.json"))
				{
					Leaderboard leaderboard = JsonConvert.DeserializeObject<Leaderboard>(File.ReadAllText(leaderboardHistoryPath, Encoding.UTF8));
					Entry entry = leaderboard.Entries.FirstOrDefault(e => e.Id == userId);

					if (entry != null && !data.Values.Any(e =>
						e.Time == entry.Time ||
						e.Time == entry.Time + 1 ||
						e.Time == entry.Time - 1)) // Off-by-one errors in the history
					{
						data[leaderboard.DateTime] = entry;
					}
				}
			}

			return data;
		}

		public static List<WorldRecord> GetWorldRecords(IWebHostEnvironment env, DateTime? date)
		{
			bool isDateParameterValid = date.HasValue && date >= GameInfo.GameVersions["V1"].ReleaseDate && date <= DateTime.Now;

			List<WorldRecord> data = new List<WorldRecord>();

			int worldRecord = 0;
			foreach (string leaderboardHistoryPath in Directory.GetFiles(Path.Combine(env.WebRootPath, "leaderboard-history"), "*.json"))
			{
				Leaderboard leaderboard = JsonConvert.DeserializeObject<Leaderboard>(File.ReadAllText(leaderboardHistoryPath, Encoding.UTF8));
				if (leaderboard.Entries[0].Time != worldRecord)
				{
					worldRecord = leaderboard.Entries[0].Time;
					if (isDateParameterValid)
					{
						if (HistoryUtils.HistoryJsonFileNameToDateTime(Path.GetFileNameWithoutExtension(leaderboardHistoryPath)) > date)
							break;
						data.Clear();
					}

					if (leaderboard.DateTime >= GameInfo.GameVersions["V1"].ReleaseDate)
						data.Add(new WorldRecord(leaderboard.DateTime, leaderboard.Entries[0]));
				}
			}

			return data;
		}

		public static (DateTime from, DateTime to) GetLatestDatePlayed(IWebHostEnvironment env, int userId)
		{
			List<(DateTime dateTime, Entry entry)> entries = new List<(DateTime, Entry)>();
			foreach (string leaderboardHistoryPath in Directory.GetFiles(Path.Combine(env.WebRootPath, "leaderboard-history"), "*.json"))
			{
				Leaderboard lb = JsonConvert.DeserializeObject<Leaderboard>(File.ReadAllText(leaderboardHistoryPath, Encoding.UTF8));
				Entry entry = lb.Entries.FirstOrDefault(e => e.Id == userId);
				if (entry != null)
					entries.Add((lb.DateTime, entry));
			}

			entries = entries.OrderByDescending(l => l.dateTime).ToList();
			ulong deaths = entries[0].entry.DeathsTotal;
			for (int i = 1; i < entries.Count; i++)
				if (entries[i].entry.DeathsTotal < deaths)
					return (entries[i].dateTime, entries[i - 1].dateTime);

			return (DateTime.Now, DateTime.Now);
		}

		public static Dictionary<DateTime, ulong> GetUserActivity(IWebHostEnvironment env, int userId)
		{
			Dictionary<DateTime, ulong> data = new Dictionary<DateTime, ulong>();
			foreach (string leaderboardHistoryPath in Directory.GetFiles(Path.Combine(env.WebRootPath, "leaderboard-history"), "*.json"))
			{
				Leaderboard lb = JsonConvert.DeserializeObject<Leaderboard>(File.ReadAllText(leaderboardHistoryPath, Encoding.UTF8));
				Entry entry = lb.Entries.FirstOrDefault(e => e.Id == userId);
				if (entry != null && entry.DeathsTotal > 0)
					data.Add(lb.DateTime, entry.DeathsTotal);
			}
			return data;
		}

		public static WebStatsResult GetWebStats()
		{
			List<TaskResult> taskResults = new List<TaskResult>();
			foreach (KeyValuePair<Type, AbstractTask> kvp in TaskInstanceKeeper.Instances)
				taskResults.Add(new TaskResult(kvp.Key.Name, kvp.Value.LastTriggered, kvp.Value.LastFinished, kvp.Value.ExecutionTime, kvp.Value.Schedule));
			return new WebStatsResult(File.GetLastWriteTime(Assembly.GetExecutingAssembly().Location), taskResults);
		}
	}
}