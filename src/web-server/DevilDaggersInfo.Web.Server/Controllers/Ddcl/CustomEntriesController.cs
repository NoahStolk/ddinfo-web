using DevilDaggersInfo.Api.Ddcl.CustomLeaderboards;
using DevilDaggersInfo.Web.Server.Converters.ApiToDomain.Ddcl;
using DevilDaggersInfo.Web.Server.Converters.DomainToApi.Ddcl;
using DevilDaggersInfo.Web.Server.Domain.Exceptions;
using DevilDaggersInfo.Web.Server.Domain.Models.CustomLeaderboards;
using DevilDaggersInfo.Web.Server.Domain.Services;

namespace DevilDaggersInfo.Web.Server.Controllers.Ddcl;

[Route("api/ddcl/custom-entries")]
[ApiController]
[Obsolete("DDCL 1.8.3 will be removed.")]
public class CustomEntriesController : ControllerBase
{
	private readonly ILogger<CustomEntriesController> _logger;
	private readonly CustomEntryProcessor _customEntryProcessor;

	public CustomEntriesController(ILogger<CustomEntriesController> logger, CustomEntryProcessor customEntryProcessor)
	{
		_logger = logger;
		_customEntryProcessor = customEntryProcessor;
	}

	// FORBIDDEN: Used by ddstats-rust (currently not working however (2022-05-28)).
	[Obsolete("DDCL 1.8.3 will be removed.")]
	[HttpPost("/api/custom-entries/submit")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public async Task<ActionResult<GetUploadSuccess>> SubmitScoreForDdclObsolete([FromBody] AddUploadRequest uploadRequest)
	{
		try
		{
			UploadResponse response = await _customEntryProcessor.ProcessUploadRequestAsync(uploadRequest.ToDomain());
			return response.ToDdclApi();
		}
		catch (Exception ex) when (ex is not CustomEntryValidationException)
		{
			ex.Data[nameof(uploadRequest.ClientVersion)] = uploadRequest.ClientVersion;
			ex.Data[nameof(uploadRequest.OperatingSystem)] = uploadRequest.OperatingSystem;
			ex.Data[nameof(uploadRequest.BuildMode)] = uploadRequest.BuildMode;
			_logger.LogError(ex, "Upload failed for user `{playerName}` (`{playerId}`) for `{spawnset}`.", uploadRequest.PlayerName, uploadRequest.PlayerId, uploadRequest.SurvivalHashMd5.ByteArrayToHexString());
			throw;
		}
	}
}
