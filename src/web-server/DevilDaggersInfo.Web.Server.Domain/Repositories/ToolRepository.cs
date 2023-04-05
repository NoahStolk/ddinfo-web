using DevilDaggersInfo.Core.Versioning;
using DevilDaggersInfo.Web.Server.Domain.Entities;
using DevilDaggersInfo.Web.Server.Domain.Entities.Enums;
using DevilDaggersInfo.Web.Server.Domain.Exceptions;
using DevilDaggersInfo.Web.Server.Domain.Models.FileSystem;
using DevilDaggersInfo.Web.Server.Domain.Models.Tools;
using DevilDaggersInfo.Web.Server.Domain.Services.Inversion;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace DevilDaggersInfo.Web.Server.Domain.Repositories;

public class ToolRepository
{
	private readonly ApplicationDbContext _dbContext;
	private readonly IFileSystemService _fileSystemService;
	private readonly ILogger<ToolRepository> _logger;

	public ToolRepository(ApplicationDbContext dbContext, IFileSystemService fileSystemService, ILogger<ToolRepository> logger)
	{
		_dbContext = dbContext;
		_fileSystemService = fileSystemService;
		_logger = logger;
	}

	private async Task<string> GetJsonString(string name)
	{
		return await _fileSystemService.ReadAllTextAsync(Path.Combine(_fileSystemService.GetPath(DataSubDirectory.Tools), $"{name}.json"));
	}

	public async Task<Tool?> GetToolAsync(string name)
	{
		ToolEntity? tool = await _dbContext.Tools.AsNoTracking().FirstOrDefaultAsync(t => t.Name == name);
		if (tool == null)
			return null;

		Dictionary<string, int> downloads = (await _dbContext.ToolDistributions
			.AsNoTracking()
			.Select(td => new { td.ToolName, td.VersionNumber, td.DownloadCount })
			.Where(td => td.ToolName == name)
			.ToListAsync())
			.GroupBy(td => td.VersionNumber)
			.ToDictionary(td => td.Key, td => td.Sum(g => g.DownloadCount));

		List<ChangelogEntry> changelog = JsonConvert.DeserializeObject<List<ChangelogEntry>>(await GetJsonString(name)) ?? throw new("Could not deserialize Changelogs.json.");
		return new()
		{
			Changelog = changelog.ConvertAll(ce => new ToolVersion
			{
				Changes = ce.Changes.Select(ToModel).ToList(),
				Date = ce.Date,
				DownloadCount = downloads.TryGetValue(ce.VersionNumber, out int value) ? value : 0,
				VersionNumber = ce.VersionNumber,
			}),
			Name = tool.Name,
			DisplayName = tool.DisplayName,
			VersionNumberRequired = tool.RequiredVersionNumber,
			VersionNumber = tool.CurrentVersionNumber,
		};

		static ToolVersionChange ToModel(Change change) => new()
		{
			Description = change.Description,
			SubChanges = change.SubChanges?.Select(ToModel).ToList(),
		};
	}

	public async Task<byte[]> GetToolDistributionFileAsync(string name, ToolPublishMethod publishMethod, ToolBuildType buildType, string version)
	{
		string path = _fileSystemService.GetToolDistributionPath(name, publishMethod, buildType, version);
		if (!_fileSystemService.FileExists(path))
		{
			_logger.LogError("Tool distribution file at '{path}' does not exist!", path);
			throw new NotFoundException("Tool distribution file does not exist.");
		}

		return await _fileSystemService.ReadAllBytesAsync(path);
	}

	public async Task<ToolDistribution?> GetLatestToolDistributionAsync(string name, ToolPublishMethod publishMethod, ToolBuildType buildType)
	{
		List<string> versions = await _dbContext.ToolDistributions.Where(td => td.ToolName == name && td.PublishMethod == publishMethod && td.BuildType == buildType).Select(td => td.VersionNumber).ToListAsync();
		string? highestVersion = versions.MaxBy(AppVersion.Parse);
		if (highestVersion == null)
			return null;

		return await GetToolDistributionByVersionAsync(name, publishMethod, buildType, highestVersion);
	}

	public async Task<ToolDistribution?> GetToolDistributionByVersionAsync(string name, ToolPublishMethod publishMethod, ToolBuildType buildType, string version)
	{
		ToolDistributionEntity? distribution = await _dbContext.ToolDistributions.AsNoTracking().FirstOrDefaultAsync(td => td.ToolName == name && td.PublishMethod == publishMethod && td.BuildType == buildType && td.VersionNumber == version);
		if (distribution == null)
			return null;

		return new()
		{
			Name = name,
			BuildType = buildType,
			PublishMethod = publishMethod,
			VersionNumber = distribution.VersionNumber,
			FileSize = GetToolDistributionFileSize(distribution.ToolName, publishMethod, buildType, distribution.VersionNumber),
		};
	}

	public async Task UpdateToolDistributionStatisticsAsync(string name, ToolPublishMethod publishMethod, ToolBuildType buildType, string version)
	{
		ToolDistributionEntity? distribution = await _dbContext.ToolDistributions.FirstOrDefaultAsync(td => td.ToolName == name && td.PublishMethod == publishMethod && td.BuildType == buildType && td.VersionNumber == version);
		if (distribution == null)
		{
			distribution = new ToolDistributionEntity
			{
				ToolName = name,
				VersionNumber = version,
			};
			_dbContext.ToolDistributions.Add(distribution);
		}

		distribution.DownloadCount++;

		await _dbContext.SaveChangesAsync();
	}

	private int GetToolDistributionFileSize(string name, ToolPublishMethod publishMethod, ToolBuildType buildType, string version)
	{
		string path = _fileSystemService.GetToolDistributionPath(name, publishMethod, buildType, version);
		if (_fileSystemService.FileExists(path))
			return (int)new FileInfo(path).Length;

		return 0;
	}
}
