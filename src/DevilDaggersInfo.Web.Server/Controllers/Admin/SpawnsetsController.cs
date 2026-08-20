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
public sealed class SpawnsetsController(SpawnsetRepository spawnsetRepository, SpawnsetService spawnsetService) : ControllerBase
{
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
		return await spawnsetRepository.GetSpawnsetsAsync(filter, pageIndex, pageSize, sortBy, ascending);
	}

	[HttpGet("names")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[Authorize(Roles = Roles.CustomLeaderboards)]
	public async Task<ActionResult<List<GetSpawnsetName>>> GetSpawnsetNames()
	{
		return await spawnsetRepository.GetSpawnsetNamesAsync();
	}

	[HttpGet("{id}")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	[Authorize(Roles = Roles.Spawnsets)]
	public async Task<ActionResult<GetSpawnset>> GetSpawnsetById(int id)
	{
		return await spawnsetRepository.GetSpawnset(id);
	}

	[HttpPost]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[Authorize(Roles = Roles.Spawnsets)]
	public async Task<ActionResult> AddSpawnset(AddSpawnset addSpawnset)
	{
		await spawnsetService.AddSpawnsetAsync(addSpawnset);
		return Ok();
	}

	[HttpPut("{id}")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	[Authorize(Roles = Roles.Spawnsets)]
	public async Task<ActionResult> EditSpawnsetById(int id, EditSpawnset editSpawnset)
	{
		await spawnsetService.EditSpawnsetAsync(id, editSpawnset);
		return Ok();
	}

	[HttpDelete("{id}")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	[Authorize(Roles = Roles.Spawnsets)]
	public async Task<ActionResult> DeleteSpawnsetById(int id)
	{
		await spawnsetService.DeleteSpawnsetAsync(id);
		return Ok();
	}
}
