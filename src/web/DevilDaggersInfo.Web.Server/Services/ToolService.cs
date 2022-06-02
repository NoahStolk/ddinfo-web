using DevilDaggersInfo.Web.Server.Converters.DomainToApi.Main;
using DevilDaggersInfo.Web.Server.Entities.Enums;
using DevilDaggersInfo.Web.Server.Enums;
using DevilDaggersInfo.Web.Server.InternalModels.Changelog;
using MainApi = DevilDaggersInfo.Api.Main.Tools;

namespace DevilDaggersInfo.Web.Server.Services;

// TODO: Implement domain models and use repository.
public class ToolService : IToolService
{
	private readonly ApplicationDbContext _dbContext;
	private readonly IFileSystemService _fileSystemService;
	private readonly ILogger<ToolService> _logger;

	public ToolService(ApplicationDbContext dbContext, IFileSystemService fileSystemService, ILogger<ToolService> logger)
	{
		_dbContext = dbContext;
		_fileSystemService = fileSystemService;
		_logger = logger;
	}

	public async Task<MainApi.GetTool?> GetToolAsync(string name)
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
		return tool.ToMainApi(downloads, changelog);
	}

	public byte[]? GetToolDistributionFile(string name, ToolPublishMethod publishMethod, ToolBuildType buildType, string version)
	{
		string path = GetToolDistributionPath(name, publishMethod, buildType, version);
		if (!IoFile.Exists(path))
		{
			_logger.LogError("Tool distribution file at '{path}' does not exist!", path);
#pragma warning disable S1168 // Empty arrays and collections should be returned instead of null
			return null;
#pragma warning restore S1168 // Empty arrays and collections should be returned instead of null
		}

		return IoFile.ReadAllBytes(path);
	}

	public async Task<MainApi.GetToolDistribution?> GetLatestToolDistributionAsync(string name, ToolPublishMethod publishMethod, ToolBuildType buildType)
	{
		List<string> versions = await _dbContext.ToolDistributions.Where(td => td.ToolName == name && td.PublishMethod == publishMethod && td.BuildType == buildType).Select(td => td.VersionNumber).ToListAsync();
		string? highestVersion = versions.OrderByDescending(Version.Parse).FirstOrDefault();
		if (highestVersion == null)
			return null;

		return await GetToolDistributionByVersionAsync(name, publishMethod, buildType, highestVersion);
	}

	public async Task<MainApi.GetToolDistribution?> GetToolDistributionByVersionAsync(string name, ToolPublishMethod publishMethod, ToolBuildType buildType, string version)
	{
		ToolDistributionEntity? distribution = await _dbContext.ToolDistributions.AsNoTracking().FirstOrDefaultAsync(td => td.ToolName == name && td.PublishMethod == publishMethod && td.BuildType == buildType && td.VersionNumber == version);
		return distribution?.ToMainApi(publishMethod, buildType, GetToolDistributionFileSize(distribution.ToolName, publishMethod, buildType, distribution.VersionNumber));
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

	public async Task AddDistribution(string name, ToolPublishMethod publishMethod, ToolBuildType buildType, string version, byte[] zipFileContents)
	{
		if (!Version.TryParse(version, out _))
			throw new InvalidOperationException($"'{version}' is not a correct version number.");

		ToolEntity? tool = await _dbContext.Tools.AsNoTracking().FirstOrDefaultAsync(t => t.Name == name);
		if (tool == null)
			throw new InvalidOperationException($"Tool with name '{name}' does not exist.");

		if (await _dbContext.ToolDistributions.AnyAsync(td => td.ToolName == name && td.PublishMethod == publishMethod && td.BuildType == buildType && td.VersionNumber == version))
			throw new InvalidOperationException("Distribution already exists.");

		string path = GetToolDistributionPath(name, publishMethod, buildType, version);
		if (File.Exists(path))
			throw new InvalidOperationException("File for distribution already exists, but does not exist in the database. Please review the database and the file system.");

		File.WriteAllBytes(path, zipFileContents);

		ToolDistributionEntity distribution = new()
		{
			BuildType = buildType,
			PublishMethod = publishMethod,
			ToolName = name,
			VersionNumber = version,
		};
		_dbContext.ToolDistributions.Add(distribution);
		await _dbContext.SaveChangesAsync();

		_logger.LogWarning("{tool} {version} {buildType} {publishMethod} was published.", name, version, buildType, publishMethod);
	}

	#region Utils

	private int GetToolDistributionFileSize(string name, ToolPublishMethod publishMethod, ToolBuildType buildType, string version)
	{
		string path = GetToolDistributionPath(name, publishMethod, buildType, version);
		if (IoFile.Exists(path))
			return (int)new FileInfo(path).Length;

		return 0;
	}

	private string GetToolDistributionPath(string name, ToolPublishMethod publishMethod, ToolBuildType buildType, string version)
	{
		return Path.Combine(_fileSystemService.GetPath(DataSubDirectory.Tools), $"{name}-{version}-{buildType}-{publishMethod}.zip");
	}

	#endregion Utils
}
