using DevilDaggersInfo.Api.Ddiam;
using DevilDaggersInfo.Web.Server.Converters.ApiToDomain.Ddiam;
using DevilDaggersInfo.Web.Server.Domain.Repositories;

namespace DevilDaggersInfo.Web.Server.Controllers.Ddiam;

[Route("api/ddiam/apps")]
[ApiController]
public class AppsController : ControllerBase
{
	private readonly ToolRepository _toolRepository;

	public AppsController(ToolRepository toolRepository)
	{
		_toolRepository = toolRepository;
	}

	[HttpGet]
	[ProducesResponseType(StatusCodes.Status200OK)]
	public async Task<ActionResult<List<GetApp>>> GetApps([Required] OperatingSystemType os)
	{
		List<Domain.Models.Tools.ToolDistribution> tools = await _toolRepository.GetLatestToolDistributionsAsync(os.ToDomain());

		return tools.ConvertAll(td => new GetApp
		{
			Name = td.Name,
			Version = td.VersionNumber,
			BuildType = td.BuildType,
		});
	}
}
