using DevilDaggersInfo.Api.Ddcl.Tools;
using DevilDaggersInfo.Types.Web;
using DevilDaggersInfo.Web.Server.Domain.Models.Tools;
using DevilDaggersInfo.Web.Server.Domain.Repositories;

namespace DevilDaggersInfo.Web.Server.Controllers.Ddcl;

[Route("api/ddcl/updates")]
[ApiController]
public class UpdatesController : ControllerBase
{
	private readonly ToolRepository _toolRepository;

	public UpdatesController(ToolRepository toolRepository)
	{
		_toolRepository = toolRepository;
	}

	// TODO: Make this endpoint generic for all tools.
	[HttpGet]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<ActionResult<GetUpdate>> GetUpdates([Required] ToolPublishMethod publishMethod, [Required] ToolBuildType buildType)
	{
		const string toolName = "DevilDaggersCustomLeaderboards";
		Tool tool = await _toolRepository.GetToolAsync(toolName) ?? throw new("DDCL not found in tool service.");
		ToolDistribution toolDistribution = await _toolRepository.GetLatestToolDistributionAsync(toolName, publishMethod, buildType) ?? throw new("No versions of DDCL found in tool service.");
		return new GetUpdate
		{
			VersionNumber = toolDistribution.VersionNumber,
			VersionNumberRequired = tool.VersionNumberRequired,
		};
	}
}
