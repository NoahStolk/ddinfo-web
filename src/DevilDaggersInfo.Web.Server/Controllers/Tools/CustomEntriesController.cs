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
public sealed class CustomEntriesController(ILogger<CustomEntriesController> logger, CustomEntryProcessor customEntryProcessor, CustomEntryRepository customEntryRepository) : ControllerBase
{
	[HttpGet("{id}/replay-buffer")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<ActionResult<GetCustomEntryReplayBuffer>> GetCustomEntryReplayBufferById([Required] int id)
	{
		return new GetCustomEntryReplayBuffer
		{
			Data = await customEntryRepository.GetCustomEntryReplayBufferByIdAsync(id),
		};
	}

	[HttpPost("submit")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public async Task<ActionResult<GetUploadResponse>> SubmitScore([FromBody] AddUploadRequest uploadRequest)
	{
		try
		{
			UploadResponse response = await customEntryProcessor.ProcessUploadRequestAsync(uploadRequest.ToDomain());
			return response.ToToolsApi();
		}
		catch (Exception ex) when (ex is not CustomEntryValidationException)
		{
			ex.Data[nameof(uploadRequest.ClientVersion)] = uploadRequest.ClientVersion;
			ex.Data[nameof(uploadRequest.OperatingSystem)] = uploadRequest.OperatingSystem;
			ex.Data[nameof(uploadRequest.BuildMode)] = uploadRequest.BuildMode;
			logger.LogError(ex, "Upload failed for user `{PlayerName}` (`{PlayerId}`) for `{Spawnset}`.", uploadRequest.PlayerName, uploadRequest.PlayerId, BitConverter.ToString(uploadRequest.SurvivalHashMd5));
			throw;
		}
	}
}
