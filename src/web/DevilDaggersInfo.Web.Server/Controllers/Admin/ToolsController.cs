using DevilDaggersInfo.Web.ApiSpec.Admin.Tools;
using DevilDaggersInfo.Web.Core.Claims;
using DevilDaggersInfo.Web.Server.Domain.Admin.Converters.ApiToDomain;
using DevilDaggersInfo.Web.Server.Domain.Admin.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DevilDaggersInfo.Web.Server.Controllers.Admin;

[Route("api/admin/tools")]
[ApiController]
[Authorize(Roles = Roles.Admin)]
public class ToolsController : ControllerBase
{
	private readonly ToolService _toolService;

	public ToolsController(ToolService toolService)
	{
		_toolService = toolService;
	}

	[HttpPost]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public async Task<ActionResult> AddDistribution(AddDistribution distribution)
	{
		await _toolService.AddDistribution(distribution.Name, distribution.PublishMethod.ToDomain(), distribution.BuildType.ToDomain(), distribution.Version, distribution.ZipFileContents, distribution.UpdateVersion, distribution.UpdateRequiredVersion);
		return Ok();
	}
}
