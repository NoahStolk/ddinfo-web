using DevilDaggersInfo.Core.Versioning;
using DevilDaggersInfo.Web.Server.Domain.Admin.Exceptions;
using DevilDaggersInfo.Web.Server.Domain.Entities;
using DevilDaggersInfo.Web.Server.Domain.Entities.Enums;
using DevilDaggersInfo.Web.Server.Domain.Services.Inversion;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DevilDaggersInfo.Web.Server.Domain.Admin.Services;

public class ToolService
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

	public async Task AddDistribution(string name, ToolPublishMethod publishMethod, ToolBuildType buildType, string version, byte[] zipFileContents, bool updateVersion, bool updateRequiredVersion)
	{
		if (!AppVersion.TryParse(version, out _))
			throw new AdminDomainException($"'{version}' is not a correct version number.");

		ToolEntity? tool = await _dbContext.Tools.FirstOrDefaultAsync(t => t.Name == name);
		if (tool == null)
			throw new AdminDomainException($"Tool with name '{name}' does not exist.");

		if (await _dbContext.ToolDistributions.AnyAsync(td => td.ToolName == name && td.PublishMethod == publishMethod && td.BuildType == buildType && td.VersionNumber == version))
			throw new AdminDomainException("Distribution already exists.");

		string path = _fileSystemService.GetToolDistributionPath(name, publishMethod, buildType, version);
		if (_fileSystemService.FileExists(path))
			throw new AdminDomainException("File for distribution already exists, but does not exist in the database. Please review the database and the file system.");

		await _fileSystemService.WriteAllBytesAsync(path, zipFileContents);

		if (updateVersion)
			tool.CurrentVersionNumber = version;

		if (updateRequiredVersion)
			tool.RequiredVersionNumber = version;

		ToolDistributionEntity distribution = new()
		{
			BuildType = buildType,
			PublishMethod = publishMethod,
			ToolName = name,
			VersionNumber = version,
		};
		_dbContext.ToolDistributions.Add(distribution);
		await _dbContext.SaveChangesAsync();

		_logger.LogInformation("{tool} {version} {buildType} {publishMethod} was published.", name, version, buildType, publishMethod);
	}
}
