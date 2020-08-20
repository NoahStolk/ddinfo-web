using DevilDaggersCore.Spawnsets;
using DevilDaggersWebsite.Code.Database;
using DevilDaggersWebsite.Code.DataTransferObjects;
using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace DevilDaggersWebsite.Code.Transients
{
	public class SpawnsetHelper
	{
		private readonly IWebHostEnvironment env;
		private readonly ApplicationDbContext dbContext;

		public SpawnsetHelper(IWebHostEnvironment env, ApplicationDbContext dbContext)
		{
			this.dbContext = dbContext;
			this.env = env;
		}

		public SpawnsetFile? CreateSpawnsetFileFromSettingsFile(string path)
		{
			if (!File.Exists(path))
				return null;

			SpawnsetFile spawnsetFile = new SpawnsetFile
			{
				Path = path,
			};
			spawnsetFile.HasCustomLeaderboard = dbContext.CustomLeaderboards.Any(cl => cl.SpawnsetFileName == spawnsetFile.FileName);

			using (StreamReader sr = new StreamReader(Path.Combine(env.WebRootPath, "spawnsets", "Settings", "Settings.json")))
			{
				Dictionary<string, SpawnsetFileSettings> dict = JsonConvert.DeserializeObject<Dictionary<string, SpawnsetFileSettings>>(sr.ReadToEnd());

				if (!dict.TryGetValue(spawnsetFile.FileName, out spawnsetFile.settings))
					spawnsetFile.settings = new SpawnsetFileSettings();
			}

			if (!Spawnset.TryGetSpawnData(File.ReadAllBytes(spawnsetFile.Path), out spawnsetFile.spawnsetData))
				throw new Exception($"Could not retrieve {nameof(SpawnsetData)} for spawnset '{spawnsetFile.FileName}'.");

			return spawnsetFile;
		}

		public List<SpawnsetFile> GetSpawnsets(string? authorFilter = null, string? nameFilter = null)
		{
			IEnumerable<SpawnsetFile> spawnsetFiles = Directory.GetFiles(Path.Combine(env.WebRootPath, "spawnsets")).Select(p => CreateSpawnsetFileFromSettingsFile(p));

			if (!string.IsNullOrEmpty(authorFilter))
			{
				authorFilter = authorFilter.ToLower(CultureInfo.InvariantCulture);
				spawnsetFiles = spawnsetFiles.Where(sf => sf.Author.ToLower(CultureInfo.InvariantCulture).Contains(authorFilter, StringComparison.InvariantCulture));
			}

			if (!string.IsNullOrEmpty(nameFilter))
			{
				nameFilter = nameFilter.ToLower(CultureInfo.InvariantCulture);
				spawnsetFiles = spawnsetFiles.Where(sf => sf.Name.ToLower(CultureInfo.InvariantCulture).Contains(nameFilter, StringComparison.InvariantCulture));
			}

			return spawnsetFiles.ToList();
		}
	}
}