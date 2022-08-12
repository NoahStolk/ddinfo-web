using DevilDaggersInfo.Api.Ddcl;
using DevilDaggersInfo.Api.Ddcl.CustomLeaderboards;
using DevilDaggersInfo.Api.Ddcl.ProcessMemory;
using DevilDaggersInfo.Api.Ddcl.Tools;
using DevilDaggersInfo.Razor.CustomLeaderboard.HttpClients;
using DevilDaggersInfo.Types.Web;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Net.Http.Json;

namespace DevilDaggersInfo.Razor.CustomLeaderboard.Services;

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

	public async Task<long> GetMarker(SupportedOperatingSystem supportedOperatingSystem)
	{
		GetMarker marker = await _apiClient.GetMarker(supportedOperatingSystem);
		return marker.Value;
	}

	public async Task<bool> CheckIfLeaderboardExists(byte[] survivalHashMd5)
	{
		const int maxAttempts = 5;
		for (int i = 0; i < maxAttempts; i++)
		{
			try
			{
				HttpResponseMessage hrm = await _apiClient.CustomLeaderboardExistsBySpawnsetHash(survivalHashMd5);
				if (hrm.StatusCode == HttpStatusCode.OK)
					return true;

				if (hrm.StatusCode == HttpStatusCode.NotFound)
					return false;

				string error = await hrm.Content.ReadAsStringAsync();
				throw new HttpRequestException($"Unexpected status code '{hrm.StatusCode}': {error}"); // TODO: Do not retry.
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error while trying to check for existing leaderboard (attempt {attempt} out of {maxAttempts}).", i + 1, maxAttempts);
				await Task.Delay(TimeSpan.FromSeconds(1));
			}
		}

		return false;
	}

	public async Task<GetUploadSuccess> SubmitScore(AddUploadRequest uploadRequest)
	{
		HttpResponseMessage hrm = await _apiClient.SubmitScoreForDdcl(uploadRequest);
		if (hrm.IsSuccessStatusCode)
			return await hrm.Content.ReadFromJsonAsync<GetUploadSuccess>() ?? throw new InvalidOperationException($"Could not deserialize the response as '{nameof(GetUploadSuccess)}'.");

		throw new(await hrm.Content.ReadAsStringAsync());
	}

	public async Task<GetCustomLeaderboard> GetLeaderboard(byte[] hash)
	{
		const int maxAttempts = 5;
		for (int i = 0; i < maxAttempts; i++)
		{
			try
			{
				return await _apiClient.GetCustomLeaderboardBySpawnsetHash(hash);
			}
			catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
			{
				throw new("Not found");
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error while trying to retrieve leaderboard (attempt {attempt} out of {maxAttempts}).", i + 1, maxAttempts);
				await Task.Delay(TimeSpan.FromSeconds(1));
			}
		}

		throw new("Couldn't retrieve leaderboard after 5 attempts.");
	}

	public async Task<Page<GetCustomLeaderboardForOverview>> GetLeaderboardOverview(CustomLeaderboardCategory category, int pageIndex, int pageSize, int selectedPlayerId, bool onlyFeatured)
	{
		const int maxAttempts = 5;
		for (int i = 0; i < maxAttempts; i++)
		{
			try
			{
				return await _apiClient.GetCustomLeaderboardOverview(category, pageIndex, pageSize, selectedPlayerId, onlyFeatured);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error while trying to retrieve leaderboard overview (attempt {attempt} out of {maxAttempts}).", i + 1, maxAttempts);
				await Task.Delay(TimeSpan.FromSeconds(1));
			}
		}

		throw new Exception("Couldn't retrieve leaderboard overview after 5 attempts.");
	}

	public async Task<byte[]?> GetReplay(int customEntryId)
	{
		try
		{
			return (await _apiClient.GetCustomEntryReplayBufferById(customEntryId)).Data;
		}
		catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
		{
			_logger.LogWarning(ex, "Custom entry ID {id} does not have a replay.", customEntryId);
			return null;
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Error while trying to download replay.");
			return null;
		}
	}

	public async Task<byte[]> GetSpawnset(int spawnsetId)
	{
		return (await _apiClient.GetSpawnsetBufferById(spawnsetId)).Data;
	}
}
