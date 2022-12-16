using DevilDaggersInfo.Api.Ddae.Mods;
using DevilDaggersInfo.Web.Server.Converters.DomainToApi.Ddae;
using DevilDaggersInfo.Web.Server.Domain.Models.FileSystem;
using DevilDaggersInfo.Web.Server.Domain.Models.ModArchives;
using DevilDaggersInfo.Web.Server.Domain.Services;
using DevilDaggersInfo.Web.Server.Domain.Services.Inversion;

namespace DevilDaggersInfo.Web.Server.Controllers.Ddae;

[Obsolete("DDAE 1.4.0 will be removed.")]
[Route("api/ddae/mods")]
[ApiController]
public class ModsController : ControllerBase
{
	private readonly ApplicationDbContext _dbContext;
	private readonly ModArchiveAccessor _modArchiveAccessor;

	public ModsController(ApplicationDbContext dbContext, ModArchiveAccessor modArchiveAccessor)
	{
		_dbContext = dbContext;
		_modArchiveAccessor = modArchiveAccessor;
	}

	[Obsolete("DDAE 1.4.0 will be removed.")]
	[HttpGet("/api/mods/ddae")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	public List<GetModDdae> GetModsObsolete(string? authorFilter = null, string? nameFilter = null, bool? isHostedFilter = null)
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
}
