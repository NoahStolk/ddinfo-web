using DevilDaggersInfo.Web.Shared.Dto.Public.Tools;

namespace DevilDaggersInfo.Web.Server.Controllers.Public;

[Route("api/tools")]
[ApiController]
public class ToolsController : ControllerBase
{
	private readonly IToolService _toolService;

	public ToolsController(IToolService toolService)
	{
		_toolService = toolService;
	}

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

	[HttpGet("{toolName}/distribution")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<ActionResult<GetToolDistribution>> GetToolDistribution([Required] string toolName, ToolPublishMethod publishMethod, ToolBuildType buildType, string version)
	{
		GetToolDistribution? distribution = await _toolService.GetToolDistributionByVersionAsync(toolName, publishMethod, buildType, version);
		if (distribution == null)
			return NotFound();

		return distribution;
	}

	[HttpGet("{toolName}/file")]
	[ProducesResponseType(typeof(FileContentResult), StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<ActionResult> GetToolDistributionFile([Required] string toolName, ToolPublishMethod publishMethod, ToolBuildType buildType, string version)
	{
		GetToolDistribution? distribution = await _toolService.GetToolDistributionByVersionAsync(toolName, publishMethod, buildType, version);
		if (distribution == null)
			return NotFound();

		byte[]? bytes = _toolService.GetToolDistributionFile(toolName, publishMethod, buildType, version);
		if (bytes == null)
			return NotFound();

		await _toolService.UpdateToolDistributionStatisticsAsync(toolName, publishMethod, buildType, version);

		return File(bytes, MediaTypeNames.Application.Zip, $"{toolName}{version}.zip");
	}
}
