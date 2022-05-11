using DevilDaggersCustomLeaderboards.Clients;
using DevilDaggersInfo.Core.CustomLeaderboards.Data;
using DevilDaggersInfo.Core.CustomLeaderboards.Utils;
using Microsoft.Extensions.Logging;

namespace DevilDaggersInfo.Core.CustomLeaderboards.Services;

public class NetworkService
{
#if TESTING
	public static readonly string BaseUrl = "https://localhost:44318";
#else
	public static readonly string BaseUrl = "https://devildaggers.info";
#endif

	private readonly ILogger<NetworkService> _logger;
	private readonly DevilDaggersInfoApiClient _apiClient;

	public NetworkService(ILogger<NetworkService> logger)
	{
		_logger = logger;
		_apiClient = new(new() { BaseAddress = new(BaseUrl) });
	}

	public async Task CheckForUpdates(ClientInfo clientInfo)
	{
		Cmd.WriteLine("Checking for updates...");

		GetTool? tool = await GetTool(clientInfo);
		GetToolDistribution? distribution = await GetDistribution(clientInfo);
		Console.Clear();

		if (tool == null || distribution == null)
		{
			Cmd.WriteLine($"Failed to check for updates (host: {BaseUrl}).\n\n(Press any key to continue.)", ColorUtils.Error);
			Console.ReadKey();
			return;
		}

		Version localVersion = Version.Parse(clientInfo.ApplicationVersion);
		if (localVersion < Version.Parse(tool.VersionNumberRequired))
		{
			Cmd.WriteLine($"You are using an unsupported and outdated version of {clientInfo.ApplicationName} ({clientInfo.ApplicationVersion}).\n\nYou must use version {tool.VersionNumberRequired} or higher in order to submit scores.\n\nPlease update the program.\n\n(Press any key to continue.)", ColorUtils.Error);
			Console.ReadKey();
		}
		else if (localVersion < Version.Parse(distribution.VersionNumber))
		{
			Cmd.WriteLine($"{clientInfo.ApplicationName} version {distribution.VersionNumber} is available.\n\n(Press any key to continue.)", ColorUtils.Warning);
			Console.ReadKey();
		}
	}

	private async Task<GetTool?> GetTool(ClientInfo clientInfo)
	{
		const int maxAttempts = 5;
		for (int i = 0; i < maxAttempts; i++)
		{
			try
			{
				return await _apiClient.Tools_GetToolAsync(clientInfo.ApplicationName);
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

	private async Task<GetToolDistribution?> GetDistribution(ClientInfo clientInfo)
	{
		const int maxAttempts = 5;
		for (int i = 0; i < maxAttempts; i++)
		{
			try
			{
				ToolPublishMethod publishMethod = clientInfo.PublishMethod switch
				{
					"Default" => ToolPublishMethod.Default,
					"SelfContained" => ToolPublishMethod.SelfContained,
					_ => throw new NotSupportedException(),
				};

				ToolBuildType buildType = clientInfo.BuildType switch
				{
					"WindowsConsole" => ToolBuildType.WindowsConsole,
					_ => throw new NotSupportedException(),
				};

				return await _apiClient.Tools_GetLatestToolDistributionAsync(clientInfo.ApplicationName, publishMethod, buildType);
			}
			catch (Exception ex)
			{
				_logger.LogError("Error while trying to retrieve distribution.", ex);
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
				Marker marker = await _apiClient.ProcessMemory_GetMarkerAsync(SupportedOperatingSystem.Windows);
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
				await _apiClient.CustomLeaderboards_CustomLeaderboardExistsBySpawnsetHashAsync(survivalHashMd5);

				return true;
			}
			catch (DevilDaggersInfoApiException ex) when (ex.StatusCode == 404)
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
			return await _apiClient.CustomEntries_SubmitScoreForDdclAsync(uploadRequest);
		}
		catch (DevilDaggersInfoApiException<ProblemDetails> ex)
		{
			Cmd.WriteLine("Upload failed", ex.Result?.Title ?? "Empty response", ColorUtils.Error);
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
		catch (DevilDaggersInfoApiException ex) when (ex.StatusCode == 404)
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
