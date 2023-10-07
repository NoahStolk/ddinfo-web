using DevilDaggersInfo.Core.Common.Extensions;
using DevilDaggersInfo.Web.ApiSpec.Main;
using DevilDaggersInfo.Web.ApiSpec.Main.Mods;
using DevilDaggersInfo.Web.Client;
using DevilDaggersInfo.Web.Server.Converters.DomainToApi.Main;
using DevilDaggersInfo.Web.Server.Domain.Entities;
using DevilDaggersInfo.Web.Server.Domain.Models.FileSystem;
using DevilDaggersInfo.Web.Server.Domain.Models.ModArchives;
using DevilDaggersInfo.Web.Server.Domain.Services;
using DevilDaggersInfo.Web.Server.Domain.Services.Inversion;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Net.Mime;

namespace DevilDaggersInfo.Web.Server.Controllers.Main;

[Route("api/mods")]
[ApiController]
public class ModsController : ControllerBase
{
	private readonly ApplicationDbContext _dbContext;
	private readonly IFileSystemService _fileSystemService;
	private readonly ModArchiveAccessor _modArchiveAccessor;

	public ModsController(ApplicationDbContext dbContext, IFileSystemService fileSystemService, ModArchiveAccessor modArchiveAccessor)
	{
		_dbContext = dbContext;
		_fileSystemService = fileSystemService;
		_modArchiveAccessor = modArchiveAccessor;
	}

	[HttpGet]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public async Task<ActionResult<Page<GetModOverview>>> GetMods(
		bool onlyHosted,
		string? modFilter = null,
		string? authorFilter = null,
		[Range(0, 1000)] int pageIndex = 0,
		[Range(Constants.PageSizeMin, Constants.PageSizeMax)] int pageSize = Constants.PageSizeDefault,
		ModSorting? sortBy = null,
		bool ascending = false)
	{
		// ! Navigation property.
		IEnumerable<ModEntity> modsQuery = _dbContext.Mods
			.AsNoTracking()
			.Include(am => am.PlayerMods!)
				.ThenInclude(pam => pam.Player)
			.Where(am => !am.IsHidden);

		// OrdinalIgnoreCase works here because this is an IEnumerable. Spawnset and custom leaderboard queries do not require this, and use IQueryable, so OrdinalIgnoreCase will not work there and casing is ignored by default.
		if (!string.IsNullOrWhiteSpace(modFilter))
			modsQuery = modsQuery.Where(m => m.Name.Contains(modFilter, StringComparison.OrdinalIgnoreCase));

		if (!string.IsNullOrWhiteSpace(authorFilter))
		{
			// ! Navigation property.
			modsQuery = modsQuery.Where(m => m.PlayerMods!.Any(pm => pm.Player!.PlayerName.Contains(authorFilter, StringComparison.OrdinalIgnoreCase)));
		}

		List<ModEntity> mods = modsQuery.ToList();

		Dictionary<ModEntity, ModFileSystemData> data = new();
		foreach (ModEntity mod in mods)
		{
			ModFileSystemData modData = await _modArchiveAccessor.GetModFileSystemDataAsync(mod.Name);
			data.Add(mod, modData);
		}

		if (onlyHosted)
			data = data.Where(kvp => kvp.Value.ModArchive != null).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

		List<GetModOverview> modApiModels = data
			.Select(kvp => kvp.Key.ToMainApiOverview(kvp.Value))
			.ToList();

		modApiModels = (sortBy switch
		{
			ModSorting.Name => modApiModels.OrderBy(m => m.Name.ToLower(), ascending),
			ModSorting.Authors => modApiModels.OrderBy(m => m.Authors.FirstOrDefault()?.ToLower(), ascending),
			ModSorting.LastUpdated => modApiModels.OrderBy(m => m.LastUpdated, ascending),
			ModSorting.ModTypes => modApiModels.OrderBy(m => m.ModTypes, ascending),
			ModSorting.Hosted => modApiModels.OrderBy(m => m.IsHosted, ascending),
			ModSorting.ProhibitedAssets => modApiModels.OrderBy(m => m.ContainsProhibitedAssets, ascending),
			_ => modApiModels.OrderBy(m => m.Id, ascending),
		}).ToList();

		int totalMods = data.Count;
		int lastPageIndex = totalMods / pageSize;
		modApiModels = modApiModels
			.Skip(Math.Min(pageIndex, lastPageIndex) * pageSize)
			.Take(pageSize)
			.ToList();

		return new Page<GetModOverview>
		{
			Results = modApiModels,
			TotalResults = totalMods,
		};
	}

	[HttpGet("{id}")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<ActionResult<GetMod>> GetModById([Required] int id)
	{
		// ! Navigation property.
		ModEntity? modEntity = _dbContext.Mods
			.AsNoTracking()
			.Include(m => m.PlayerMods!)
				.ThenInclude(pm => pm.Player)
			.FirstOrDefault(m => m.Id == id);
		if (modEntity == null)
			return NotFound();

		ModFileSystemData data = await _modArchiveAccessor.GetModFileSystemDataAsync(modEntity.Name);

		return modEntity.ToMainApi(data);
	}

	[HttpGet("total-data")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	public ActionResult<GetTotalModData> GetTotalModData()
	{
		return new GetTotalModData
		{
			Count = _dbContext.Mods.AsNoTracking().Select(m => m.Id).Count(),
		};
	}

	// FORBIDDEN: Used by DDAE 1.4.0.0.
	[HttpGet("{modName}/file")]
	[ProducesResponseType(typeof(FileContentResult), StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<ActionResult> GetModFile([Required] string modName)
	{
		if (!_dbContext.Mods.Any(m => m.Name == modName))
			return NotFound();

		string fileName = $"{modName}.zip";
		string path = Path.Combine(_fileSystemService.GetPath(DataSubDirectory.Mods), fileName);
		if (!IoFile.Exists(path))
			return BadRequest($"Mod file '{fileName}' does not exist.");

		return File(await IoFile.ReadAllBytesAsync(path), MediaTypeNames.Application.Zip, fileName);
	}

	[HttpGet("by-author")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	public ActionResult<List<GetModName>> GetModsByAuthorId([Required] int playerId)
	{
		// ! Navigation property.
		var mods = _dbContext.Mods
			.AsNoTracking()
			.Include(m => m.PlayerMods)
			.Select(m => new { m.Id, m.Name, m.PlayerMods, m.LastUpdated })
			.Where(m => m.PlayerMods!.Any(pm => pm.PlayerId == playerId))
			.OrderByDescending(m => m.LastUpdated)
			.ToList();

		return mods.ConvertAll(s => new GetModName
		{
			Id = s.Id,
			Name = s.Name,
		});
	}
}
