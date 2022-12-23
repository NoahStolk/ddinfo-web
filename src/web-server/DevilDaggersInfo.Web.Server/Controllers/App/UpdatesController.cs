using DevilDaggersInfo.Api.App.Tools;
using DevilDaggersInfo.Types.Web;
using DevilDaggersInfo.Web.Server.Converters.DomainToApi.App;
using DevilDaggersInfo.Web.Server.Domain.Models.Tools;
using DevilDaggersInfo.Web.Server.Domain.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Net.Mime;

namespace DevilDaggersInfo.Web.Server.Controllers.App;

[Route("api/app/updates")]
[ApiController]
public class UpdatesController : ControllerBase
{
	private const string _toolName = "ddinfo-tools";

	private readonly ToolRepository _toolRepository;

	public UpdatesController(ToolRepository toolRepository)
	{
		_toolRepository = toolRepository;
	}

	[HttpGet("version")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<ActionResult<GetToolDistribution>> GetVersion([Required] ToolPublishMethod publishMethod, [Required] ToolBuildType buildType)
	{
		ToolDistribution? distribution = await _toolRepository.GetLatestToolDistributionAsync(_toolName, publishMethod, buildType);
		if (distribution == null)
			return NotFound();

		return distribution.ToAppApi();
	}
}
