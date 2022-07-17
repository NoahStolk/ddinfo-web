using DevilDaggersInfo.Web.Server.Converters.ApiToDomain.Ddiam;
using DevilDaggersInfo.Web.Server.Domain.Models.Tools;
using DevilDaggersInfo.Web.Server.Domain.Services;
using ApiDdiam = DevilDaggersInfo.Api.Ddiam;

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
	public async Task<ActionResult<List<ApiDdiam.GetApp>>> GetApps([Required] ApiDdiam.OperatingSystemType os)
	{
		List<ToolDistribution> tools = await _toolService.GetLatestToolDistributionsAsync(os.ToDomain());

		return tools.ConvertAll(t => new ApiDdiam.GetApp
		{
			Name = t.Name,
			Version = t.VersionNumber,
		});
	}
}
