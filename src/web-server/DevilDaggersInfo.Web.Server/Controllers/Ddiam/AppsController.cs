using DevilDaggersInfo.Api.Ddiam;
using DevilDaggersInfo.Web.Server.Converters.ApiToDomain.Ddiam;
using DevilDaggersInfo.Web.Server.Domain.Services;

namespace DevilDaggersInfo.Web.Server.Controllers.Ddiam;

[Route("api/ddiam/apps")]
[ApiController]
public class AppsController : ControllerBase
{
	private readonly IToolService _toolService;

	public AppsController(IToolService toolService)
	{
		_toolService = toolService;
	}

	[HttpGet]
	[ProducesResponseType(StatusCodes.Status200OK)]
	public async Task<ActionResult<List<GetApp>>> GetApps([Required] OperatingSystemType os)
	{
		List<Domain.Models.Tools.ToolDistribution> tools = await _toolService.GetLatestToolDistributionsAsync(os.ToDomain());

		return tools.ConvertAll(td => new GetApp
		{
			Name = td.Name,
			Version = td.VersionNumber,
			BuildType = td.BuildType,
		});
	}
}
