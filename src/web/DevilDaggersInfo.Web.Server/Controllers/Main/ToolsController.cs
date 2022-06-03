using DevilDaggersInfo.Api.Main.Tools;
using DevilDaggersInfo.Web.Server.Converters.ApiToDomain.Main;

namespace DevilDaggersInfo.Web.Server.Controllers.Main;

[Route("api/tools")]
[ApiController]
public class ToolsController : ControllerBase
{
	private readonly IToolService _toolService;

	public ToolsController(IToolService toolService)
	{
		_toolService = toolService;
	}

	// FORBIDDEN: Used by DDSE 2.45.0.0.
	// FORBIDDEN: Used by DDAE 1.4.0.0.
	// FORBIDDEN: Used by DDCL 1.8.3.0.
	[HttpGet("{toolName}")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<ActionResult<GetTool>> GetTool([Required] string toolName)
	{
		GetTool? tool = await _toolService.GetToolAsync(toolName);
		if (tool == null)
			return NotFound();

		return tool;
	}

	// FORBIDDEN: Used by DDSE 2.45.0.0.
	// FORBIDDEN: Used by DDAE 1.4.0.0.
	[HttpGet("{toolName}/file")]
	[ProducesResponseType(typeof(FileContentResult), StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<ActionResult> GetToolDistributionFile([Required] string toolName, ToolPublishMethod publishMethod, ToolBuildType buildType, string? version = null)
	{
		GetToolDistribution? distribution;
		if (version == null)
			distribution = await _toolService.GetLatestToolDistributionAsync(toolName, publishMethod.ToDomain(), buildType.ToDomain());
		else
			distribution = await _toolService.GetToolDistributionByVersionAsync(toolName, publishMethod.ToDomain(), buildType.ToDomain(), version);

		if (distribution == null)
			return NotFound();

		byte[]? bytes = _toolService.GetToolDistributionFile(toolName, publishMethod.ToDomain(), buildType.ToDomain(), distribution.VersionNumber);
		if (bytes == null)
			return NotFound();

		await _toolService.UpdateToolDistributionStatisticsAsync(toolName, publishMethod.ToDomain(), buildType.ToDomain(), distribution.VersionNumber);

		return File(bytes, MediaTypeNames.Application.Zip, $"{toolName}{distribution.VersionNumber}.zip");
	}

	// FORBIDDEN: Used by DDSE 2.45.0.0.
	// FORBIDDEN: Used by DDAE 1.4.0.0.
	// FORBIDDEN: Used by DDCL 1.8.3.0.
	[HttpGet("{toolName}/distribution-latest")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<ActionResult<GetToolDistribution>> GetLatestToolDistribution([Required] string toolName, ToolPublishMethod publishMethod, ToolBuildType buildType)
	{
		GetToolDistribution? distribution = await _toolService.GetLatestToolDistributionAsync(toolName, publishMethod.ToDomain(), buildType.ToDomain());
		if (distribution == null)
			return NotFound();

		return distribution;
	}

	[HttpGet("{toolName}/distribution")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<ActionResult<GetToolDistribution>> GetToolDistributionByVersion([Required] string toolName, ToolPublishMethod publishMethod, ToolBuildType buildType, string version)
	{
		GetToolDistribution? distribution = await _toolService.GetToolDistributionByVersionAsync(toolName, publishMethod.ToDomain(), buildType.ToDomain(), version);
		if (distribution == null)
			return NotFound();

		return distribution;
	}
}
