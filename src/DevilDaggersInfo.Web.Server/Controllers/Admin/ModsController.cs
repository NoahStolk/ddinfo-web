using DevilDaggersInfo.Web.ApiSpec.Admin;
using DevilDaggersInfo.Web.ApiSpec.Admin.Mods;
using DevilDaggersInfo.Web.Client;
using DevilDaggersInfo.Web.Core.Claims;
using DevilDaggersInfo.Web.Server.Domain.Admin.Repositories;
using DevilDaggersInfo.Web.Server.Domain.Admin.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace DevilDaggersInfo.Web.Server.Controllers.Admin;

[Route("api/admin/mods")]
[ApiController]
public sealed class ModsController(ModRepository modRepository, ModService modService) : ControllerBase
{
	[HttpGet]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[Authorize(Roles = Roles.Mods)]
	public async Task<ActionResult<Page<GetModForOverview>>> GetMods(
		string? filter = null,
		[Range(0, 1000)] int pageIndex = 0,
		[Range(Constants.PageSizeMin, Constants.PageSizeMax)] int pageSize = Constants.PageSizeDefault,
		ModSorting? sortBy = null,
		bool ascending = false)
	{
		return await modRepository.GetModsAsync(filter, pageIndex, pageSize, sortBy, ascending);
	}

	[HttpGet("names")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[Authorize(Roles = Roles.Players)]
	public async Task<ActionResult<List<GetModName>>> GetModNames()
	{
		return await modRepository.GetModNamesAsync();
	}

	[HttpGet("{id}")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	[Authorize(Roles = Roles.Mods)]
	public async Task<ActionResult<GetMod>> GetModById(int id)
	{
		return await modRepository.GetModAsync(id);
	}

	[HttpPost]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[Authorize(Roles = Roles.Mods)]
	public async Task<ActionResult> AddMod(AddMod addMod)
	{
		await modService.AddModAsync(addMod);
		return Ok();
	}

	[HttpPut("{id}")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	[Authorize(Roles = Roles.Mods)]
	public async Task<ActionResult> EditModById(int id, EditMod editMod)
	{
		await modService.EditModAsync(id, editMod);
		return Ok();
	}

	[HttpDelete("{id}")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	[Authorize(Roles = Roles.Mods)]
	public async Task<ActionResult> DeleteModById(int id)
	{
		await modService.DeleteModAsync(id);
		return Ok();
	}
}
