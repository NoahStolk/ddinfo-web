using DevilDaggersCore.Spawnsets;
using DevilDaggersWebsite.Caches.SpawnsetData;
using DevilDaggersWebsite.Entities;
using DevilDaggersWebsite.Enumerators;
using DevilDaggersWebsite.Extensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
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

			_spawnsetsWithCustomLeaderboardIds = dbContext.CustomLeaderboards
				.Where(cl => cl.Category != CustomLeaderboardCategory.Challenge && !cl.IsArchived)
				.Select(cl => cl.SpawnsetFileId)
				.ToList();
		}

		public List<Dto.SpawnsetFile> GetSpawnsets(string? authorFilter = null, string? nameFilter = null)
		{
			IEnumerable<SpawnsetFile> query = _dbContext.SpawnsetFiles.Include(sf => sf.Player);

			if (!string.IsNullOrWhiteSpace(authorFilter))
				query = query.Where(sf => sf.Player.PlayerName.Contains(authorFilter, StringComparison.InvariantCultureIgnoreCase));

			if (!string.IsNullOrWhiteSpace(nameFilter))
				query = query.Where(sf => sf.Name.Contains(nameFilter, StringComparison.InvariantCultureIgnoreCase));

			return query
				.Where(sf => File.Exists(Path.Combine(_env.WebRootPath, "spawnsets", sf.Name)))
				.Select(sf => Map(sf))
				.ToList();

			Dto.SpawnsetFile Map(SpawnsetFile spawnsetFile)
			{
				SpawnsetData spawnsetData = SpawnsetDataCache.Instance.GetSpawnsetDataByFilePath(Path.Combine(_env.WebRootPath, "spawnsets", spawnsetFile.Name));
				return spawnsetFile.ToDto(spawnsetData, _spawnsetsWithCustomLeaderboardIds.Contains(spawnsetFile.Id));
			}
		}
	}
}
