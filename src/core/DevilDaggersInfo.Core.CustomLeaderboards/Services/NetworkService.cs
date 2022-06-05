using DevilDaggersInfo.Api.Ddcl.CustomLeaderboards;
using DevilDaggersInfo.Api.Ddcl.ProcessMemory;
using DevilDaggersInfo.Api.Ddcl.Tools;
using DevilDaggersInfo.Core.CustomLeaderboards.Data;
using DevilDaggersInfo.Core.CustomLeaderboards.HttpClients;
using DevilDaggersInfo.Core.CustomLeaderboards.Utils;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Net.Http.Json;

namespace DevilDaggersInfo.Core.CustomLeaderboards.Services;

public class NetworkService
{
#if TESTING
	public static readonly string BaseUrl = "https://localhost:44318";
#else
	public static readonly string BaseUrl = "https://devildaggers.info";
#endif

	private readonly ILogger<NetworkService> _logger;
	private readonly DdclApiHttpClient _apiClient;

	public NetworkService(ILogger<NetworkService> logger)
	{
		_logger = logger;
		_apiClient = new(new() { BaseAddress = new(BaseUrl) });
	}

	public async Task CheckForUpdates(ClientInfo clientInfo)
	{
		Cmd.WriteLine("Checking for updates...");

		GetUpdate? update = await GetTool(clientInfo);
		Console.Clear();

		if (update == null)
		{
			Cmd.WriteLine($"Failed to check for updates (host: {BaseUrl}).\n\n(Press any key to continue.)", ColorUtils.Error);
			Console.ReadKey();
			return;
		}

		Version localVersion = Version.Parse(clientInfo.ApplicationVersion);
		if (localVersion < Version.Parse(update.VersionNumberRequired))
		{
			Cmd.WriteLine($"You are using an unsupported and outdated version of {clientInfo.ApplicationName} ({clientInfo.ApplicationVersion}).\n\nYou must use version {update.VersionNumberRequired} or higher in order to submit scores.\n\nPlease update the program.\n\n(Press any key to continue.)", ColorUtils.Error);
			Console.ReadKey();
		}
		else if (localVersion < Version.Parse(update.VersionNumber))
		{
			Cmd.WriteLine($"{clientInfo.ApplicationName} version {update.VersionNumber} is available.\n\n(Press any key to continue.)", ColorUtils.Warning);
			Console.ReadKey();
		}
	}

	private async Task<GetUpdate?> GetTool(ClientInfo clientInfo)
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
				_logger.LogError("Error while trying to retrieve tool.", ex);
				string message = $"An error occurred while trying to check for updates. Retrying in 1 second... (attempt {i + 1} out of {maxAttempts})";
				Cmd.WriteLine(message, string.Empty, ColorUtils.Error);

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
				_logger.LogError("Error while trying to get marker.", ex);
				const string message = "An error occurred while trying to retrieve marker. Retrying in 1 second...";
				Cmd.WriteLine(message, string.Empty, ColorUtils.Error);

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
				Cmd.WriteLine("This spawnset does not have a leaderboard.", string.Empty, ColorUtils.Warning);

				return false;
			}
			catch (Exception ex)
			{
				_logger.LogError("Error while trying to check for existing leaderboard.", ex);
				string message = $"An error occurred while trying to check for existing leaderboard. Retrying in 1 second... (attempt {i + 1} out of {maxAttempts})";
				Cmd.WriteLine(message, string.Empty, ColorUtils.Error);

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
			Cmd.WriteLine("Upload failed", ex.Message ?? "Empty response", ColorUtils.Error);
			return null;
		}
		catch (Exception ex)
		{
			Cmd.WriteLine("Upload failed", ex.Message, ColorUtils.Error);
			_logger.LogError(ex, "Error trying to submit score");
			return null;
		}
	}

	public async Task<byte[]?> GetReplay(int customEntryId)
	{
		try
		{
			FileResponse fr = await _apiClient.CustomEntries_GetCustomEntryReplayByIdAsync(customEntryId);

			using MemoryStream ms = new();
			fr.Stream.CopyTo(ms);
			return ms.ToArray();
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
