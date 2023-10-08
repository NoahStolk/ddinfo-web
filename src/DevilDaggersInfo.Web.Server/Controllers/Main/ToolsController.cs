using DevilDaggersInfo.Web.ApiSpec.Main.Tools;
using DevilDaggersInfo.Web.Server.Converters.ApiToDomain.Main;
using DevilDaggersInfo.Web.Server.Converters.DomainToApi.Main;
using DevilDaggersInfo.Web.Server.Domain.Models.Tools;
using DevilDaggersInfo.Web.Server.Domain.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Net.Mime;

namespace DevilDaggersInfo.Web.Server.Controllers.Main;

[Route("api/tools")]
[ApiController]
public class ToolsController : ControllerBase
{
	private readonly ToolRepository _toolRepository;

	public ToolsController(ToolRepository toolRepository)
	{
		_toolRepository = toolRepository;
	}

	// FORBIDDEN: Used by DDSE 2.45.0.0.
	// FORBIDDEN: Used by DDAE 1.4.0.0.
	[HttpGet("{toolName}")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<ActionResult<GetTool>> GetTool([Required] string toolName)
	{
		Tool? tool = await _toolRepository.GetToolAsync(toolName);
		if (tool == null)
			return NotFound();

		GetTool model = tool.ToMainApi();

		// TODO: This is only used as a fallback for DDSE/DDAE changelog windows. Remove this when DDSE/DDAE are no longer supported.
		if (toolName is "DevilDaggersSurvivalEditor" or "DevilDaggersAssetEditor")
		{
			model.Changelog = new List<GetToolVersion>
			{
				new()
				{
					VersionNumber = "Changelog moved to GitHub",
					Date = new DateTime(2023, 10, 9),
					Changes = new List<GetToolVersionChange>
					{
						new()
						{
							Description = "The changelog has been moved to GitHub. Please visit the GitHub page for the changelog.",
						},
					},
				},
			};
		}

		return model;
	}

	// FORBIDDEN: Used by DDSE 2.45.0.0.
	// FORBIDDEN: Used by DDAE 1.4.0.0.
	[HttpGet("{toolName}/file")]
	[ProducesResponseType(typeof(FileContentResult), StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<ActionResult> GetToolDistributionFile([Required] string toolName, ToolPublishMethod publishMethod, ToolBuildType buildType, string? version = null)
	{
		ToolDistribution? distribution;
		if (version == null)
			distribution = await _toolRepository.GetLatestToolDistributionAsync(toolName, publishMethod.ToDomain(), buildType.ToDomain());
		else
			distribution = await _toolRepository.GetToolDistributionByVersionAsync(toolName, publishMethod.ToDomain(), buildType.ToDomain(), version);

		if (distribution == null)
			return NotFound();

		byte[] bytes = await _toolRepository.GetToolDistributionFileAsync(toolName, publishMethod.ToDomain(), buildType.ToDomain(), distribution.VersionNumber);

		await _toolRepository.UpdateToolDistributionStatisticsAsync(toolName, publishMethod.ToDomain(), buildType.ToDomain(), distribution.VersionNumber);

		return File(bytes, MediaTypeNames.Application.Zip, $"{toolName}{distribution.VersionNumber}.zip");
	}

	// FORBIDDEN: Used by DDSE 2.45.0.0.
	// FORBIDDEN: Used by DDAE 1.4.0.0.
	[HttpGet("{toolName}/distribution-latest")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<ActionResult<GetToolDistribution>> GetLatestToolDistribution([Required] string toolName, [Required] ToolPublishMethod publishMethod, [Required] ToolBuildType buildType)
	{
		ToolDistribution? distribution = await _toolRepository.GetLatestToolDistributionAsync(toolName, publishMethod.ToDomain(), buildType.ToDomain());
		if (distribution == null)
			return NotFound();

		return distribution.ToMainApi();
	}

	[HttpGet("{toolName}/distribution")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<ActionResult<GetToolDistribution>> GetToolDistributionByVersion([Required] string toolName, [Required] ToolPublishMethod publishMethod, [Required] ToolBuildType buildType, string version)
	{
		ToolDistribution? distribution = await _toolRepository.GetToolDistributionByVersionAsync(toolName, publishMethod.ToDomain(), buildType.ToDomain(), version);
		if (distribution == null)
			return NotFound();

		return distribution.ToMainApi();
	}
}
