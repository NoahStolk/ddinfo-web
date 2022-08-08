using DevilDaggersInfo.Core.Spawnset;
using DevilDaggersInfo.Web.Server.Domain.Commands.Spawnsets;
using DevilDaggersInfo.Web.Server.Domain.Entities;
using DevilDaggersInfo.Web.Server.Domain.Exceptions;
using DevilDaggersInfo.Web.Server.Domain.Extensions;
using DevilDaggersInfo.Web.Server.Domain.Models.FileSystem;
using DevilDaggersInfo.Web.Server.Domain.Models.Spawnsets;
using System.Security.Claims;
using System.Security.Cryptography;

namespace DevilDaggersInfo.Web.Server.Domain.Services;

public class SpawnsetService
{
	private readonly SpawnsetHashCache _spawnsetHashCache;
	private readonly ApplicationDbContext _dbContext;
	private readonly IFileSystemService _fileSystemService;
	private readonly IAuditLogger _auditLogger;

	public SpawnsetService(SpawnsetHashCache spawnsetHashCache, ApplicationDbContext dbContext, IFileSystemService fileSystemService, IAuditLogger auditLogger)
	{
		_spawnsetHashCache = spawnsetHashCache;
		_dbContext = dbContext;
		_fileSystemService = fileSystemService;
		_auditLogger = auditLogger;
	}

	// TODO: Get ClaimsPrincipal from HttpContextAccessor.
	public async Task AddSpawnsetAsync(AddSpawnset addSpawnset, ClaimsPrincipal claimsPrincipal)
	{
		ValidateName(addSpawnset.Name);

		if (!SpawnsetBinary.TryParse(addSpawnset.FileContents, out _))
			throw new AdminDomainException("File could not be parsed to a proper survival file.");

		byte[] spawnsetHash = MD5.HashData(addSpawnset.FileContents);
		SpawnsetHashCacheData? existingSpawnset = _spawnsetHashCache.GetSpawnset(spawnsetHash);
		if (existingSpawnset != null)
			throw new AdminDomainException($"Spawnset is exactly the same as an already existing spawnset named '{existingSpawnset.Name}'.");

		// Entity validation.
		if (!_dbContext.Players.Any(p => p.Id == addSpawnset.PlayerId))
			throw new AdminDomainException($"Player with ID '{addSpawnset.PlayerId}' does not exist.");

		if (_dbContext.Spawnsets.Any(m => m.Name == addSpawnset.Name))
			throw new AdminDomainException($"Spawnset with name '{addSpawnset.Name}' already exists.");

		// Add file.
		string path = Path.Combine(_fileSystemService.GetPath(DataSubDirectory.Spawnsets), addSpawnset.Name);
		File.WriteAllBytes(path, addSpawnset.FileContents);

		// Add entity.
		SpawnsetEntity spawnset = new()
		{
			HtmlDescription = addSpawnset.HtmlDescription,
			IsPractice = addSpawnset.IsPractice,
			MaxDisplayWaves = addSpawnset.MaxDisplayWaves,
			Name = addSpawnset.Name,
			PlayerId = addSpawnset.PlayerId,
			LastUpdated = DateTime.UtcNow,
		};
		_dbContext.Spawnsets.Add(spawnset);
		await _dbContext.SaveChangesAsync();

		_auditLogger.LogAdd(addSpawnset.GetLog(), claimsPrincipal, spawnset.Id, new() { new($"File {_fileSystemService.FormatPath(path)} was added.", FileSystemInformationType.Add) });
	}

	public async Task EditSpawnsetAsync(EditSpawnset editSpawnset, ClaimsPrincipal claimsPrincipal)
	{
		ValidateName(editSpawnset.Name);

		if (!_dbContext.Players.Any(p => p.Id == editSpawnset.PlayerId))
			throw new AdminDomainException($"Player with ID '{editSpawnset.PlayerId}' does not exist.");

		SpawnsetEntity? spawnset = _dbContext.Spawnsets.FirstOrDefault(s => s.Id == editSpawnset.SpawnsetId);
		if (spawnset == null)
			throw new NotFoundException($"Spawnset with ID '{editSpawnset.SpawnsetId}' does not exist.");

		string? moveInfo = null;
		if (spawnset.Name != editSpawnset.Name)
		{
			if (_dbContext.Spawnsets.Any(m => m.Name == editSpawnset.Name))
				throw new AdminDomainException($"Spawnset with name '{editSpawnset.Name}' already exists.");

			string directory = _fileSystemService.GetPath(DataSubDirectory.Spawnsets);
			string oldPath = Path.Combine(directory, spawnset.Name);
			string newPath = Path.Combine(directory, editSpawnset.Name);
			File.Move(oldPath, newPath);
			moveInfo = $"File {_fileSystemService.FormatPath(oldPath)} was moved to {_fileSystemService.FormatPath(newPath)}.";
		}

		EditSpawnset logDto = new()
		{
			HtmlDescription = spawnset.HtmlDescription,
			IsPractice = spawnset.IsPractice,
			MaxDisplayWaves = spawnset.MaxDisplayWaves,
			Name = spawnset.Name,
			PlayerId = spawnset.PlayerId,
		};

		// Do not update LastUpdated here. This value is based only on the file which cannot be edited.
		spawnset.HtmlDescription = editSpawnset.HtmlDescription;
		spawnset.IsPractice = editSpawnset.IsPractice;
		spawnset.MaxDisplayWaves = editSpawnset.MaxDisplayWaves;
		spawnset.Name = editSpawnset.Name;
		spawnset.PlayerId = editSpawnset.PlayerId;
		await _dbContext.SaveChangesAsync();

		_auditLogger.LogEdit(logDto.GetLog(), editSpawnset.GetLog(), claimsPrincipal, spawnset.Id, moveInfo == null ? null : new() { new(moveInfo, FileSystemInformationType.Move) });
	}

	public async Task DeleteSpawnsetAsync(int id, ClaimsPrincipal claimsPrincipal)
	{
		SpawnsetEntity? spawnset = _dbContext.Spawnsets.FirstOrDefault(s => s.Id == id);
		if (spawnset == null)
			throw new NotFoundException($"Spawnset with ID '{id}' does not exist.");

		if (_dbContext.CustomLeaderboards.Any(cl => cl.SpawnsetId == id))
			throw new AdminDomainException("Spawnset with custom leaderboard cannot be deleted.");

		string path = Path.Combine(_fileSystemService.GetPath(DataSubDirectory.Spawnsets), spawnset.Name);
		bool fileExists = File.Exists(path);
		if (fileExists)
		{
			File.Delete(path);
			_spawnsetHashCache.Clear();
		}

		_dbContext.Spawnsets.Remove(spawnset);
		await _dbContext.SaveChangesAsync();

		string message = fileExists ? $"File {_fileSystemService.FormatPath(path)} was deleted." : $"File {_fileSystemService.FormatPath(path)} was not deleted because it does not exist.";
		_auditLogger.LogDelete(spawnset.GetLog(), claimsPrincipal, spawnset.Id, new() { new(message, fileExists ? FileSystemInformationType.Delete : FileSystemInformationType.NotFoundUnexpected) });
	}

	public void ValidateName(string name)
	{
		if (string.IsNullOrWhiteSpace(name))
			throw new AdminDomainException("Spawnset name must not be empty or consist of white space only.");

		if (name.Any(c => Path.GetInvalidFileNameChars().Contains(c)))
			throw new AdminDomainException("Spawnset name must not contain invalid file name characters.");

		// Temporarily disallow + because it breaks old API calls where the spawnset name is in the URL. TODO: Remove this after old API calls have been removed.
		if (name.Any(c => c == '+'))
			throw new AdminDomainException("Spawnset name must not contain the + character.");
	}
}
