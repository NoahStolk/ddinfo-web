using DevilDaggersInfo.Api.Ddcl.CustomLeaderboards;
using DevilDaggersInfo.Api.Ddcl.ProcessMemory;
using DevilDaggersInfo.Api.Ddcl.Tools;
using DevilDaggersInfo.Core.CustomLeaderboards.HttpClients;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Net.Http.Json;

namespace DevilDaggersInfo.Core.CustomLeaderboard.Services;

public class NetworkService
{
	private readonly ILogger<NetworkService> _logger;
	private readonly IClientConfiguration _clientConfiguration;

	private readonly DdclApiHttpClient _apiClient;

	public NetworkService(ILogger<NetworkService> logger, IClientConfiguration clientConfiguration)
	{
		_logger = logger;
		_clientConfiguration = clientConfiguration;

		_apiClient = new(new() { BaseAddress = new(clientConfiguration.GetHostBaseUrl()) });
	}

	public async Task<GetUpdate?> GetUpdate()
	{
		const int maxAttempts = 5;
		for (int i = 0; i < maxAttempts; i++)
		{
			try
			{
				return await _apiClient.GetUpdates(_clientConfiguration.GetToolPublishMethod(), _clientConfiguration.GetToolBuildType());
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error while trying to retrieve tool (attempt {attempt} out of {maxAttempts}).", i + 1, maxAttempts);
				await Task.Delay(TimeSpan.FromSeconds(1));
			}
		}

		return null;
	}

	public async Task<long?> GetMarker(SupportedOperatingSystem supportedOperatingSystem)
	{
		try
		{
			Marker marker = await _apiClient.GetMarker(supportedOperatingSystem);
			return marker.Value;
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Error while trying to get marker.");
			return null;
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
			if (hrm.IsSuccessStatusCode)
				return await hrm.Content.ReadFromJsonAsync<GetUploadSuccess>();

			// TODO: Return the error.
			string error = await hrm.Content.ReadAsStringAsync();
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
