using DevilDaggersInfo.Common.Extensions;
using DevilDaggersInfo.Core.Mod;
using DevilDaggersInfo.Web.Server.Domain.Admin.Exceptions;
using DevilDaggersInfo.Web.Server.Domain.Entities;
using DevilDaggersInfo.Web.Server.Domain.Entities.Enums;
using DevilDaggersInfo.Web.Server.Domain.Exceptions;
using DevilDaggersInfo.Web.Server.Domain.Services;
using Microsoft.EntityFrameworkCore;

namespace DevilDaggersInfo.Web.Server.Domain.Admin.Services;

public class ModService
{
	private readonly ApplicationDbContext _dbContext;
	private readonly ModArchiveProcessor _modArchiveProcessor;
	private readonly ModScreenshotProcessor _modScreenshotProcessor;

	public ModService(ApplicationDbContext dbContext, ModArchiveProcessor modArchiveProcessor, ModScreenshotProcessor modScreenshotProcessor)
	{
		_dbContext = dbContext;
		_modArchiveProcessor = modArchiveProcessor;
		_modScreenshotProcessor = modScreenshotProcessor;
	}

	public async Task AddModAsync(Api.Admin.Mods.AddMod addMod)
	{
		ValidateName(addMod.Name);

		if (addMod.PlayerIds == null || addMod.PlayerIds.Count == 0)
			throw new AdminDomainException("Mod must have at least one author.");

		if (_dbContext.Mods.Any(m => m.Name == addMod.Name))
			throw new AdminDomainException($"Mod with name '{addMod.Name}' already exists.");

		foreach (int playerId in addMod.PlayerIds)
		{
			if (!_dbContext.Players.Any(p => p.Id == playerId))
				throw new AdminDomainException($"Player with ID '{playerId}' does not exist.");
		}

		if (addMod.Binaries.Count > 0)
			await _modArchiveProcessor.ProcessModBinaryUploadAsync(addMod.Name, GetBinaryNames(addMod.Binaries.ConvertAll(bd => (bd.Name, bd.Data))));

		_modScreenshotProcessor.ProcessModScreenshotUpload(addMod.Name, addMod.Screenshots);

		ModEntity mod = new()
		{
			ModTypes = addMod.ModTypes?.ToFlagEnum<ModTypes>() ?? ModTypes.None,
			HtmlDescription = addMod.HtmlDescription,
			IsHidden = addMod.IsHidden,
			LastUpdated = DateTime.UtcNow,
			Name = addMod.Name,
			TrailerUrl = addMod.TrailerUrl,
			Url = addMod.Url ?? string.Empty,
		};
		_dbContext.Mods.Add(mod);
		await _dbContext.SaveChangesAsync(); // Save changes here so PlayerMods entities can be assigned properly.

		UpdatePlayerMods(addMod.PlayerIds ?? new(), mod.Id);
		await _dbContext.SaveChangesAsync();
	}

	public async Task EditModAsync(int id, Api.Admin.Mods.EditMod editMod)
	{
		ValidateName(editMod.Name);

		if (editMod.PlayerIds == null || editMod.PlayerIds.Count == 0)
			throw new AdminDomainException("Mod must have at least one author.");

		foreach (int playerId in editMod.PlayerIds)
		{
			if (!_dbContext.Players.Any(p => p.Id == playerId))
				throw new AdminDomainException($"Player with ID '{playerId}' does not exist.");
		}

		ModEntity? mod = _dbContext.Mods
			.Include(m => m.PlayerMods)
			.FirstOrDefault(m => m.Id == id);
		if (mod == null)
			throw new NotFoundException($"Mod with ID '{id}' does not exist.");

		if (mod.Name != editMod.Name && _dbContext.Mods.Any(m => m.Name == editMod.Name))
			throw new AdminDomainException($"Mod with name '{editMod.Name}' already exists.");

		bool isUpdated = await _modArchiveProcessor.TransformBinariesInModArchiveAsync(mod.Name, editMod.Name, editMod.BinariesToDelete.ConvertAll(s => BinaryName.Parse(s, mod.Name)), GetBinaryNames(editMod.Binaries.ConvertAll(bd => (bd.Name, bd.Data))));
		if (isUpdated)
			mod.LastUpdated = DateTime.UtcNow;

		_modScreenshotProcessor.MoveScreenshotsDirectory(mod.Name, editMod.Name);

		foreach (string screenshotToDelete in editMod.ScreenshotsToDelete)
			_modScreenshotProcessor.DeleteScreenshot(editMod.Name, screenshotToDelete);

		_modScreenshotProcessor.ProcessModScreenshotUpload(editMod.Name, editMod.Screenshots);

		mod.ModTypes = editMod.ModTypes?.ToFlagEnum<ModTypes>() ?? ModTypes.None;
		mod.HtmlDescription = editMod.HtmlDescription;
		mod.IsHidden = editMod.IsHidden;
		mod.Name = editMod.Name;
		mod.TrailerUrl = editMod.TrailerUrl;
		mod.Url = editMod.Url ?? string.Empty;
		await _dbContext.SaveChangesAsync(); // Save changes here so PlayerMods entities can be assigned properly.

		UpdatePlayerMods(editMod.PlayerIds ?? new(), mod.Id);
		await _dbContext.SaveChangesAsync();
	}

	public async Task DeleteModAsync(int id)
	{
		ModEntity? mod = _dbContext.Mods.FirstOrDefault(m => m.Id == id);
		if (mod == null)
			throw new NotFoundException($"Mod with ID '{id}' does not exist.");

		_modArchiveProcessor.DeleteModFilesAndClearCache(mod.Name);
		_modScreenshotProcessor.DeleteScreenshotsDirectory(mod.Name);

		_dbContext.Mods.Remove(mod);
		await _dbContext.SaveChangesAsync();
	}

	private void UpdatePlayerMods(List<int> playerIds, int modId)
	{
		foreach (PlayerModEntity newEntity in playerIds.ConvertAll(pi => new PlayerModEntity { ModId = modId, PlayerId = pi }))
		{
			if (!_dbContext.PlayerMods.Any(pam => pam.ModId == newEntity.ModId && pam.PlayerId == newEntity.PlayerId))
				_dbContext.PlayerMods.Add(newEntity);
		}

		foreach (PlayerModEntity entityToRemove in _dbContext.PlayerMods.Where(pam => pam.ModId == modId && !playerIds.Contains(pam.PlayerId)))
			_dbContext.PlayerMods.Remove(entityToRemove);
	}

	private static void ValidateName(string name)
	{
		if (string.IsNullOrWhiteSpace(name))
			throw new AdminDomainException("Mod name must not be empty or consist of white space only.");

		if (name.Any(c => Path.GetInvalidFileNameChars().Contains(c)))
			throw new AdminDomainException("Mod name must not contain invalid file name characters.");

		// Temporarily disallow + because it breaks old API calls where the mod name is in the URL. TODO: Remove this after old API calls have been removed.
		if (name.Any(c => c == '+'))
			throw new AdminDomainException("Mod name must not contain the + character.");
	}

	private static Dictionary<BinaryName, byte[]> GetBinaryNames(List<(string Name, byte[] Data)> binaries)
	{
		Dictionary<BinaryName, byte[]> dict = new();

		foreach ((string name, byte[] data) in binaries)
		{
			BinaryName binaryName = new(ModBinaryToc.DetermineType(data), name);
			if (dict.ContainsKey(binaryName))
				throw new InvalidModArchiveException("Binary names must all be unique.");

			dict.Add(binaryName, data);
		}

		return dict;
	}
}
