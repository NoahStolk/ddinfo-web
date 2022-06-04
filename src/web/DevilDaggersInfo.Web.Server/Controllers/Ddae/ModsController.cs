using DevilDaggersInfo.Api.Ddae.Mods;
using DevilDaggersInfo.Web.Server.Converters.DomainToApi.Ddae;
using DevilDaggersInfo.Web.Server.Domain.Models.FileSystem;
using DevilDaggersInfo.Web.Server.Domain.Services;
using DevilDaggersInfo.Web.Server.InternalModels.Mods;

namespace DevilDaggersInfo.Web.Server.Controllers.Ddae;

[Route("api/ddae/mods")]
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
	public List<GetModDdae> GetMods(string? authorFilter = null, string? nameFilter = null, bool? isHostedFilter = null)
	{
		return GetModsRepo(authorFilter, nameFilter, isHostedFilter);
	}

	// Used by DDAE 1.4.0.0.
	[Obsolete("Use the new route instead.")]
	[HttpGet("/api/mods/ddae")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	public List<GetModDdae> GetModsObsolete(string? authorFilter = null, string? nameFilter = null, bool? isHostedFilter = null)
	{
		return GetModsRepo(authorFilter, nameFilter, isHostedFilter);
	}

	private List<GetModDdae> GetModsRepo(string? authorFilter, string? nameFilter, bool? isHostedFilter)
	{
		IEnumerable<ModEntity> modsQuery = _dbContext.Mods
			.AsNoTracking()
			.Include(m => m.PlayerMods)
				.ThenInclude(pm => pm.Player)
			.Where(m => !m.IsHidden);

		if (!string.IsNullOrWhiteSpace(authorFilter))
			modsQuery = modsQuery.Where(m => m.PlayerMods.Any(pm => pm.Player.PlayerName.Contains(authorFilter, StringComparison.InvariantCultureIgnoreCase)));
		if (!string.IsNullOrWhiteSpace(nameFilter))
			modsQuery = modsQuery.Where(m => m.Name.Contains(nameFilter, StringComparison.InvariantCultureIgnoreCase));

		List<ModEntity> mods = modsQuery.ToList();

		Dictionary<ModEntity, ModFileSystemData> data = mods.ToDictionary(m => m, m => _modArchiveAccessor.GetModFileSystemData(m.Name));
		if (isHostedFilter.HasValue)
			data = data.Where(kvp => kvp.Value.ModArchive != null).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

		return data
			.Select(kvp => kvp.Key.ToDdaeApi(kvp.Value))
			.ToList();
	}

	// Not yet used.
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
}
