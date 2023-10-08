using DevilDaggersInfo.Web.ApiSpec.Tools.CustomLeaderboards;
using DevilDaggersInfo.Web.Server.Converters.ApiToDomain.Tools;
using DevilDaggersInfo.Web.Server.Converters.DomainToApi.Tools;
using DevilDaggersInfo.Web.Server.Domain.Exceptions;
using DevilDaggersInfo.Web.Server.Domain.Models.CustomLeaderboards;
using DevilDaggersInfo.Web.Server.Domain.Repositories;
using DevilDaggersInfo.Web.Server.Domain.Services;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace DevilDaggersInfo.Web.Server.Controllers.Tools;

[Route("api/app/custom-entries")]
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
	public async Task<ActionResult<GetCustomEntryReplayBuffer>> GetCustomEntryReplayBufferById([Required] int id)
	{
		return new GetCustomEntryReplayBuffer
		{
			Data = await _customEntryRepository.GetCustomEntryReplayBufferByIdAsync(id),
		};
	}

	[HttpPost("submit")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public async Task<ActionResult<GetUploadResponse>> SubmitScore([FromBody] AddUploadRequest uploadRequest)
	{
		try
		{
			UploadResponse response = await _customEntryProcessor.ProcessUploadRequestAsync(uploadRequest.ToDomain());
			return response.ToAppApi();
		}
		catch (Exception ex) when (ex is not CustomEntryValidationException)
		{
			ex.Data[nameof(uploadRequest.ClientVersion)] = uploadRequest.ClientVersion;
			ex.Data[nameof(uploadRequest.OperatingSystem)] = uploadRequest.OperatingSystem;
			ex.Data[nameof(uploadRequest.BuildMode)] = uploadRequest.BuildMode;
			_logger.LogError(ex, "Upload failed for user `{playerName}` (`{playerId}`) for `{spawnset}`.", uploadRequest.PlayerName, uploadRequest.PlayerId, BitConverter.ToString(uploadRequest.SurvivalHashMd5));
			throw;
		}
	}
}
