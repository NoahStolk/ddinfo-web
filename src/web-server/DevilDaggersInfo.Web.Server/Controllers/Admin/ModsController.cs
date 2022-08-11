using DevilDaggersInfo.Api.Admin;
using DevilDaggersInfo.Api.Admin.Mods;
using DevilDaggersInfo.Web.Client;
using DevilDaggersInfo.Web.Core.Claims;
using DevilDaggersInfo.Web.Server.Domain.Admin.Repositories;
using DevilDaggersInfo.Web.Server.Domain.Admin.Services;
using Microsoft.AspNetCore.Authorization;

namespace DevilDaggersInfo.Web.Server.Controllers.Admin;

[Route("api/admin/mods")]
[ApiController]
public class ModsController : ControllerBase
{
	private readonly ModRepository _modRepository;
	private readonly ModService _modService;

	public ModsController(ModRepository modRepository, ModService modService)
	{
		_modRepository = modRepository;
		_modService = modService;
	}

	[HttpGet]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[Authorize(Roles = Roles.Mods)]
	public async Task<ActionResult<Page<GetModForOverview>>> GetMods(
		[Range(0, 1000)] int pageIndex = 0,
		[Range(Constants.PageSizeMin, Constants.PageSizeMax)] int pageSize = Constants.PageSizeDefault,
		ModSorting? sortBy = null,
		bool ascending = false)
		=> await _modRepository.GetModsAsync(pageIndex, pageSize, sortBy, ascending);

	[HttpGet("names")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[Authorize(Roles = Roles.Players)]
	public async Task<ActionResult<List<GetModName>>> GetModNames()
		=> await _modRepository.GetModNamesAsync();

	[HttpGet("{id}")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	[Authorize(Roles = Roles.Mods)]
	public async Task<ActionResult<GetMod>> GetModById(int id)
		=> await _modRepository.GetModAsync(id);

	[HttpPost]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[Authorize(Roles = Roles.Mods)]
	public async Task<ActionResult> AddMod(AddMod addMod)
	{
		await _modService.AddModAsync(new Domain.Admin.Commands.Mods.AddMod
		{
			Name = addMod.Name,
			Binaries = addMod.Binaries.ConvertAll(b => new Domain.Admin.Commands.Mods.Models.BinaryData
			{
				Data = b.Data,
				Name = b.Name,
			}),
			HtmlDescription = addMod.HtmlDescription,
			IsHidden = addMod.IsHidden,
			ModTypes = addMod.ModTypes,
			PlayerIds = addMod.PlayerIds,
			Screenshots = addMod.Screenshots,
			TrailerUrl = addMod.TrailerUrl,
			Url = addMod.Url,
		});
		return Ok();
	}

	[HttpPut("{id}")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	[Authorize(Roles = Roles.Mods)]
	public async Task<ActionResult> EditModById(int id, EditMod editMod)
	{
		await _modService.EditModAsync(new Domain.Admin.Commands.Mods.EditMod
		{
			Binaries = editMod.Binaries.ConvertAll(b => new Domain.Admin.Commands.Mods.Models.BinaryData
			{
				Data = b.Data,
				Name = b.Name,
			}),
			BinariesToDelete = editMod.BinariesToDelete,
			HtmlDescription = editMod.HtmlDescription,
			Id = id,
			IsHidden = editMod.IsHidden,
			ModTypes = editMod.ModTypes,
			Name = editMod.Name,
			PlayerIds = editMod.PlayerIds,
			Screenshots = editMod.Screenshots,
			ScreenshotsToDelete = editMod.ScreenshotsToDelete,
			TrailerUrl = editMod.TrailerUrl,
			Url = editMod.Url,
		});
		return Ok();
	}

	[HttpDelete("{id}")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	[Authorize(Roles = Roles.Mods)]
	public async Task<ActionResult> DeleteModById(int id)
	{
		await _modService.DeleteModAsync(id);
		return Ok();
	}
}
