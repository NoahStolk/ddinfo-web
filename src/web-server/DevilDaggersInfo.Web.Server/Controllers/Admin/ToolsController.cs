using DevilDaggersInfo.Api.Admin.Tools;
using DevilDaggersInfo.Web.Core.Claims;
using DevilDaggersInfo.Web.Server.Converters.ApiToDomain.Admin;
using DevilDaggersInfo.Web.Server.Domain.Services;
using Microsoft.AspNetCore.Authorization;

namespace DevilDaggersInfo.Web.Server.Controllers.Admin;

[Route("api/admin/tools")]
[ApiController]
[Authorize(Roles = Roles.Admin)]
public class ToolsController : ControllerBase
{
	private readonly IToolService _toolService;

	public ToolsController(IToolService toolService)
	{
		_toolService = toolService;
	}

	[HttpPost]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public async Task<ActionResult> AddDistribution(AddDistribution distribution)
	{
		await _toolService.AddDistribution(distribution.Name, distribution.PublishMethod.ToDomain(), distribution.BuildType.ToDomain(), distribution.Version, distribution.ZipFileContents);
		return Ok();
	}
}
