using DevilDaggersInfo.Api.App.Tools;
using DevilDaggersInfo.Types.Web;
using DevilDaggersInfo.Web.Server.Converters.DomainToApi.App;
using DevilDaggersInfo.Web.Server.Domain.Models.Tools;
using DevilDaggersInfo.Web.Server.Domain.Repositories;

namespace DevilDaggersInfo.Web.Server.Controllers.App;

[Route("api/app/tools")]
[ApiController]
public class ToolsController : ControllerBase
{
	private readonly ToolRepository _toolRepository;

	public ToolsController(ToolRepository toolRepository)
	{
		_toolRepository = toolRepository;
	}

	[HttpGet("{toolName}/distribution-latest")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<ActionResult<GetToolDistribution>> GetLatestToolDistribution([Required] string toolName, [Required] ToolPublishMethod publishMethod, [Required] ToolBuildType buildType)
	{
		ToolDistribution? distribution = await _toolRepository.GetLatestToolDistributionAsync(toolName, publishMethod, buildType);
		if (distribution == null)
			return NotFound();

		return distribution.ToAppApi();
	}
}
