using DevilDaggersInfo.Api.Admin;
using DevilDaggersInfo.Api.Admin.CustomLeaderboards;
using DevilDaggersInfo.Web.Client;
using DevilDaggersInfo.Web.Core.Claims;
using DevilDaggersInfo.Web.Server.Converters.ApiToDomain.Admin;
using DevilDaggersInfo.Web.Server.Converters.DomainToApi.Admin;
using DevilDaggersInfo.Web.Server.Domain.Extensions;
using DevilDaggersInfo.Web.Server.Domain.Services;
using Microsoft.AspNetCore.Authorization;

namespace DevilDaggersInfo.Web.Server.Controllers.Admin;

[Route("api/admin/custom-leaderboards")]
[ApiController]
[Authorize(Roles = Roles.CustomLeaderboards)]
public class CustomLeaderboardsController : ControllerBase
{
	private readonly ApplicationDbContext _dbContext;
	private readonly CustomLeaderboardValidator _validator;

	public CustomLeaderboardsController(ApplicationDbContext dbContext, CustomLeaderboardValidator validator)
	{
		_dbContext = dbContext;
		_validator = validator;
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
		if (addCustomLeaderboard.Category == CustomLeaderboardCategory.Speedrun)
			return BadRequest("The Speedrun category is obsolete and should not be used anymore. Consider using the Race category.");

		if (_dbContext.CustomLeaderboards.Any(cl => cl.SpawnsetId == addCustomLeaderboard.SpawnsetId))
			return BadRequest("A leaderboard for this spawnset already exists.");

		_validator.ValidateCustomLeaderboard(addCustomLeaderboard.SpawnsetId, addCustomLeaderboard.Category.ToDomain(), addCustomLeaderboard.Daggers.ToDomain(), addCustomLeaderboard.IsFeatured);

		CustomLeaderboardEntity customLeaderboard = new()
		{
			DateCreated = DateTime.UtcNow,
			SpawnsetId = addCustomLeaderboard.SpawnsetId,
			Category = addCustomLeaderboard.Category.ToDomain(),
			TimeBronze = addCustomLeaderboard.Daggers.Bronze.To10thMilliTime(),
			TimeSilver = addCustomLeaderboard.Daggers.Silver.To10thMilliTime(),
			TimeGolden = addCustomLeaderboard.Daggers.Golden.To10thMilliTime(),
			TimeDevil = addCustomLeaderboard.Daggers.Devil.To10thMilliTime(),
			TimeLeviathan = addCustomLeaderboard.Daggers.Leviathan.To10thMilliTime(),
			IsFeatured = addCustomLeaderboard.IsFeatured,
		};
		_dbContext.CustomLeaderboards.Add(customLeaderboard);
		await _dbContext.SaveChangesAsync();

		return Ok(customLeaderboard.Id);
	}

	[HttpPut("{id}")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<ActionResult> EditCustomLeaderboardById(int id, EditCustomLeaderboard editCustomLeaderboard)
	{
		CustomLeaderboardEntity? customLeaderboard = _dbContext.CustomLeaderboards.FirstOrDefault(cl => cl.Id == id);
		if (customLeaderboard == null)
			return NotFound();

		if (customLeaderboard.Category != editCustomLeaderboard.Category.ToDomain() && _dbContext.CustomEntries.Any(ce => ce.CustomLeaderboardId == id))
			return BadRequest("Cannot change category for custom leaderboard with scores.");

		_validator.ValidateCustomLeaderboard(customLeaderboard.SpawnsetId, editCustomLeaderboard.Category.ToDomain(), editCustomLeaderboard.Daggers.ToDomain(), editCustomLeaderboard.IsFeatured);

		customLeaderboard.Category = editCustomLeaderboard.Category.ToDomain();
		customLeaderboard.TimeBronze = editCustomLeaderboard.Daggers.Bronze.To10thMilliTime();
		customLeaderboard.TimeSilver = editCustomLeaderboard.Daggers.Silver.To10thMilliTime();
		customLeaderboard.TimeGolden = editCustomLeaderboard.Daggers.Golden.To10thMilliTime();
		customLeaderboard.TimeDevil = editCustomLeaderboard.Daggers.Devil.To10thMilliTime();
		customLeaderboard.TimeLeviathan = editCustomLeaderboard.Daggers.Leviathan.To10thMilliTime();

		customLeaderboard.IsFeatured = editCustomLeaderboard.IsFeatured;
		await _dbContext.SaveChangesAsync();

		return Ok();
	}

	[HttpDelete("{id}")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<ActionResult> DeleteCustomLeaderboardById(int id)
	{
		CustomLeaderboardEntity? customLeaderboard = _dbContext.CustomLeaderboards.FirstOrDefault(cl => cl.Id == id);
		if (customLeaderboard == null)
			return NotFound();

		if (_dbContext.CustomEntries.Any(ce => ce.CustomLeaderboardId == id))
			return BadRequest("Custom leaderboard with scores cannot be deleted.");

		_dbContext.CustomLeaderboards.Remove(customLeaderboard);
		await _dbContext.SaveChangesAsync();

		return Ok();
	}
}
