using DevilDaggersCore.Spawnsets;
using DevilDaggersInfo.Web.BlazorWasm.Server.Caches.SpawnsetData;
using DevilDaggersInfo.Web.BlazorWasm.Server.Controllers.Attributes;
using DevilDaggersInfo.Web.BlazorWasm.Server.Converters.Public;
using DevilDaggersInfo.Web.BlazorWasm.Server.Entities;
using DevilDaggersInfo.Web.BlazorWasm.Server.Enums;
using DevilDaggersInfo.Web.BlazorWasm.Server.Transients;
using DevilDaggersInfo.Web.BlazorWasm.Shared.Dto.Public.Spawnsets;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Net.Mime;
using Io = System.IO;

namespace DevilDaggersInfo.Web.BlazorWasm.Server.Controllers.Public;

[Route("api/spawnsets")]
[ApiController]
public class SpawnsetsController : ControllerBase
{
	private readonly ApplicationDbContext _dbContext;
	private readonly IFileSystemService _fileSystemService;
	private readonly SpawnsetDataCache _spawnsetDataCache;

	public SpawnsetsController(ApplicationDbContext dbContext, IFileSystemService fileSystemService, SpawnsetDataCache spawnsetDataCache)
	{
		_dbContext = dbContext;
		_fileSystemService = fileSystemService;
		_spawnsetDataCache = spawnsetDataCache;
	}

	[HttpGet]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[EndpointConsumer(EndpointConsumers.Ddse)]
	public List<GetSpawnset> GetPublicSpawnsets(string? authorFilter = null, string? nameFilter = null)
	{
		List<int> spawnsetsWithCustomLeaderboardIds = _dbContext.CustomLeaderboards
			.AsNoTracking()
			.Where(cl => !cl.IsArchived)
			.Select(cl => cl.SpawnsetId)
			.ToList();

		IEnumerable<SpawnsetEntity> query = _dbContext.Spawnsets.AsNoTracking().Include(sf => sf.Player);

		if (!string.IsNullOrWhiteSpace(authorFilter))
			query = query.Where(sf => sf.Player.PlayerName.Contains(authorFilter, StringComparison.InvariantCultureIgnoreCase));

		if (!string.IsNullOrWhiteSpace(nameFilter))
			query = query.Where(sf => sf.Name.Contains(nameFilter, StringComparison.InvariantCultureIgnoreCase));

		return query
			.Where(sf => Io.File.Exists(Path.Combine(_fileSystemService.GetPath(DataSubDirectory.Spawnsets), sf.Name)))
			.Select(sf => Map(sf))
			.ToList();

		GetSpawnset Map(SpawnsetEntity spawnset)
		{
			SpawnsetData spawnsetData = _spawnsetDataCache.GetSpawnsetDataByFilePath(Path.Combine(_fileSystemService.GetPath(DataSubDirectory.Spawnsets), spawnset.Name));
			return spawnset.ToGetSpawnsetPublic(spawnsetData, spawnsetsWithCustomLeaderboardIds.Contains(spawnset.Id));
		}
	}

	[HttpGet("{fileName}/file")]
	[ProducesResponseType(typeof(FileContentResult), StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	[EndpointConsumer(EndpointConsumers.Ddse | EndpointConsumers.Website)]
	public ActionResult GetSpawnsetFile([Required] string fileName)
	{
		string path = Path.Combine(_fileSystemService.GetPath(DataSubDirectory.Spawnsets), fileName);
		if (!Io.File.Exists(path))
			return new NotFoundObjectResult(new ProblemDetails { Title = $"Spawnset '{fileName}' was not found." });

		return File(Io.File.ReadAllBytes(path), MediaTypeNames.Application.Octet, fileName);
	}
}
