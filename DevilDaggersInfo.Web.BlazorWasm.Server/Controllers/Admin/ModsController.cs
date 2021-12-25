using DevilDaggersInfo.Web.BlazorWasm.Server.Converters.Admin;
using DevilDaggersInfo.Web.BlazorWasm.Server.InternalModels;
using DevilDaggersInfo.Web.BlazorWasm.Shared.Dto;
using DevilDaggersInfo.Web.BlazorWasm.Shared.Dto.Admin.Mods;
using DevilDaggersInfo.Web.BlazorWasm.Shared.Enums.Sortings.Admin;

namespace DevilDaggersInfo.Web.BlazorWasm.Server.Controllers.Admin;

[Route("api/admin/mods")]
[ApiController]
public class ModsController : ControllerBase
{
	private readonly ApplicationDbContext _dbContext;
	private readonly AuditLogger _auditLogger;
	private readonly ModFileSystemProcessor _modFileSystemProcessor;

	public ModsController(ApplicationDbContext dbContext, AuditLogger auditLogger, ModFileSystemProcessor modFileSystemProcessor)
	{
		_dbContext = dbContext;
		_auditLogger = auditLogger;
		_modFileSystemProcessor = modFileSystemProcessor;
	}

	[HttpGet]
	[ProducesResponseType(StatusCodes.Status200OK)]
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
	public ActionResult<GetMod> GetModById(int id)
	{
		ModEntity? mod = _dbContext.Mods
			.AsSingleQuery()
			.AsNoTracking()
			.Include(m => m.PlayerMods)
			.FirstOrDefault(m => m.Id == id);
		if (mod == null)
			return NotFound();

		return mod.ToGetMod();
	}

	[HttpPost]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public async Task<ActionResult> AddMod(AddMod addMod)
	{
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
				await _modFileSystemProcessor.ProcessModBinaryUploadAsync(addMod.Name, addMod.Binaries, fsi);
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
			_modFileSystemProcessor.ProcessModScreenshotUpload(addMod.Name, addMod.Screenshots, fsi);

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

		await _auditLogger.LogAdd(addMod, User, mod.Id, fsi);

		return Ok(mod.Id);
	}

	[HttpPut("{id}")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<ActionResult> EditModById(int id, EditMod editMod)
	{
		// Validate DTO.
		//if (editMod.RemoveExistingFile && editMod.FileContents != null)
		//	return BadRequest("Requested to remove the existing file, but a new file was also provided.");

		//if (editMod.PlayerIds == null || editMod.PlayerIds.Count == 0)
		//	return BadRequest("Mod must have at least one author.");

		//foreach (int playerId in editMod.PlayerIds)
		//{
		//	if (!_dbContext.Players.Any(p => p.Id == playerId))
		//		return BadRequest($"Player with ID '{playerId}' does not exist.");
		//}

		//// Validate file.
		//if (editMod.FileContents != null)
		//{
		//	DirectoryInfo di = new(_fileSystemService.GetPath(DataSubDirectory.Mods));
		//	long usedSpace = di.EnumerateFiles("*.*", SearchOption.AllDirectories).Sum(fi => fi.Length);

		//	int additionalAddedSpace = editMod.FileContents.Length; // TODO: New file length - old file length
		//	if (additionalAddedSpace + usedSpace > ModConstants.MaxHostingSpace)
		//		return BadRequest($"This file is {editMod.FileContents.Length:n0} bytes in size, but only {ModConstants.MaxHostingSpace - usedSpace:n0} bytes of free space is available.");

		//	string? validationError = ValidateModArchive(editMod.FileContents, editMod.Name);
		//	if (validationError != null)
		//		return BadRequest(validationError);
		//}

		//// Validate against database.
		//ModEntity? mod = _dbContext.Mods
		//	.Include(m => m.PlayerMods)
		//	.FirstOrDefault(m => m.Id == id);
		//if (mod == null)
		//	return NotFound();

		//if (mod.Name != editMod.Name && _dbContext.Mods.Any(m => m.Name == editMod.Name))
		//	return BadRequest($"Mod with name '{editMod.Name}' already exists.");

		//// Request is accepted.
		//List<FileSystemInformation> fileSystemInformation = new();

		//// Remove existing file if requested (this does NOT remove screenshots). Otherwise; move files if mod is renamed (this DOES include screenshots).
		//if (editMod.RemoveExistingFile)
		//	DeleteModFilesAndClearCache(mod, fileSystemInformation);
		//else if (mod.Name != editMod.Name)
		//	MoveModFilesAndClearCache(newName: editMod.Name, currentName: mod.Name, fileSystemInformation);

		//// Update file.
		//if (editMod.FileContents != null)
		//{
		//	// At this point we already know RemoveExistingFile is false, and that the old files are moved already.
		//	string path = Path.Combine(_fileSystemService.GetPath(DataSubDirectory.Mods), $"{editMod.Name}.zip");
		//	IoFile.WriteAllBytes(path, editMod.FileContents);
		//	fileSystemInformation.Add(new($"File {_fileSystemService.FormatPath(path)} was added.", FileSystemInformationType.Add));

		//	// Update LastUpdated when updating the file only.
		//	mod.LastUpdated = DateTime.UtcNow;
		//}

		//EditMod logDto = new()
		//{
		//	ModTypes = mod.ModTypes.AsEnumerable().ToList(),
		//	HtmlDescription = mod.HtmlDescription,
		//	IsHidden = mod.IsHidden,
		//	Name = mod.Name,
		//	TrailerUrl = mod.TrailerUrl,
		//	Url = mod.Url,
		//	PlayerIds = mod.PlayerMods.ConvertAll(pam => pam.PlayerId),
		//};

		//mod.ModTypes = editMod.ModTypes?.ToFlagEnum<ModTypes>() ?? ModTypes.None;
		//mod.HtmlDescription = editMod.HtmlDescription;
		//mod.IsHidden = editMod.IsHidden;
		//mod.Name = editMod.Name;
		//mod.TrailerUrl = editMod.TrailerUrl;
		//mod.Url = editMod.Url ?? string.Empty;
		//_dbContext.SaveChanges(); // Save changes here so PlayerMods entities can be assigned properly.

		//UpdatePlayerMods(editMod.PlayerIds ?? new(), mod.Id);
		//_dbContext.SaveChanges();

		//await _auditLogger.LogEdit(logDto, editMod, User, mod.Id, fileSystemInformation);

		return Ok();
	}

	[HttpDelete("{id}")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<ActionResult> DeleteModById(int id)
	{
		ModEntity? mod = _dbContext.Mods.FirstOrDefault(d => d.Id == id);
		if (mod == null)
			return NotFound();

		List<FileSystemInformation> fileSystemInformation = new();
		_modFileSystemProcessor.DeleteModFilesAndClearCache(mod.Name, fileSystemInformation);
		_modFileSystemProcessor.DeleteScreenshotsDirectory(mod.Name, fileSystemInformation);

		_dbContext.Mods.Remove(mod);
		_dbContext.SaveChanges();

		await _auditLogger.LogDelete(mod, User, mod.Id, fileSystemInformation);

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
