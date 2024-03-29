using DevilDaggersInfo.Core.Common;
using DevilDaggersInfo.Core.Spawnset;
using DevilDaggersInfo.Web.ApiSpec.Admin.Spawnsets;
using DevilDaggersInfo.Web.Server.Domain.Admin.Exceptions;
using DevilDaggersInfo.Web.Server.Domain.Converters.CoreToDomain;
using DevilDaggersInfo.Web.Server.Domain.Entities;
using DevilDaggersInfo.Web.Server.Domain.Exceptions;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;

namespace DevilDaggersInfo.Web.Server.Domain.Admin.Services;

public class SpawnsetService
{
	private readonly ApplicationDbContext _dbContext;

	public SpawnsetService(ApplicationDbContext dbContext)
	{
		_dbContext = dbContext;
	}

	public async Task AddSpawnsetAsync(AddSpawnset addSpawnset)
	{
		ValidateName(addSpawnset.Name);

		if (!SpawnsetBinary.TryParse(addSpawnset.FileContents, out SpawnsetBinary? spawnsetBinary))
			throw new AdminDomainException("File could not be parsed to a proper survival file.");

		byte[] spawnsetHash = MD5.HashData(addSpawnset.FileContents);
		var existingSpawnset = await _dbContext.Spawnsets.Select(s => new { s.Name, s.Md5Hash }).FirstOrDefaultAsync(s => s.Md5Hash == spawnsetHash);
		if (existingSpawnset != null)
			throw new AdminDomainException($"Spawnset is exactly the same as an already existing spawnset named '{existingSpawnset.Name}'.");

		if (!_dbContext.Players.Any(p => p.Id == addSpawnset.PlayerId))
			throw new AdminDomainException($"Player with ID '{addSpawnset.PlayerId}' does not exist.");

		if (_dbContext.Spawnsets.Any(m => m.Name == addSpawnset.Name))
			throw new AdminDomainException($"Spawnset with name '{addSpawnset.Name}' already exists.");

		(SpawnSectionInfo PreLoopSection, SpawnSectionInfo LoopSection) sections = spawnsetBinary.CalculateSections();
		EffectivePlayerSettings effectivePlayerSettings = spawnsetBinary.GetEffectivePlayerSettings();

		SpawnsetEntity spawnset = new()
		{
			HtmlDescription = addSpawnset.HtmlDescription,
			IsPractice = addSpawnset.IsPractice,
			MaxDisplayWaves = addSpawnset.MaxDisplayWaves,
			Name = addSpawnset.Name,
			PlayerId = addSpawnset.PlayerId,
			LastUpdated = DateTime.UtcNow,
			File = addSpawnset.FileContents,
			Md5Hash = MD5.HashData(addSpawnset.FileContents),
			GameMode = spawnsetBinary.GameMode.ToDomain(),
			SpawnVersion = spawnsetBinary.SpawnVersion,
			WorldVersion = spawnsetBinary.WorldVersion,
			PreLoopLength = sections.PreLoopSection.Length.HasValue ? (int)GameTime.FromSeconds(sections.PreLoopSection.Length.Value).GameUnits : null,
			PreLoopSpawnCount = sections.PreLoopSection.SpawnCount,
			LoopLength = sections.LoopSection.Length.HasValue ? (int)GameTime.FromSeconds(sections.LoopSection.Length.Value).GameUnits : null,
			LoopSpawnCount = sections.LoopSection.SpawnCount,
			HandLevel = spawnsetBinary.HandLevel.ToDomain(),
			AdditionalGems = spawnsetBinary.AdditionalGems,
			TimerStart = (int)GameTime.FromSeconds(spawnsetBinary.TimerStart).GameUnits,
			EffectiveHandLevel = effectivePlayerSettings.HandLevel.ToDomain(),
			EffectiveGemsOrHoming = effectivePlayerSettings.GemsOrHoming,
			EffectiveHandMesh = effectivePlayerSettings.HandMesh.ToDomain(),
		};
		_dbContext.Spawnsets.Add(spawnset);
		await _dbContext.SaveChangesAsync();
	}

	public async Task EditSpawnsetAsync(int id, EditSpawnset editSpawnset)
	{
		ValidateName(editSpawnset.Name);

		if (!_dbContext.Players.Any(p => p.Id == editSpawnset.PlayerId))
			throw new AdminDomainException($"Player with ID '{editSpawnset.PlayerId}' does not exist.");

		SpawnsetEntity? spawnset = await _dbContext.Spawnsets.FirstOrDefaultAsync(s => s.Id == id);
		if (spawnset == null)
			throw new NotFoundException($"Spawnset with ID '{id}' does not exist.");

		if (spawnset.Name != editSpawnset.Name && _dbContext.Spawnsets.Any(m => m.Name == editSpawnset.Name))
			throw new AdminDomainException($"Spawnset with name '{editSpawnset.Name}' already exists.");

		// Do not update LastUpdated here. This value is based only on the file which cannot be edited.
		spawnset.HtmlDescription = editSpawnset.HtmlDescription;
		spawnset.IsPractice = editSpawnset.IsPractice;
		spawnset.MaxDisplayWaves = editSpawnset.MaxDisplayWaves;
		spawnset.Name = editSpawnset.Name;
		spawnset.PlayerId = editSpawnset.PlayerId;
		await _dbContext.SaveChangesAsync();
	}

	public async Task DeleteSpawnsetAsync(int id)
	{
		SpawnsetEntity? spawnset = await _dbContext.Spawnsets.FirstOrDefaultAsync(s => s.Id == id);
		if (spawnset == null)
			throw new NotFoundException($"Spawnset with ID '{id}' does not exist.");

		if (_dbContext.CustomLeaderboards.Any(cl => cl.SpawnsetId == id))
			throw new AdminDomainException("Spawnset with custom leaderboard cannot be deleted.");

		_dbContext.Spawnsets.Remove(spawnset);
		await _dbContext.SaveChangesAsync();
	}

	private static void ValidateName(string name)
	{
		if (string.IsNullOrWhiteSpace(name))
			throw new AdminDomainException("Spawnset name must not be empty or consist of white space only.");

		if (name.Any(c => Path.GetInvalidFileNameChars().Contains(c)))
			throw new AdminDomainException("Spawnset name must not contain invalid file name characters.");
	}
}
