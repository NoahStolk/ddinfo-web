using DevilDaggersInfo.Web.BlazorWasm.Server.Caches.ModArchives;
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
	private readonly IFileSystemService _fileSystemService;
	private readonly ModArchiveCache _modArchiveCache;
	private readonly AuditLogger _auditLogger;

	public ModsController(ApplicationDbContext dbContext, IFileSystemService fileSystemService, ModArchiveCache modArchiveCache, AuditLogger auditLogger)
	{
		_dbContext = dbContext;
		_fileSystemService = fileSystemService;
		_modArchiveCache = modArchiveCache;
		_auditLogger = auditLogger;
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

		string? addInfo = null;
		if (addMod.FileContents != null)
		{
			string modsDirectory = _fileSystemService.GetPath(DataSubDirectory.Mods);
			DirectoryInfo di = new(modsDirectory);
			long usedSpace = di.EnumerateFiles("*.*", SearchOption.AllDirectories).Sum(fi => fi.Length);
			if (addMod.FileContents.Length + usedSpace > ModFileConstants.MaxHostingSpace)
				return BadRequest($"This file is {addMod.FileContents.Length:n0} bytes in size, but only {ModFileConstants.MaxHostingSpace - usedSpace:n0} bytes of free space is available.");

			string? validationError = ValidateModArchive(addMod.FileContents, addMod.Name);
			if (validationError != null)
				return BadRequest(validationError);

			string path = Path.Combine(modsDirectory, $"{addMod.Name}.zip");
			IoFile.WriteAllBytes(path, addMod.FileContents);
			addInfo = $"File {_fileSystemService.FormatPath(path)} was added.";
		}

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

		await _auditLogger.LogAdd(addMod, User, mod.Id, addInfo == null ? null : new() { new(addInfo, FileSystemInformationType.Add) });

		return Ok(mod.Id);
	}

	[HttpPut("{id}")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<ActionResult> EditModById(int id, EditMod editMod)
	{
		// Validate DTO.
		if (editMod.RemoveExistingFile && editMod.FileContents != null)
			return BadRequest("Requested to remove the existing file, but a new file was also provided.");

		if (editMod.PlayerIds == null || editMod.PlayerIds.Count == 0)
			return BadRequest("Mod must have at least one author.");

		foreach (int playerId in editMod.PlayerIds)
		{
			if (!_dbContext.Players.Any(p => p.Id == playerId))
				return BadRequest($"Player with ID '{playerId}' does not exist.");
		}

		// Validate file.
		if (editMod.FileContents != null)
		{
			DirectoryInfo di = new(_fileSystemService.GetPath(DataSubDirectory.Mods));
			long usedSpace = di.EnumerateFiles("*.*", SearchOption.AllDirectories).Sum(fi => fi.Length);

			int additionalAddedSpace = editMod.FileContents.Length; // TODO: New file length - old file length
			if (additionalAddedSpace + usedSpace > ModFileConstants.MaxHostingSpace)
				return BadRequest($"This file is {editMod.FileContents.Length:n0} bytes in size, but only {ModFileConstants.MaxHostingSpace - usedSpace:n0} bytes of free space is available.");

			string? validationError = ValidateModArchive(editMod.FileContents, editMod.Name);
			if (validationError != null)
				return BadRequest(validationError);
		}

		// Validate against database.
		ModEntity? mod = _dbContext.Mods
			.Include(m => m.PlayerMods)
			.FirstOrDefault(m => m.Id == id);
		if (mod == null)
			return NotFound();

		if (mod.Name != editMod.Name && _dbContext.Mods.Any(m => m.Name == editMod.Name))
			return BadRequest($"Mod with name '{editMod.Name}' already exists.");

		// Request is accepted.
		List<FileSystemInformation> fileSystemInformation = new();

		// Remove existing file if requested (this does NOT remove screenshots). Otherwise; move files if mod is renamed (this DOES include screenshots).
		if (editMod.RemoveExistingFile)
			DeleteModFilesAndClearCache(mod, fileSystemInformation);
		else if (mod.Name != editMod.Name)
			MoveModFilesAndClearCache(newName: editMod.Name, currentName: mod.Name, fileSystemInformation);

		// Update file.
		if (editMod.FileContents != null)
		{
			// At this point we already know RemoveExistingFile is false, and that the old files are moved already.
			string path = Path.Combine(_fileSystemService.GetPath(DataSubDirectory.Mods), $"{editMod.Name}.zip");
			IoFile.WriteAllBytes(path, editMod.FileContents);
			fileSystemInformation.Add(new($"File {_fileSystemService.FormatPath(path)} was added.", FileSystemInformationType.Add));

			// Update LastUpdated when updating the file only.
			mod.LastUpdated = DateTime.UtcNow;
		}

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

		await _auditLogger.LogEdit(logDto, editMod, User, mod.Id, fileSystemInformation);

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

		// Delete mod file and cache.
		DeleteModFilesAndClearCache(mod, fileSystemInformation);

		// Delete screenshots directory.
		string screenshotsDirectory = Path.Combine(_fileSystemService.GetPath(DataSubDirectory.ModScreenshots), mod.Name);
		if (Directory.Exists(screenshotsDirectory))
		{
			Directory.Delete(screenshotsDirectory, true);
			fileSystemInformation.Add(new($"Directory {_fileSystemService.FormatPath(screenshotsDirectory)} was deleted because removal was requested.", FileSystemInformationType.Delete));
		}
		else
		{
			fileSystemInformation.Add(new($"Directory {_fileSystemService.FormatPath(screenshotsDirectory)} was not deleted because it does not exist.", FileSystemInformationType.NotFound));
		}

		_dbContext.Mods.Remove(mod);
		_dbContext.SaveChanges();

		await _auditLogger.LogDelete(mod, User, mod.Id, fileSystemInformation);

		return Ok();
	}

	/// <summary>
	/// Moves the mod archive, mod archive cache, and the screenshots to a new path.
	/// </summary>
	private void MoveModFilesAndClearCache(string newName, string currentName, List<FileSystemInformation> fileSystemInformation)
	{
		string directory = _fileSystemService.GetPath(DataSubDirectory.Mods);
		string oldPath = Path.Combine(directory, $"{currentName}.zip");
		if (IoFile.Exists(oldPath))
		{
			string newPath = Path.Combine(directory, newName);
			IoFile.Move(oldPath, newPath);
			fileSystemInformation.Add(new($"File {_fileSystemService.FormatPath(oldPath)} was moved to {_fileSystemService.FormatPath(newPath)}.", FileSystemInformationType.Move));

			// Clear entire memory cache (can't clear individual entries).
			_modArchiveCache.Clear();
		}
		else
		{
			fileSystemInformation.Add(new($"File {_fileSystemService.FormatPath(oldPath)} was not moved because it does not exist.", FileSystemInformationType.NotFound));
		}

		string cacheDirectory = _fileSystemService.GetPath(DataSubDirectory.ModArchiveCache);
		string oldCachePath = Path.Combine(cacheDirectory, $"{currentName}.json");
		if (IoFile.Exists(oldCachePath))
		{
			string newCachePath = Path.Combine(directory, newName);
			IoFile.Move(oldCachePath, newCachePath);
			fileSystemInformation.Add(new($"File {_fileSystemService.FormatPath(oldCachePath)} was moved to {_fileSystemService.FormatPath(newCachePath)}.", FileSystemInformationType.Move));
		}
		else
		{
			fileSystemInformation.Add(new($"File {_fileSystemService.FormatPath(oldCachePath)} was not moved because it does not exist.", FileSystemInformationType.NotFound));
		}

		// Always move screenshots directory (not removed when removal is requested as screenshots are separate entities).
		string oldScreenshotsDirectory = Path.Combine(_fileSystemService.GetPath(DataSubDirectory.ModScreenshots), currentName);
		if (Directory.Exists(oldScreenshotsDirectory))
		{
			string newScreenshotsDirectory = Path.Combine(_fileSystemService.GetPath(DataSubDirectory.ModScreenshots), newName);
			Directory.Move(oldScreenshotsDirectory, newScreenshotsDirectory);
			fileSystemInformation.Add(new($"Directory {_fileSystemService.FormatPath(oldScreenshotsDirectory)} was moved to {_fileSystemService.FormatPath(newScreenshotsDirectory)}.", FileSystemInformationType.Move));
		}
		else
		{
			fileSystemInformation.Add(new($"Directory {_fileSystemService.FormatPath(oldScreenshotsDirectory)} was not moved because it does not exist.", FileSystemInformationType.NotFound));
		}
	}

	/// <summary>
	/// Deletes the mod archive and the mod archive cache for this mod, and also clears the memory cache.
	/// <b>This method does not delete mod screenshot files</b>.
	/// </summary>
	private void DeleteModFilesAndClearCache(ModEntity mod, List<FileSystemInformation> fileSystemInformation)
	{
		// Delete file.
		string path = Path.Combine(_fileSystemService.GetPath(DataSubDirectory.Mods), $"{mod.Name}.zip");
		if (IoFile.Exists(path))
		{
			IoFile.Delete(path);
			fileSystemInformation.Add(new($"File {_fileSystemService.FormatPath(path)} was deleted because removal was requested.", FileSystemInformationType.Delete));

			// Clear entire memory cache (can't clear individual entries).
			_modArchiveCache.Clear();
		}
		else
		{
			fileSystemInformation.Add(new($"File {_fileSystemService.FormatPath(path)} was not deleted because it does not exist.", FileSystemInformationType.NotFound));
		}

		// Clear file cache for this mod.
		string cachePath = Path.Combine(_fileSystemService.GetPath(DataSubDirectory.ModArchiveCache), $"{mod.Name}.json");
		if (IoFile.Exists(cachePath))
		{
			IoFile.Delete(cachePath);
			fileSystemInformation.Add(new($"File {_fileSystemService.FormatPath(cachePath)} was deleted because removal was requested.", FileSystemInformationType.Delete));
		}
		else
		{
			fileSystemInformation.Add(new($"File {_fileSystemService.FormatPath(cachePath)} was not deleted because it does not exist.", FileSystemInformationType.NotFound));
		}
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

	private string? ValidateModArchive(byte[] fileContents, string modName)
	{
		try
		{
			List<ModBinaryCacheData> archive = _modArchiveCache.GetArchiveDataByBytes(modName, fileContents).Binaries;
			if (archive.Count == 0)
				throw new InvalidModArchiveException("Mod archive does not contain any binaries.");

			foreach (ModBinaryCacheData binary in archive)
			{
				if (binary.Chunks.Count == 0)
					throw new InvalidModBinaryException($"Mod binary '{binary.Name}' does not contain any assets.");

				string expectedPrefix = binary.ModBinaryType switch
				{
					ModBinaryType.Audio => $"audio-{modName}-",
					ModBinaryType.Dd => $"dd-{modName}-",
					_ => throw new InvalidModBinaryException($"Mod binary '{binary.Name}' is a '{binary.ModBinaryType}' mod which is not allowed."),
				};

				if (!binary.Name.StartsWith(expectedPrefix))
					throw new InvalidModBinaryException($"Name of mod binary '{binary.Name}' must start with '{expectedPrefix}'.");

				if (binary.Name.Length == expectedPrefix.Length)
					throw new InvalidModBinaryException($"Name of mod binary '{binary.Name}' must not be equal to '{expectedPrefix}'.");
			}
		}
		catch (InvalidModArchiveException ex)
		{
			// TODO: Remove from cache again.
			return $"The mod archive is invalid. {ex.Message}";
		}
		catch (InvalidModBinaryException ex)
		{
			// TODO: Remove from cache again.
			return $"A mod binary inside the mod archive is invalid. {ex.Message}";
		}

		return null;
	}
}
