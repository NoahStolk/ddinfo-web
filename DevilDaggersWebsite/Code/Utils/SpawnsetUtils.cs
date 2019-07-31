using CoreBase.Services;
using DevilDaggersCore.Spawnsets;
using DevilDaggersCore.Spawnsets.Web;
using DevilDaggersWebsite.Code.Database;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DevilDaggersWebsite.Code.Utils
{
	public static class SpawnsetUtils
	{
		public const int DefaultMaxWaves = 28;
		public const int MaxSpawns = 2500;

		public static void SaveSpawnsetSettingsJsonFile(ApplicationDbContext context, ICommonObjects commonObjects)
		{
			Dictionary<string, SpawnsetFileSettings> dict = new Dictionary<string, SpawnsetFileSettings>();
			foreach (string path in Directory.GetFiles(Path.Combine(commonObjects.Env.WebRootPath, "spawnsets"), "*_*", SearchOption.TopDirectoryOnly))
			{
				string fileName = Path.GetFileName(path);
				SpawnsetFile sf = CreateSpawnsetFileFromSettingsFile(context, commonObjects, path);
				sf.settings.LastUpdated = File.GetLastWriteTime(path);

				dict[fileName] = sf.settings;
			}

			JsonSerializer serializer = JsonSerializer.Create(new JsonSerializerSettings()
			{
				DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate
			});
			using (StreamWriter sw = new StreamWriter(File.Create(@"C:\Users\NOAH\source\repos\DevilDaggersWebsite\DevilDaggersWebsite\wwwroot\spawnsets\Settings\Settings.json")))
			using (JsonTextWriter jtw = new JsonTextWriter(sw) { Formatting = Formatting.Indented, IndentChar = '\t', Indentation = 1 })
				serializer.Serialize(jtw, dict);
		}

		public static SpawnsetFile CreateSpawnsetFileFromSettingsFile(ApplicationDbContext context, ICommonObjects commonObjects, string path)
		{
			SpawnsetFile spawnsetFile = new SpawnsetFile
			{
				Path = path,
				HasLeaderboard = context.CustomLeaderboards.Any(l => l.SpawnsetFileName == Path.GetFileName(path))
			};

			using (StreamReader sr = new StreamReader(Path.Combine(commonObjects.Env.WebRootPath, "spawnsets", "Settings", "Settings.json")))
			{
				Dictionary<string, SpawnsetFileSettings> dict = JsonConvert.DeserializeObject<Dictionary<string, SpawnsetFileSettings>>(sr.ReadToEnd());

				if (!dict.TryGetValue(spawnsetFile.FileName, out spawnsetFile.settings))
					spawnsetFile.settings = new SpawnsetFileSettings();
			}

			using (FileStream fs = new FileStream(spawnsetFile.Path, FileMode.Open, FileAccess.Read))
			{
				if (!Spawnset.TryGetSpawnData(fs, out spawnsetFile.spawnsetData))
					throw new Exception($"Could not retrieve {nameof(SpawnsetData)} for spawnset '{spawnsetFile.FileName}'.");
			}

			return spawnsetFile;
		}
	}
}