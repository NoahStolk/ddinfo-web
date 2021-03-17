using DevilDaggersCore.Spawnsets;
using DevilDaggersWebsite.Entities;
using DevilDaggersWebsite.Enumerators;
using DevilDaggersWebsite.Extensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
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
				.Where(cl => cl.Category != CustomLeaderboardCategory.Challenge)
				.Select(cl => cl.SpawnsetFileId)
				.ToList();
		}

		public List<Dto.SpawnsetFile> GetSpawnsets(string? authorFilter = null, string? nameFilter = null)
		{
			IEnumerable<SpawnsetFile> query = _dbContext.SpawnsetFiles.Include(sf => sf.Player);

			if (!string.IsNullOrWhiteSpace(authorFilter))
			{
				authorFilter = authorFilter.ToLower();
				query = query.Where(sf => sf.Player.PlayerName.ToLower().Contains(authorFilter));
			}

			if (!string.IsNullOrWhiteSpace(nameFilter))
			{
				nameFilter = nameFilter.ToLower();
				query = query.Where(sf => sf.Name.ToLower().Contains(nameFilter));
			}

			return query.Select(sf => Map(sf)).ToList();
		}

		private Dto.SpawnsetFile Map(SpawnsetFile spawnsetFile)
		{
			if (!Spawnset.TryGetSpawnsetData(File.ReadAllBytes(Path.Combine(_env.WebRootPath, "spawnsets", spawnsetFile.Name)), out SpawnsetData spawnsetData))
				throw new($"Failed to get spawn data from spawnset file: '{spawnsetFile.Name}'.");

			return spawnsetFile.ToDto(spawnsetData, _spawnsetsWithCustomLeaderboardIds.Contains(spawnsetFile.Id));
		}
	}
}
