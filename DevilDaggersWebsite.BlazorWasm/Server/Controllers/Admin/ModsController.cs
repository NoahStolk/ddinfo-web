using DevilDaggersWebsite.BlazorWasm.Server.Caches.ModArchive;
using DevilDaggersWebsite.BlazorWasm.Server.Converters.Admin;
using DevilDaggersWebsite.BlazorWasm.Server.Entities;
using DevilDaggersWebsite.BlazorWasm.Server.Exceptions;
using DevilDaggersWebsite.BlazorWasm.Server.Extensions;
using DevilDaggersWebsite.BlazorWasm.Server.Singletons.AuditLog;
using DevilDaggersWebsite.BlazorWasm.Server.Utils;
using DevilDaggersWebsite.BlazorWasm.Shared;
using DevilDaggersWebsite.BlazorWasm.Shared.Constants;
using DevilDaggersWebsite.BlazorWasm.Shared.Dto;
using DevilDaggersWebsite.BlazorWasm.Shared.Dto.Admin.Mods;
using DevilDaggersWebsite.BlazorWasm.Shared.Enums;
using DevilDaggersWebsite.BlazorWasm.Shared.Enums.Sortings;
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

namespace DevilDaggersWebsite.BlazorWasm.Server.Controllers.Admin
{
	[Route("api/admin/mods")]
	[Authorize(Roles = Roles.Mods)]
	[ApiController]
	public class ModsController : ControllerBase
	{
		private readonly IWebHostEnvironment _environment;
		private readonly ApplicationDbContext _dbContext;
		private readonly ModArchiveCache _modArchiveCache;
		private readonly AuditLogger _auditLogger;

		public ModsController(IWebHostEnvironment environment, ApplicationDbContext dbContext, ModArchiveCache modArchiveCache, AuditLogger auditLogger)
		{
			_environment = environment;
			_dbContext = dbContext;
			_modArchiveCache = modArchiveCache;
			_auditLogger = auditLogger;
		}

		[HttpGet]
		[ProducesResponseType(StatusCodes.Status200OK)]
		public ActionResult<Page<GetModForOverview>> GetMods(
			[Range(0, 1000)] int pageIndex = 0,
			[Range(AdminPagingConstants.PageSizeMin, AdminPagingConstants.PageSizeMax)] int pageSize = AdminPagingConstants.PageSizeDefault,
			ModSorting? sortBy = null,
			bool ascending = false)
		{
			IQueryable<AssetMod> modsQuery = _dbContext.AssetMods.AsNoTracking();

			modsQuery = sortBy switch
			{
				ModSorting.AssetModTypes => modsQuery.OrderBy(m => m.AssetModTypes, ascending),
				ModSorting.HtmlDescription => modsQuery.OrderBy(m => m.HtmlDescription, ascending),
				ModSorting.IsHidden => modsQuery.OrderBy(m => m.IsHidden, ascending),
				ModSorting.LastUpdated => modsQuery.OrderBy(m => m.LastUpdated, ascending),
				ModSorting.Name => modsQuery.OrderBy(m => m.Name, ascending),
				ModSorting.TrailerUrl => modsQuery.OrderBy(m => m.TrailerUrl, ascending),
				ModSorting.Url => modsQuery.OrderBy(m => m.Url, ascending),
				_ => modsQuery.OrderBy(m => m.Id, ascending),
			};

			List<AssetMod> mods = modsQuery
				.Skip(pageIndex * pageSize)
				.Take(pageSize)
				.ToList();

			return new Page<GetModForOverview>
			{
				Results = mods.ConvertAll(m => m.ToGetModForOverview()),
				TotalResults = _dbContext.AssetMods.Count(),
			};
		}

		[HttpGet("names")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		public ActionResult<List<GetModName>> GetModNames()
		{
			var mods = _dbContext.AssetMods
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
		public ActionResult<GetMod> GetModById(int id)
		{
			AssetMod? mod = _dbContext.AssetMods
				.AsSingleQuery()
				.AsNoTracking()
				.Include(m => m.PlayerAssetMods)
				.FirstOrDefault(m => m.Id == id);
			if (mod == null)
				return NotFound();

			return mod.ToGetMod();
		}

		[HttpPost]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
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

			string? addInfo = null;
			if (addMod.FileContents != null)
			{
				string modsDirectory = DataUtils.GetPath("Mods");
				DirectoryInfo di = new(modsDirectory);
				long usedSpace = di.EnumerateFiles("*.*", SearchOption.AllDirectories).Sum(fi => fi.Length);
				if (addMod.FileContents.Length + usedSpace > ModFileConstants.MaxHostingSpace)
					return BadRequest($"This file is {addMod.FileContents.Length:n0} bytes in size, but only {ModFileConstants.MaxHostingSpace - usedSpace:n0} bytes of free space is available.");

				string? validationError = ValidateModArchive(addMod.FileContents, addMod.Name);
				if (validationError != null)
					return BadRequest(validationError);

				string path = Path.Combine(modsDirectory, $"{addMod.Name}.zip");
				Io.File.WriteAllBytes(path, addMod.FileContents);
				addInfo = $"File '{DataUtils.GetRelevantDisplayPath(path)}' was added.";
			}

			if (addMod.Screenshots.Count > 0)
			{
				string directory = Path.Combine(DataUtils.GetPath("ModScreenshots"), addMod.Name);
				Directory.CreateDirectory(directory);
				DirectoryInfo directoryInfo = new(directory);

				foreach (AddModScreenshot addModScreenshot in addMod.Screenshots)
				{
					int screenshots = directoryInfo.EnumerateFiles().Count();
					string filePath = Path.Combine(directory, $"{screenshots:00}.png");
					Io.File.WriteAllBytes(filePath, addModScreenshot.FileContents);
				}
			}

			AssetMod mod = new()
			{
				AssetModTypes = addMod.AssetModTypes,
				HtmlDescription = addMod.HtmlDescription,
				IsHidden = addMod.IsHidden,
				LastUpdated = DateTime.Now,
				Name = addMod.Name,
				TrailerUrl = addMod.TrailerUrl,
				Url = addMod.Url ?? string.Empty,
			};
			_dbContext.AssetMods.Add(mod);
			_dbContext.SaveChanges(); // Save changes here so PlayerMods entities can be assigned properly.

			UpdatePlayerMods(addMod.PlayerIds ?? new(), mod.Id);
			_dbContext.SaveChanges();

			await _auditLogger.LogAdd(addMod, User, mod.Id, addInfo == null ? null : new() { new(addInfo, FileSystemInformationType.Add) });

			return Ok(mod.Id);
		}

		[HttpPut("{id}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<ActionResult> EditModById(int id, EditMod editMod)
		{
			if (editMod.PlayerIds == null || editMod.PlayerIds.Count == 0)
				return BadRequest("Mod must have at least one author.");

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

			List<FileSystemInformation> fileSystemInformation = new();
			if (mod.Name != editMod.Name)
			{
				if (_dbContext.AssetMods.Any(m => m.Name == editMod.Name))
					return BadRequest($"Mod with name '{editMod.Name}' already exists.");

				string directory = DataUtils.GetPath("Mods");
				string oldPath = Path.Combine(directory, $"{mod.Name}.zip");
				if (Io.File.Exists(oldPath))
				{
					string newPath = Path.Combine(directory, editMod.Name);
					Io.File.Move(oldPath, newPath);
					fileSystemInformation.Add(new($"File '{DataUtils.GetRelevantDisplayPath(oldPath)}' was moved to {DataUtils.GetRelevantDisplayPath(newPath)}.", FileSystemInformationType.Move));
				}
				else
				{
					fileSystemInformation.Add(new($"File '{DataUtils.GetRelevantDisplayPath(oldPath)}' was not moved because it does not exist.", FileSystemInformationType.NotFound));
				}

				string cacheDirectory = DataUtils.GetPath("ModArchiveCache");
				string oldCachePath = Path.Combine(cacheDirectory, $"{mod.Name}.json");
				if (Io.File.Exists(oldCachePath))
				{
					string newCachePath = Path.Combine(directory, editMod.Name);
					Io.File.Move(oldCachePath, newCachePath);
					fileSystemInformation.Add(new($"File '{DataUtils.GetRelevantDisplayPath(oldCachePath)}' was moved to {DataUtils.GetRelevantDisplayPath(newCachePath)}.", FileSystemInformationType.Move));
				}
				else
				{
					fileSystemInformation.Add(new($"File {DataUtils.GetRelevantDisplayPath(oldCachePath)} was not moved because it does not exist.", FileSystemInformationType.NotFound));
				}

				string oldScreenshotsDirectory = Path.Combine(DataUtils.GetPath("ModScreenshots"), mod.Name);
				if (Directory.Exists(oldScreenshotsDirectory))
				{
					string newScreenshotsDirectory = Path.Combine(DataUtils.GetPath("ModScreenshots"), editMod.Name);
					Directory.Move(oldScreenshotsDirectory, newScreenshotsDirectory);
					fileSystemInformation.Add(new($"Directory '{DataUtils.GetRelevantDisplayPath(oldScreenshotsDirectory)}' was moved to {DataUtils.GetRelevantDisplayPath(newScreenshotsDirectory)}.", FileSystemInformationType.Move));
				}
				else
				{
					fileSystemInformation.Add(new($"Directory {DataUtils.GetRelevantDisplayPath(oldScreenshotsDirectory)} was not moved because it does not exist.", FileSystemInformationType.NotFound));
				}
			}

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
			_dbContext.SaveChanges(); // Save changes here so PlayerMods entities can be assigned properly.

			UpdatePlayerMods(editMod.PlayerIds ?? new(), mod.Id);
			_dbContext.SaveChanges();

			await _auditLogger.LogEdit(logDto, editMod, User, mod.Id, fileSystemInformation);

			return Ok();
		}

		[HttpDelete("{id}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<ActionResult> DeleteModById(int id)
		{
			AssetMod? mod = _dbContext.AssetMods.FirstOrDefault(d => d.Id == id);
			if (mod == null)
				return NotFound();

			List<FileSystemInformation> fileSystemInformation = new();
			string path = Path.Combine(DataUtils.GetPath("Mods"), $"{mod.Name}.zip");
			if (Io.File.Exists(path))
				Io.File.Delete(path);
			else
				fileSystemInformation.Add(new($"File {DataUtils.GetRelevantDisplayPath(path)} was not deleted because it does not exist.", FileSystemInformationType.NotFound));

			// Clear entire memory cache (can't clear individual entries).
			_modArchiveCache.Clear();

			// Clear file cache for this mod.
			string cachePath = Path.Combine(DataUtils.GetPath("ModArchiveCache"), $"{mod.Name}.json");
			if (Io.File.Exists(cachePath))
				Io.File.Delete(cachePath);
			else
				fileSystemInformation.Add(new($"File {DataUtils.GetRelevantDisplayPath(cachePath)} was not deleted because it does not exist.", FileSystemInformationType.NotFound));

			string screenshotsDirectory = Path.Combine(DataUtils.GetPath("ModScreenshots"), mod.Name);
			if (Directory.Exists(screenshotsDirectory))
				Directory.Delete(screenshotsDirectory, true);
			else
				fileSystemInformation.Add(new($"Directory {DataUtils.GetRelevantDisplayPath(screenshotsDirectory)} was not deleted because it does not exist.", FileSystemInformationType.NotFound));

			_dbContext.AssetMods.Remove(mod);
			_dbContext.SaveChanges();

			await _auditLogger.LogDelete(mod, User, mod.Id, fileSystemInformation);

			return Ok();
		}

		private void UpdatePlayerMods(List<int> playerIds, int modId)
		{
			foreach (PlayerAssetMod newEntity in playerIds.ConvertAll(pi => new PlayerAssetMod { AssetModId = modId, PlayerId = pi }))
			{
				if (!_dbContext.PlayerAssetMods.Any(pam => pam.AssetModId == newEntity.AssetModId && pam.PlayerId == newEntity.PlayerId))
					_dbContext.PlayerAssetMods.Add(newEntity);
			}

			foreach (PlayerAssetMod entityToRemove in _dbContext.PlayerAssetMods.Where(pam => pam.AssetModId == modId && !playerIds.Contains(pam.PlayerId)))
				_dbContext.PlayerAssetMods.Remove(entityToRemove);
		}

		private string? ValidateModArchive(byte[] fileContents, string modName)
		{
			try
			{
				List<ModBinaryCacheData> archive = _modArchiveCache.GetArchiveDataByBytes(modName, fileContents).Binaries;
				if (archive.Count == 0)
					throw new InvalidModArchiveException("Mod archive does not contain any binaries.");

				foreach (ModBinaryCacheData binary in archive)
				{
					if (binary.Chunks.Count == 0)
						throw new InvalidModBinaryException($"Mod binary '{binary.Name}' does not contain any assets.");

					string expectedPrefix = binary.ModBinaryType switch
					{
						ModBinaryType.Audio => $"audio-{modName}-",
						ModBinaryType.Dd => $"dd-{modName}-",
						_ => throw new InvalidModBinaryException($"Mod binary '{binary.Name}' is a '{binary.ModBinaryType}' mod which is not allowed."),
					};

					if (!binary.Name.StartsWith(expectedPrefix))
						throw new InvalidModBinaryException($"Name of mod binary '{binary.Name}' must start with '{expectedPrefix}'.");

					if (binary.Name.Length == expectedPrefix.Length)
						throw new InvalidModBinaryException($"Name of mod binary '{binary.Name}' must not be equal to '{expectedPrefix}'.");
				}
			}
			catch (InvalidModArchiveException ex)
			{
				return $"The mod archive is invalid. {ex.Message}";
			}
			catch (InvalidModBinaryException ex)
			{
				return $"A mod binary inside the mod archive is invalid. {ex.Message}";
			}

			return null;
		}

		[HttpPost("upload-screenshot")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
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

			return Ok();
		}

		[HttpDelete("delete-screenshot")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<ActionResult> DeleteModScreenshot(string fileName)
		{
			string path = Path.Combine(_environment.WebRootPath, "mod-screenshots", fileName);
			if (!Io.File.Exists(path))
				return BadRequest($"File '{fileName}' does not exist.");

			Io.File.Delete(path);

			return Ok();
		}
	}
}
