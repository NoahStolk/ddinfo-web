using DevilDaggersInfo.Core.Common.Extensions;
using DevilDaggersInfo.Core.Mod;
using DevilDaggersInfo.Web.Server.Domain.Admin.Exceptions;
using DevilDaggersInfo.Web.Server.Domain.Entities;
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

	public async Task AddModAsync(ApiSpec.Admin.Mods.AddMod addMod)
	{
		ValidateName(addMod.Name);

		if (addMod.PlayerIds == null || addMod.PlayerIds.Count == 0)
			throw new AdminDomainException("Mod must have at least one author.");

		if (await _dbContext.Mods.AnyAsync(m => m.Name == addMod.Name))
			throw new AdminDomainException($"Mod with name '{addMod.Name}' already exists.");

		foreach (int playerId in addMod.PlayerIds)
		{
			if (!await _dbContext.Players.AnyAsync(p => p.Id == playerId))
				throw new AdminDomainException($"Player with ID '{playerId}' does not exist.");
		}

		if (addMod.Binaries.Count > 0)
			await _modArchiveProcessor.ProcessModBinaryUploadAsync(addMod.Name, GetBinaryNames(addMod.Binaries.ConvertAll(bd => (bd.Name, bd.Data))));

		_modScreenshotProcessor.ProcessModScreenshotUpload(addMod.Name, addMod.Screenshots);

		ModEntity mod = new()
		{
			ModTypes = addMod.ModTypes?.ToFlagEnum<Entities.Enums.ModTypes>() ?? Entities.Enums.ModTypes.None,
			HtmlDescription = addMod.HtmlDescription,
			IsHidden = addMod.IsHidden,
			LastUpdated = DateTime.UtcNow,
			Name = addMod.Name,
			TrailerUrl = addMod.TrailerUrl,
			Url = addMod.Url ?? string.Empty,
		};
		_dbContext.Mods.Add(mod);
		await _dbContext.SaveChangesAsync(); // Save changes here so PlayerMods entities can be assigned properly.

		UpdatePlayerMods(addMod.PlayerIds ?? [], mod.Id);
		await _dbContext.SaveChangesAsync();
	}

	public async Task EditModAsync(int id, ApiSpec.Admin.Mods.EditMod editMod)
	{
		ValidateName(editMod.Name);

		if (editMod.PlayerIds == null || editMod.PlayerIds.Count == 0)
			throw new AdminDomainException("Mod must have at least one author.");

		foreach (int playerId in editMod.PlayerIds)
		{
			if (!await _dbContext.Players.AnyAsync(p => p.Id == playerId))
				throw new AdminDomainException($"Player with ID '{playerId}' does not exist.");
		}

		ModEntity? mod = await _dbContext.Mods
			.Include(m => m.PlayerMods)
			.FirstOrDefaultAsync(m => m.Id == id);
		if (mod == null)
			throw new NotFoundException($"Mod with ID '{id}' does not exist.");

		if (mod.Name != editMod.Name && await _dbContext.Mods.AnyAsync(m => m.Name == editMod.Name))
			throw new AdminDomainException($"Mod with name '{editMod.Name}' already exists.");

		bool isUpdated = await _modArchiveProcessor.TransformBinariesInModArchiveAsync(mod.Name, editMod.Name, editMod.BinariesToDelete.ConvertAll(s => BinaryName.Parse(s, mod.Name)), GetBinaryNames(editMod.Binaries.ConvertAll(bd => (bd.Name, bd.Data))));
		if (isUpdated)
			mod.LastUpdated = DateTime.UtcNow;

		_modScreenshotProcessor.MoveScreenshotsDirectory(mod.Name, editMod.Name);

		foreach (string screenshotToDelete in editMod.ScreenshotsToDelete)
			_modScreenshotProcessor.DeleteScreenshot(editMod.Name, screenshotToDelete);

		_modScreenshotProcessor.ProcessModScreenshotUpload(editMod.Name, editMod.Screenshots);

		mod.ModTypes = editMod.ModTypes?.ToFlagEnum<Entities.Enums.ModTypes>() ?? Entities.Enums.ModTypes.None;
		mod.HtmlDescription = editMod.HtmlDescription;
		mod.IsHidden = editMod.IsHidden;
		mod.Name = editMod.Name;
		mod.TrailerUrl = editMod.TrailerUrl;
		mod.Url = editMod.Url ?? string.Empty;
		await _dbContext.SaveChangesAsync(); // Save changes here so PlayerMods entities can be assigned properly.

		UpdatePlayerMods(editMod.PlayerIds ?? [], mod.Id);
		await _dbContext.SaveChangesAsync();
	}

	public async Task DeleteModAsync(int id)
	{
		ModEntity? mod = await _dbContext.Mods.FirstOrDefaultAsync(m => m.Id == id);
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
