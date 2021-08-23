using DevilDaggersInfo.Core.Asset.Enums;
using DevilDaggersInfo.Core.Mod.Enums;
using DevilDaggersInfo.Web.BlazorWasm.Server.Caches.ModArchives;
using DevilDaggersInfo.Web.BlazorWasm.Server.Controllers.Attributes;
using DevilDaggersInfo.Web.BlazorWasm.Server.Entities;
using DevilDaggersInfo.Web.BlazorWasm.Server.Enums;
using DevilDaggersInfo.Web.BlazorWasm.Server.Transients;
using DevilDaggersInfo.Web.BlazorWasm.Shared.Dto.Public.Mods;
using DevilDaggersInfo.Web.BlazorWasm.Shared.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Net.Mime;
using Io = System.IO;

namespace DevilDaggersInfo.Web.BlazorWasm.Server.Controllers.Public;

[Route("api/mods")]
[ApiController]
public class ModsController : ControllerBase
{
	private readonly ApplicationDbContext _dbContext;
	private readonly IFileSystemService _fileSystemService;
	private readonly ModArchiveCache _modArchiveCache;

	public ModsController(ApplicationDbContext dbContext, IFileSystemService fileSystemService, ModArchiveCache modArchiveCache)
	{
		_dbContext = dbContext;
		_fileSystemService = fileSystemService;
		_modArchiveCache = modArchiveCache;
	}

	[HttpGet]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[EndpointConsumer(EndpointConsumers.Ddae)]
	public List<GetMod> GetPublicMods(string? authorFilter = null, string? nameFilter = null, bool? isHostedFilter = null)
	{
		IEnumerable<ModEntity> modsQuery = _dbContext.Mods
			.AsNoTracking()
			.Include(am => am.PlayerMods)
				.ThenInclude(pam => pam.Player)
			.Where(am => !am.IsHidden);

		if (!string.IsNullOrWhiteSpace(authorFilter))
			modsQuery = modsQuery.Where(am => am.PlayerMods.Any(pam => pam.Player.PlayerName.Contains(authorFilter, StringComparison.InvariantCultureIgnoreCase)));
		if (!string.IsNullOrWhiteSpace(nameFilter))
			modsQuery = modsQuery.Where(am => am.Name.Contains(nameFilter, StringComparison.InvariantCultureIgnoreCase));

		List<ModEntity> mods = modsQuery.ToList();

		Dictionary<ModEntity, (bool FileExists, string? Path)> modsWithFileInfo = mods.ToDictionary(am => am, am =>
		{
			string filePath = Path.Combine(_fileSystemService.GetPath(DataSubDirectory.Mods), $"{am.Name}.zip");
			bool fileExists = Io.File.Exists(filePath);
			return (fileExists, fileExists ? filePath : null);
		});

		if (isHostedFilter.HasValue)
			modsWithFileInfo = modsWithFileInfo.Where(kvp => kvp.Value.FileExists == isHostedFilter.Value).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

		return modsWithFileInfo
			.Select(amwfi =>
			{
				bool? containsProhibitedAssets = null;
				GetModArchive? modArchive = null;
				ModTypes modTypes;
				if (amwfi.Value.FileExists)
				{
					ModArchiveCacheData archiveData = _modArchiveCache.GetArchiveDataByFilePath(amwfi.Value.Path!);
					containsProhibitedAssets = archiveData.Binaries.Any(md => md.Chunks.Any(mad => mad.IsProhibited));
					modArchive = new()
					{
						FileSize = archiveData.FileSize,
						FileSizeExtracted = archiveData.FileSizeExtracted,
						Binaries = archiveData.Binaries.ConvertAll(b => new GetModBinary
						{
							Name = b.Name,
							Size = b.Size,
							ModBinaryType = b.ModBinaryType,
						}),
					};

					ModBinaryCacheData? ddBinary = archiveData.Binaries.Find(md => md.ModBinaryType == ModBinaryType.Dd);

					modTypes = ModTypes.None;
					if (archiveData.Binaries.Any(md => md.ModBinaryType == ModBinaryType.Audio))
						modTypes |= ModTypes.Audio;
					if (archiveData.Binaries.Any(md => md.ModBinaryType == ModBinaryType.Core) || ddBinary?.Chunks.Any(mad => mad.AssetType == AssetType.Shader) == true)
						modTypes |= ModTypes.Shader;
					if (ddBinary?.Chunks.Any(mad => mad.AssetType == AssetType.ModelBinding || mad.AssetType == AssetType.Model) == true)
						modTypes |= ModTypes.Model;
					if (ddBinary?.Chunks.Any(mad => mad.AssetType == AssetType.Texture) == true)
						modTypes |= ModTypes.Texture;
				}
				else
				{
					modTypes = amwfi.Key.ModTypes;
				}

				string modScreenshotsDirectory = Path.Combine(_fileSystemService.GetPath(DataSubDirectory.ModScreenshots), amwfi.Key.Name);
				List<string> screenshotFileNames;
				if (Directory.Exists(modScreenshotsDirectory))
					screenshotFileNames = Directory.GetFiles(modScreenshotsDirectory).Select(p => Path.GetFileName(p)).ToList();
				else
					screenshotFileNames = new();

				return new GetMod
				{
					Name = amwfi.Key.Name,
					HtmlDescription = amwfi.Key.HtmlDescription,
					TrailerUrl = amwfi.Key.TrailerUrl,
					Authors = amwfi.Key.PlayerMods.Select(pam => pam.Player.PlayerName).OrderBy(s => s).ToList(),
					LastUpdated = amwfi.Key.LastUpdated,
					ModTypes = modTypes,
					IsHostedOnDdInfo = amwfi.Value.FileExists,
					ContainsProhibitedAssets = containsProhibitedAssets,
					ModArchive = modArchive,
					ScreenshotFileNames = screenshotFileNames,
				};
			})
			.ToList();
	}

	[HttpGet("{modName}/file")]
	[ProducesResponseType(typeof(FileContentResult), StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	[EndpointConsumer(EndpointConsumers.Ddae | EndpointConsumers.Website)]
	public ActionResult GetModFile([Required] string modName)
	{
		if (!_dbContext.Mods.Any(m => m.Name == modName))
			return NotFound();

		string fileName = $"{modName}.zip";
		string path = Path.Combine(_fileSystemService.GetPath(DataSubDirectory.Mods), fileName);
		if (!Io.File.Exists(path))
			return BadRequest($"Mod file '{fileName}' does not exist.");

		return File(Io.File.ReadAllBytes(path), MediaTypeNames.Application.Zip, fileName);
	}
}
