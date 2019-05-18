using CoreBase.Services;
using DevilDaggersWebsite.Models.Spawnsets;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace DevilDaggersWebsite.Utils
{
	public static class SpawnsetUtils
	{
		public static void SaveSpawnsetSettingsJsonFile(ICommonObjects commonObjects)
		{
			Dictionary<string, SpawnsetFileSettings> dict = new Dictionary<string, SpawnsetFileSettings>();
			foreach (string path in Directory.GetFiles(Path.Combine(commonObjects.Env.WebRootPath, "spawnsets"), "*_*", SearchOption.TopDirectoryOnly))
			{
				string fileName = Path.GetFileName(path);
				SpawnsetFile sf = new SpawnsetFile(commonObjects, path);
				sf.settings.LastUpdated = File.GetLastWriteTime(path);

				dict[fileName] = sf.settings;
			}

			JsonSerializer serializer = JsonSerializer.Create(new JsonSerializerSettings()
			{
				DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate
			});
			using (StreamWriter sw = new StreamWriter(File.Create(@"C:\Users\NOAH\source\repos\DevilDaggersWebsite\DevilDaggersWebsite\wwwroot\spawnsets\Settings/Settings.json")))
			using (JsonTextWriter jtw = new JsonTextWriter(sw) { Formatting = Formatting.Indented, IndentChar = '\t', Indentation = 1 })
				serializer.Serialize(jtw, dict);
		}
	}
}