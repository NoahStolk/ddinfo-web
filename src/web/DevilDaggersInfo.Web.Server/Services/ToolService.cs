using DevilDaggersInfo.Web.Server.Converters.Public;
using DevilDaggersInfo.Web.Server.InternalModels.Json;
using DevilDaggersInfo.Web.Shared.Dto.Public.Tools;

namespace DevilDaggersInfo.Web.Server.Services;

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

	public async Task<GetToolDistribution?> GetLatestToolDistributionAsync(string name, ToolPublishMethod publishMethod, ToolBuildType buildType)
	{
		ToolEntity? tool = await _dbContext.Tools.FirstOrDefaultAsync(t => t.Name == name);
		if (tool == null)
			return null;

		return await GetToolDistributionByVersionAsync(name, publishMethod, buildType, tool.CurrentVersionNumber);
	}

	public async Task<GetToolDistribution?> GetToolDistributionByVersionAsync(string name, ToolPublishMethod publishMethod, ToolBuildType buildType, string version)
	{
		// Temporary until DDCL 1.8.3.0 is obsolete.
		if (name == "DevilDaggersCustomLeaderboards" && buildType == ToolBuildType.WindowsWpf)
			buildType = ToolBuildType.WindowsConsole;

		ToolDistributionEntity? distribution = await _dbContext.ToolDistributions.AsNoTracking().FirstOrDefaultAsync(t => t.ToolName == name && t.PublishMethod == publishMethod && t.BuildType == buildType && t.VersionNumber == version);
		return distribution?.ToDto(publishMethod, buildType, GetToolDistributionFileSize(distribution.ToolName, publishMethod, buildType, distribution.VersionNumber));
	}

	public async Task<GetTool?> GetToolAsync(string name)
	{
		ToolEntity? tool = await _dbContext.Tools.AsNoTracking().FirstOrDefaultAsync(t => t.Name == name);
		if (tool == null)
			return null;

		List<ToolDistributionEntity> distributions = await _dbContext.ToolDistributions
			.AsNoTracking()
			.Where(td => td.ToolName == name)
			.ToListAsync();

		Dictionary<string, List<ChangelogEntry>> deserialized = JsonConvert.DeserializeObject<Dictionary<string, List<ChangelogEntry>>>(File.ReadAllText(Path.Combine(_fileSystemService.GetPath(DataSubDirectory.Tools), "Changelogs.json"))) ?? throw new("Could not deserialize Changelogs.json.");
		deserialized.TryGetValue(name, out List<ChangelogEntry>? changelog);
		return tool.ToDto(distributions.ToDictionary(d => d, d => GetToolDistributionFileSize(d.ToolName, d.PublishMethod, d.BuildType, d.VersionNumber)), changelog);
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

	#region Utils

	private int GetToolDistributionFileSize(string name, ToolPublishMethod publishMethod, ToolBuildType buildType, string version)
	{
		string path = GetToolDistributionPath(name, publishMethod, buildType, version);
		if (IoFile.Exists(path))
			return (int)new FileInfo(path).Length;

		_logger.LogError("Tool file '{path}' does not exist.", path);
		return 0;
	}

	private string GetToolDistributionPath(string name, ToolPublishMethod publishMethod, ToolBuildType buildType, string version)
	{
		return Path.Combine(_fileSystemService.GetPath(DataSubDirectory.Tools), name, $"{name}-{version}-{buildType}-{publishMethod}.zip");
	}

	#endregion Utils
}
