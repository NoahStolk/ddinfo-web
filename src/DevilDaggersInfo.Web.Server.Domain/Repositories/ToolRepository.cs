using DevilDaggersInfo.Web.Server.Domain.Entities;
using DevilDaggersInfo.Web.Server.Domain.Entities.Enums;
using DevilDaggersInfo.Web.Server.Domain.Exceptions;
using DevilDaggersInfo.Web.Server.Domain.Models.Tools;
using DevilDaggersInfo.Web.Server.Domain.Services.Inversion;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

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

	public async Task<Tool?> GetToolAsync(string name)
	{
		ToolEntity? tool = await _dbContext.Tools.AsNoTracking().FirstOrDefaultAsync(t => t.Name == name);
		if (tool == null)
			return null;

		Dictionary<string, int> downloadCounts = (await _dbContext.ToolDistributions
			.AsNoTracking()
			.Select(td => new { td.ToolName, td.VersionNumber, td.DownloadCount })
			.Where(td => td.ToolName == name)
			.ToListAsync())
			.GroupBy(td => td.VersionNumber)
			.ToDictionary(td => td.Key, td => td.Sum(g => g.DownloadCount));

		return new()
		{
			DownloadCounts = downloadCounts,
			Name = tool.Name,
			DisplayName = tool.DisplayName,
			VersionNumberRequired = tool.RequiredVersionNumber,
			VersionNumber = tool.CurrentVersionNumber,
		};
	}

	public async Task<byte[]> GetToolDistributionFileAsync(string name, ToolPublishMethod publishMethod, ToolBuildType buildType, string version)
	{
		string path = _fileSystemService.GetToolDistributionPath(name, publishMethod, buildType, version);
		if (!File.Exists(path))
		{
			_logger.LogError("Tool distribution file at '{path}' does not exist!", path);
			throw new NotFoundException("Tool distribution file does not exist.");
		}

		return await File.ReadAllBytesAsync(path);
	}

	public async Task<ToolDistribution?> GetLatestToolDistributionAsync(string name, ToolPublishMethod publishMethod, ToolBuildType buildType)
	{
		List<string> versions = await _dbContext.ToolDistributions.Where(td => td.ToolName == name && td.PublishMethod == publishMethod && td.BuildType == buildType).Select(td => td.VersionNumber).ToListAsync();
		string? highestVersion = versions.MaxBy(Version.Parse);
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
		if (File.Exists(path))
			return (int)new FileInfo(path).Length;

		return 0;
	}
}
