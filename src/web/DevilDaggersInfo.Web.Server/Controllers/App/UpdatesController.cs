using DevilDaggersInfo.Api.App;
using DevilDaggersInfo.Api.App.Updates;
using DevilDaggersInfo.Web.Server.Converters.DomainToApi.App;
using DevilDaggersInfo.Web.Server.Domain.Entities.Enums;
using DevilDaggersInfo.Web.Server.Domain.Models.Tools;
using DevilDaggersInfo.Web.Server.Domain.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace DevilDaggersInfo.Web.Server.Controllers.App;

[Route("api/app/updates")]
[ApiController]
public class UpdatesController : ControllerBase
{
	private const string _toolName = "ddinfo-tools";

	private readonly ToolRepository _toolRepository;

	public UpdatesController(ToolRepository toolRepository)
	{
		_toolRepository = toolRepository;
	}

	[HttpGet("latest-version")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<ActionResult<GetLatestVersion>> GetLatestVersion([Required] AppOperatingSystem appOperatingSystem)
	{
		ToolBuildType? buildType = appOperatingSystem switch
		{
			AppOperatingSystem.Windows => ToolBuildType.WindowsWarp,
			AppOperatingSystem.Linux => ToolBuildType.LinuxWarp,
			_ => null,
		};

		if (!buildType.HasValue)
			return NotFound();

		ToolDistribution? distribution = await _toolRepository.GetLatestToolDistributionAsync(_toolName, ToolPublishMethod.SelfContained, buildType.Value);
		if (distribution == null)
			return NotFound();

		return distribution.ToAppApi();
	}

	[HttpGet("latest-version-file")]
	[ProducesResponseType(typeof(FileStreamResult), StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<ActionResult> GetLatestVersionFile([Required] AppOperatingSystem appOperatingSystem)
	{
		ToolBuildType? buildType = appOperatingSystem switch
		{
			AppOperatingSystem.Windows => ToolBuildType.WindowsWarp,
			AppOperatingSystem.Linux => ToolBuildType.LinuxWarp,
			_ => null,
		};

		if (!buildType.HasValue)
			return NotFound();

		ToolDistribution? distribution = await _toolRepository.GetLatestToolDistributionAsync(_toolName, ToolPublishMethod.SelfContained, buildType.Value);
		if (distribution == null)
			return NotFound();

		byte[] bytes = await _toolRepository.GetToolDistributionFileAsync(_toolName, ToolPublishMethod.SelfContained, buildType.Value, distribution.VersionNumber);

		await _toolRepository.UpdateToolDistributionStatisticsAsync(_toolName, ToolPublishMethod.SelfContained, buildType.Value, distribution.VersionNumber);

		MemoryStream ms = new(bytes);
		return new FileStreamResult(ms, "application/octet-stream");
	}
}
