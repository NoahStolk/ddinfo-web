using DevilDaggersWebsite.Api.Attributes;
using DevilDaggersWebsite.Caches.ModArchive;
using DevilDaggersWebsite.Constants;
using DevilDaggersWebsite.Dto.Mods;
using DevilDaggersWebsite.Entities;
using DevilDaggersWebsite.Enumerators;
using DevilDaggersWebsite.Exceptions;
using DevilDaggersWebsite.Extensions;
using DevilDaggersWebsite.HostedServices.DdInfoDiscordBot;
using DevilDaggersWebsite.Singletons;
using DevilDaggersWebsite.Transients;
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
using System.Threading.Tasks;
using Io = System.IO;

namespace DevilDaggersWebsite.Api
{
	[Route("api/mods")]
	[ApiController]
	public class ModsController : ControllerBase
	{
		private readonly IWebHostEnvironment _environment;
		private readonly ApplicationDbContext _dbContext;
		private readonly ModHelper _modHelper;
		private readonly ModArchiveCache _modArchiveCache;
		private readonly DiscordLogger _discordLogger;

		public ModsController(IWebHostEnvironment environment, ApplicationDbContext dbContext, ModHelper modHelper, ModArchiveCache modArchiveCache, DiscordLogger discordLogger)
		{
			_environment = environment;
			_dbContext = dbContext;
			_modHelper = modHelper;
			_modArchiveCache = modArchiveCache;
			_discordLogger = discordLogger;
		}

		// TODO: Re-route to /public.
		[HttpGet]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[EndpointConsumer(EndpointConsumers.Ddae)]
		public List<GetPublicMod> GetPublicMods(string? authorFilter = null, string? nameFilter = null, bool? isHostedFilter = null)
			=> _modHelper.GetPublicMods(authorFilter, nameFilter, isHostedFilter);

		// TODO: Remove private.
		[HttpGet("private")]
		//[Authorize(Policies.AssetModsPolicy)]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[EndpointConsumer(EndpointConsumers.Admin)]
		public List<GetMod> GetMods()
		{
			List<AssetMod> mods = _dbContext.AssetMods.AsNoTracking().ToList();

			return mods.ConvertAll(ce => new GetMod
			{
				Id = ce.Id,
				AssetModTypes = ce.AssetModTypes,
				HtmlDescription = ce.HtmlDescription,
				IsHidden = ce.IsHidden,
				LastUpdated = ce.LastUpdated,
				Name = ce.Name,
				PlayerIds = ce.PlayerAssetMods.ConvertAll(pam => pam.PlayerId),
				TrailerUrl = ce.TrailerUrl,
				Url = ce.Url,
			});
		}

		[HttpGet("{modName}/file")]
		[ProducesResponseType(typeof(FileContentResult), StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[EndpointConsumer(EndpointConsumers.Ddae | EndpointConsumers.Website)]
		public ActionResult GetModFile([Required] string modName)
		{
			if (!_dbContext.AssetMods.Any(am => am.Name == modName))
				return new NotFoundObjectResult(new ProblemDetails { Title = $"Mod '{modName}' was not found." });

			string fileName = $"{modName}.zip";
			string path = Path.Combine("mods", fileName);
			if (!Io.File.Exists(Path.Combine(_environment.WebRootPath, path)))
				return new BadRequestObjectResult(new ProblemDetails { Title = $"Mod file '{fileName}' does not exist." });

			return File(Io.File.ReadAllBytes(Path.Combine(_environment.WebRootPath, path)), MediaTypeNames.Application.Zip, fileName);
		}

		[HttpPost]
		//[Authorize(Policies.AssetModsPolicy)]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[EndpointConsumer(EndpointConsumers.Admin)]
		public ActionResult AddMod(AddMod addMod)
		{
			if (addMod.PlayerIds == null || addMod.PlayerIds.Count == 0)
				return BadRequest("Mod must have at least one author.");

			foreach (int playerId in addMod.PlayerIds)
			{
				if (!_dbContext.Players.Any(p => p.Id == playerId))
					return BadRequest($"Player with ID '{playerId}' does not exist.");
			}

			AssetMod mod = new()
			{
				AssetModTypes = addMod.AssetModTypes,
				HtmlDescription = addMod.HtmlDescription,
				IsHidden = addMod.IsHidden,
				LastUpdated = DateTime.UtcNow,
				Name = addMod.Name,
				TrailerUrl = addMod.TrailerUrl,
				Url = addMod.Url ?? string.Empty,
			};
			_dbContext.AssetMods.Add(mod);
			_dbContext.SaveChanges();

			return Ok();
		}

		[HttpPut("{id}")]
		//[Authorize(Policies.SpawnsetsPolicy)]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[EndpointConsumer(EndpointConsumers.None)]
		public ActionResult EditMod(int id, EditMod editMod)
		{
			if (editMod.PlayerIds == null || editMod.PlayerIds.Count == 0)
				return BadRequest("Mod must have at least one author.");

			foreach (int playerId in editMod.PlayerIds)
			{
				if (!_dbContext.Players.Any(p => p.Id == playerId))
					return BadRequest($"Player with ID '{playerId}' does not exist.");
			}

			AssetMod? mod = _dbContext.AssetMods.FirstOrDefault(s => s.Id == id);
			if (mod == null)
				return NotFound();

			// Do not update LastUpdated. Update this value when updating the file only.
			mod.HtmlDescription = editMod.HtmlDescription;
			mod.IsHidden = editMod.IsHidden;
			mod.Name = editMod.Name;
			mod.TrailerUrl = editMod.TrailerUrl;
			mod.Url = editMod.Url ?? string.Empty;
			_dbContext.SaveChanges();

			return Ok();
		}

		[HttpPost("upload-file")]
		//[Authorize(Policies.AssetModsPolicy)]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[EndpointConsumer(EndpointConsumers.Admin)]
		public async Task<ActionResult> UploadModFile(IFormFile file)
		{
			string? filePath = null;

			string modsDirectory = Path.Combine(_environment.WebRootPath, "mods");

			try
			{
				if (file == null)
					return BadRequest("No file.");

				if (file.Length > ModFileConstants.MaxFileSize)
					return BadRequest($"File too large ({file.Length:n0} / max {ModFileConstants.MaxFileSize:n0} bytes).");

				DirectoryInfo di = new(modsDirectory);
				long usedSpace = di.EnumerateFiles("*.*", SearchOption.AllDirectories).Sum(fi => fi.Length);
				if (file.Length + usedSpace > ModFileConstants.MaxHostingSpace)
					return BadRequest($"This file is {file.Length:n0} bytes in size, but only {ModFileConstants.MaxHostingSpace - usedSpace:n0} bytes of free space is available.");

				if (file.FileName.Length > ModFileConstants.MaxFileNameLength)
					return BadRequest($"File name too long ({file.FileName.Length} / max {ModFileConstants.MaxFileNameLength} characters).");

				if (!file.FileName.EndsWith(".zip"))
					return BadRequest("File name must have the .zip extension.");

				filePath = Path.Combine(modsDirectory, file.FileName);
				if (Io.File.Exists(filePath))
					return BadRequest($"File '{file.FileName}' already exists.");

				byte[] formFileBytes = new byte[file.Length];
				using (MemoryStream ms = new())
				{
					file.CopyTo(ms);
					formFileBytes = ms.ToArray();
				}

				List<ModBinaryCacheData> archive = _modArchiveCache.GetArchiveDataByBytes(Path.GetFileNameWithoutExtension(file.FileName), formFileBytes).Binaries;
				if (archive.Count == 0)
					throw new InvalidModBinaryException($"File '{file.FileName}' does not contain any binaries.");

				string archiveNameWithoutExtension = Path.GetFileNameWithoutExtension(file.FileName);

				foreach (ModBinaryCacheData binary in archive)
				{
					if (binary.Chunks.Count == 0)
						throw new InvalidModBinaryException($"Binary '{binary.Name}' does not contain any assets.");

					string expectedPrefix = binary.ModBinaryType switch
					{
						ModBinaryType.Audio => $"audio-{archiveNameWithoutExtension}-",
						ModBinaryType.Dd => $"dd-{archiveNameWithoutExtension}-",
						_ => throw new InvalidModBinaryException($"Binary '{binary.Name}' is a '{binary.ModBinaryType}' mod which is not allowed."),
					};

					if (!binary.Name.StartsWith(expectedPrefix))
						throw new InvalidModBinaryException($"Name of binary '{binary.Name}' must start with '{expectedPrefix}'.");

					if (binary.Name.Length == expectedPrefix.Length)
						throw new InvalidModBinaryException($"Name of binary '{binary.Name}' must not be equal to '{expectedPrefix}'.");
				}

				Io.File.WriteAllBytes(filePath, formFileBytes);
				await _discordLogger.TryLog(Channel.MonitoringAuditLog, $"`{User.GetShortName()}` uploaded new ASSETMOD file :file_folder: `{file.FileName}` (`{formFileBytes.Length:n0}` bytes)");

				return Ok();
			}
			catch (InvalidModBinaryException ex)
			{
				return BadRequest($"A binary file inside the file '{file?.FileName}' is invalid. {ex.Message}");
			}
		}

		[HttpDelete("delete-file")]
		//[Authorize(Policies.AssetModsPolicy)]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[EndpointConsumer(EndpointConsumers.Admin)]
		public async Task<ActionResult> DeleteModFile(string fileName)
		{
			string path = Path.Combine(_environment.WebRootPath, "mods", fileName);
			if (!Io.File.Exists(path))
				return BadRequest($"File '{fileName}' does not exist.");

			Io.File.Delete(path);

			await _discordLogger.TryLog(Channel.MonitoringAuditLog, $"`{User.GetShortName()}` deleted ASSETMOD file :file_folder: `{fileName}`.");

			// Clear entire memory cache (can't clear individual entries).
			_modArchiveCache.Clear();

			// Clear file cache for this mod.
			string cacheFilePath = Path.Combine(_environment.WebRootPath, "mod-archive-cache", $"{Path.GetFileNameWithoutExtension(fileName)}.json");
			if (Io.File.Exists(cacheFilePath))
				Io.File.Delete(cacheFilePath);

			return Ok();
		}

		[HttpPost("upload-screenshot")]
		//[Authorize(Policies.AssetModsPolicy)]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[EndpointConsumer(EndpointConsumers.Admin)]
		public async Task<ActionResult> UploadModScreenshot(string modName, IFormFile file)
		{
			if (string.IsNullOrEmpty(modName))
				return BadRequest("No mod selected.");

			if (file == null)
				return BadRequest("No file.");

			if (file.Length > ModScreenshotConstants.MaxFileSize)
				return BadRequest($"File too large ({file.Length:n0} / max {ModScreenshotConstants.MaxFileSize:n0} bytes).");

			if (file.FileName.Length > ModScreenshotConstants.MaxFileNameLength)
				return BadRequest($"File name too long ({file.FileName.Length} / max {ModScreenshotConstants.MaxFileNameLength} characters).");

			if (!file.FileName.EndsWith(".png"))
				return BadRequest("File name must have the .png extension.");

			string fileDirectory = Path.Combine(_environment.WebRootPath, "mod-screenshots", modName);
			string filePath = Path.Combine(fileDirectory, file.FileName);
			if (Io.File.Exists(filePath))
				return BadRequest($"File '{file.FileName}' already exists.");

			if (!_dbContext.AssetMods.Any(am => am.Name == modName))
				return BadRequest($"Mod '{modName}' does not exist.");

			if (Directory.Exists(fileDirectory) && Directory.GetFiles(fileDirectory).Length >= ModScreenshotConstants.MaxScreenshots)
				return BadRequest($"Mod '{modName}' already has {ModScreenshotConstants.MaxScreenshots} screenshots.");

			byte[] formFileBytes = new byte[file.Length];
			using (MemoryStream ms = new())
			{
				file.CopyTo(ms);
				formFileBytes = ms.ToArray();
			}

			Directory.CreateDirectory(fileDirectory);
			Io.File.WriteAllBytes(filePath, formFileBytes);

			await _discordLogger.TryLog(Channel.MonitoringAuditLog, $"`{User.GetShortName()}` uploaded new ASSETMOD screenshot :frame_photo: `{file.FileName}` for mod `{modName}` (`{formFileBytes.Length:n0}` bytes)");

			return Ok();
		}

		[HttpDelete("delete-screenshot")]
		//[Authorize(Policies.AssetModsPolicy)]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[EndpointConsumer(EndpointConsumers.Admin)]
		public async Task<ActionResult> DeleteModScreenshot(string fileName)
		{
			string path = Path.Combine(_environment.WebRootPath, "mod-screenshots", fileName);
			if (!Io.File.Exists(path))
				return BadRequest($"File '{fileName}' does not exist.");

			Io.File.Delete(path);

			await _discordLogger.TryLog(Channel.MonitoringAuditLog, $"`{User.GetShortName()}` deleted ASSETMOD screenshot :frame_photo: `{fileName}`.");

			return Ok();
		}
	}
}
