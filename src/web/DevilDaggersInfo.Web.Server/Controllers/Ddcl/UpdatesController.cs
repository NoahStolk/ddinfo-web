using DevilDaggersInfo.Api.Ddcl.Tools;
using DevilDaggersInfo.Web.Server.Converters.ApiToDomain.Ddcl;
using DevilDaggersInfo.Web.Server.Domain.Models.Tools;
using DevilDaggersInfo.Web.Server.Domain.Services;

namespace DevilDaggersInfo.Web.Server.Controllers.Ddcl;

[Route("api/ddcl/updates")]
[ApiController]
public class UpdatesController : ControllerBase
{
	private readonly IToolService _toolService;

	public UpdatesController(IToolService toolService)
	{
		_toolService = toolService;
	}

	[HttpGet]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<ActionResult<GetUpdate>> GetUpdates(ToolPublishMethod publishMethod, ToolBuildType buildType)
	{
		const string toolName = "DevilDaggersCustomLeaderboards";
		Tool tool = await _toolService.GetToolAsync(toolName) ?? throw new("DDCL not found in tool service.");
		ToolDistribution? toolDistribution = await _toolService.GetLatestToolDistributionAsync(toolName, publishMethod.ToDomain(), buildType.ToDomain()) ?? throw new("No versions of DDCL found in tool service.");
		return new GetUpdate
		{
			VersionNumber = toolDistribution.VersionNumber,
			VersionNumberRequired = tool.VersionNumberRequired,
		};
	}
}
