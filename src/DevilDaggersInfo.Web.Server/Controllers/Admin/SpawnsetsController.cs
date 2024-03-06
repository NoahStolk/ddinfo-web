using DevilDaggersInfo.Web.ApiSpec.Admin;
using DevilDaggersInfo.Web.ApiSpec.Admin.Spawnsets;
using DevilDaggersInfo.Web.Client;
using DevilDaggersInfo.Web.Core.Claims;
using DevilDaggersInfo.Web.Server.Domain.Admin.Repositories;
using DevilDaggersInfo.Web.Server.Domain.Admin.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

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
		string? filter = null,
		[Range(0, 1000)] int pageIndex = 0,
		[Range(Constants.PageSizeMin, Constants.PageSizeMax)] int pageSize = Constants.PageSizeDefault,
		SpawnsetSorting? sortBy = null,
		bool ascending = false)
	{
		return await _spawnsetRepository.GetSpawnsetsAsync(filter, pageIndex, pageSize, sortBy, ascending);
	}

	[HttpGet("names")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[Authorize(Roles = Roles.CustomLeaderboards)]
	public async Task<ActionResult<List<GetSpawnsetName>>> GetSpawnsetNames()
	{
		return await _spawnsetRepository.GetSpawnsetNamesAsync();
	}

	[HttpGet("{id}")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	[Authorize(Roles = Roles.Spawnsets)]
	public async Task<ActionResult<GetSpawnset>> GetSpawnsetById(int id)
	{
		return await _spawnsetRepository.GetSpawnset(id);
	}

	[HttpPost]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[Authorize(Roles = Roles.Spawnsets)]
	public async Task<ActionResult> AddSpawnset(AddSpawnset addSpawnset)
	{
		await _spawnsetService.AddSpawnsetAsync(addSpawnset);
		return Ok();
	}

	[HttpPut("{id}")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	[Authorize(Roles = Roles.Spawnsets)]
	public async Task<ActionResult> EditSpawnsetById(int id, EditSpawnset editSpawnset)
	{
		await _spawnsetService.EditSpawnsetAsync(id, editSpawnset);
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
