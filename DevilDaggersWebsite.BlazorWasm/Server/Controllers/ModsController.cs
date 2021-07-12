using DevilDaggersCore.Mods;
using DevilDaggersWebsite.BlazorWasm.Server.Caches.ModArchive;
using DevilDaggersWebsite.BlazorWasm.Server.Controllers.Attributes;
using DevilDaggersWebsite.BlazorWasm.Server.Entities;
using DevilDaggersWebsite.BlazorWasm.Shared.Dto.Mods;
using DevilDaggersWebsite.BlazorWasm.Shared.Enums;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Net.Mime;
using Io = System.IO;

namespace DevilDaggersWebsite.BlazorWasm.Server.Controllers
{
	[Route("api/mods")]
	[ApiController]
	public class ModsController : ControllerBase
	{
		private readonly IWebHostEnvironment _environment;
		private readonly ApplicationDbContext _dbContext;
		private readonly ModArchiveCache _modArchiveCache;

		public ModsController(IWebHostEnvironment environment, ApplicationDbContext dbContext, ModArchiveCache modArchiveCache)
		{
			_environment = environment;
			_dbContext = dbContext;
			_modArchiveCache = modArchiveCache;
		}

		[HttpGet]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[EndpointConsumer(EndpointConsumers.Ddae)]
		public List<GetModPublic> GetPublicMods(string? authorFilter = null, string? nameFilter = null, bool? isHostedFilter = null)
		{
			IEnumerable<AssetMod> assetModsQuery = _dbContext.AssetMods
				.AsNoTracking()
				.Include(am => am.PlayerAssetMods)
					.ThenInclude(pam => pam.Player)
				.Where(am => !am.IsHidden);

			if (!string.IsNullOrWhiteSpace(authorFilter))
				assetModsQuery = assetModsQuery.Where(am => am.PlayerAssetMods.Any(pam => pam.Player.PlayerName.Contains(authorFilter, StringComparison.InvariantCultureIgnoreCase)));
			if (!string.IsNullOrWhiteSpace(nameFilter))
				assetModsQuery = assetModsQuery.Where(am => am.Name.Contains(nameFilter, StringComparison.InvariantCultureIgnoreCase));

			List<AssetMod> assetMods = assetModsQuery.ToList();

			Dictionary<AssetMod, (bool FileExists, string? Path)> assetModsWithFileInfo = assetMods.ToDictionary(am => am, am =>
			{
				string filePath = Path.Combine(_environment.WebRootPath, "mods", $"{am.Name}.zip");
				bool fileExists = Io.File.Exists(filePath);
				return (fileExists, fileExists ? filePath : null);
			});

			if (isHostedFilter.HasValue)
				assetModsWithFileInfo = assetModsWithFileInfo.Where(kvp => kvp.Value.FileExists == isHostedFilter.Value).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

			return assetModsWithFileInfo
				.Select(amwfi =>
				{
					bool? containsProhibitedAssets = null;
					ModArchivePublic? modArchive = null;
					AssetModTypes assetModTypes;
					if (amwfi.Value.FileExists)
					{
						ModArchiveCacheData archiveData = _modArchiveCache.GetArchiveDataByFilePath(amwfi.Value.Path!);
						containsProhibitedAssets = archiveData.Binaries.Any(md => md.Chunks.Any(mad => mad.IsProhibited));
						modArchive = new()
						{
							FileSize = archiveData.FileSize,
							FileSizeExtracted = archiveData.FileSizeExtracted,
							Binaries = archiveData.Binaries.ConvertAll(b => new ModBinaryPublic
							{
								Name = b.Name,
								Size = b.Size,
								ModBinaryType = b.ModBinaryType,
							}),
						};

						ModBinaryCacheData? ddBinary = archiveData.Binaries.Find(md => md.ModBinaryType == ModBinaryType.Dd);

						assetModTypes = AssetModTypes.None;
						if (archiveData.Binaries.Any(md => md.ModBinaryType == ModBinaryType.Audio))
							assetModTypes |= AssetModTypes.Audio;
						if (archiveData.Binaries.Any(md => md.ModBinaryType == ModBinaryType.Core) || ddBinary?.Chunks.Any(mad => mad.AssetType == AssetType.Shader) == true)
							assetModTypes |= AssetModTypes.Shader;
						if (ddBinary?.Chunks.Any(mad => mad.AssetType == AssetType.ModelBinding || mad.AssetType == AssetType.Model) == true)
							assetModTypes |= AssetModTypes.Model;
						if (ddBinary?.Chunks.Any(mad => mad.AssetType == AssetType.Texture) == true)
							assetModTypes |= AssetModTypes.Texture;
					}
					else
					{
						assetModTypes = amwfi.Key.AssetModTypes;
					}

					string modScreenshotsDirectory = Path.Combine(_environment.WebRootPath, "mod-screenshots", amwfi.Key.Name);
					List<string> screenshotFileNames;
					if (Directory.Exists(modScreenshotsDirectory))
						screenshotFileNames = Directory.GetFiles(modScreenshotsDirectory).Select(p => Path.GetFileName(p)).ToList();
					else
						screenshotFileNames = new();

					return new GetModPublic
					{
						Name = amwfi.Key.Name,
						HtmlDescription = amwfi.Key.HtmlDescription,
						TrailerUrl = amwfi.Key.TrailerUrl,
						Authors = amwfi.Key.PlayerAssetMods.Select(pam => pam.Player.PlayerName).OrderBy(s => s).ToList(),
						LastUpdated = amwfi.Key.LastUpdated,
						AssetModTypes = assetModTypes,
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
			if (!_dbContext.AssetMods.Any(m => m.Name == modName))
				return new NotFoundObjectResult(new ProblemDetails { Title = $"Mod '{modName}' was not found." });

			string fileName = $"{modName}.zip";
			string path = Path.Combine("mods", fileName);
			if (!Io.File.Exists(Path.Combine(_environment.WebRootPath, path)))
				return new BadRequestObjectResult(new ProblemDetails { Title = $"Mod file '{fileName}' does not exist." });

			return File(Io.File.ReadAllBytes(Path.Combine(_environment.WebRootPath, path)), MediaTypeNames.Application.Zip, fileName);
		}
	}
}
