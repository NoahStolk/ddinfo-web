using DevilDaggersCore.Spawnsets;
using DevilDaggersWebsite.BlazorWasm.Server.Caches.SpawnsetData;
using DevilDaggersWebsite.BlazorWasm.Server.Entities;
using DevilDaggersWebsite.BlazorWasm.Server.Extensions;
using DevilDaggersWebsite.Dto.Spawnsets;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DevilDaggersWebsite.BlazorWasm.Server.Transients
{
	public class SpawnsetHelper
	{
		private readonly IWebHostEnvironment _environment;
		private readonly ApplicationDbContext _dbContext;
		private readonly SpawnsetDataCache _spawnsetDataCache;

		private readonly List<int> _spawnsetsWithCustomLeaderboardIds;

		public SpawnsetHelper(IWebHostEnvironment environment, ApplicationDbContext dbContext, SpawnsetDataCache spawnsetDataCache)
		{
			_environment = environment;
			_dbContext = dbContext;
			_spawnsetDataCache = spawnsetDataCache;

			_spawnsetsWithCustomLeaderboardIds = dbContext.CustomLeaderboards
				.AsNoTracking()
				.Where(cl => !cl.IsArchived)
				.Select(cl => cl.SpawnsetFileId)
				.ToList();
		}

		public List<GetSpawnsetPublic> GetSpawnsets(string? authorFilter = null, string? nameFilter = null)
		{
			IEnumerable<SpawnsetFile> query = _dbContext.SpawnsetFiles.AsNoTracking().Include(sf => sf.Player);

			if (!string.IsNullOrWhiteSpace(authorFilter))
				query = query.Where(sf => sf.Player.PlayerName.Contains(authorFilter, StringComparison.InvariantCultureIgnoreCase));

			if (!string.IsNullOrWhiteSpace(nameFilter))
				query = query.Where(sf => sf.Name.Contains(nameFilter, StringComparison.InvariantCultureIgnoreCase));

			return query
				.Where(sf => File.Exists(Path.Combine(_environment.WebRootPath, "spawnsets", sf.Name)))
				.Select(sf => Map(sf))
				.ToList();

			GetSpawnsetPublic Map(SpawnsetFile spawnsetFile)
			{
				SpawnsetData spawnsetData = _spawnsetDataCache.GetSpawnsetDataByFilePath(Path.Combine(_environment.WebRootPath, "spawnsets", spawnsetFile.Name));
				return spawnsetFile.ToDto(spawnsetData, _spawnsetsWithCustomLeaderboardIds.Contains(spawnsetFile.Id));
			}
		}
	}
}
