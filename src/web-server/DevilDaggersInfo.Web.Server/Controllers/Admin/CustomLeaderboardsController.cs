using DevilDaggersInfo.Api.Admin;
using DevilDaggersInfo.Api.Admin.CustomLeaderboards;
using DevilDaggersInfo.Types.Web;
using DevilDaggersInfo.Web.Client;
using DevilDaggersInfo.Web.Core.Claims;
using DevilDaggersInfo.Web.Core.CriteriaExpression;
using DevilDaggersInfo.Web.Server.Domain.Admin.Repositories;
using DevilDaggersInfo.Web.Server.Domain.Admin.Services;
using DevilDaggersInfo.Web.Server.Domain.Entities.Values;
using Microsoft.AspNetCore.Authorization;

namespace DevilDaggersInfo.Web.Server.Controllers.Admin;

[Route("api/admin/custom-leaderboards")]
[ApiController]
[Authorize(Roles = Roles.CustomLeaderboards)]
public class CustomLeaderboardsController : ControllerBase
{
	private readonly CustomLeaderboardRepository _customLeaderboardRepository;
	private readonly CustomLeaderboardService _customLeaderboardService;
	private readonly ILogger<CustomLeaderboardsController> _logger;
	private readonly ApplicationDbContext _dbContext;

	public CustomLeaderboardsController(CustomLeaderboardRepository customLeaderboardRepository, CustomLeaderboardService customLeaderboardService, ILogger<CustomLeaderboardsController> logger, ApplicationDbContext dbContext)
	{
		_customLeaderboardRepository = customLeaderboardRepository;
		_customLeaderboardService = customLeaderboardService;
		_logger = logger;
		_dbContext = dbContext;
	}

	[AllowAnonymous]
	[HttpPost("temp-migrate")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public async Task<ActionResult> MigrateValuesToExpressions()
	{
		_logger.LogWarning("Executing migrate values to expressions");

		var cls = _dbContext.CustomLeaderboards.ToList();

		foreach (var cl in cls)
		{
			Migrate(cl, c => c.GemsCollectedCriteria);
			Migrate(cl, c => c.GemsDespawnedCriteria);
			Migrate(cl, c => c.GemsEatenCriteria);
			Migrate(cl, c => c.EnemiesKilledCriteria);
			Migrate(cl, c => c.DaggersFiredCriteria);
			Migrate(cl, c => c.DaggersHitCriteria);
			Migrate(cl, c => c.HomingStoredCriteria);
			Migrate(cl, c => c.HomingEatenCriteria);
			Migrate2(cl, c => c.Skull1KillsCriteria);
			Migrate2(cl, c => c.Skull2KillsCriteria);
			Migrate2(cl, c => c.Skull3KillsCriteria);
			Migrate2(cl, c => c.Skull4KillsCriteria);
			Migrate2(cl, c => c.SpiderlingKillsCriteria);
			Migrate2(cl, c => c.SpiderEggKillsCriteria);
			Migrate2(cl, c => c.Squid1KillsCriteria);
			Migrate2(cl, c => c.Squid2KillsCriteria);
			Migrate2(cl, c => c.Squid3KillsCriteria);
			Migrate2(cl, c => c.CentipedeKillsCriteria);
			Migrate2(cl, c => c.GigapedeKillsCriteria);
			Migrate2(cl, c => c.GhostpedeKillsCriteria);
			Migrate2(cl, c => c.Spider1KillsCriteria);
			Migrate2(cl, c => c.Spider2KillsCriteria);
			Migrate2(cl, c => c.LeviathanKillsCriteria);
			Migrate2(cl, c => c.OrbKillsCriteria);
			Migrate2(cl, c => c.ThornKillsCriteria);
		}

		_dbContext.SaveChanges();

		return Ok();

		static void Migrate(CustomLeaderboardEntity cl, Func<CustomLeaderboardEntity, CustomLeaderboardCriteriaEntityValue> selector)
		{
			var crit = selector(cl);
			if (crit.Operator != CustomLeaderboardCriteriaOperator.Any)
				crit.Expression = Expression.Parse(crit.Value.ToString()).ToBytes();
		}

		static void Migrate2(CustomLeaderboardEntity cl, Func<CustomLeaderboardEntity, CustomLeaderboardEnemyCriteriaEntityValue> selector)
		{
			var crit = selector(cl);
			if (crit.Operator != CustomLeaderboardCriteriaOperator.Any)
				crit.Expression = Expression.Parse(crit.Value.ToString()).ToBytes();
		}
	}

	[HttpGet]
	[ProducesResponseType(StatusCodes.Status200OK)]
	public async Task<ActionResult<Page<GetCustomLeaderboardForOverview>>> GetCustomLeaderboards(
		[Range(0, 1000)] int pageIndex = 0,
		[Range(Constants.PageSizeMin, Constants.PageSizeMax)] int pageSize = Constants.PageSizeDefault,
		CustomLeaderboardSorting? sortBy = null,
		bool ascending = false)
		=> await _customLeaderboardRepository.GetCustomLeaderboardsAsync(pageIndex, pageSize, sortBy, ascending);

	[HttpGet("{id}")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<ActionResult<GetCustomLeaderboard>> GetCustomLeaderboardById(int id)
		=> await _customLeaderboardRepository.GetCustomLeaderboardAsync(id);

	[HttpPost]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public async Task<ActionResult> AddCustomLeaderboard(AddCustomLeaderboard addCustomLeaderboard)
	{
		await _customLeaderboardService.AddCustomLeaderboardAsync(addCustomLeaderboard);
		return Ok();
	}

	[HttpPut("{id}")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<ActionResult> EditCustomLeaderboardById(int id, EditCustomLeaderboard editCustomLeaderboard)
	{
		await _customLeaderboardService.EditCustomLeaderboardAsync(id, editCustomLeaderboard);
		return Ok();
	}

	[HttpDelete("{id}")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<ActionResult> DeleteCustomLeaderboardById(int id)
	{
		await _customLeaderboardService.DeleteCustomLeaderboardAsync(id);
		return Ok();
	}
}
