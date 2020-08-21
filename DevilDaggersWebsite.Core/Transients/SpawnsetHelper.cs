using DevilDaggersCore.Spawnsets;
using DevilDaggersWebsite.Core.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace DevilDaggersWebsite.Core.Transients
{
	public class SpawnsetHelper
	{
		private readonly IWebHostEnvironment env;
		private readonly ApplicationDbContext dbContext;

		private readonly List<int> spawnsetsWithCustomLeaderboardIds;

		public SpawnsetHelper(IWebHostEnvironment env, ApplicationDbContext dbContext)
		{
			this.env = env;
			this.dbContext = dbContext;

			spawnsetsWithCustomLeaderboardIds = dbContext.CustomLeaderboards.Select(cl => cl.SpawnsetFileId).ToList();
		}

		public List<Dto.SpawnsetFile> GetSpawnsets(string? authorFilter = null, string? nameFilter = null)
		{
			IEnumerable<SpawnsetFile> query = dbContext.SpawnsetFiles.Include(sf => sf.Player);

			if (!string.IsNullOrEmpty(authorFilter))
			{
				authorFilter = authorFilter.ToLower(CultureInfo.InvariantCulture);
				query = query.Where(sf => sf.Player.Username.ToLower(CultureInfo.InvariantCulture).Contains(authorFilter, StringComparison.InvariantCulture));
			}

			if (!string.IsNullOrEmpty(nameFilter))
			{
				nameFilter = nameFilter.ToLower(CultureInfo.InvariantCulture);
				query = query.Where(sf => sf.Name.ToLower(CultureInfo.InvariantCulture).Contains(nameFilter, StringComparison.InvariantCulture));
			}

			return query.Select(sf => Map(sf)).ToList();
		}

		private Dto.SpawnsetFile Map(SpawnsetFile spawnsetFile)
		{
			if (!Spawnset.TryGetSpawnData(File.ReadAllBytes(Path.Combine(env.WebRootPath, "spawnsets", spawnsetFile.Name)), out SpawnsetData spawnsetData))
				throw new Exception($"Failed to get spawn data from spawnset file: '{spawnsetFile.Name}'.");

			return new Dto.SpawnsetFile
			{
				AuthorName = spawnsetFile.Player.Username,
				HtmlDescription = spawnsetFile.HtmlDescription,
				HasCustomLeaderboard = spawnsetsWithCustomLeaderboardIds.Contains(spawnsetFile.Id),
				LastUpdated = spawnsetFile.LastUpdated,
				MaxDisplayWaves = spawnsetFile.MaxDisplayWaves,
				Name = spawnsetFile.Name,
				SpawnsetData = spawnsetData,
			};
		}
	}
}