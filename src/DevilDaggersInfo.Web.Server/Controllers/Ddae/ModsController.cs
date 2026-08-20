using DevilDaggersInfo.Web.ApiSpec.Ddae.Mods;
using DevilDaggersInfo.Web.Server.Converters.DomainToApi.Ddae;
using DevilDaggersInfo.Web.Server.Domain.Entities;
using DevilDaggersInfo.Web.Server.Domain.Models.FileSystem;
using DevilDaggersInfo.Web.Server.Domain.Models.ModArchives;
using DevilDaggersInfo.Web.Server.Domain.Services;
using DevilDaggersInfo.Web.Server.Domain.Services.Inversion;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Net.Mime;

namespace DevilDaggersInfo.Web.Server.Controllers.Ddae;

[Route("api/ddae/mods")]
[ApiController]
public sealed class ModsController(ApplicationDbContext dbContext, ModArchiveAccessor modArchiveAccessor, IFileSystemService fileSystemService) : ControllerBase
{
	[HttpGet]
	[ProducesResponseType(StatusCodes.Status200OK)]
	public async Task<List<GetModDdae>> GetMods(string? authorFilter = null, string? nameFilter = null, bool? isHostedFilter = null)
	{
		// ! Navigation property.
		IEnumerable<ModEntity> modsQuery = dbContext.Mods
			.AsNoTracking()
			.Include(m => m.PlayerMods!)
				.ThenInclude(pm => pm.Player)
			.Where(m => !m.IsHidden);

		if (!string.IsNullOrWhiteSpace(authorFilter))
		{
			authorFilter = authorFilter.Trim();

			// ! Navigation property.
			modsQuery = modsQuery.Where(m => m.PlayerMods!.Exists(pm => pm.Player!.PlayerName.Contains(authorFilter, StringComparison.InvariantCultureIgnoreCase)));
		}

		if (!string.IsNullOrWhiteSpace(nameFilter))
		{
			nameFilter = nameFilter.Trim();
			modsQuery = modsQuery.Where(m => m.Name.Contains(nameFilter, StringComparison.InvariantCultureIgnoreCase));
		}

		List<ModEntity> mods = [.. modsQuery];

		Dictionary<ModEntity, ModFileSystemData> data = new();
		foreach (ModEntity mod in mods)
		{
			ModFileSystemData modData = await modArchiveAccessor.GetModFileSystemDataAsync(mod.Name);
			data.Add(mod, modData);
		}

		if (isHostedFilter.HasValue)
			data = data.Where(kvp => kvp.Value.ModArchive != null).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

		return [.. data.Select(kvp => kvp.Key.ToDdaeApi(kvp.Value))];
	}

	[HttpGet("{modName}/file")]
	[ProducesResponseType(typeof(FileContentResult), StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<ActionResult> GetModFile([Required] string modName)
	{
		if (!await dbContext.Mods.AnyAsync(m => m.Name == modName))
			return NotFound();

		string fileName = $"{modName}.zip";
		string path = Path.Combine(fileSystemService.GetPath(DataSubDirectory.Mods), fileName);
		if (!IoFile.Exists(path))
			return BadRequest($"Mod file '{fileName}' does not exist.");

		return File(await IoFile.ReadAllBytesAsync(path), MediaTypeNames.Application.Zip, fileName);
	}
}
