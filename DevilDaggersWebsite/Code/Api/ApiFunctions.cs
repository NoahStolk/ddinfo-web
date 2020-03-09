using CoreBase.Services;
using DevilDaggersCore.CustomLeaderboards;
using DevilDaggersCore.Game;
using DevilDaggersCore.Leaderboards;
using DevilDaggersCore.Leaderboards.History;
using DevilDaggersCore.Spawnsets.Web;
using DevilDaggersCore.Tools.Website;
using DevilDaggersCore.Website;
using DevilDaggersWebsite.Code.Database;
using DevilDaggersWebsite.Code.Database.CustomLeaderboards;
using DevilDaggersWebsite.Code.Tasks;
using DevilDaggersWebsite.Code.Utils;
using DevilDaggersWebsite.Code.Utils.Web;
using NetBase.Utils;
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
		public static IEnumerable<CustomLeaderboardBase> GetCustomLeaderboards(ApplicationDbContext context)
		{
			foreach (CustomLeaderboard leaderboard in context.CustomLeaderboards)
				yield return new CustomLeaderboardBase(
					leaderboard.SpawnsetFileName,
					leaderboard.Bronze,
					leaderboard.Silver,
					leaderboard.Golden,
					leaderboard.Devil,
					leaderboard.Homing == 0 ? 0 :
					context.CustomEntries
						.Where(e => e.CustomLeaderboard == leaderboard)
						.Any(e => e.Time > leaderboard.Homing) ? leaderboard.Homing : -1,
					leaderboard.DateLastPlayed,
					leaderboard.DateCreated);
		}

		public static IEnumerable<Death> GetDeaths(int? deathType, string gameVersion)
		{
			if (!GameInfo.GameVersions.TryGetValue(gameVersion, out GameVersion version))
				version = GameInfo.GameVersions[GameInfo.DEFAULT_GAME_VERSION];

			if (deathType.HasValue)
			{
				yield return GameInfo.GetDeathFromDeathType(deathType.Value, version);
				yield break;
			}

			foreach (Death death in GameInfo.GetEntities<Death>(version))
				yield return death;
		}

		public static IEnumerable<Enemy> GetEnemies(string enemyName, string gameVersion)
		{
			if (!GameInfo.GameVersions.TryGetValue(gameVersion, out GameVersion version))
				version = GameInfo.GameVersions[GameInfo.DEFAULT_GAME_VERSION];

			if (!string.IsNullOrEmpty(enemyName))
			{
				yield return GameInfo.GetEntities<Enemy>(version).FirstOrDefault(e => e.Name == enemyName);
				yield break;
			}

			foreach (Enemy enemy in GameInfo.GetEntities<Enemy>(version))
				yield return enemy;
		}

		public static Dictionary<string, GameVersion> GetGameVersions()
		{
			return GameInfo.GameVersions;
		}

		public static async Task<Leaderboard> GetLeaderboard(int rank)
		{
			return await Hasmodai.GetScores(Math.Max(1, rank));
		}

		public static bool TryGetSpawnsetPath(ICommonObjects commonObjects, string fileName, out string path)
		{
			if (!string.IsNullOrEmpty(fileName) && File.Exists(Path.Combine(commonObjects.Env.WebRootPath, "spawnsets", fileName)))
			{
				path = Path.Combine("spawnsets", fileName);
				return true;
			}

			path = "";
			return false;
		}

		public static IEnumerable<SpawnsetFile> GetSpawnsets(ICommonObjects commonObjects, string searchAuthor, string searchName)
		{
			foreach (string spawnsetPath in Directory.GetFiles(Path.Combine(commonObjects.Env.WebRootPath, "spawnsets")))
			{
				SpawnsetFile sf = SpawnsetUtils.CreateSpawnsetFileFromSettingsFile(commonObjects, spawnsetPath);
				if (!string.IsNullOrEmpty(searchAuthor) && !sf.Author.ToLower().Contains(searchAuthor.ToLower()) ||
					!string.IsNullOrEmpty(searchName) && !sf.Name.ToLower().Contains(searchName.ToLower()))
					continue;
				if (sf != null)
					yield return sf;
			}
		}

		public static bool TryGetToolPath(string toolName, out string fileName, out string path)
		{
			Tool tool = ToolList.Tools.FirstOrDefault(t => t.Name == toolName);
			if (tool != null)
			{
				fileName = $"{toolName}{tool.VersionNumber}.zip";
				path = Path.Combine("tools", toolName, fileName);

				// TODO: Add File.Exists in case the .zip file name for the tool is misspelled or something.

				return true;
			}

			fileName = "";
			path = "";
			return false;
		}

		public static List<Tool> GetTools()
		{
			return ToolList.Tools;
		}

		public static async Task<Entry> GetUserById(int userId)
		{
			return await Hasmodai.GetUserById(userId);
		}

		public static async Task<List<Entry>> GetUserByUsername(string username)
		{
			Leaderboard leaderboard = await Hasmodai.GetUserSearch(username);

			return leaderboard.Entries;
		}

		public static SortedDictionary<DateTime, Entry> GetUserProgressionById(ICommonObjects commonObjects, int userId)
		{
			SortedDictionary<DateTime, Entry> data = new SortedDictionary<DateTime, Entry>();

			if (userId != 0)
			{
				foreach (string leaderboardHistoryPath in Directory.GetFiles(Path.Combine(commonObjects.Env.WebRootPath, "leaderboard-history"), "*.json"))
				{
					Leaderboard leaderboard = JsonConvert.DeserializeObject<Leaderboard>(FileUtils.GetContents(leaderboardHistoryPath, Encoding.UTF8));
					Entry entry = leaderboard.Entries.FirstOrDefault(e => e.ID == userId);

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

		public static List<WorldRecord> GetWorldRecords(ICommonObjects commonObjects, DateTime? date)
		{
			bool isDateParameterValid = date.HasValue && date >= GameInfo.GameVersions["V1"].ReleaseDate && date <= DateTime.Now;

			List<WorldRecord> data = new List<WorldRecord>();

			int worldRecord = 0;
			foreach (string leaderboardHistoryPath in Directory.GetFiles(Path.Combine(commonObjects.Env.WebRootPath, "leaderboard-history"), "*.json"))
			{
				Leaderboard leaderboard = JsonConvert.DeserializeObject<Leaderboard>(FileUtils.GetContents(leaderboardHistoryPath, Encoding.UTF8));
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

		// TODO
		public static (DateTime, DateTime) GetLatestDatePlayed(ICommonObjects commonObjects, int userID)
		{
			List<(DateTime dateTime, Entry entry)> entries = new List<(DateTime, Entry)>();
			foreach (string leaderboardHistoryPath in Directory.GetFiles(Path.Combine(commonObjects.Env.WebRootPath, "leaderboard-history"), "*.json"))
			{
				Leaderboard lb = JsonConvert.DeserializeObject<Leaderboard>(FileUtils.GetContents(leaderboardHistoryPath, Encoding.UTF8));
				Entry entry = lb.Entries.Where(e => e.ID == userID).FirstOrDefault();
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

		public static WebStatsResult GetWebStats()
		{
			List<TaskResult> taskResults = new List<TaskResult>();
			foreach (KeyValuePair<Type, AbstractTask> kvp in TaskInstanceKeeper.Instances)
				taskResults.Add(new TaskResult(kvp.Key.Name, kvp.Value.LastTriggered, kvp.Value.LastFinished, kvp.Value.ExecutionTime, kvp.Value.Schedule));
			return new WebStatsResult(File.GetLastWriteTime(Assembly.GetExecutingAssembly().Location), taskResults);
		}
	}
}