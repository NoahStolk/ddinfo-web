using DevilDaggersInfo.Api.Admin;
using DevilDaggersInfo.Api.Admin.Mods;
using DevilDaggersInfo.Web.Client;
using DevilDaggersInfo.Web.Core.Claims;
using DevilDaggersInfo.Web.Server.Converters.DomainToApi.Admin;
using DevilDaggersInfo.Web.Server.Domain.Admin.Services;
using DevilDaggersInfo.Web.Server.Domain.Extensions;
using DevilDaggersInfo.Web.Server.Domain.Models.ModArchives;
using DevilDaggersInfo.Web.Server.Domain.Services;
using Microsoft.AspNetCore.Authorization;

namespace DevilDaggersInfo.Web.Server.Controllers.Admin;

[Route("api/admin/mods")]
[ApiController]
public class ModsController : ControllerBase
{
	private readonly ApplicationDbContext _dbContext;
	private readonly ModArchiveAccessor _modArchiveAccessor;
	private readonly ModService _modService;

	public ModsController(ApplicationDbContext dbContext, ModArchiveAccessor modArchiveAccessor, ModService modService)
	{
		_dbContext = dbContext;
		_modArchiveAccessor = modArchiveAccessor;
		_modService = modService;
	}

	[HttpGet]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[Authorize(Roles = Roles.Mods)]
	public ActionResult<Page<GetModForOverview>> GetMods(
		[Range(0, 1000)] int pageIndex = 0,
		[Range(Constants.PageSizeMin, Constants.PageSizeMax)] int pageSize = Constants.PageSizeDefault,
		ModSorting? sortBy = null,
		bool ascending = false)
	{
		IQueryable<ModEntity> modsQuery = _dbContext.Mods.AsNoTracking();

		modsQuery = sortBy switch
		{
			ModSorting.ModTypes => modsQuery.OrderBy(m => m.ModTypes, ascending).ThenBy(m => m.Id),
			ModSorting.HtmlDescription => modsQuery.OrderBy(m => m.HtmlDescription, ascending).ThenBy(m => m.Id),
			ModSorting.IsHidden => modsQuery.OrderBy(m => m.IsHidden, ascending).ThenBy(m => m.Id),
			ModSorting.LastUpdated => modsQuery.OrderBy(m => m.LastUpdated, ascending).ThenBy(m => m.Id),
			ModSorting.Name => modsQuery.OrderBy(m => m.Name, ascending).ThenBy(m => m.Id),
			ModSorting.TrailerUrl => modsQuery.OrderBy(m => m.TrailerUrl, ascending).ThenBy(m => m.Id),
			ModSorting.Url => modsQuery.OrderBy(m => m.Url, ascending).ThenBy(m => m.Id),
			_ => modsQuery.OrderBy(m => m.Id, ascending),
		};

		List<ModEntity> mods = modsQuery
			.Skip(pageIndex * pageSize)
			.Take(pageSize)
			.ToList();

		return new Page<GetModForOverview>
		{
			Results = mods.ConvertAll(m => m.ToGetModForOverview()),
			TotalResults = _dbContext.Mods.Count(),
		};
	}

	[HttpGet("names")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[Authorize(Roles = Roles.Players)]
	public ActionResult<List<GetModName>> GetModNames()
	{
		var mods = _dbContext.Mods
			.AsNoTracking()
			.Select(m => new { m.Id, m.Name })
			.ToList();

		return mods.ConvertAll(m => new GetModName
		{
			Id = m.Id,
			Name = m.Name,
		});
	}

	[HttpGet("{id}")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	[Authorize(Roles = Roles.Mods)]
	public ActionResult<GetMod> GetModById(int id)
	{
		ModEntity? mod = _dbContext.Mods
			.AsSingleQuery()
			.AsNoTracking()
			.Include(m => m.PlayerMods)
			.FirstOrDefault(m => m.Id == id);
		if (mod == null)
			return NotFound();

		ModFileSystemData mfsd = _modArchiveAccessor.GetModFileSystemData(mod.Name);
		return mod.ToGetMod(mfsd.ModArchive?.Binaries.ConvertAll(macd => macd.Name), mfsd?.ScreenshotFileNames);
	}

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
