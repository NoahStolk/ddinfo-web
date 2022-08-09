using DevilDaggersInfo.Api.Admin;
using DevilDaggersInfo.Api.Admin.Spawnsets;
using DevilDaggersInfo.Web.Client;
using DevilDaggersInfo.Web.Core.Claims;
using DevilDaggersInfo.Web.Server.Converters.DomainToApi.Admin;
using DevilDaggersInfo.Web.Server.Domain.Extensions;
using DevilDaggersInfo.Web.Server.Domain.Services;
using Microsoft.AspNetCore.Authorization;

namespace DevilDaggersInfo.Web.Server.Controllers.Admin;

[Route("api/admin/spawnsets")]
[ApiController]
public class SpawnsetsController : ControllerBase
{
	private readonly ApplicationDbContext _dbContext;
	private readonly SpawnsetService _spawnsetService;

	public SpawnsetsController(ApplicationDbContext dbContext, SpawnsetService spawnsetService)
	{
		_dbContext = dbContext;
		_spawnsetService = spawnsetService;
	}

	[HttpGet]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[Authorize(Roles = Roles.Spawnsets)]
	public ActionResult<Page<GetSpawnsetForOverview>> GetSpawnsets(
		[Range(0, 1000)] int pageIndex = 0,
		[Range(Constants.PageSizeMin, Constants.PageSizeMax)] int pageSize = Constants.PageSizeDefault,
		SpawnsetSorting? sortBy = null,
		bool ascending = false)
	{
		IQueryable<SpawnsetEntity> spawnsetsQuery = _dbContext.Spawnsets.AsNoTracking().Include(s => s.Player);

		spawnsetsQuery = sortBy switch
		{
			SpawnsetSorting.Author => spawnsetsQuery.OrderBy(s => s.Player.PlayerName, ascending).ThenBy(s => s.Id),
			SpawnsetSorting.HtmlDescription => spawnsetsQuery.OrderBy(s => s.HtmlDescription, ascending).ThenBy(s => s.Id),
			SpawnsetSorting.IsPractice => spawnsetsQuery.OrderBy(s => s.IsPractice, ascending).ThenBy(s => s.Id),
			SpawnsetSorting.LastUpdated => spawnsetsQuery.OrderBy(s => s.LastUpdated, ascending).ThenBy(s => s.Id),
			SpawnsetSorting.MaxDisplayWaves => spawnsetsQuery.OrderBy(s => s.MaxDisplayWaves, ascending).ThenBy(s => s.Id),
			SpawnsetSorting.Name => spawnsetsQuery.OrderBy(s => s.Name, ascending).ThenBy(s => s.Id),
			_ => spawnsetsQuery.OrderBy(s => s.Id, ascending),
		};

		List<SpawnsetEntity> spawnsets = spawnsetsQuery
			.Skip(pageIndex * pageSize)
			.Take(pageSize)
			.ToList();

		return new Page<GetSpawnsetForOverview>
		{
			Results = spawnsets.ConvertAll(s => s.ToGetSpawnsetForOverview()),
			TotalResults = _dbContext.Spawnsets.Count(),
		};
	}

	[HttpGet("names")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[Authorize(Roles = Roles.CustomLeaderboards)]
	public ActionResult<List<GetSpawnsetName>> GetSpawnsetNames()
	{
		var spawnsets = _dbContext.Spawnsets
			.AsNoTracking()
			.Select(s => new { s.Id, s.Name })
			.ToList();

		return spawnsets.ConvertAll(s => new GetSpawnsetName
		{
			Id = s.Id,
			Name = s.Name,
		});
	}

	[HttpGet("{id}")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	[Authorize(Roles = Roles.Spawnsets)]
	public ActionResult<GetSpawnset> GetSpawnsetById(int id)
	{
		SpawnsetEntity? spawnset = _dbContext.Spawnsets
			.AsNoTracking()
			.FirstOrDefault(p => p.Id == id);
		if (spawnset == null)
			return NotFound();

		return spawnset.ToGetSpawnset();
	}

	[HttpPost]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[Authorize(Roles = Roles.Spawnsets)]
	public async Task<ActionResult> AddSpawnset(AddSpawnset addSpawnset)
	{
		await _spawnsetService.AddSpawnsetAsync(new Domain.Commands.Spawnsets.AddSpawnset
		{
			FileContents = addSpawnset.FileContents,
			HtmlDescription = addSpawnset.HtmlDescription,
			IsPractice = addSpawnset.IsPractice,
			MaxDisplayWaves = addSpawnset.MaxDisplayWaves,
			Name = addSpawnset.Name,
			PlayerId = addSpawnset.PlayerId,
		});

		return Ok();
	}

	[HttpPut("{id}")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	[Authorize(Roles = Roles.Spawnsets)]
	public async Task<ActionResult> EditSpawnsetById(int id, EditSpawnset editSpawnset)
	{
		await _spawnsetService.EditSpawnsetAsync(new Domain.Commands.Spawnsets.EditSpawnset
		{
			HtmlDescription = editSpawnset.HtmlDescription,
			IsPractice = editSpawnset.IsPractice,
			MaxDisplayWaves = editSpawnset.MaxDisplayWaves,
			Name = editSpawnset.Name,
			PlayerId = editSpawnset.PlayerId,
			SpawnsetId = id,
		});

		return Ok();
	}

	[HttpDelete("{id}")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	[Authorize(Roles = Roles.Spawnsets)]
	public async Task<ActionResult> DeleteSpawnsetById(int id)
	{
		await _spawnsetService.DeleteSpawnsetAsync(id);

		return Ok();
	}
}
