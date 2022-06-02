using DevilDaggersInfo.Api.Ddcl.CustomLeaderboards;
using DevilDaggersInfo.Web.Server.Converters.ApiToDomain.Ddcl;
using DevilDaggersInfo.Web.Server.Converters.DomainToApi.Ddcl;
using DevilDaggersInfo.Web.Server.InternalModels.CustomLeaderboards;

namespace DevilDaggersInfo.Web.Server.Controllers.Ddcl;

[Route("api/ddcl/custom-entries")]
[ApiController]
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
	[HttpPost("/api/custom-entries/submit")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public async Task<ActionResult<GetUploadSuccess>> SubmitScoreForDdcl([FromBody] AddUploadRequest uploadRequest)
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
