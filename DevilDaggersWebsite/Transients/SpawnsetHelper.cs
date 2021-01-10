using DevilDaggersCore.Spawnsets;
using DevilDaggersWebsite.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace DevilDaggersWebsite.Transients
{
	public class SpawnsetHelper
	{
		private readonly IWebHostEnvironment _env;
		private readonly ApplicationDbContext _dbContext;

		private readonly List<int> _spawnsetsWithCustomLeaderboardIds;

		public SpawnsetHelper(IWebHostEnvironment env, ApplicationDbContext dbContext)
		{
			_env = env;
			_dbContext = dbContext;

			_spawnsetsWithCustomLeaderboardIds = dbContext.CustomLeaderboards.Select(cl => cl.SpawnsetFileId).ToList();
		}

		public List<Dto.SpawnsetFile> GetSpawnsets(string? authorFilter = null, string? nameFilter = null)
		{
			IEnumerable<SpawnsetFile> query = _dbContext.SpawnsetFiles.Include(sf => sf.Player);

			if (!string.IsNullOrWhiteSpace(authorFilter))
			{
				authorFilter = authorFilter.ToLower(CultureInfo.InvariantCulture);
				query = query.Where(sf => sf.Player.Username.ToLower(CultureInfo.InvariantCulture).Contains(authorFilter, StringComparison.InvariantCulture));
			}

			if (!string.IsNullOrWhiteSpace(nameFilter))
			{
				nameFilter = nameFilter.ToLower(CultureInfo.InvariantCulture);
				query = query.Where(sf => sf.Name.ToLower(CultureInfo.InvariantCulture).Contains(nameFilter, StringComparison.InvariantCulture));
			}

			return query.Select(sf => Map(sf)).ToList();
		}

		private Dto.SpawnsetFile Map(SpawnsetFile spawnsetFile)
		{
			if (!Spawnset.TryGetSpawnData(File.ReadAllBytes(Path.Combine(_env.WebRootPath, "spawnsets", spawnsetFile.Name)), out SpawnsetData spawnsetData))
				throw new Exception($"Failed to get spawn data from spawnset file: '{spawnsetFile.Name}'.");

			return new Dto.SpawnsetFile
			{
				AuthorName = spawnsetFile.Player.Username,
				HtmlDescription = spawnsetFile.HtmlDescription,
				HasCustomLeaderboard = _spawnsetsWithCustomLeaderboardIds.Contains(spawnsetFile.Id),
				LastUpdated = spawnsetFile.LastUpdated,
				MaxDisplayWaves = spawnsetFile.MaxDisplayWaves,
				Name = spawnsetFile.Name,
				SpawnsetData = spawnsetData,
			};
		}
	}
}