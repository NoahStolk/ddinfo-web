using CoreBase.Services;
using DevilDaggersCore.Spawnset;
using Newtonsoft.Json;
using System;
using System.IO;

namespace DevilDaggersWebsite.Models.Spawnset
{
	[JsonObject(MemberSerialization.OptIn)]
	public class SpawnsetFile
	{
		public string Path { get; private set; }

		public string FileName => System.IO.Path.GetFileName(Path);

		public SpawnsetFileSettings Settings { get; private set; }

		[JsonProperty]
		public string Name => GetName(FileName);
		[JsonProperty]
		public string Author => GetAuthor(FileName);

		public SpawnsetFile(ICommonObjects commonObjects, string path)
		{
			Path = path;

			string settingsPath = System.IO.Path.Combine(commonObjects.Env.WebRootPath, "spawnsets", "Settings", $"{Name}.json");
			if (File.Exists(settingsPath))
				using (StreamReader sr = new StreamReader(System.IO.Path.Combine(commonObjects.Env.WebRootPath, "spawnsets", "Settings", $"{Name}.json")))
					Settings = JsonConvert.DeserializeObject<SpawnsetFileSettings>(sr.ReadToEnd());
			else
				Settings = new SpawnsetFileSettings();
		}

		public SpawnsetData GetSpawnsetData()
		{
			using (FileStream fs = new FileStream(Path, FileMode.Open, FileAccess.Read))
			{
				if (DevilDaggersCore.Spawnset.Spawnset.TryGetSpawnData(fs, out SpawnsetData spawnsetData))
					return spawnsetData;

				throw new Exception($"Could not retrieve spawnset data for spawnset: {FileName}");
			}
		}

		public static string GetName(string fileNameOrPath)
		{
			return fileNameOrPath.Substring(0, fileNameOrPath.LastIndexOf('_'));
		}

		public static string GetAuthor(string fileNameOrPath)
		{
			return fileNameOrPath.Substring(fileNameOrPath.LastIndexOf('_') + 1);
		}
	}
}