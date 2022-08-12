using DevilDaggersInfo.Common.Exceptions;
using DevilDaggersInfo.Core.Versioning;
using DevilDaggersInfo.Types.Web;
using DevilDaggersInfo.Web.Server.Domain.Entities;
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

		Dictionary<string, List<ChangelogEntry>> deserialized = JsonConvert.DeserializeObject<Dictionary<string, List<ChangelogEntry>>>(File.ReadAllText(Path.Combine(_fileSystemService.GetPath(DataSubDirectory.Tools), "Changelogs.json"))) ?? throw new("Could not deserialize Changelogs.json.");
		deserialized.TryGetValue(name, out List<ChangelogEntry>? changelog);
		return new()
		{
			Changelog = changelog?.ConvertAll(ce => new ToolVersion
			{
				Changes = ce.Changes.Select(c => ToModel(c)).ToList(),
				Date = ce.Date,
				DownloadCount = downloads.ContainsKey(ce.VersionNumber) ? downloads[ce.VersionNumber] : 0,
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
			SubChanges = change.SubChanges?.Select(c => ToModel(c)).ToList(),
		};
	}

	public byte[]? GetToolDistributionFile(string name, ToolPublishMethod publishMethod, ToolBuildType buildType, string version)
	{
		string path = _fileSystemService.GetToolDistributionPath(name, publishMethod, buildType, version);
		if (!File.Exists(path))
		{
			_logger.LogError("Tool distribution file at '{path}' does not exist!", path);
#pragma warning disable S1168 // Empty arrays and collections should be returned instead of null
			return null;
#pragma warning restore S1168 // Empty arrays and collections should be returned instead of null
		}

		return File.ReadAllBytes(path);
	}

	public async Task<List<ToolDistribution>> GetLatestToolDistributionsAsync(OperatingSystemType operatingSystem)
	{
		List<ToolDistributionEntity> distributionEntities = await (operatingSystem switch
		{
			OperatingSystemType.Windows => _dbContext.ToolDistributions.Where(td => td.PublishMethod == ToolPublishMethod.SelfContained && (td.BuildType == ToolBuildType.WindowsWpf || td.BuildType == ToolBuildType.WindowsConsole || td.BuildType == ToolBuildType.WindowsPhotino)),
			OperatingSystemType.Windows7 => _dbContext.ToolDistributions.Where(td => td.PublishMethod == ToolPublishMethod.Default && (td.BuildType == ToolBuildType.WindowsWpf || td.BuildType == ToolBuildType.WindowsConsole || td.BuildType == ToolBuildType.WindowsPhotino)),
			OperatingSystemType.Linux => _dbContext.ToolDistributions.Where(td => td.PublishMethod == ToolPublishMethod.SelfContained && td.BuildType == ToolBuildType.LinuxPhotino),
			_ => throw new InvalidEnumConversionException(operatingSystem),
		}).ToListAsync();

		List<ToolDistribution> distributions = new();
		foreach (ToolDistributionEntity distribution in distributionEntities.OrderByDescending(td => AppVersion.Parse(td.VersionNumber)))
		{
			if (distributions.Any(td => td.Name == distribution.ToolName && td.BuildType == distribution.BuildType && td.PublishMethod == distribution.PublishMethod))
				continue;

			distributions.Add(new ToolDistribution
			{
				Name = distribution.ToolName,
				BuildType = distribution.BuildType,
				FileSize = GetToolDistributionFileSize(distribution.ToolName, distribution.PublishMethod, distribution.BuildType, distribution.VersionNumber),
				PublishMethod = distribution.PublishMethod,
				VersionNumber = distribution.VersionNumber,
			});
		}

		return distributions;
	}

	public async Task<ToolDistribution?> GetLatestToolDistributionAsync(string name, ToolPublishMethod publishMethod, ToolBuildType buildType)
	{
		List<string> versions = await _dbContext.ToolDistributions.Where(td => td.ToolName == name && td.PublishMethod == publishMethod && td.BuildType == buildType).Select(td => td.VersionNumber).ToListAsync();
		string? highestVersion = versions.OrderByDescending(AppVersion.Parse).FirstOrDefault();
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

	// TODO: Move to service?
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
