using DevilDaggersInfo.Api.Ddcl.CustomLeaderboards;
using DevilDaggersInfo.Web.Server.Converters.ApiToDomain.Ddcl;
using DevilDaggersInfo.Web.Server.Converters.DomainToApi.Ddcl;
using DevilDaggersInfo.Web.Server.Domain.Models.CustomLeaderboards;
using DevilDaggersInfo.Web.Server.Domain.Repositories;
using DevilDaggersInfo.Web.Server.Domain.Services;

namespace DevilDaggersInfo.Web.Server.Controllers.Ddcl;

[Route("api/ddcl/custom-entries")]
[ApiController]
public class CustomEntriesController : ControllerBase
{
	private readonly ILogger<CustomEntriesController> _logger;
	private readonly CustomEntryProcessor _customEntryProcessor;
	private readonly CustomEntryRepository _customEntryRepository;

	public CustomEntriesController(ILogger<CustomEntriesController> logger, CustomEntryProcessor customEntryProcessor, CustomEntryRepository customEntryRepository)
	{
		_logger = logger;
		_customEntryProcessor = customEntryProcessor;
		_customEntryRepository = customEntryRepository;
	}

	[HttpGet("{id}/replay-buffer")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public ActionResult<GetCustomEntryReplayBuffer> GetCustomEntryReplayBufferById([Required] int id)
	{
		return new GetCustomEntryReplayBuffer
		{
			Data = _customEntryRepository.GetCustomEntryReplayBufferById(id),
		};
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
