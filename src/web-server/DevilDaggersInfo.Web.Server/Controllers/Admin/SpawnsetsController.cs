using DevilDaggersInfo.Api.Admin;
using DevilDaggersInfo.Api.Admin.Spawnsets;
using DevilDaggersInfo.Web.Client;
using DevilDaggersInfo.Web.Core.Claims;
using DevilDaggersInfo.Web.Server.Converters.DomainToApi.Admin;
using DevilDaggersInfo.Web.Server.Domain.Extensions;
using DevilDaggersInfo.Web.Server.Domain.Models.FileSystem;
using DevilDaggersInfo.Web.Server.Domain.Models.Spawnsets;
using DevilDaggersInfo.Web.Server.Domain.Services;
using Microsoft.AspNetCore.Authorization;

namespace DevilDaggersInfo.Web.Server.Controllers.Admin;

[Route("api/admin/spawnsets")]
[ApiController]
public class SpawnsetsController : ControllerBase
{
	private readonly ApplicationDbContext _dbContext;
	private readonly IFileSystemService _fileSystemService;
	private readonly SpawnsetHashCache _spawnsetHashCache;
	private readonly IAuditLogger _auditLogger;
	private readonly SpawnsetService _spawnsetService;

	public SpawnsetsController(ApplicationDbContext dbContext, IFileSystemService fileSystemService, SpawnsetHashCache spawnsetHashCache, IAuditLogger auditLogger, SpawnsetService spawnsetService)
	{
		_dbContext = dbContext;
		_fileSystemService = fileSystemService;
		_spawnsetHashCache = spawnsetHashCache;
		_auditLogger = auditLogger;
		_spawnsetService = spawnsetService;
	}

	[HttpGet]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[Authorize(Roles = Roles.Spawnsets)]
	public ActionResult<Page<GetSpawnsetForOverview>> GetSpawnsets(
		[Range(0, 1000)] int pageIndex = 0,
		[Range(Constants.PageSizeMin, Constants.PageSizeMax)] int pageSize = Constants.PageSizeDefault,
		SpawnsetSorting? sortBy = null,
		bool ascending = false)
	{
		IQueryable<SpawnsetEntity> spawnsetsQuery = _dbContext.Spawnsets.AsNoTracking().Include(s => s.Player);

		spawnsetsQuery = sortBy switch
		{
			SpawnsetSorting.Author => spawnsetsQuery.OrderBy(s => s.Player.PlayerName, ascending).ThenBy(s => s.Id),
			SpawnsetSorting.HtmlDescription => spawnsetsQuery.OrderBy(s => s.HtmlDescription, ascending).ThenBy(s => s.Id),
			SpawnsetSorting.IsPractice => spawnsetsQuery.OrderBy(s => s.IsPractice, ascending).ThenBy(s => s.Id),
			SpawnsetSorting.LastUpdated => spawnsetsQuery.OrderBy(s => s.LastUpdated, ascending).ThenBy(s => s.Id),
			SpawnsetSorting.MaxDisplayWaves => spawnsetsQuery.OrderBy(s => s.MaxDisplayWaves, ascending).ThenBy(s => s.Id),
			SpawnsetSorting.Name => spawnsetsQuery.OrderBy(s => s.Name, ascending).ThenBy(s => s.Id),
			_ => spawnsetsQuery.OrderBy(s => s.Id, ascending),
		};

		List<SpawnsetEntity> spawnsets = spawnsetsQuery
			.Skip(pageIndex * pageSize)
			.Take(pageSize)
			.ToList();

		return new Page<GetSpawnsetForOverview>
		{
			Results = spawnsets.ConvertAll(s => s.ToGetSpawnsetForOverview()),
			TotalResults = _dbContext.Spawnsets.Count(),
		};
	}

	[HttpGet("names")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[Authorize(Roles = Roles.CustomLeaderboards)]
	public ActionResult<List<GetSpawnsetName>> GetSpawnsetNames()
	{
		var spawnsets = _dbContext.Spawnsets
			.AsNoTracking()
			.Select(s => new { s.Id, s.Name })
			.ToList();

		return spawnsets.ConvertAll(s => new GetSpawnsetName
		{
			Id = s.Id,
			Name = s.Name,
		});
	}

	[HttpGet("{id}")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	[Authorize(Roles = Roles.Spawnsets)]
	public ActionResult<GetSpawnset> GetSpawnsetById(int id)
	{
		SpawnsetEntity? spawnset = _dbContext.Spawnsets
			.AsNoTracking()
			.FirstOrDefault(p => p.Id == id);
		if (spawnset == null)
			return NotFound();

		return spawnset.ToGetSpawnset();
	}

	[HttpPost]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[Authorize(Roles = Roles.Spawnsets)]
	public async Task<ActionResult> AddSpawnset(AddSpawnset addSpawnset)
	{
		_spawnsetService.ValidateName(addSpawnset.Name);

		if (!SpawnsetBinary.TryParse(addSpawnset.FileContents, out _))
			return BadRequest("File could not be parsed to a proper survival file.");

		byte[] spawnsetHash = MD5.HashData(addSpawnset.FileContents);
		SpawnsetHashCacheData? existingSpawnset = _spawnsetHashCache.GetSpawnset(spawnsetHash);
		if (existingSpawnset != null)
			return BadRequest($"Spawnset is exactly the same as an already existing spawnset named '{existingSpawnset.Name}'.");

		// Entity validation.
		if (!_dbContext.Players.Any(p => p.Id == addSpawnset.PlayerId))
			return BadRequest($"Player with ID '{addSpawnset.PlayerId}' does not exist.");

		if (_dbContext.Spawnsets.Any(m => m.Name == addSpawnset.Name))
			return BadRequest($"Spawnset with name '{addSpawnset.Name}' already exists.");

		// Add file.
		string path = Path.Combine(_fileSystemService.GetPath(DataSubDirectory.Spawnsets), addSpawnset.Name);
		IoFile.WriteAllBytes(path, addSpawnset.FileContents);

		// Add entity.
		SpawnsetEntity spawnset = new()
		{
			HtmlDescription = addSpawnset.HtmlDescription,
			IsPractice = addSpawnset.IsPractice,
			MaxDisplayWaves = addSpawnset.MaxDisplayWaves,
			Name = addSpawnset.Name,
			PlayerId = addSpawnset.PlayerId,
			LastUpdated = DateTime.UtcNow,
		};
		_dbContext.Spawnsets.Add(spawnset);
		await _dbContext.SaveChangesAsync();

		_auditLogger.LogAdd(addSpawnset.GetLog(), User, spawnset.Id, new() { new($"File {_fileSystemService.FormatPath(path)} was added.", FileSystemInformationType.Add) });

		return Ok(spawnset.Id);
	}

	[HttpPut("{id}")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	[Authorize(Roles = Roles.Spawnsets)]
	public async Task<ActionResult> EditSpawnsetById(int id, EditSpawnset editSpawnset)
	{
		_spawnsetService.ValidateName(editSpawnset.Name);

		if (!_dbContext.Players.Any(p => p.Id == editSpawnset.PlayerId))
			return BadRequest($"Player with ID '{editSpawnset.PlayerId}' does not exist.");

		SpawnsetEntity? spawnset = _dbContext.Spawnsets.FirstOrDefault(s => s.Id == id);
		if (spawnset == null)
			return NotFound();

		string? moveInfo = null;
		if (spawnset.Name != editSpawnset.Name)
		{
			if (_dbContext.Spawnsets.Any(m => m.Name == editSpawnset.Name))
				return BadRequest($"Spawnset with name '{editSpawnset.Name}' already exists.");

			string directory = _fileSystemService.GetPath(DataSubDirectory.Spawnsets);
			string oldPath = Path.Combine(directory, spawnset.Name);
			string newPath = Path.Combine(directory, editSpawnset.Name);
			IoFile.Move(oldPath, newPath);
			moveInfo = $"File {_fileSystemService.FormatPath(oldPath)} was moved to {_fileSystemService.FormatPath(newPath)}.";
		}

		EditSpawnset logDto = new()
		{
			HtmlDescription = spawnset.HtmlDescription,
			IsPractice = spawnset.IsPractice,
			MaxDisplayWaves = spawnset.MaxDisplayWaves,
			Name = spawnset.Name,
			PlayerId = spawnset.PlayerId,
		};

		// Do not update LastUpdated here. This value is based only on the file which cannot be edited.
		spawnset.HtmlDescription = editSpawnset.HtmlDescription;
		spawnset.IsPractice = editSpawnset.IsPractice;
		spawnset.MaxDisplayWaves = editSpawnset.MaxDisplayWaves;
		spawnset.Name = editSpawnset.Name;
		spawnset.PlayerId = editSpawnset.PlayerId;
		await _dbContext.SaveChangesAsync();

		_auditLogger.LogEdit(logDto.GetLog(), editSpawnset.GetLog(), User, spawnset.Id, moveInfo == null ? null : new() { new(moveInfo, FileSystemInformationType.Move) });

		return Ok();
	}

	[HttpDelete("{id}")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	[Authorize(Roles = Roles.Spawnsets)]
	public async Task<ActionResult> DeleteSpawnsetById(int id)
	{
		SpawnsetEntity? spawnset = _dbContext.Spawnsets.FirstOrDefault(s => s.Id == id);
		if (spawnset == null)
			return NotFound();

		if (_dbContext.CustomLeaderboards.Any(cl => cl.SpawnsetId == id))
			return BadRequest("Spawnset with custom leaderboard cannot be deleted.");

		string path = Path.Combine(_fileSystemService.GetPath(DataSubDirectory.Spawnsets), spawnset.Name);
		bool fileExists = IoFile.Exists(path);
		if (fileExists)
		{
			IoFile.Delete(path);
			_spawnsetHashCache.Clear();
		}

		_dbContext.Spawnsets.Remove(spawnset);
		await _dbContext.SaveChangesAsync();

		string message = fileExists ? $"File {_fileSystemService.FormatPath(path)} was deleted." : $"File {_fileSystemService.FormatPath(path)} was not deleted because it does not exist.";
		_auditLogger.LogDelete(spawnset.GetLog(), User, spawnset.Id, new() { new(message, fileExists ? FileSystemInformationType.Delete : FileSystemInformationType.NotFoundUnexpected) });

		return Ok();
	}
}
