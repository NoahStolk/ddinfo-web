using DevilDaggersInfo.Api.Admin;
using DevilDaggersInfo.Api.Admin.Spawnsets;
using DevilDaggersInfo.Web.Client;
using DevilDaggersInfo.Web.Core.Claims;
using DevilDaggersInfo.Web.Server.Domain.Admin.Repositories;
using DevilDaggersInfo.Web.Server.Domain.Admin.Services;
using Microsoft.AspNetCore.Authorization;

namespace DevilDaggersInfo.Web.Server.Controllers.Admin;

[Route("api/admin/spawnsets")]
[ApiController]
public class SpawnsetsController : ControllerBase
{
	private readonly SpawnsetRepository _spawnsetRepository;
	private readonly SpawnsetService _spawnsetService;

	public SpawnsetsController(SpawnsetRepository spawnsetRepository, SpawnsetService spawnsetService)
	{
		_spawnsetRepository = spawnsetRepository;
		_spawnsetService = spawnsetService;
	}

	[HttpGet]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[Authorize(Roles = Roles.Spawnsets)]
	public async Task<ActionResult<Page<GetSpawnsetForOverview>>> GetSpawnsets(
		[Range(0, 1000)] int pageIndex = 0,
		[Range(Constants.PageSizeMin, Constants.PageSizeMax)] int pageSize = Constants.PageSizeDefault,
		SpawnsetSorting? sortBy = null,
		bool ascending = false)
		=> await _spawnsetRepository.GetSpawnsetsAsync(pageIndex, pageSize, sortBy, ascending);

	[HttpGet("names")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[Authorize(Roles = Roles.CustomLeaderboards)]
	public async Task<ActionResult<List<GetSpawnsetName>>> GetSpawnsetNames()
		=> await _spawnsetRepository.GetSpawnsetNamesAsync();

	[HttpGet("{id}")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	[Authorize(Roles = Roles.Spawnsets)]
	public async Task<ActionResult<GetSpawnset>> GetSpawnsetById(int id)
		=> await _spawnsetRepository.GetSpawnset(id);

	[HttpPost]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[Authorize(Roles = Roles.Spawnsets)]
	public async Task<ActionResult> AddSpawnset(AddSpawnset addSpawnset)
	{
		await _spawnsetService.AddSpawnsetAsync(new Domain.Admin.Commands.Spawnsets.AddSpawnset
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
		await _spawnsetService.EditSpawnsetAsync(new Domain.Admin.Commands.Spawnsets.EditSpawnset
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
