using DevilDaggersInfo.Core.Versioning;
using DevilDaggersInfo.Web.Server.Domain.Entities.Enums;
using DevilDaggersInfo.Web.Server.Domain.Models.Tools;
using DevilDaggersInfo.Web.Server.Domain.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace DevilDaggersInfo.Web.Server.Controllers.AppLauncher;

[Route("api/app-launcher")]
[ApiController]
public class AppLauncherController : ControllerBase
{
	private const string _toolName = "ddinfo-tools";

	private readonly ToolRepository _toolRepository;

	public AppLauncherController(ToolRepository toolRepository)
	{
		_toolRepository = toolRepository;
	}

	[HttpHead("is-latest-version")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	[ProducesResponseType(StatusCodes.Status426UpgradeRequired)]
	public async Task<ActionResult> IsLatestVersion([Required] string toolName, [Required] string version, [Required] ToolPublishMethod publishMethod, [Required] ToolBuildType buildType)
	{
		if (!AppVersion.TryParse(version, out AppVersion? currentVersion))
			return BadRequest("Could not parse version number.");

		ToolDistribution? distribution = await _toolRepository.GetLatestToolDistributionAsync(toolName, publishMethod, buildType);
		if (distribution == null)
			return NotFound();

		if (!AppVersion.TryParse(distribution.VersionNumber, out AppVersion? latestVersion))
			throw new InvalidOperationException($"Could not parse version number '{distribution.VersionNumber}'. Please check the database.");

		if (currentVersion >= latestVersion)
			return Ok();

		return StatusCode(426, "Update required.");
	}

	[HttpGet("latest-version-file")]
	[ProducesResponseType(typeof(FileStreamResult), StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<ActionResult> GetLatestVersionFile([Required] ToolPublishMethod publishMethod, [Required] ToolBuildType buildType)
	{
		ToolDistribution? distribution = await _toolRepository.GetLatestToolDistributionAsync(_toolName, publishMethod, buildType);
		if (distribution == null)
			return NotFound();

		byte[] bytes = await _toolRepository.GetToolDistributionFileAsync(_toolName, publishMethod, buildType, distribution.VersionNumber);

		await _toolRepository.UpdateToolDistributionStatisticsAsync(_toolName, publishMethod, buildType, distribution.VersionNumber);

		MemoryStream ms = new(bytes);
		return new FileStreamResult(ms, "application/octet-stream");
	}
}
