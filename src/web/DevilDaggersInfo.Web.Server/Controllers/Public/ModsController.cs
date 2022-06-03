using DevilDaggersInfo.Api.Main;
using DevilDaggersInfo.Api.Main.Mods;
using DevilDaggersInfo.Web.Server.Converters.DomainToApi.Main;
using DevilDaggersInfo.Web.Server.Enums;
using DevilDaggersInfo.Web.Server.InternalModels.Mods;

namespace DevilDaggersInfo.Web.Server.Controllers.Public;

[Route("api/mods")]
[ApiController]
public class ModsController : ControllerBase
{
	private readonly ApplicationDbContext _dbContext;
	private readonly IFileSystemService _fileSystemService;
	private readonly ModArchiveAccessor _modArchiveAccessor;

	public ModsController(ApplicationDbContext dbContext, IFileSystemService fileSystemService, ModArchiveAccessor modArchiveAccessor)
	{
		_dbContext = dbContext;
		_fileSystemService = fileSystemService;
		_modArchiveAccessor = modArchiveAccessor;
	}

	[HttpGet]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public ActionResult<Page<GetModOverview>> GetMods(
		bool onlyHosted,
		string? modFilter = null,
		string? authorFilter = null,
		[Range(0, 1000)] int pageIndex = 0,
		[Range(PagingConstants.PageSizeMin, PagingConstants.PageSizeMax)] int pageSize = PagingConstants.PageSizeDefault,
		ModSorting? sortBy = null,
		bool ascending = false)
	{
		IEnumerable<ModEntity> modsQuery = _dbContext.Mods
			.AsNoTracking()
			.Include(am => am.PlayerMods)
				.ThenInclude(pam => pam.Player)
			.Where(am => !am.IsHidden);

		// OrdinalIgnoreCase works here because this is an IEnumerable. Spawnset and custom leaderboard queries do not require this, and use IQueryable, so OrdinalIgnoreCase will not work there and casing is ignored by default.
		if (!string.IsNullOrWhiteSpace(modFilter))
			modsQuery = modsQuery.Where(m => m.Name.Contains(modFilter, StringComparison.OrdinalIgnoreCase));

		if (!string.IsNullOrWhiteSpace(authorFilter))
			modsQuery = modsQuery.Where(m => m.PlayerMods.Any(pm => pm.Player.PlayerName.Contains(authorFilter, StringComparison.OrdinalIgnoreCase)));

		List<ModEntity> mods = modsQuery.ToList();

		Dictionary<ModEntity, ModFileSystemData> data = mods.ToDictionary(m => m, m => _modArchiveAccessor.GetModFileSystemData(m.Name));
		if (onlyHosted)
			data = data.Where(kvp => kvp.Value.ModArchive != null).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

		List<GetModOverview> modDtos = data
			.Select(kvp => kvp.Key.ToGetModOverview(kvp.Value))
			.ToList();

		modDtos = (sortBy switch
		{
			ModSorting.Name => modDtos.OrderBy(m => m.Name, ascending),
			ModSorting.Authors => modDtos.OrderBy(m => m.Authors.FirstOrDefault(), ascending),
			ModSorting.LastUpdated => modDtos.OrderBy(m => m.LastUpdated, ascending),
			ModSorting.ModTypes => modDtos.OrderBy(m => m.ModTypes, ascending),
			ModSorting.Hosted => modDtos.OrderBy(m => m.IsHosted, ascending),
			ModSorting.ProhibitedAssets => modDtos.OrderBy(m => m.ContainsProhibitedAssets, ascending),
			_ => modDtos.OrderBy(m => m.Id, ascending),
		}).ToList();

		int totalMods = data.Count;
		int lastPageIndex = totalMods / pageSize;
		modDtos = modDtos
			.Skip(Math.Min(pageIndex, lastPageIndex) * pageSize)
			.Take(pageSize)
			.ToList();

		return new Page<GetModOverview>
		{
			Results = modDtos,
			TotalResults = totalMods,
		};
	}

	[HttpGet("{id}")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public ActionResult<GetMod> GetModById([Required] int id)
	{
		ModEntity? modEntity = _dbContext.Mods
			.AsNoTracking()
			.Include(m => m.PlayerMods)
				.ThenInclude(pm => pm.Player)
			.FirstOrDefault(m => m.Id == id);
		if (modEntity == null)
			return NotFound();

		ModFileSystemData mfsd = _modArchiveAccessor.GetModFileSystemData(modEntity.Name);

		return modEntity.ToGetMod(mfsd);
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

	// FORBIDDEN: Used by DDAE 1.4.0.0.
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

	[HttpGet("by-author")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	public ActionResult<List<GetModName>> GetModsByAuthorId([Required] int playerId)
	{
		var mods = _dbContext.Mods
			.AsNoTracking()
			.Include(m => m.PlayerMods)
			.Select(m => new { m.Id, m.Name, m.PlayerMods, m.LastUpdated })
			.Where(m => m.PlayerMods.Any(pm => pm.PlayerId == playerId))
			.OrderByDescending(m => m.LastUpdated)
			.ToList();

		return mods.ConvertAll(s => new GetModName
		{
			Id = s.Id,
			Name = s.Name,
		});
	}
}
