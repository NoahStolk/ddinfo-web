using DevilDaggersInfo.Api.App;
using DevilDaggersInfo.Api.App.Updates;
using DevilDaggersInfo.Web.Server.Converters.DomainToApi.App;
using DevilDaggersInfo.Web.Server.Domain.Exceptions;
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

	[HttpGet("latest")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<ActionResult<GetLatestVersion>> GetLatest([Required] AppOperatingSystem appOperatingSystem)
	{
		Domain.Entities.Enums.ToolBuildType toolBuildType = GetToolBuildType(appOperatingSystem);
		ToolDistribution? distribution = await _toolRepository.GetLatestToolDistributionAsync(_toolName, Domain.Entities.Enums.ToolPublishMethod.SelfContained, toolBuildType);
		if (distribution == null)
			return NotFound();

		return distribution.ToAppApi();
	}

	[HttpGet("latest-file")]
	[ProducesResponseType(typeof(FileStreamResult), StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<ActionResult> GetLatestFile([Required] AppOperatingSystem appOperatingSystem)
	{
		Domain.Entities.Enums.ToolBuildType toolBuildType = GetToolBuildType(appOperatingSystem);
		ToolDistribution? distribution = await _toolRepository.GetLatestToolDistributionAsync(_toolName, Domain.Entities.Enums.ToolPublishMethod.SelfContained, toolBuildType);
		if (distribution == null)
			return NotFound();

		byte[] bytes = await _toolRepository.GetToolDistributionFileAsync(_toolName, Domain.Entities.Enums.ToolPublishMethod.SelfContained, toolBuildType, distribution.VersionNumber);

		await _toolRepository.UpdateToolDistributionStatisticsAsync(_toolName, Domain.Entities.Enums.ToolPublishMethod.SelfContained, toolBuildType, distribution.VersionNumber);

		MemoryStream ms = new(bytes);
		return new FileStreamResult(ms, "application/octet-stream");
	}

	private static Domain.Entities.Enums.ToolBuildType GetToolBuildType(AppOperatingSystem appOperatingSystem)
	{
		return appOperatingSystem switch
		{
			AppOperatingSystem.Windows => Domain.Entities.Enums.ToolBuildType.WindowsWarp,
			AppOperatingSystem.Linux => Domain.Entities.Enums.ToolBuildType.LinuxWarp,
			_ => throw new NotFoundException($"Operator system {appOperatingSystem} does not exist."),
		};
	}
}
