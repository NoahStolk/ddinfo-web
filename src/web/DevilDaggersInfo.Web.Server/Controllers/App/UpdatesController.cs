using DevilDaggersInfo.Api.App.Updates;
using DevilDaggersInfo.Web.Server.Converters.ApiToDomain.App;
using DevilDaggersInfo.Web.Server.Converters.DomainToApi.App;
using DevilDaggersInfo.Web.Server.Domain.Models.Tools;
using DevilDaggersInfo.Web.Server.Domain.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

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

	[HttpGet("latest-version")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<ActionResult<GetLatestVersion>> GetLatestVersion([Required] ToolPublishMethod publishMethod, [Required] ToolBuildType buildType)
	{
		ToolDistribution? distribution = await _toolRepository.GetLatestToolDistributionAsync(_toolName, publishMethod.ToDomain(), buildType.ToDomain());
		if (distribution == null)
			return NotFound();

		return distribution.ToAppApi();
	}

	[HttpGet("latest-version-file")]
	[ProducesResponseType(typeof(FileStreamResult), StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<ActionResult> GetLatestVersionFile([Required] ToolPublishMethod publishMethod, [Required] ToolBuildType buildType)
	{
		ToolDistribution? distribution = await _toolRepository.GetLatestToolDistributionAsync(_toolName, publishMethod.ToDomain(), buildType.ToDomain());
		if (distribution == null)
			return NotFound();

		byte[] bytes = await _toolRepository.GetToolDistributionFileAsync(_toolName, publishMethod.ToDomain(), buildType.ToDomain(), distribution.VersionNumber);

		await _toolRepository.UpdateToolDistributionStatisticsAsync(_toolName, publishMethod.ToDomain(), buildType.ToDomain(), distribution.VersionNumber);

		MemoryStream ms = new(bytes);
		return new FileStreamResult(ms, "application/octet-stream");
	}
}
