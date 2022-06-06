using DevilDaggersInfo.Api.Ddcl.CustomLeaderboards;
using DevilDaggersInfo.Api.Ddcl.ProcessMemory;
using DevilDaggersInfo.Api.Ddcl.Tools;
using DevilDaggersInfo.Core.CustomLeaderboard.Configuration;
using DevilDaggersInfo.Core.CustomLeaderboards.HttpClients;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Http.Json;

namespace DevilDaggersInfo.Core.CustomLeaderboard.Services;

public class NetworkService
{
	//#if TESTING
	//	public static readonly string BaseUrl = "https://localhost:44318";
	//#else
	//	public static readonly string BaseUrl = "https://devildaggers.info";
	//#endif

	private readonly ILogger<NetworkService> _logger;
	private readonly DdclApiHttpClient _apiClient;

	public NetworkService(ILogger<NetworkService> logger, IOptions<HostOptions> hostOptions)
	{
		_logger = logger;
		_apiClient = new(new() { BaseAddress = new(hostOptions.Value.HostBaseUrl) });
	}

	public async Task<GetUpdate?> GetUpdate(ClientOptions clientInfo)
	{
		const int maxAttempts = 5;
		for (int i = 0; i < maxAttempts; i++)
		{
			try
			{
				return await _apiClient.GetUpdates(clientInfo.PublishMethod, clientInfo.BuildType);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error while trying to retrieve tool (attempt {attempt} out of {maxAttempts}).", i + 1, maxAttempts);
				await Task.Delay(TimeSpan.FromSeconds(1));
			}
		}

		return null;
	}

	public async Task<long> GetMarker()
	{
		while (true)
		{
			try
			{
				Marker marker = await _apiClient.GetMarker(SupportedOperatingSystem.Windows);
				return marker.Value;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error while trying to get marker.");
				await Task.Delay(TimeSpan.FromSeconds(1));
			}
		}
	}

	public async Task<bool> CheckIfLeaderboardExists(byte[] survivalHashMd5)
	{
		const int maxAttempts = 5;
		for (int i = 0; i < maxAttempts; i++)
		{
			try
			{
				await _apiClient.CustomLeaderboardExistsBySpawnsetHash(survivalHashMd5);

				return true;
			}
			catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
			{
				return false;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error while trying to check for existing leaderboard (attempt {attempt} out of {maxAttempts}).", i + 1, maxAttempts);
				await Task.Delay(TimeSpan.FromSeconds(1));
			}
		}

		return false;
	}

	public async Task<GetUploadSuccess?> SubmitScore(AddUploadRequest uploadRequest)
	{
		try
		{
			HttpResponseMessage hrm = await _apiClient.SubmitScoreForDdcl(uploadRequest);
			return await hrm.Content.ReadFromJsonAsync<GetUploadSuccess>();
		}
		catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.BadRequest)
		{
			// TODO: Return bad request message.
			return null;
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Error trying to submit score");
			return null;
		}
	}

	public async Task<byte[]?> GetReplay(int customEntryId)
	{
		try
		{
			return (await _apiClient.GetCustomEntryReplayBufferById(customEntryId)).Data;
		}
		catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
		{
			return null;
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Error while trying to download replay.");
			return null;
		}
	}
}
