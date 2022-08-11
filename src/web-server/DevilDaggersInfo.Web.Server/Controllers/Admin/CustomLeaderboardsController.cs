using DevilDaggersInfo.Api.Admin;
using DevilDaggersInfo.Api.Admin.CustomLeaderboards;
using DevilDaggersInfo.Web.Client;
using DevilDaggersInfo.Web.Core.Claims;
using DevilDaggersInfo.Web.Server.Converters.ApiToDomain.Admin;
using DevilDaggersInfo.Web.Server.Converters.DomainToApi.Admin;
using DevilDaggersInfo.Web.Server.Domain.Admin.Services;
using DevilDaggersInfo.Web.Server.Domain.Extensions;
using Microsoft.AspNetCore.Authorization;

namespace DevilDaggersInfo.Web.Server.Controllers.Admin;

[Route("api/admin/custom-leaderboards")]
[ApiController]
[Authorize(Roles = Roles.CustomLeaderboards)]
public class CustomLeaderboardsController : ControllerBase
{
	private readonly ApplicationDbContext _dbContext;
	private readonly CustomLeaderboardService _customLeaderboardService;

	public CustomLeaderboardsController(ApplicationDbContext dbContext, CustomLeaderboardService customLeaderboardService)
	{
		_dbContext = dbContext;
		_customLeaderboardService = customLeaderboardService;
	}

	[HttpGet]
	[ProducesResponseType(StatusCodes.Status200OK)]
	public ActionResult<Page<GetCustomLeaderboardForOverview>> GetCustomLeaderboards(
		[Range(0, 1000)] int pageIndex = 0,
		[Range(Constants.PageSizeMin, Constants.PageSizeMax)] int pageSize = Constants.PageSizeDefault,
		CustomLeaderboardSorting? sortBy = null,
		bool ascending = false)
	{
		IQueryable<CustomLeaderboardEntity> customLeaderboardsQuery = _dbContext.CustomLeaderboards
			.AsNoTracking()
			.Include(cl => cl.Spawnset);

		customLeaderboardsQuery = sortBy switch
		{
			CustomLeaderboardSorting.Category => customLeaderboardsQuery.OrderBy(cl => cl.Category, ascending).ThenBy(cl => cl.Id),
			CustomLeaderboardSorting.DateCreated => customLeaderboardsQuery.OrderBy(cl => cl.DateCreated, ascending).ThenBy(cl => cl.Id),
			CustomLeaderboardSorting.IsFeatured => customLeaderboardsQuery.OrderBy(cl => cl.IsFeatured, ascending).ThenBy(cl => cl.Id),
			CustomLeaderboardSorting.SpawnsetName => customLeaderboardsQuery.OrderBy(cl => cl.Spawnset.Name, ascending).ThenBy(cl => cl.Id),
			CustomLeaderboardSorting.TimeBronze => customLeaderboardsQuery.OrderBy(cl => cl.TimeBronze, ascending).ThenBy(cl => cl.Id),
			CustomLeaderboardSorting.TimeSilver => customLeaderboardsQuery.OrderBy(cl => cl.TimeSilver, ascending).ThenBy(cl => cl.Id),
			CustomLeaderboardSorting.TimeGolden => customLeaderboardsQuery.OrderBy(cl => cl.TimeGolden, ascending).ThenBy(cl => cl.Id),
			CustomLeaderboardSorting.TimeDevil => customLeaderboardsQuery.OrderBy(cl => cl.TimeDevil, ascending).ThenBy(cl => cl.Id),
			CustomLeaderboardSorting.TimeLeviathan => customLeaderboardsQuery.OrderBy(cl => cl.TimeLeviathan, ascending).ThenBy(cl => cl.Id),
			_ => customLeaderboardsQuery.OrderBy(cl => cl.Id, ascending),
		};

		List<CustomLeaderboardEntity> customLeaderboards = customLeaderboardsQuery
			.Skip(pageIndex * pageSize)
			.Take(pageSize)
			.ToList();

		return new Page<GetCustomLeaderboardForOverview>
		{
			Results = customLeaderboards.ConvertAll(cl => cl.ToGetCustomLeaderboardForOverview()),
			TotalResults = _dbContext.CustomLeaderboards.Count(),
		};
	}

	[HttpGet("{id}")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public ActionResult<GetCustomLeaderboard> GetCustomLeaderboardById(int id)
	{
		CustomLeaderboardEntity? customLeaderboard = _dbContext.CustomLeaderboards
			.AsNoTracking()
			.Include(cl => cl.Spawnset)
				.ThenInclude(sf => sf.Player)
			.FirstOrDefault(cl => cl.Id == id);

		if (customLeaderboard == null)
			return NotFound($"Leaderboard with ID '{id}' was not found.");

		return customLeaderboard.ToGetCustomLeaderboard();
	}

	[HttpPost]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public async Task<ActionResult> AddCustomLeaderboard(AddCustomLeaderboard addCustomLeaderboard)
	{
		await _customLeaderboardService.AddCustomLeaderboardAsync(new Domain.Admin.Commands.CustomLeaderboards.AddCustomLeaderboard
		{
			Category = addCustomLeaderboard.Category.ToDomain(),
			Daggers = new Domain.Admin.Commands.CustomLeaderboards.Models.CustomLeaderboardDaggers
			{
				Bronze = addCustomLeaderboard.Daggers.Bronze.To10thMilliTime(),
				Silver = addCustomLeaderboard.Daggers.Silver.To10thMilliTime(),
				Golden = addCustomLeaderboard.Daggers.Golden.To10thMilliTime(),
				Devil = addCustomLeaderboard.Daggers.Devil.To10thMilliTime(),
				Leviathan = addCustomLeaderboard.Daggers.Leviathan.To10thMilliTime(),
			},
			IsFeatured = addCustomLeaderboard.IsFeatured,
			SpawnsetId = addCustomLeaderboard.SpawnsetId,
		});
		return Ok();
	}

	[HttpPut("{id}")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<ActionResult> EditCustomLeaderboardById(int id, EditCustomLeaderboard editCustomLeaderboard)
	{
		await _customLeaderboardService.EditCustomLeaderboardAsync(new Domain.Admin.Commands.CustomLeaderboards.EditCustomLeaderboard
		{
			Category = editCustomLeaderboard.Category.ToDomain(),
			Daggers = new Domain.Admin.Commands.CustomLeaderboards.Models.CustomLeaderboardDaggers
			{
				Bronze = editCustomLeaderboard.Daggers.Bronze.To10thMilliTime(),
				Silver = editCustomLeaderboard.Daggers.Silver.To10thMilliTime(),
				Golden = editCustomLeaderboard.Daggers.Golden.To10thMilliTime(),
				Devil = editCustomLeaderboard.Daggers.Devil.To10thMilliTime(),
				Leviathan = editCustomLeaderboard.Daggers.Leviathan.To10thMilliTime(),
			},
			Id = id,
			IsFeatured = editCustomLeaderboard.IsFeatured,
		});
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
