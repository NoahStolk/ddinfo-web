using DevilDaggersInfo.Web.BlazorWasm.Server.Converters.Admin;
using DevilDaggersInfo.Web.BlazorWasm.Server.InternalModels;
using DevilDaggersInfo.Web.BlazorWasm.Shared.Dto;
using DevilDaggersInfo.Web.BlazorWasm.Shared.Dto.Admin.Mods;
using DevilDaggersInfo.Web.BlazorWasm.Shared.Enums.Sortings.Admin;
using Microsoft.AspNetCore.Authorization;

namespace DevilDaggersInfo.Web.BlazorWasm.Server.Controllers.Admin;

[Route("api/admin/mods")]
[ApiController]
public class ModsController : ControllerBase
{
	private readonly ApplicationDbContext _dbContext;
	private readonly AuditLogger _auditLogger;
	private readonly ModArchiveAccessor _modArchiveAccessor;
	private readonly ModArchiveProcessor _modArchiveProcessor;
	private readonly ModScreenshotProcessor _modScreenshotProcessor;

	public ModsController(ApplicationDbContext dbContext, AuditLogger auditLogger, ModArchiveAccessor modArchiveAccessor, ModArchiveProcessor modArchiveProcessor, ModScreenshotProcessor modScreenshotProcessor)
	{
		_dbContext = dbContext;
		_auditLogger = auditLogger;
		_modArchiveAccessor = modArchiveAccessor;
		_modArchiveProcessor = modArchiveProcessor;
		_modScreenshotProcessor = modScreenshotProcessor;
	}

	[HttpGet]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[Authorize(Roles = Roles.Mods)]
	public ActionResult<Page<GetModForOverview>> GetMods(
		[Range(0, 1000)] int pageIndex = 0,
		[Range(PagingConstants.PageSizeMin, PagingConstants.PageSizeMax)] int pageSize = PagingConstants.PageSizeDefault,
		ModSorting? sortBy = null,
		bool ascending = false)
	{
		IQueryable<ModEntity> modsQuery = _dbContext.Mods.AsNoTracking();

		modsQuery = sortBy switch
		{
			ModSorting.ModTypes => modsQuery.OrderBy(m => m.ModTypes, ascending).ThenBy(m => m.Id),
			ModSorting.HtmlDescription => modsQuery.OrderBy(m => m.HtmlDescription, ascending).ThenBy(m => m.Id),
			ModSorting.IsHidden => modsQuery.OrderBy(m => m.IsHidden, ascending).ThenBy(m => m.Id),
			ModSorting.LastUpdated => modsQuery.OrderBy(m => m.LastUpdated, ascending).ThenBy(m => m.Id),
			ModSorting.Name => modsQuery.OrderBy(m => m.Name, ascending).ThenBy(m => m.Id),
			ModSorting.TrailerUrl => modsQuery.OrderBy(m => m.TrailerUrl, ascending).ThenBy(m => m.Id),
			ModSorting.Url => modsQuery.OrderBy(m => m.Url, ascending).ThenBy(m => m.Id),
			_ => modsQuery.OrderBy(m => m.Id, ascending),
		};

		List<ModEntity> mods = modsQuery
			.Skip(pageIndex * pageSize)
			.Take(pageSize)
			.ToList();

		return new Page<GetModForOverview>
		{
			Results = mods.ConvertAll(m => m.ToGetModForOverview()),
			TotalResults = _dbContext.Mods.Count(),
		};
	}

	[HttpGet("names")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[Authorize(Roles = Roles.Players)]
	public ActionResult<List<GetModName>> GetModNames()
	{
		var mods = _dbContext.Mods
			.AsNoTracking()
			.Select(m => new { m.Id, m.Name })
			.ToList();

		return mods.ConvertAll(m => new GetModName
		{
			Id = m.Id,
			Name = m.Name,
		});
	}

	[HttpGet("{id}")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	[Authorize(Roles = Roles.Mods)]
	public ActionResult<GetMod> GetModById(int id)
	{
		ModEntity? mod = _dbContext.Mods
			.AsSingleQuery()
			.AsNoTracking()
			.Include(m => m.PlayerMods)
			.FirstOrDefault(m => m.Id == id);
		if (mod == null)
			return NotFound();

		ModFileSystemData mfsd = _modArchiveAccessor.GetModFileSystemData(mod.Name);
		return mod.ToGetMod(mfsd.ModArchive?.Binaries.ConvertAll(macd => macd.Name), mfsd?.ScreenshotFileNames);
	}

	[HttpPost]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[Authorize(Roles = Roles.Mods)]
	public async Task<ActionResult> AddMod(AddMod addMod)
	{
		if (string.IsNullOrWhiteSpace(addMod.Name))
			return BadRequest("Mod name must not be empty or consist of white space only.");

		if (addMod.PlayerIds == null || addMod.PlayerIds.Count == 0)
			return BadRequest("Mod must have at least one author.");

		if (_dbContext.Mods.Any(m => m.Name == addMod.Name))
			return BadRequest($"Mod with name '{addMod.Name}' already exists.");

		foreach (int playerId in addMod.PlayerIds)
		{
			if (!_dbContext.Players.Any(p => p.Id == playerId))
				return BadRequest($"Player with ID '{playerId}' does not exist.");
		}

		List<FileSystemInformation> fsi = new();
		if (addMod.Binaries.Count > 0)
		{
			try
			{
				await _modArchiveProcessor.ProcessModBinaryUploadAsync(addMod.Name, addMod.Binaries, fsi);
			}
			catch (InvalidModArchiveException ex)
			{
				return BadRequest($"The mod archive is invalid. {ex.Message}");
			}
			catch (InvalidModBinaryException ex)
			{
				return BadRequest($"A mod binary inside the mod archive is invalid. {ex.Message}");
			}
		}

		if (addMod.Screenshots.Count > 0)
			_modScreenshotProcessor.ProcessModScreenshotUpload(addMod.Name, addMod.Screenshots, fsi);

		ModEntity mod = new()
		{
			ModTypes = addMod.ModTypes?.ToFlagEnum<ModTypes>() ?? ModTypes.None,
			HtmlDescription = addMod.HtmlDescription,
			IsHidden = addMod.IsHidden,
			LastUpdated = DateTime.UtcNow,
			Name = addMod.Name,
			TrailerUrl = addMod.TrailerUrl,
			Url = addMod.Url ?? string.Empty,
		};
		_dbContext.Mods.Add(mod);
		_dbContext.SaveChanges(); // Save changes here so PlayerMods entities can be assigned properly.

		UpdatePlayerMods(addMod.PlayerIds ?? new(), mod.Id);
		_dbContext.SaveChanges();

		await _auditLogger.LogAdd(addMod.GetLog(), User, mod.Id, fsi);

		return Ok(mod.Id);
	}

	[HttpPut("{id}")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	[Authorize(Roles = Roles.Mods)]
	public async Task<ActionResult> EditModById(int id, EditMod editMod)
	{
		if (string.IsNullOrWhiteSpace(editMod.Name))
			return BadRequest("Mod name must not be empty or consist of white space only.");

		if (editMod.PlayerIds == null || editMod.PlayerIds.Count == 0)
			return BadRequest("Mod must have at least one author.");

		foreach (int playerId in editMod.PlayerIds)
		{
			if (!_dbContext.Players.Any(p => p.Id == playerId))
				return BadRequest($"Player with ID '{playerId}' does not exist.");
		}

		ModEntity? mod = _dbContext.Mods
			.Include(m => m.PlayerMods)
			.FirstOrDefault(m => m.Id == id);
		if (mod == null)
			return NotFound();

		if (mod.Name != editMod.Name && _dbContext.Mods.Any(m => m.Name == editMod.Name))
			return BadRequest($"Mod with name '{editMod.Name}' already exists.");

		List<FileSystemInformation> fsi = new();

		try
		{
			bool isUpdated = await _modArchiveProcessor.TransformBinariesInModArchiveAsync(mod.Name, editMod.Name, editMod.BinariesToDelete, editMod.Binaries, fsi);
			if (isUpdated)
				mod.LastUpdated = DateTime.UtcNow;
		}
		catch (InvalidModArchiveException ex)
		{
			return BadRequest($"The mod archive is invalid. {ex.Message}");
		}
		catch (InvalidModBinaryException ex)
		{
			return BadRequest($"A mod binary inside the mod archive is invalid. {ex.Message}");
		}

		_modScreenshotProcessor.MoveScreenshotsDirectory(mod.Name, editMod.Name, fsi);

		foreach (string screenshotToDelete in editMod.ScreenshotsToDelete)
			_modScreenshotProcessor.DeleteScreenshot(editMod.Name, screenshotToDelete, fsi);

		_modScreenshotProcessor.ProcessModScreenshotUpload(editMod.Name, editMod.Screenshots, fsi);

		EditMod logDto = new()
		{
			ModTypes = mod.ModTypes.AsEnumerable().ToList(),
			HtmlDescription = mod.HtmlDescription,
			IsHidden = mod.IsHidden,
			Name = mod.Name,
			TrailerUrl = mod.TrailerUrl,
			Url = mod.Url,
			PlayerIds = mod.PlayerMods.ConvertAll(pam => pam.PlayerId),
		};

		mod.ModTypes = editMod.ModTypes?.ToFlagEnum<ModTypes>() ?? ModTypes.None;
		mod.HtmlDescription = editMod.HtmlDescription;
		mod.IsHidden = editMod.IsHidden;
		mod.Name = editMod.Name;
		mod.TrailerUrl = editMod.TrailerUrl;
		mod.Url = editMod.Url ?? string.Empty;
		_dbContext.SaveChanges(); // Save changes here so PlayerMods entities can be assigned properly.

		UpdatePlayerMods(editMod.PlayerIds ?? new(), mod.Id);
		_dbContext.SaveChanges();

		await _auditLogger.LogEdit(logDto.GetLog(), editMod.GetLog(), User, mod.Id, fsi);

		return Ok();
	}

	[HttpDelete("{id}")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	[Authorize(Roles = Roles.Mods)]
	public async Task<ActionResult> DeleteModById(int id)
	{
		ModEntity? mod = _dbContext.Mods.FirstOrDefault(m => m.Id == id);
		if (mod == null)
			return NotFound();

		List<FileSystemInformation> fileSystemInformation = new();
		_modArchiveProcessor.DeleteModFilesAndClearCache(mod.Name, fileSystemInformation);
		_modScreenshotProcessor.DeleteScreenshotsDirectory(mod.Name, fileSystemInformation);

		_dbContext.Mods.Remove(mod);
		_dbContext.SaveChanges();

		await _auditLogger.LogDelete(mod.GetLog(), User, mod.Id, fileSystemInformation);

		return Ok();
	}

	private void UpdatePlayerMods(List<int> playerIds, int modId)
	{
		foreach (PlayerModEntity newEntity in playerIds.ConvertAll(pi => new PlayerModEntity { ModId = modId, PlayerId = pi }))
		{
			if (!_dbContext.PlayerMods.Any(pam => pam.ModId == newEntity.ModId && pam.PlayerId == newEntity.PlayerId))
				_dbContext.PlayerMods.Add(newEntity);
		}

		foreach (PlayerModEntity entityToRemove in _dbContext.PlayerMods.Where(pam => pam.ModId == modId && !playerIds.Contains(pam.PlayerId)))
			_dbContext.PlayerMods.Remove(entityToRemove);
	}
}
