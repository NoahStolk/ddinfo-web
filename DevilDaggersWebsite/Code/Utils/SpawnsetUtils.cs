using DevilDaggersCore.Spawnsets;
using DevilDaggersCore.Spawnsets.Web;
using Microsoft.AspNetCore.Hosting;
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

		public static SpawnsetFile CreateSpawnsetFileFromSettingsFile(IWebHostEnvironment env, string path)
		{
			if (!File.Exists(path))
				return null;

			SpawnsetFile spawnsetFile = new SpawnsetFile
			{
				Path = path
			};

			using (StreamReader sr = new StreamReader(Path.Combine(env.WebRootPath, "spawnsets", "Settings", "Settings.json")))
			{
				Dictionary<string, SpawnsetFileSettings> dict = JsonConvert.DeserializeObject<Dictionary<string, SpawnsetFileSettings>>(sr.ReadToEnd());

				if (!dict.TryGetValue(spawnsetFile.FileName, out spawnsetFile.settings))
					spawnsetFile.settings = new SpawnsetFileSettings();
			}

			using (FileStream fs = new FileStream(spawnsetFile.Path, FileMode.Open, FileAccess.Read))
				if (!Spawnset.TryGetSpawnData(fs, out spawnsetFile.spawnsetData))
					throw new Exception($"Could not retrieve {nameof(SpawnsetData)} for spawnset '{spawnsetFile.FileName}'.");

			return spawnsetFile;
		}

		public static List<SpawnsetFile> GetSpawnsets(IWebHostEnvironment env, string searchAuthor, string searchName)
		{
			IEnumerable<SpawnsetFile> spawnsetFiles = Directory.GetFiles(Path.Combine(env.WebRootPath, "spawnsets")).Select(p => CreateSpawnsetFileFromSettingsFile(env, p));

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

			return spawnsetFiles.ToList();
		}
	}
}