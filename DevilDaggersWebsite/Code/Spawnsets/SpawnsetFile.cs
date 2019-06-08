using CoreBase.Services;
using DevilDaggersCore.Spawnset;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace DevilDaggersWebsite.Code.Spawnsets
{
	[JsonObject(MemberSerialization.OptIn)]
	public class SpawnsetFile
	{
		public string Path { get; private set; }

		public string FileName => System.IO.Path.GetFileName(Path);

		[JsonProperty]
		public string Name => GetName(FileName);
		[JsonProperty]
		public string Author => GetAuthor(FileName);
		[JsonProperty]
		public SpawnsetFileSettings settings;
		[JsonProperty]
		public SpawnsetData spawnsetData;

		public SpawnsetFile(ICommonObjects commonObjects, string path)
		{
			Path = path;

			using (StreamReader sr = new StreamReader(System.IO.Path.Combine(commonObjects.Env.WebRootPath, "spawnsets", "Settings", "Settings.json")))
			{
				Dictionary<string, SpawnsetFileSettings> dict = JsonConvert.DeserializeObject<Dictionary<string, SpawnsetFileSettings>>(sr.ReadToEnd());

				if (!dict.TryGetValue(FileName, out settings))
					settings = new SpawnsetFileSettings();
			}

			using (FileStream fs = new FileStream(Path, FileMode.Open, FileAccess.Read))
			{
				if (!Spawnset.TryGetSpawnData(fs, out spawnsetData))
					throw new Exception($"Could not retrieve spawnset data for spawnset '{FileName}'.");
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