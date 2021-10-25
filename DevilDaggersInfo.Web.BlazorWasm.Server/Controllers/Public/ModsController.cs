using DevilDaggersInfo.Web.BlazorWasm.Server.Caches.ModArchives;
using DevilDaggersInfo.Web.BlazorWasm.Shared.Dto;
using DevilDaggersInfo.Web.BlazorWasm.Shared.Dto.Public.Mods;
using DevilDaggersInfo.Web.BlazorWasm.Shared.Enums.Sortings.Public;

namespace DevilDaggersInfo.Web.BlazorWasm.Server.Controllers.Public;

[Route("api/mods")]
[ApiController]
public class ModsController : ControllerBase
{
	private readonly ApplicationDbContext _dbContext;
	private readonly IFileSystemService _fileSystemService;
	private readonly ModArchiveCache _modArchiveCache;

	public ModsController(ApplicationDbContext dbContext, IFileSystemService fileSystemService, ModArchiveCache modArchiveCache)
	{
		_dbContext = dbContext;
		_fileSystemService = fileSystemService;
		_modArchiveCache = modArchiveCache;
	}

	[HttpGet]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public ActionResult<Page<GetModOverview>> GetMods(
		bool onlyHosted,
		[Range(0, 1000)] int pageIndex = 0,
		[Range(PublicPagingConstants.PageSizeMin, PublicPagingConstants.PageSizeMax)] int pageSize = PublicPagingConstants.PageSizeDefault,
		ModSorting? sortBy = null,
		bool ascending = false)
	{
		IEnumerable<ModEntity> modsQuery = _dbContext.Mods
			.AsNoTracking()
			.Include(am => am.PlayerMods)
				.ThenInclude(pam => pam.Player)
			.Where(am => !am.IsHidden);

		List<ModEntity> mods = modsQuery.ToList();
		Dictionary<ModEntity, (bool FileExists, string? Path)> modsWithFileInfo = GetModsWithFileInfo(mods);
		if (onlyHosted)
			modsWithFileInfo = modsWithFileInfo.Where(kvp => kvp.Value.FileExists).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

		List<GetModOverview> modDtos = modsWithFileInfo
			.Select(amwfi =>
			{
				bool? containsProhibitedAssets = null;
				ModTypes modTypes;
				if (amwfi.Value.FileExists)
				{
					ModArchiveCacheData archiveData = _modArchiveCache.GetArchiveDataByFilePath(amwfi.Value.Path!);
					containsProhibitedAssets = archiveData.ContainsProhibitedAssets();
					modTypes = archiveData.GetModTypes();
				}
				else
				{
					modTypes = amwfi.Key.ModTypes;
				}

				return new GetModOverview
				{
					Id = amwfi.Key.Id,
					Name = amwfi.Key.Name,
					Authors = amwfi.Key.PlayerMods.Select(pam => pam.Player.PlayerName).OrderBy(s => s).ToList(),
					LastUpdated = amwfi.Key.LastUpdated,
					ModTypes = modTypes,
					IsHosted = amwfi.Value.FileExists,
					ContainsProhibitedAssets = containsProhibitedAssets,
				};
			})
			.ToList();

		modDtos = sortBy switch
		{
			ModSorting.Name => modDtos.OrderBy(m => m.Name, ascending).ToList(),
			ModSorting.Authors => modDtos.OrderBy(m => m.Authors.FirstOrDefault(), ascending).ToList(),
			ModSorting.LastUpdated => modDtos.OrderBy(m => m.LastUpdated, ascending).ToList(),
			ModSorting.ModTypes => modDtos.OrderBy(m => m.ModTypes, ascending).ToList(),
			ModSorting.Hosted => modDtos.OrderBy(m => m.IsHosted, ascending).ToList(),
			ModSorting.ProhibitedAssets => modDtos.OrderBy(m => m.ContainsProhibitedAssets, ascending).ToList(),
			_ => modDtos.OrderBy(m => m.Id, ascending).ToList(),
		};

		modDtos = modDtos
			.Skip(pageIndex * pageSize)
			.Take(pageSize)
			.ToList();

		return new Page<GetModOverview>
		{
			Results = modDtos,
			TotalResults = modsWithFileInfo.Count,
		};
	}

	[HttpGet("ddae")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	public List<GetModDdae> GetModsForDdae(string? authorFilter = null, string? nameFilter = null, bool? isHostedFilter = null)
	{
		IEnumerable<ModEntity> modsQuery = _dbContext.Mods
			.AsNoTracking()
			.Include(am => am.PlayerMods)
				.ThenInclude(pam => pam.Player)
			.Where(am => !am.IsHidden);

		if (!string.IsNullOrWhiteSpace(authorFilter))
			modsQuery = modsQuery.Where(am => am.PlayerMods.Any(pam => pam.Player.PlayerName.Contains(authorFilter, StringComparison.InvariantCultureIgnoreCase)));
		if (!string.IsNullOrWhiteSpace(nameFilter))
			modsQuery = modsQuery.Where(am => am.Name.Contains(nameFilter, StringComparison.InvariantCultureIgnoreCase));

		List<ModEntity> mods = modsQuery.ToList();
		Dictionary<ModEntity, (bool FileExists, string? Path)> modsWithFileInfo = GetModsWithFileInfo(mods);
		if (isHostedFilter.HasValue)
			modsWithFileInfo = modsWithFileInfo.Where(kvp => kvp.Value.FileExists == isHostedFilter.Value).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

		return modsWithFileInfo
			.Select(amwfi =>
			{
				bool? containsProhibitedAssets = null;
				GetModArchiveDdae? modArchive = null;
				ModTypes modTypes;
				if (amwfi.Value.FileExists)
				{
					ModArchiveCacheData archiveData = _modArchiveCache.GetArchiveDataByFilePath(amwfi.Value.Path!);
					modArchive = new()
					{
						FileSize = archiveData.FileSize,
						FileSizeExtracted = archiveData.FileSizeExtracted,
						Binaries = archiveData.Binaries.ConvertAll(b => new GetModBinaryDdae
						{
							Name = b.Name,
							Size = b.Size,
							ModBinaryType = b.ModBinaryType,
						}),
					};

					containsProhibitedAssets = archiveData.ContainsProhibitedAssets();
					modTypes = archiveData.GetModTypes();
				}
				else
				{
					modTypes = amwfi.Key.ModTypes;
				}

				string modScreenshotsDirectory = Path.Combine(_fileSystemService.GetPath(DataSubDirectory.ModScreenshots), amwfi.Key.Name);
				List<string> screenshotFileNames;
				if (Directory.Exists(modScreenshotsDirectory))
					screenshotFileNames = Directory.GetFiles(modScreenshotsDirectory).Select(p => Path.GetFileName(p)).ToList();
				else
					screenshotFileNames = new();

				return new GetModDdae
				{
					Name = amwfi.Key.Name,
					HtmlDescription = amwfi.Key.HtmlDescription,
					TrailerUrl = amwfi.Key.TrailerUrl,
					Authors = amwfi.Key.PlayerMods.Select(pam => pam.Player.PlayerName).OrderBy(s => s).ToList(),
					LastUpdated = amwfi.Key.LastUpdated,
					ModTypes = modTypes,
					IsHosted = amwfi.Value.FileExists,
					ContainsProhibitedAssets = containsProhibitedAssets,
					ModArchive = modArchive,
					ScreenshotFileNames = screenshotFileNames,
				};
			})
			.ToList();
	}

	[HttpGet("total-data")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	public ActionResult<GetTotalModData> GetTotalModData()
	{
		return new GetTotalModData
		{
			Count = _dbContext.Mods.AsNoTracking().Select(m => m.Id).Count(),
		};
	}

	[HttpGet("{modName}/file")]
	[ProducesResponseType(typeof(FileContentResult), StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public ActionResult GetModFile([Required] string modName)
	{
		if (!_dbContext.Mods.Any(m => m.Name == modName))
			return NotFound();

		string fileName = $"{modName}.zip";
		string path = Path.Combine(_fileSystemService.GetPath(DataSubDirectory.Mods), fileName);
		if (!IoFile.Exists(path))
			return BadRequest($"Mod file '{fileName}' does not exist.");

		return File(IoFile.ReadAllBytes(path), MediaTypeNames.Application.Zip, fileName);
	}

	private Dictionary<ModEntity, (bool FileExists, string? Path)> GetModsWithFileInfo(List<ModEntity> mods)
		=> mods.ToDictionary(m => m, m =>
		{
			string filePath = Path.Combine(_fileSystemService.GetPath(DataSubDirectory.Mods), $"{m.Name}.zip");
			bool fileExists = IoFile.Exists(filePath);
			return (fileExists, fileExists ? filePath : null);
		});

	[HttpGet("by-author")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	public ActionResult<List<GetModName>> GetModsByAuthorId([Required] int playerId)
	{
		var mods = _dbContext.Mods.AsNoTracking().Include(m => m.PlayerMods).Where(s => s.PlayerMods.Any(pm => pm.PlayerId == playerId)).Select(s => new { s.Id, s.Name }).ToList();

		return mods.ConvertAll(s => new GetModName
		{
			Id = s.Id,
			Name = s.Name,
		});
	}
}
