using DevilDaggersInfo.Web.Shared.Dto.Public.CustomEntries;

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
			return await _customEntryProcessor.ProcessUploadRequestAsync(uploadRequest);
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
