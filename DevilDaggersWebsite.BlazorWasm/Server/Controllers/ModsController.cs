using DevilDaggersCore.Extensions;
using DevilDaggersWebsite.Api.Attributes;
using DevilDaggersWebsite.BlazorWasm.Server.Constants;
using DevilDaggersWebsite.BlazorWasm.Server.Singletons;
using DevilDaggersWebsite.BlazorWasm.Shared;
using DevilDaggersWebsite.BlazorWasm.Shared.Mods;
using DevilDaggersWebsite.Caches.ModArchive;
using DevilDaggersWebsite.Entities;
using DevilDaggersWebsite.Enumerators;
using DevilDaggersWebsite.Exceptions;
using DevilDaggersWebsite.Extensions;
using DevilDaggersWebsite.HostedServices.DdInfoDiscordBot;
using DevilDaggersWebsite.Singletons;
using DevilDaggersWebsite.Transients;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Io = System.IO;

namespace DevilDaggersWebsite.BlazorWasm.Server.Controllers
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
		private readonly AuditLogger _auditLogger;

		public ModsController(IWebHostEnvironment environment, ApplicationDbContext dbContext, ModHelper modHelper, ModArchiveCache modArchiveCache, DiscordLogger discordLogger, AuditLogger auditLogger)
		{
			_environment = environment;
			_dbContext = dbContext;
			_modHelper = modHelper;
			_modArchiveCache = modArchiveCache;
			_discordLogger = discordLogger;
			_auditLogger = auditLogger;
		}

		[HttpGet("admin")]
		[Authorize(Roles = Roles.AssetMods)]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[EndpointConsumer(EndpointConsumers.Admin)]
		public ActionResult<Page<GetMod>> GetMods([Range(0, 1000)] int pageIndex = 0, [Range(5, 50)] int pageSize = 25)
		{
			List<AssetMod> mods = _dbContext.AssetMods
				.AsNoTracking()
				.Include(m => m.PlayerAssetMods)
				.Skip(pageIndex * pageSize)
				.Take(pageSize)
				.ToList();

			return new Page<GetMod>
			{
				Results = mods.ConvertAll(m => new GetMod
				{
					Id = m.Id,
					AssetModTypes = m.AssetModTypes,
					HtmlDescription = m.HtmlDescription?.TrimAfter(40, true),
					IsHidden = m.IsHidden,
					LastUpdated = m.LastUpdated,
					Name = m.Name,
					PlayerIds = m.PlayerAssetMods.ConvertAll(pam => pam.PlayerId),
					TrailerUrl = m.TrailerUrl?.TrimAfter(40, true),
					Url = m.Url?.TrimAfter(40, true),
				}),
				TotalResults = _dbContext.AssetMods.Count(),
			};
		}

		[HttpPost("admin")]
		[Authorize(Roles = Roles.AssetMods)]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[EndpointConsumer(EndpointConsumers.Admin)]
		public async Task<ActionResult> AddMod(AddMod addMod)
		{
			if (addMod.PlayerIds == null || addMod.PlayerIds.Count == 0)
				return BadRequest("Mod must have at least one author.");

			if (_dbContext.AssetMods.Any(m => m.Name == addMod.Name))
				return BadRequest($"Mod with name '{addMod.Name}' already exists.");

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
				Name = addMod.Name,
				TrailerUrl = addMod.TrailerUrl,
				Url = addMod.Url ?? string.Empty,
			};
			_dbContext.AssetMods.Add(mod);
			_dbContext.SaveChanges();

			await _auditLogger.LogAdd(addMod, User, mod.Id);

			return Ok(mod.Id);
		}

		[HttpPut("admin/{id}")]
		[Authorize(Roles = Roles.AssetMods)]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[EndpointConsumer(EndpointConsumers.None)]
		public async Task<ActionResult> EditModById(int id, EditMod editMod)
		{
			if (editMod.PlayerIds == null || editMod.PlayerIds.Count == 0)
				return BadRequest("Mod must have at least one author.");

			if (_dbContext.AssetMods.Any(m => m.Name == editMod.Name))
				return BadRequest($"Mod with name '{editMod.Name}' already exists.");

			foreach (int playerId in editMod.PlayerIds)
			{
				if (!_dbContext.Players.Any(p => p.Id == playerId))
					return BadRequest($"Player with ID '{playerId}' does not exist.");
			}

			AssetMod? mod = _dbContext.AssetMods
				.Include(m => m.PlayerAssetMods)
				.FirstOrDefault(m => m.Id == id);
			if (mod == null)
				return NotFound();

			EditMod logDto = new()
			{
				AssetModTypes = mod.AssetModTypes,
				HtmlDescription = mod.HtmlDescription,
				IsHidden = mod.IsHidden,
				Name = mod.Name,
				TrailerUrl = mod.TrailerUrl,
				Url = mod.Url,
				PlayerIds = mod.PlayerAssetMods.ConvertAll(pam => pam.PlayerId),
			};

			// Do not update LastUpdated. Update this value when updating the file only.
			mod.AssetModTypes = editMod.AssetModTypes;
			mod.HtmlDescription = editMod.HtmlDescription;
			mod.IsHidden = editMod.IsHidden;
			mod.Name = editMod.Name;
			mod.TrailerUrl = editMod.TrailerUrl;
			mod.Url = editMod.Url ?? string.Empty;
			_dbContext.SaveChanges();

			await _auditLogger.LogEdit(logDto, editMod, User, mod.Id);

			return Ok();
		}

		[HttpDelete("admin/{id}")]
		[Authorize(Roles = Roles.Admin)]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[EndpointConsumer(EndpointConsumers.None)]
		public async Task<ActionResult> DeleteModById(int id)
		{
			AssetMod? assetMod = _dbContext.AssetMods.FirstOrDefault(d => d.Id == id);
			if (assetMod == null)
				return NotFound();

			_dbContext.AssetMods.Remove(assetMod);
			_dbContext.SaveChanges();

			await _auditLogger.LogDelete(assetMod, User, assetMod.Id);

			return Ok();
		}

		[HttpPost("admin/upload-file")]
		[Authorize(Roles = Roles.AssetMods)]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[EndpointConsumer(EndpointConsumers.Admin)]
		public async Task<ActionResult> UploadModFile(IFormFile file)
		{
			if (file == null)
				return BadRequest("No file.");

			if (file.Length > ModFileConstants.MaxFileSize)
				return BadRequest($"File too large ({file.Length:n0} / max {ModFileConstants.MaxFileSize:n0} bytes).");

			string modsDirectory = Path.Combine(_environment.WebRootPath, "mods");
			DirectoryInfo di = new(modsDirectory);
			long usedSpace = di.EnumerateFiles("*.*", SearchOption.AllDirectories).Sum(fi => fi.Length);
			if (file.Length + usedSpace > ModFileConstants.MaxHostingSpace)
				return BadRequest($"This file is {file.Length:n0} bytes in size, but only {ModFileConstants.MaxHostingSpace - usedSpace:n0} bytes of free space is available.");

			if (file.FileName.Length > ModFileConstants.MaxFileNameLength)
				return BadRequest($"File name too long ({file.FileName.Length} / max {ModFileConstants.MaxFileNameLength} characters).");

			if (!file.FileName.EndsWith(".zip"))
				return BadRequest("File name must have the .zip extension.");

			string filePath = Path.Combine(modsDirectory, file.FileName);
			if (Io.File.Exists(filePath))
				return BadRequest($"File '{file.FileName}' already exists.");

			string archiveNameWithoutExtension = Path.GetFileNameWithoutExtension(file.FileName);
			AssetMod? mod = _dbContext.AssetMods.FirstOrDefault(m => m.Name == archiveNameWithoutExtension);
			if (mod == null)
				return BadRequest($"There is no mod named '{archiveNameWithoutExtension}'.");

			mod.LastUpdated = DateTime.UtcNow;
			_dbContext.SaveChanges();

			byte[] formFileBytes = new byte[file.Length];
			using (MemoryStream ms = new())
			{
				file.CopyTo(ms);
				formFileBytes = ms.ToArray();
			}

			try
			{
				List<ModBinaryCacheData> archive = _modArchiveCache.GetArchiveDataByBytes(Path.GetFileNameWithoutExtension(file.FileName), formFileBytes).Binaries;
				if (archive.Count == 0)
					throw new InvalidModArchiveException($"Mod archive '{file.FileName}' does not contain any binaries.");

				foreach (ModBinaryCacheData binary in archive)
				{
					if (binary.Chunks.Count == 0)
						throw new InvalidModBinaryException($"Mod binary '{binary.Name}' does not contain any assets.");

					string expectedPrefix = binary.ModBinaryType switch
					{
						ModBinaryType.Audio => $"audio-{archiveNameWithoutExtension}-",
						ModBinaryType.Dd => $"dd-{archiveNameWithoutExtension}-",
						_ => throw new InvalidModBinaryException($"Mod binary  '{binary.Name}' is a '{binary.ModBinaryType}' mod which is not allowed."),
					};

					if (!binary.Name.StartsWith(expectedPrefix))
						throw new InvalidModBinaryException($"Name of mod binary '{binary.Name}' must start with '{expectedPrefix}'.");

					if (binary.Name.Length == expectedPrefix.Length)
						throw new InvalidModBinaryException($"Name of mod binary '{binary.Name}' must not be equal to '{expectedPrefix}'.");
				}

				Io.File.WriteAllBytes(filePath, formFileBytes);
				await _discordLogger.TryLog(Channel.MonitoringAuditLog, $"`{User.GetShortName()}` uploaded new ASSETMOD file :file_folder: `{file.FileName}` (`{formFileBytes.Length:n0}` bytes)");

				return Ok();
			}
			catch (InvalidModArchiveException ex)
			{
				return BadRequest($"The mod archive '{file?.FileName}' is invalid. {ex.Message}");
			}
			catch (InvalidModBinaryException ex)
			{
				return BadRequest($"A mod binary inside the mod archive '{file?.FileName}' is invalid. {ex.Message}");
			}
		}

		[HttpDelete("admin/delete-file")]
		[Authorize(Roles = Roles.AssetMods)]
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

		[HttpPost("admin/upload-screenshot")]
		[Authorize(Roles = Roles.AssetMods)]
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

		[HttpDelete("admin/delete-screenshot")]
		[Authorize(Roles = Roles.AssetMods)]
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
